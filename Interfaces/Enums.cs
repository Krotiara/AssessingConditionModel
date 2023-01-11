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
        [Display(Name = "-")]
        None = 0,
        [Display(Name = "Антигипоксическое")]
        Antihypoxic = 1,
        [Display(Name = "Антиоксидантное")]
        Antioxidant = 2,
        [Display(Name = "Противовоспалительное")]
        AntiInflammatory = 3,
        [Display(Name = "Биологически активная добавка")]
        BiologicallyActiveAdditive = 4
    }

    public enum GenderEnum
    {
        [Display(Name="-")]
        None,
        [Display(Name = "Мужской")]
        Male,
        [Display(Name = "Женский")]
        Female
    }


    public enum TreatmentType
    {
        [Display(Name = "-")]
        None,
        [Display(Name = "Амбулаторно")]
        Outpatient,
        [Display(Name = "Стационар")]
        Inpatient
    }

    public enum AgentBioAgeStates
    {
        [Display(Name = "Резко замедленный темп старения")]
        RangI,
        [Display(Name = "Замедленный темп старения")]
        RangII,
        [Display(Name = "Примерное соответствие БВ и КВ")]
        RangIII,
        [Display(Name = "Ускоренный темп старения")]
        RangIV,
        [Display(Name = "Резко ускоренный темп старения.")]
        RangV,
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
        [NoAllowToSelect]
        Id,
        [Display(Name = "Возраст")]
        [ParamDescription(Descriptions = new string[] { "возраст" })]
        Age,
        [Display(Name = "Пол")]
        [ParamDescription(Descriptions = new string[] { "пол" })]
        [NoAllowToSelect]
        Gender,
        [Display(Name = "Дата вступления")]
        [ParamDescription(Descriptions = new string[] { "дата вступления" })]
        HospitalizationDate,
        [Display(Name = "Дата внесения")]
        [ParamDescription(Descriptions = new string[] { "дата внесения" })]
        ParameterTimestamp,
        [Display(Name = "Анамнез")]
        [ParamDescription(Descriptions = new string[] { "анамнез" })]
        Anamnesis,
        [Display(Name = "Артериальная гипертензия")]
        [ParamDescription(Descriptions = new string[] { "артериальная гипертензия", "аг" })]
        ArterialHypertension,
        [Display(Name = "Фп пароксизмальная")]
        [ParamDescription(Descriptions = new string[] { "фп пароксизмальная" })]
        ParoxysmalAtrialFibrillation,
        [Display(Name = "Желудочковые нр")]
        [ParamDescription(Descriptions = new string[] { "желудочковые нр" })]
        VentricularArrhythmias,
        [Display(Name = "Жэс")]
        [ParamDescription(Descriptions = new string[] { "жэс" })]
        VentricularExtrasystole,
        [Display(Name = "Пробежки/пароксизмы жт")]
        [ParamDescription(Descriptions = new string[] { "пробежки/пароксизмы жт", "жт" })]
        VentricularTachycardia,
        [Display(Name = "Инфаркт миокарда")]
        [ParamDescription(Descriptions = new string[] { "инфаркт миокарда" })]
        MyocardialInfarction,
        [Display(Name = "Стенокардия")]
        [ParamDescription(Descriptions = new string[] { "стенокардия" })]
        Stenocardia,
        [Display(Name = "Фк стенокардии")]
        [ParamDescription(Descriptions = new string[] { "фк стенокардии" })]
        AnginaPectorisFunctionalClass,
        [Display(Name = "Сахарный диабет")]
        [ParamDescription(Descriptions = new string[] { "cахарный диабет 2", "cахарный диабет" })]
        DiabetesMellitus,
        [Display(Name = "Курение")]
        [ParamDescription(Descriptions = new string[] { "курение" })]
        Smoking,
        [Display(Name = "Привержен к лечению")]
        [ParamDescription(Descriptions = new string[] { "привержен к лечению" })]
        TreatmentAdherence,
        [Display(Name = "Баллы по мориски")]
        [ParamDescription(Descriptions = new string[] { "баллы по мориски" })]
        Moriski,
        [Display(Name = "Баллы по шокс")]
        [ParamDescription(Descriptions = new string[] { "баллы по шокс" })]
        Shock,
        [Display(Name = "Сад")]
        [ParamDescription(Descriptions = new string[] { "сад","адс" })]
        SystolicPressure,
        [Display(Name = "Дад")]
        [ParamDescription(Descriptions = new string[] { "дад","адд" })]
        DiastolicPressure,
        [Display(Name = "Чсс")]
        [ParamDescription(Descriptions = new string[] { "чсс" })]
        HeartRate,
        [Display(Name = "Гемоглобин")]
        [ParamDescription(Descriptions = new string[] { "гемоглобин", "hb", "hgb" })]
        Hemoglobin,
        [Display(Name = "Мочевина")]
        [ParamDescription(Descriptions = new string[] { "мочевина" })]
        Urea,
        [Display(Name = "Креатинин")]
        [ParamDescription(Descriptions = new string[] { "креатинин" })]
        Creatinine,
        [Display(Name = "Калий")]
        [ParamDescription(Descriptions = new string[] { "калий" })]
        Potassium,
        [Display(Name = "Глюкоза")]
        [ParamDescription(Descriptions = new string[] { "глюкоза" })]
        Glucose,
        [Display(Name = "Общий билирубин")]
        [ParamDescription(Descriptions = new string[] { "общий билирубин" })]
        TotalBilirubin,
        [Display(Name = "Охс")]
        [ParamDescription(Descriptions = new string[] { "охс" })]
        TotalCholesterol,
        [Display(Name = "Хбп")]
        [ParamDescription(Descriptions = new string[] { "хбп" })]
        StageOfChronicKidneyDisease,
        [Display(Name = "Вес")]
        [ParamDescription(Descriptions = new string[] { "вес", "масса тела" })]
        Weight,
        [Display(Name = "Рост")]
        [ParamDescription(Descriptions = new string[] { "рост" })]
        Height,
        [Display(Name = "Имт")]
        [ParamDescription(Descriptions = new string[] { "имт" })]
        BodyMassIndex,
        [Display(Name = "Bnp")]
        [ParamDescription(Descriptions = new string[] { "bnp" })]
        BNP,
        [Display(Name = "Качество жизни")]
        [ParamDescription(Descriptions = new string[] { "качество жизни" })]
        LifeQuality,
        [Display(Name = "Кср")]
        [ParamDescription(Descriptions = new string[] { "кср" })]
        CSR,
        [Display(Name = "Кдр")]
        [ParamDescription(Descriptions = new string[] { "кдр" })]
        EchocardiographicAssessment,
        [Display(Name = "Ксо")]
        [ParamDescription(Descriptions = new string[] { "ксо" })]
        FinalSystolicVolume,
        [Display(Name = "Кдо")]
        [ParamDescription(Descriptions = new string[] { "кдо" })]
        FinalDiastolicVolume,
        [Display(Name = "Фв")]
        [ParamDescription(Descriptions = new string[] { "фв" })]
        EjectionFraction,
        [Display(Name = "Здвдох")]
        [ParamDescription(Descriptions = new string[] { "здвдох" })]
        InhaleBreathHolding,
        [Display(Name = "Здвыдох")]
        [ParamDescription(Descriptions = new string[] { "здвыдох" })]
        OuthaleBreathHolding,
        [Display(Name = "Жел")]
        [ParamDescription(Descriptions = new string[] { "жел" })]
        LungCapacity,
        [Display(Name = "Аккомодация")]
        [ParamDescription(Descriptions = new string[] { "аккомодация" })]
        Accommodation,
        [Display(Name = "Острота слуха")]
        [ParamDescription(Descriptions = new string[] { "острота слуха" })]
        HearingAcuity,
        [Display(Name = "Стат.балансировка")]
        [ParamDescription(Descriptions = new string[] { "стат.балансировка" })]
        StaticBalancing
    }
}
