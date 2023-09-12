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

        private readonly ConcurrentDictionary<(string, string), Patient> _patients;

        public PatientsDataService(PatientsStore patientsStore)
        {
            _patientsStore = patientsStore;
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


        public async Task Insert(Patient p)
        {
            await _patientsStore.Insert(p);
            _patients[(p.PatientId, p.Affiliation)] = p;
        }


        public async Task Insert(IEnumerable<Patient> patients)
        {
            foreach (var patient in patients)
                await Insert(patient);
        }


        //TODO - заменить store на istore
        public async Task<IEnumerable<Parameter>> GetPatientParameters(string patientId, string affiliation, DateTime start, DateTime end, List<string> names)
        {
            var namesHashes = new HashSet<string>(names);
            var patient = await Get(patientId, affiliation);
            if (patient == null || patient.Parameters == null)
                return Enumerable.Empty<Parameter>();

            return patient.Parameters.Where(x => namesHashes.Contains(x.Key.Item1)
                                                && x.Key.Item2 <= end
                                                && x.Key.Item2 >= start)
                                     .Select(x => new Parameter()
                                     {
                                         Name = x.Key.Item1,
                                         Timestamp = x.Key.Item2,
                                         Value = x.Value
                                     });
        }


        public async Task AddPatientParameters(string id, string affiliation, IEnumerable<Parameter> parameters)
        {
            var p = await Get(id, affiliation);

            if (p == null)
                return;

            if (p.Parameters == null)
                p.Parameters = new();

            foreach (var par in parameters)
                p.Parameters[(par.Name, par.Timestamp)] = par.Value;

            await _patientsStore.Update(x => x.Id == p.Id)
                .Set(x => x.Parameters, p.Parameters)
                .Execute();
        }


        public async Task<IEnumerable<Patient>> GetPatients(Expression<Func<Patient, bool>> filter)
        {
            return await _patientsStore.Query(filter);
        }
    }
}
