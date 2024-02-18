using Interfaces;
using MongoDB.Driver;
using PatientsResolver.API.Data.Store;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Services
{
    public class PatientsDataService
    {
        private readonly IPatientsStore _patientsStore;
        private readonly IParametersStore _parametersStore;
        private readonly InfluencesDataService _influencesDataService;

        private readonly ConcurrentDictionary<(string, string), IPatient> _patients;

        public PatientsDataService(IPatientsStore patientsStore,
            IParametersStore parametersStore,
            InfluencesDataService influencesDataService)
        {
            _patientsStore = patientsStore;
            _parametersStore = parametersStore;
            _influencesDataService = influencesDataService;
            _patients = new();
        }


        public async Task<IPatient> Get(string patientId, string affiliation)
        {
            if (_patients.TryGetValue((patientId, affiliation), out IPatient p))
                return p;
            p = await _patientsStore.Get(patientId, affiliation);

            if (p == null)
            {
                p = await _patientsStore.Insert(patientId, affiliation);
                _patients[(p.PatientId, p.Affiliation)] = p;
            }

            _patients[(patientId, affiliation)] = p;
            return p;
        }



        public async Task Update(string id, IPatient patient)
        {
            await _patientsStore.Update(id, patient);
            _patients[(patient.PatientId, patient.Affiliation)] = patient;
        }



        public async Task Delete(string id)
        {
            var patient = await _patientsStore.Get(id);
            if (patient == null)
                return;
            await _patientsStore.Delete(id);
            _patients.TryRemove((patient.PatientId, patient.Affiliation), out _);
        }


        public async Task<IPatient> Insert(IPatient p)
        {
            await _patientsStore.Insert(p);
            _patients[(p.PatientId, p.Affiliation)] = p;
            return p;
        }


        public async Task Insert(IEnumerable<IPatient> patients)
        {
            foreach (var p in patients)
            {
                await _patientsStore.Insert(p);
                _patients[(p.PatientId, p.Affiliation)] = p;
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
            await _parametersStore.Insert(parameters);
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
