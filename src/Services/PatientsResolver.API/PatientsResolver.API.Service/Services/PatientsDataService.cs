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
        private readonly InfluencesDataService _influencesDataService;
        private readonly ILogger<PatientsDataService> _logger;
        private readonly IEventBus _eventBus;
        private readonly ConcurrentDictionary<(string, string), IPatient> _patients;

        public PatientsDataService(IPatientsStore patientsStore,
            IParametersStore parametersStore,
            InfluencesDataService influencesDataService,
            ILogger<PatientsDataService> logger,
            IEventBus eventBus = null)
        {
            _patientsStore = patientsStore;
            _parametersStore = parametersStore;
            _influencesDataService = influencesDataService;
            _logger = logger;
            _eventBus = eventBus;
            _patients = new();
        }


        public async Task<IPatient> Get(string patientId, string affiliation)
        {
            if (_patients.TryGetValue((patientId, affiliation), out IPatient p))
                return p;
            p = await _patientsStore.Get(patientId, affiliation);

            if (p == null)
                throw new KeyNotFoundException($"Не найден пациент {patientId}:{affiliation}.");

            _patients[(patientId, affiliation)] = p;
            return p;
        }



        public async Task Update(string id, IPatient patient)
        {
            var dbPatient = await _patientsStore.Get(id);
            if (dbPatient == null)
                throw new KeyNotFoundException($"Не найден пациент.");
            await _patientsStore.Update(id, patient);
            _patients[(patient.PatientId, patient.Affiliation)] = patient;
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
            _patients.TryRemove((patient.PatientId, patient.Affiliation), out _);
            _eventBus?.Publish(new UpdatePatientEvent()
            {
                PatientId = patient.PatientId,
                PatientAffiliation = patient.Affiliation
            });
        }


        public async Task DeleteAllParameters(string patientId)
        {
            var patient = await _patientsStore.Get(patientId);
            if (patient == null)
                throw new KeyNotFoundException($"Не найден пациент.");
            await _parametersStore.DeleteAll(patientId);
            _eventBus?.Publish(new UpdatePatientParametersEvent()
            {
                PatientId = patient.PatientId,
                PatientAffiliation = patient.Affiliation
            });
        }


        public async Task<IPatient> Insert(IPatient p)
        {
            try
            {
                p = await _patientsStore.Insert(p);
                _patients[(p.PatientId, p.Affiliation)] = p;
                _eventBus?.Publish(new AddPatientEvent()
                {
                    PatientId = p.PatientId,
                    PatientAffiliation = p.Affiliation
                });
                return p;
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
            {
                var patient = await _patientsStore.Insert(p);
                _patients[(patient.PatientId, patient.Affiliation)] = patient;
            }
        }


        public async Task<IEnumerable<PatientParameter>> GetPatientParameters(string patientId, string affiliation, DateTime start, DateTime end, List<string> names)
        {
            var patient = await Get(patientId, affiliation);
            return await _parametersStore.GetParameters(patient.Id, start, end, names);
        }


        public async Task AddPatientParameters(string id, string affiliation, IEnumerable<PatientParameter> parameters)
        {
            var p = await Get(id, affiliation);
            foreach (var parameter in parameters)
                parameter.PatientId = p.Id;
            await _parametersStore.Insert(parameters);
            _eventBus?.Publish(new UpdatePatientParametersEvent()
            {
                PatientId = p.PatientId,
                PatientAffiliation = p.Affiliation
            });
        }


        public async Task<IEnumerable<IPatient>> GetPatients(string affiliation,
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

            return filteredPatients;
        }


        private int GetAge(DateTime dateOfBirth, DateTime dateOfMeasurement)
        {
            var a = ((dateOfMeasurement.Year * 100) + dateOfMeasurement.Month) * 100 + dateOfMeasurement.Day;
            var b = (((dateOfBirth.Year * 100) + dateOfBirth.Month) * 100) + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
