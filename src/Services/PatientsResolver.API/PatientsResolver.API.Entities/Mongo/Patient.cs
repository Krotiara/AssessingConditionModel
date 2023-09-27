using Interfaces;
using Interfaces.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities.Mongo
{
    public class Patient : Document
    {
        public string PatientId { get; set; }
        public string Affiliation { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public GenderEnum Gender { get; set; }
        public TreatmentStatus TreatmentStatus { get; set; }

        /// <summary>
        /// key = name_date
        /// </summary>
        [JsonIgnore]
        public ConcurrentDictionary<string, double> Parameters { get; set; }


        public void SetParameter(Parameter p)
        {
            string key = GetKey(p.Name, p.Timestamp);
            Parameters[key] = p.Value;
        }


        public IEnumerable<Parameter> GetParameters(DateTime start, DateTime end, List<string> names)
        {
            var namesHashes = new HashSet<string>(names);
            return Parameters.Where(x =>
            {
                var keyFields = GetKeyFields(x.Key);
                DateTime time = keyFields.Item1;
                string name = keyFields.Item2;
                return namesHashes.Contains(name) && time <= end && time >= start;
            }).Select(x =>
            {
                var keyFields = GetKeyFields(x.Key);
                DateTime time = keyFields.Item1;
                string name = keyFields.Item2;
                return new Parameter()
                {
                    Name = name,
                    Timestamp = time,
                    Value = x.Value
                };
            });
        }


        private string GetKey(string name, DateTime date) => $"{name}_{date}";


        private (DateTime, string) GetKeyFields(string key)
        {
            var s = key.Split("_");
            return (DateTime.Parse(s[0]), s[1]);
        }

    }
}
