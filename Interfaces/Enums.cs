using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public enum InfluenceTypes
    {
        [Display(Name = "Антигипоксическое")]
        Antihypoxic = 0,
        [Display(Name = "Антиоксидантное")]
        Antioxidant = 1,
        [Display(Name = "Противовоспалительное")]
        AntiInflammatory = 2,
        [Display(Name = "Биологически активная добавка")]
        BiologicallyActiveAdditive = 3
    }

    public enum BioAgeCalculationType
    {
        ByFunctionalParameters = 0
    }

    public enum ParameterNames
    {
        None = 0,
        [Display(Name = "Id")]
        [ParamDescription(Descriptions = new string[] { "id", "номер", "номер истории болезни" })]
        Id,
        [Display(Name = "возраст")]
        [ParamDescription(Descriptions = new string[] { "возраст" })]
        Age,
        [Display(Name = "пол")]
        [ParamDescription(Descriptions = new string[] { "пол" })]
        Gender,
        [Display(Name = "дата вступления")]
        [ParamDescription(Descriptions = new string[] { "дата вступления" })]
        HospitalizationDate,
        [Display(Name = "анамнез")]
        [ParamDescription(Descriptions = new string[] { "анамнез" })]
        Anamnesis,
        [Display(Name = "артериальная гипертензия")]
        [ParamDescription(Descriptions = new string[] { "артериальная гипертензия", "аг" })]
        ArterialHypertension,
        [Display(Name = "фп пароксизмальная")]
        [ParamDescription(Descriptions = new string[] { "фп пароксизмальная" })]
        ParoxysmalAtrialFibrillation,
        [Display(Name = "желудочковые нр")]
        [ParamDescription(Descriptions = new string[] { "желудочковые нр" })]
        VentricularArrhythmias,
        [Display(Name = "жэс")]
        [ParamDescription(Descriptions = new string[] { "жэс" })]
        VentricularExtrasystole,
        [Display(Name = "пробежки/пароксизмы жт")]
        [ParamDescription(Descriptions = new string[] { "пробежки/пароксизмы жт", "жт" })]
        VentricularTachycardia,
        [Display(Name = "инфаркт миокарда")]
        [ParamDescription(Descriptions = new string[] { "инфаркт миокарда" })]
        MyocardialInfarction,
        [Display(Name = "стенокардия")]
        [ParamDescription(Descriptions = new string[] { "стенокардия" })]
        Stenocardia,
        [Display(Name = "фк стенокардии")]
        [ParamDescription(Descriptions = new string[] { "фк стенокардии" })]
        AnginaPectorisFunctionalClass,
        [Display(Name = "cахарный диабет")]
        [ParamDescription(Descriptions = new string[] { "cахарный диабет 2", "cахарный диабет" })]
        DiabetesMellitus,
        [Display(Name = "курение")]
        [ParamDescription(Descriptions = new string[] { "курение" })]
        Smoking,
        [Display(Name = "привержен к лечению")]
        [ParamDescription(Descriptions = new string[] { "привержен к лечению" })]
        TreatmentAdherence,
        [Display(Name = "баллы по мориски")]
        [ParamDescription(Descriptions = new string[] { "баллы по мориски" })]
        Moriski,
        [Display(Name = "баллы по шокс")]
        [ParamDescription(Descriptions = new string[] { "баллы по шокс" })]
        Shock,
        [Display(Name = "сад")]
        [ParamDescription(Descriptions = new string[] { "сад" })]
        SystolicPressure,
        [Display(Name = "дад")]
        [ParamDescription(Descriptions = new string[] { "дад" })]
        DiastolicPressure,
        [Display(Name = "чсс")]
        [ParamDescription(Descriptions = new string[] { "чсс" })]
        HeartRate,
        [Display(Name = "гемоглобин")]
        [ParamDescription(Descriptions = new string[] { "гемоглобин", "hb", "hgb" })]
        Hemoglobin,
        [Display(Name = "мочевина")]
        [ParamDescription(Descriptions = new string[] { "мочевина" })]
        Urea,
        [Display(Name = "креатинин")]
        [ParamDescription(Descriptions = new string[] { "креатинин" })]
        Creatinine,
        [Display(Name = "калий")]
        [ParamDescription(Descriptions = new string[] { "калий" })]
        Potassium,
        [Display(Name = "глюкоза")]
        [ParamDescription(Descriptions = new string[] { "глюкоза" })]
        Glucose,
        [Display(Name = "общий билирубин")]
        [ParamDescription(Descriptions = new string[] { "общий билирубин" })]
        TotalBilirubin,
        [Display(Name = "охс")]
        [ParamDescription(Descriptions = new string[] { "охс" })]
        TotalCholesterol,
        [Display(Name = "хбп")]
        [ParamDescription(Descriptions = new string[] { "хбп" })]
        StageOfChronicKidneyDisease,
        [Display(Name = "вес")]
        [ParamDescription(Descriptions = new string[] { "вес" })]
        Weight,
        [Display(Name = "рост")]
        [ParamDescription(Descriptions = new string[] { "рост" })]
        Height,
        [Display(Name = "имт")]
        [ParamDescription(Descriptions = new string[] { "имт" })]
        BodyMassIndex,
        [Display(Name = "bnp")]
        [ParamDescription(Descriptions = new string[] { "bnp" })]
        BNP,
        [Display(Name = "качество жизни")]
        [ParamDescription(Descriptions = new string[] { "качество жизни" })]
        LifeQuality,
        [Display(Name = "кср")]
        [ParamDescription(Descriptions = new string[] { "кср" })]
        CSR,
        [Display(Name = "кдр")]
        [ParamDescription(Descriptions = new string[] { "кдр" })]
        EchocardiographicAssessment,
        [Display(Name = "ксо")]
        [ParamDescription(Descriptions = new string[] { "ксо" })]
        FinalSystolicVolume,
        [Display(Name = "кдо")]
        [ParamDescription(Descriptions = new string[] { "кдо" })]
        FinalDiastolicVolume,
        [Display(Name = "фв")]
        [ParamDescription(Descriptions = new string[] { "фв" })]
        EjectionFraction,
        [Display(Name = "здвдох")]
        [ParamDescription(Descriptions = new string[] { "здвдох" })]
        InhaleBreathHolding,
        [Display(Name = "здвыдох")]
        [ParamDescription(Descriptions = new string[] { "здвыдох" })]
        OuthaleBreathHolding,
        [Display(Name = "жел")]
        [ParamDescription(Descriptions = new string[] { "жел" })]
        LungCapacity,
        [Display(Name = "аккомодация")]
        [ParamDescription(Descriptions = new string[] { "аккомодация" })]
        Accommodation,
        [Display(Name = "острота слуха")]
        [ParamDescription(Descriptions = new string[] { "острота слуха" })]
        HearingAcuity,
        [Display(Name = "стат.балансировка")]
        [ParamDescription(Descriptions = new string[] { "cтат.балансировка" })]
        StaticBalancing
    }
}
