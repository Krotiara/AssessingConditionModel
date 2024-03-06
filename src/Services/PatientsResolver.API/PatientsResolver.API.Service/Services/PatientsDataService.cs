using ASMLib;
using ASMLib.EventBus;
using Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PatientsResolver.API.Data.Store;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Events;
using System.Collections.Concurrent;

namespace PatientsResolver.API.Service.Services
{
    public class PatientsDataService
    {
        private readonly IPatientsStore _patientsStore;
        private readonly IParametersStore _parametersStore;
        private readonly IPatientsMetaStore _patientsMetaStore;
        private readonly InfluencesDataService _influencesDataService;
        private readonly ILogger<PatientsDataService> _logger;
        private readonly IEventBus _eventBus;

        private readonly ConcurrentDictionary<(string, string), PatientInfo> _patients;

        public PatientsDataService(IPatientsStore patientsStore,
            IParametersStore parametersStore,
            IPatientsMetaStore patientsMetaStore,
            InfluencesDataService influencesDataService,
            ILogger<PatientsDataService> logger,
            IEventBus eventBus = null)
        {
            _patientsStore = patientsStore;
            _parametersStore = parametersStore;
            _patientsMetaStore = patientsMetaStore;
            _influencesDataService = influencesDataService;
            _logger = logger;
            _eventBus = eventBus;
            _patients = new();
        }


        public async Task<PatientInfo> Get(string patientId, string affiliation)
        {
            if (_patients.TryGetValue((patientId, affiliation), out PatientInfo p))
                return p;
            var patient = await _patientsStore.Get(patientId, affiliation);
            var meta = await _patientsMetaStore.Get(patient.Id);
            if (patient == null || meta == null)
                throw new KeyNotFoundException($"Не найден пациент {patientId}:{affiliation}.");

            _patients[(patientId, affiliation)] = new PatientInfo()
            {
                Patient = patient,
                Meta = meta
            };
            return _patients[(patientId, affiliation)];
        }



        public async Task Update(string id, IPatient patient)
        {
            var dbPatient = await _patientsStore.Get(id);
            if (dbPatient == null || !_patients.ContainsKey((patient.PatientId, patient.Affiliation)))
                throw new KeyNotFoundException($"Не найден пациент.");

            await _patientsStore.Update(id, patient);
            _patients[(patient.PatientId, patient.Affiliation)].Patient = patient;
            _eventBus?.Publish(new UpdatePatientEvent()
            {
                PatientId = patient.PatientId,
                PatientAffiliation = patient.Affiliation
            });
        }



        public async Task Delete(string id)
        {
            var patient = await _patientsStore.Get(id);
            if (patient == null)
                throw new KeyNotFoundException($"Не найден пациент.");
            await _patientsStore.Delete(id);
            await _patientsMetaStore.Delete(id);
            _patients.TryRemove((patient.PatientId, patient.Affiliation), out _);
            _eventBus?.Publish(new UpdatePatientEvent()
            {
                PatientId = patient.PatientId,
                PatientAffiliation = patient.Affiliation
            });
        }


        public async Task DeleteParameters(string patientId, string affiliation, DateTime timestamp)
        {
            var patientInfo = await Get(patientId, affiliation);
            if (patientInfo == null)
                throw new KeyNotFoundException($"Не найден пациент {patientId}:{affiliation}.");

            await _parametersStore.DeleteAll(patientId, timestamp);
            if (patientInfo.Meta.InputParametersTimestamps.Remove(timestamp))
                await _patientsMetaStore.Insert(patientInfo.Meta);
            
            _eventBus?.Publish(new UpdatePatientParametersEvent()
            {
                PatientId = patientId,
                PatientAffiliation = affiliation,
                InputParametersTimestamps = patientInfo.Meta.InputParametersTimestamps
            });
        }


        public async Task<PatientInfo> Insert(IPatient p)
        {
            try
            {
                p = await _patientsStore.Insert(p);
                var meta = await _patientsMetaStore.Insert(new PatientMeta(p.Id));
                _patients[(p.PatientId, p.Affiliation)] = new PatientInfo()
                {
                    Patient = p,
                    Meta = meta
                };
                _eventBus?.Publish(new AddPatientEvent()
                {
                    PatientId = p.PatientId,
                    PatientAffiliation = p.Affiliation
                });
                return _patients[(p.PatientId, p.Affiliation)];
            }
            catch (EntityAlreadyExistException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public async Task Insert(IEnumerable<IPatient> patients)
        {
            foreach (var p in patients)
                await Insert(p);
        }


        public async Task<IEnumerable<PatientParameter>> GetPatientParameters(string patientId, string affiliation, DateTime start, DateTime end, List<string> names)
        {
            var patient = await Get(patientId, affiliation);
            return await _parametersStore.GetParameters(patient.Patient.Id, start, end, names);
        }


        public async Task AddPatientParameters(string id, string affiliation, IEnumerable<PatientParameter> parameters)
        {
            var p = await Get(id, affiliation);
            if (p == null)
                throw new KeyNotFoundException($"Не найден пациент {id}:{affiliation}.");

            foreach (var parameter in parameters)
                parameter.PatientId = p.Patient.Id;

            var latest = parameters.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            if (latest != null)
                p.Meta.InputParametersTimestamps.Add(latest.Timestamp);
            await _parametersStore.Insert(parameters);
            await _patientsMetaStore.Insert(p.Meta);
            _eventBus?.Publish(new UpdatePatientParametersEvent()
            {
                PatientId = p.Patient.PatientId,
                PatientAffiliation = p.Patient.Affiliation,
                InputParametersTimestamps = p.Meta.InputParametersTimestamps
            });
        }


        public async Task<IEnumerable<PatientInfo>> GetPatients(string affiliation,
            GenderEnum? gender, string? influenceName, int? startAge, int? endAge, DateTime? start, DateTime? end)
        {
            int searchStartAge = startAge == null ? 0 : startAge.Value;
            int searchEndAge = endAge == null ? 100 : endAge.Value;
            start = start == null ? DateTime.MinValue : start;
            end = end == null ? DateTime.MaxValue : end;
            DateTime now = DateTime.Now;


            List<IPatient> patients =
                (await _patientsStore.GetAll(affiliation))
                .Where(x =>
                {
                    if (x.Birthday == null)
                        return true;
                    int age = GetAge((DateTime)x.Birthday, now);
                    return age >= searchStartAge && age <= searchEndAge;
                })
                .ToList();

            IEnumerable<IPatient> filteredPatients = patients;

            if (gender != null && gender != GenderEnum.None)
                filteredPatients = patients.Where(x => x.Gender == gender);

            if (influenceName != null)
            {
                filteredPatients = await _influencesDataService.FilterByInfluence(patients, influenceName, (DateTime)start, (DateTime)end);
            }

            var result = new List<PatientInfo>();
            foreach (var p in filteredPatients)
            {
                var meta = await _patientsMetaStore.Get(p.Id);
                if (meta != null)
                    result.Add(new PatientInfo()
                    {
                        Patient = p,
                        Meta = meta
                    });
            }

            return result;
        }


        private int GetAge(DateTime dateOfBirth, DateTime dateOfMeasurement)
        {
            var a = ((dateOfMeasurement.Year * 100) + dateOfMeasurement.Month) * 100 + dateOfMeasurement.Day;
            var b = (((dateOfBirth.Year * 100) + dateOfBirth.Month) * 100) + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
