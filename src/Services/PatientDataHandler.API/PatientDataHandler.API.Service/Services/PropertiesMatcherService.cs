using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Service.Services
{
    public class PropertiesMatcherService : IPropertiesMatcherService
    {
        private readonly Dictionary<string[], Parameters> matchingDictionary = new()
        {
            { new string[] {"id","номер","номер истории болезни" }, Parameters.Id },
            { new string[] {"возраст" }, Parameters.Age },
            { new string[] {"пол"}, Parameters.Gender },
            { new string[] {"дата вступления" }, Parameters.HospitalizationDate},
            { new string[] {"анамнез" }, Parameters.Anamnesis},
            { new string[] {"артериальная гипертензия", "аг"}, Parameters.ArterialHypertension },
            { new string[] {"фп пароксизмальная" }, Parameters.ParoxysmalAtrialFibrillation},
            { new string[] {"желудочковые нр" }, Parameters.VentricularArrhythmias },
            { new string[] {"жэс" }, Parameters.VentricularExtrasystole },
            { new string[] {"пробежки/пароксизмы жт", "жт" }, Parameters.VentricularTachycardia },
            { new string[] {"инфаркт миокарда" }, Parameters.MyocardialInfarction },
            { new string[] {"стенокардия" }, Parameters.Stenocardia },
            { new string[] {"фк стенокардии" }, Parameters.AnginaPectorisFunctionalClass },
            { new string[] {"cахарный диабет 2", "cахарный диабет" }, Parameters.DiabetesMellitus },
            { new string[] { "курение"  }, Parameters.Smoking },
            { new string[] {"баллы по мориски" }, Parameters.Moriski },
            { new string[] {"баллы по шокс"  }, Parameters.Shock },
            { new string[] {"сад"}, Parameters.SystolicPressure },
            { new string[] {"дад"}, Parameters.SystolicPressure },
            { new string[] {"чсс"}, Parameters.SystolicPressure },
            { new string[] {"гемоглобин","hb", "hgb"}, Parameters.SystolicPressure },
            { new string[] { "мочевина" }, Parameters.SystolicPressure },
            { new string[] { "креатинин"}, Parameters.SystolicPressure },
            { new string[] {"калий"}, Parameters.SystolicPressure },
            { new string[] {"глюкоза"}, Parameters.SystolicPressure },
            { new string[] {"общий билирубин"}, Parameters.SystolicPressure },
            { new string[] {"охс" }, Parameters.SystolicPressure },
            { new string[] { "хбп"}, Parameters.SystolicPressure },
            { new string[] { "вес" }, Parameters.SystolicPressure },
            { new string[] {"рост"}, Parameters.SystolicPressure },
            { new string[] {"имт" }, Parameters.SystolicPressure },
            { new string[] {"bnp" }, Parameters.SystolicPressure },
            { new string[] {"качество жизни" }, Parameters.SystolicPressure },
            { new string[] {"кср" }, Parameters.SystolicPressure },
            { new string[] {"кдр" }, Parameters.SystolicPressure },
            { new string[] { "ксо" }, Parameters.SystolicPressure },
            { new string[] { "кдо"}, Parameters.SystolicPressure },
            { new string[] { "фв"}, Parameters.SystolicPressure }
        };


        public Parameters GetParameterBy(string header)
        {
            header = header.ToLower();
            foreach (KeyValuePair < string[],Parameters> pair in matchingDictionary)
            {
                //TODO - сейчас сложность чуть ли не n^2. Нужно быстрее.
                if (pair.Key.Contains(header))
                    return pair.Value;

            }
            throw new KeyNotFoundException();
        }
    }
}
