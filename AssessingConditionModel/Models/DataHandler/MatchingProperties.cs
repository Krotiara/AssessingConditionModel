using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.DataHandler
{
    public class MatchingProperties
    {
        /// <summary>
        /// Словарь соответствий колонок входных данных и относительных путей до соответсвующих свойств объекта Patient. 
        /// </summary>
        private readonly Dictionary<string, string> matchingPropertiesNames = new Dictionary<string, string>()
        {
            {"номер истории болезни", "MedicalHistoryNumber" },
            {"кашель","ClinicalParameters.IsCough" },
            {"температура максимально", "ClinicalParameters.Temperature" },
            {"сатурация", "ClinicalParameters.Saturation" },
            {"чдд", "ClinicalParameters.FRM" },
            {"чсс", "ClinicalParameters.HeartRate" },
            {"срб","ClinicalParameters.CReactiveProtein" },
            
            {"пол", "FunctionalParameters.Gender" },
            {"возраст", "FunctionalParameters.Age" },
            {"возраст ребенка", "FunctionalParameters.Age" },
           
            {"дата поступления", "ClinicalParameters.Date"},
            {"правостороннее","ClinicalParameters.LungTissueDamage.IsRightHandDamage" },
            {"левостороннее","ClinicalParameters.LungTissueDamage.IsLeftHandDamage" },
            {"двухстороннее","ClinicalParameters.LungTissueDamage.IsTwoWayDamage" },
            {"правое легкое","ClinicalParameters.LungTissueDamage.RightLungDamageDescription" },
            {"левое легкое","ClinicalParameters.LungTissueDamage.LeftLungDamageDescription" },
            {"объем поражения","ClinicalParameters.LungTissueDamage.DamageVolumeDescription" },
            
                      
            {"Er","ClinicalParameters.GeneralBloodTest.Er" },
            {"Hb","ClinicalParameters.GeneralBloodTest.Hb" },
            {"Ley","ClinicalParameters.GeneralBloodTest.Ley" },
            {"Gran %","ClinicalParameters.GeneralBloodTest.GranPercent" },
            {"Gran","ClinicalParameters.GeneralBloodTest.Gran" },
            {"Lym %","ClinicalParameters.GeneralBloodTest.LymPercent" },
            {"lym","ClinicalParameters.GeneralBloodTest.Lym" },
            {"Mono %","ClinicalParameters.GeneralBloodTest.MonoPercent" },
            {"mono","ClinicalParameters.GeneralBloodTest.Mono" },
            {"Tr","ClinicalParameters.GeneralBloodTest.Tr" },

           // { "Лейкоциты","ClinicalParameters.GeneralUrineAnalysis.WhiteBloodCells" },
           // { "эритроциты свежие","ClinicalParameters.GeneralUrineAnalysis.FreshRedBloodCells" },
          //  { "эритроциты измененные","ClinicalParameters.GeneralUrineAnalysis.AlteredRedBloodCells" },
            { "белок","ClinicalParameters.GeneralUrineAnalysis.Protein" }
            // TODO соответствия и затем дебаг.
        };


        public Dictionary<string, string> MatchingPropertiesNames => matchingPropertiesNames.ToDictionary(x => x.Key.ToLower().Trim(), x => x.Value);
    }
}
