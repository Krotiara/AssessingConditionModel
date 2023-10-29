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
        private readonly PatientsStore _patientsStore;
        private readonly InfluencesDataService _influencesDataService;

        private readonly ConcurrentDictionary<(string, string), Patient> _patients;

        public PatientsDataService(PatientsStore patientsStore, InfluencesDataService influencesDataService)
        {
            _patientsStore = patientsStore;
            _influencesDataService = influencesDataService;
            _patients = new();
        }


        public async Task<Patient> Get(string patientId, string affiliation)
        {
            if (_patients.TryGetValue((patientId, affiliation), out Patient p))
                return p;
            p = await _patientsStore.Get(x => x.PatientId == patientId && x.Affiliation == affiliation);
            if (p != null)
                _patients[(patientId, affiliation)] = p;
            return p;
        }



        public async Task Update(string id, Patient patient)
        {
            await _patientsStore.Update(x => x.Id == id)
                        .Set(x => x.Affiliation, patient.Affiliation)
                        .Set(x => x.PatientId, patient.PatientId)
                        .Set(x => x.TreatmentStatus, patient.TreatmentStatus)
                        .Set(x => x.Name, patient.Name)
                        .Set(x => x.Birthday, patient.Birthday)
                        .Set(x => x.Gender, patient.Gender)
                        .Execute();
            _patients[(patient.PatientId, patient.Affiliation)] = patient;
        }



        public async Task Delete(string id)
        {
            var patient = await _patientsStore.Get(x => x.Id == id);
            if (patient == null)
                return;
            await _patientsStore.Delete(x => x.Id == id);
            _patients.TryRemove((patient.PatientId, patient.Affiliation), out _);
        }


        public async Task<Patient> Insert(Patient p)
        {
            await _patientsStore.Insert(p);
            _patients[(p.PatientId, p.Affiliation)] = p;
            return p;
        }


        public async Task Insert(IEnumerable<Patient> patients)
        {
            foreach (var patient in patients)
                await Insert(patient);
        }


        //TODO - заменить store на istore
        public async Task<IEnumerable<Parameter>> GetPatientParameters(string patientId, string affiliation, DateTime start, DateTime end, List<string> names)
        {
            var patient = await Get(patientId, affiliation);
            if (patient == null || patient.Parameters == null)
                return Enumerable.Empty<Parameter>();

            return patient.GetParameters(start, end, names);
        }


        public async Task AddPatientParameters(string id, string affiliation, IEnumerable<Parameter> parameters)
        {
            var p = await Get(id, affiliation);

            if (p == null)
                p = await Insert(new Patient()
                {
                    PatientId = id,
                    Affiliation = affiliation
                });

            if (p.Parameters == null)
                p.Parameters = new();

            foreach (var par in parameters)
                p.SetParameter(par);

            await _patientsStore.Update(x => x.Id == p.Id)
                .Set(x => x.Parameters, p.Parameters)
                .Execute();
        }


        public async Task<IEnumerable<Patient>> GetPatients(Expression<Func<Patient, bool>> filter)
        {
            return await _patientsStore.Query(filter);
        }


        public async Task<IEnumerable<Patient>> GetPatients(string affiliation, GenderEnum? gender, string? influenceName, int? startAge, int? endAge, DateTime? start, DateTime? end)
        {
            int searchStartAge = startAge == null ? 0 : startAge.Value;
            int searchEndAge = endAge == null ? 100 : endAge.Value;
            start = start == null ? DateTime.MinValue : start;
            end = end == null ? DateTime.MaxValue : end;
            DateTime now = DateTime.Now;

            //TODO избавиться от двух расчетов GetAge.
            IEnumerable<Patient> patients = 
                await GetPatients(x => x.Affiliation == affiliation && GetAge(x.Birthday, now) >= startAge && GetAge(x.Birthday, now) <= endAge);

            if (gender != null)
                patients = patients.Where(x => x.Gender == gender);


            if (influenceName != null)
            {
                patients = await _influencesDataService.FilterByInfluence(patients, influenceName, (DateTime)start, (DateTime)end);
            }

            return patients;
        }


        private int GetAge(DateTime dateOfBirth, DateTime dateOfMeasurement)
        {
            var a = ((dateOfMeasurement.Year * 100) + dateOfMeasurement.Month) * 100 + dateOfMeasurement.Day;
            var b = (((dateOfBirth.Year * 100) + dateOfBirth.Month) * 100) + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
