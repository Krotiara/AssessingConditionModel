using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{

    public enum CommonArgs
    {
        ObservedId,
        MedicalOrganization,
        StartDateTime,
        EndDateTime
    }

    public enum AgentType
    {
        Custom,
        AgingPatient,
        DentistPatient
    }

    public enum SystemCommands
    {
        GetLatestPatientParameters,
        GetAge,
        GetBioageByFunctionalParameters,
        GetAgeRangBy,
        GetInfluences,
        GetAgingDynamics,
        GetAgingState,
        GetInfluencesWithoutParameters,
        GetDentistSum
    }


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
        [ParamDescription(Descriptions = new string[] { "id", "номер", "номер истории болезни", "№"})]
        [NoAllowToSelect]
        Id,
        [Display(Name = "Возраст")]
        [ParamDescription(Descriptions = new string[] { "возраст", "age" })]
        [ParamValueType(ValueType = typeof(int))]
        Age,
        [Display(Name = "Пол")]
        [ParamDescription(Descriptions = new string[] { "пол" })]
        [NoAllowToSelect]
        Gender,
        [Display(Name = "Дата вступления")]
        [ParamDescription(Descriptions = new string[] { "дата вступления" })]
        [ParamValueType(ValueType = typeof(DateTime))]
        HospitalizationDate,
        [Display(Name = "Дата внесения")]
        [ParamDescription(Descriptions = new string[] { "дата внесения", "дата последнего посещения" })]
        [ParamValueType(ValueType = typeof(DateTime))]
        ParameterTimestamp,
        [Display(Name = "Анамнез")]
        [ParamDescription(Descriptions = new string[] { "анамнез" })]
        [ParamValueType(ValueType = typeof(string))]
        Anamnesis,
        [Display(Name = "Артериальная гипертензия")]
        [ParamDescription(Descriptions = new string[] { "артериальная гипертензия", "аг" })]
        [ParamValueType(ValueType = typeof(double))]
        ArterialHypertension,
        [Display(Name = "Фп пароксизмальная")]
        [ParamDescription(Descriptions = new string[] { "фп пароксизмальная" })]
        [ParamValueType(ValueType = typeof(double))]
        ParoxysmalAtrialFibrillation,
        [Display(Name = "Желудочковые нр")]
        [ParamDescription(Descriptions = new string[] { "желудочковые нр" })]
        [ParamValueType(ValueType = typeof(double))]
        VentricularArrhythmias,
        [Display(Name = "Жэс")]
        [ParamDescription(Descriptions = new string[] { "жэс" })]
        [ParamValueType(ValueType = typeof(double))]
        VentricularExtrasystole,
        [Display(Name = "Пробежки/пароксизмы жт")]
        [ParamDescription(Descriptions = new string[] { "пробежки/пароксизмы жт", "жт" })]
        [ParamValueType(ValueType = typeof(double))]
        VentricularTachycardia,
        [Display(Name = "Инфаркт миокарда")]
        [ParamDescription(Descriptions = new string[] { "инфаркт миокарда" })]
        [ParamValueType(ValueType = typeof(bool))]
        MyocardialInfarction,
        [Display(Name = "Стенокардия")]
        [ParamDescription(Descriptions = new string[] { "стенокардия" })]
        [ParamValueType(ValueType = typeof(bool))]
        Stenocardia,
        [Display(Name = "Фк стенокардии")]
        [ParamDescription(Descriptions = new string[] { "фк стенокардии" })]
        [ParamValueType(ValueType = typeof(bool))]
        AnginaPectorisFunctionalClass,
        [Display(Name = "Сахарный диабет")]
        [ParamDescription(Descriptions = new string[] { "cахарный диабет 2", "cахарный диабет" })]
        [ParamValueType(ValueType = typeof(bool))]
        DiabetesMellitus,
        [Display(Name = "Курение")]
        [ParamDescription(Descriptions = new string[] { "курение" })]
        [ParamValueType(ValueType = typeof(bool))]
        Smoking,
        [Display(Name = "Привержен к лечению")]
        [ParamDescription(Descriptions = new string[] { "привержен к лечению" })]
        [ParamValueType(ValueType = typeof(bool))]
        TreatmentAdherence,
        [Display(Name = "Баллы по мориски")]
        [ParamDescription(Descriptions = new string[] { "баллы по мориски" })]
        [ParamValueType(ValueType = typeof(double))]
        Moriski,
        [Display(Name = "Баллы по шокс")]
        [ParamDescription(Descriptions = new string[] { "баллы по шокс" })]
        [ParamValueType(ValueType = typeof(double))]
        Shock,
        [Display(Name = "Сад")]
        [ParamDescription(Descriptions = new string[] { "сад","адс" })]
        [ParamValueType(ValueType = typeof(double))]
        SystolicPressure,
        [Display(Name = "Дад")]
        [ParamDescription(Descriptions = new string[] { "дад","адд" })]
        [ParamValueType(ValueType = typeof(double))]
        DiastolicPressure,
        [Display(Name = "Чсс")]
        [ParamDescription(Descriptions = new string[] { "чсс" })]
        [ParamValueType(ValueType = typeof(double))]
        HeartRate,
        [Display(Name = "Гемоглобин")]
        [ParamDescription(Descriptions = new string[] { "гемоглобин", "hb", "hgb" })]
        [ParamValueType(ValueType = typeof(double))]
        Hemoglobin,
        [Display(Name = "Мочевина")]
        [ParamDescription(Descriptions = new string[] { "мочевина" })]
        [ParamValueType(ValueType = typeof(double))]
        Urea,
        [Display(Name = "Креатинин")]
        [ParamDescription(Descriptions = new string[] { "креатинин" })]
        [ParamValueType(ValueType = typeof(double))]
        Creatinine,
        [Display(Name = "Калий")]
        [ParamDescription(Descriptions = new string[] { "калий" })]
        [ParamValueType(ValueType = typeof(double))]
        Potassium,
        [Display(Name = "Глюкоза")]
        [ParamDescription(Descriptions = new string[] { "глюкоза" })]
        [ParamValueType(ValueType = typeof(double))]
        Glucose,
        [Display(Name = "Общий билирубин")]
        [ParamDescription(Descriptions = new string[] { "общий билирубин" })]
        [ParamValueType(ValueType = typeof(double))]
        TotalBilirubin,
        [Display(Name = "Охс")]
        [ParamDescription(Descriptions = new string[] { "охс" })]
        [ParamValueType(ValueType = typeof(double))]
        TotalCholesterol,
        [Display(Name = "Хбп")]
        [ParamDescription(Descriptions = new string[] { "хбп" })]
        [ParamValueType(ValueType = typeof(double))]
        StageOfChronicKidneyDisease,
        [Display(Name = "Вес")]
        [ParamDescription(Descriptions = new string[] { "вес", "масса тела" })]
        [ParamValueType(ValueType = typeof(double))]
        Weight,
        [Display(Name = "Рост")]
        [ParamDescription(Descriptions = new string[] { "рост" })]
        [ParamValueType(ValueType = typeof(double))]
        Height,
        [Display(Name = "Имт")]
        [ParamDescription(Descriptions = new string[] { "имт" })]
        [ParamValueType(ValueType = typeof(double))]
        BodyMassIndex,
        [Display(Name = "Bnp")]
        [ParamDescription(Descriptions = new string[] { "bnp" })]
        [ParamValueType(ValueType = typeof(double))]
        BNP,
        [Display(Name = "Качество жизни")]
        [ParamDescription(Descriptions = new string[] { "качество жизни" })]
        [ParamValueType(ValueType = typeof(double))]
        LifeQuality,
        [Display(Name = "Кср")]
        [ParamDescription(Descriptions = new string[] { "кср" })]
        [ParamValueType(ValueType = typeof(double))]
        CSR,
        [Display(Name = "Кдр")]
        [ParamDescription(Descriptions = new string[] { "кдр" })]
        [ParamValueType(ValueType = typeof(double))]
        EchocardiographicAssessment,
        [Display(Name = "Ксо")]
        [ParamDescription(Descriptions = new string[] { "ксо" })]
        [ParamValueType(ValueType = typeof(double))]
        FinalSystolicVolume,
        [Display(Name = "Кдо")]
        [ParamDescription(Descriptions = new string[] { "кдо" })]
        [ParamValueType(ValueType = typeof(double))]
        FinalDiastolicVolume,
        [Display(Name = "Фв")]
        [ParamDescription(Descriptions = new string[] { "фв" })]
        [ParamValueType(ValueType = typeof(double))]
        EjectionFraction,
        [Display(Name = "Здвдох")]
        [ParamDescription(Descriptions = new string[] { "здвдох" })]
        [ParamValueType(ValueType = typeof(double))]
        InhaleBreathHolding,
        [Display(Name = "Здвыдох")]
        [ParamDescription(Descriptions = new string[] { "здвыдох" })]
        [ParamValueType(ValueType = typeof(double))]
        OuthaleBreathHolding,
        [Display(Name = "Жел")]
        [ParamDescription(Descriptions = new string[] { "жел" })]
        [ParamValueType(ValueType = typeof(double))]
        LungCapacity,
        [Display(Name = "Аккомодация")]
        [ParamDescription(Descriptions = new string[] { "аккомодация" })]
        [ParamValueType(ValueType = typeof(double))]
        Accommodation,
        [Display(Name = "Острота слуха")]
        [ParamDescription(Descriptions = new string[] { "острота слуха" })]
        [ParamValueType(ValueType = typeof(double))]
        HearingAcuity,
        [Display(Name = "Стат.балансировка")]
        [ParamDescription(Descriptions = new string[] { "стат.балансировка" })]
        [ParamValueType(ValueType = typeof(double))]
        StaticBalancing,
        [Display(Name = "Обратная сагиттальная щель")]
        [ParamDescription(Descriptions = new string[] { "обратная сагиттальная щель" })]
        [ParamValueType(ValueType = typeof(double))]
        ReverseSagittalGap,
        [Display(Name = "Сужение верхнего зубного ряда в области первых моляров")]
        [ParamDescription(Descriptions = 
            new string[] { "сужение верхнего зубного ряда в области первых моляров" })]
        [ParamValueType(ValueType = typeof(double))]
        FirstMolarsNarrowing,
        [Display(Name = "Сумма баллов")]
        [ParamDescription(Descriptions = new string[] { "сумма баллов" })]
        [ParamValueType(ValueType = typeof(double))]
        DentistPointsSum,
        [Display(Name = "Сагиттальная щель")]
        [ParamDescription(Descriptions = new string[] { "сагиттальная щель" })]
        [ParamValueType(ValueType = typeof(double))]
        SagittalSlit,
        [Display(Name = "вертикальная дизокклюзия во фронтальном/боковом участке")]
        [ParamDescription(Descriptions = 
            new string[] { "вертикальная дизокклюзия во фронтальном/боковом участке" })]
        [ParamValueType(ValueType = typeof(double))]
        VerticalDysocclusion,
        [Display(Name = "Резцовое перекрытие менее 3,5 мм")]
        [ParamDescription(Descriptions = new string[] { "резцовое перекрытие менее 3,5 мм" })]
        [ParamValueType(ValueType = typeof(double))]
        LessIncisorOverlap,
        [Display(Name = "Резцовое перекрытие до контакта с десной/нёбом, без травмы")]
        [ParamDescription(Descriptions = 
            new string[] { "резцовое перекрытие до контакта с десной/нёбом, без травмы" })]
        [ParamValueType(ValueType = typeof(double))]
        ContactIncisorOverlapWithoutInjury,
        [Display(Name = "Резцовое перекрытие с травмой десны или неба")]
        [ParamDescription(Descriptions = new string[] { "резцовое перекрытие с травмой десны или неба" })]
        [ParamValueType(ValueType = typeof(double))]
        ContactIncisorOverlapWithInjury,
        [Display(Name = "Смещение нижней челюсти назад, мм")]
        [ParamDescription(Descriptions = new string[] { "смещение нижней челюсти назад, мм" })]
        [ParamValueType(ValueType = typeof(double))]
        LowerJawBackwardDisplacement,
        [Display(Name = "Смещение нижней челюсти в сторону, мм")]
        [ParamDescription(Descriptions = new string[] { "смещение нижней челюсти в сторону, мм" })]
        [ParamValueType(ValueType = typeof(double))]
        LowerJawSideDisplacement,
        [Display(Name = "Смещение нижней челюсти вперед, мм")]
        [ParamDescription(Descriptions = new string[] { "смещение нижней челюсти вперед, мм" })]
        [ParamValueType(ValueType = typeof(double))]
        LowerJawForwardDisplacement,
        [Display(Name = "Уменьшение общей длины зубного ряда (ретенция) на 1 зуб")]
        [ParamDescription(Descriptions = 
            new string[] { "уменьшение общей длины зубного ряда (ретенция) на 1 зуб" })]
        [ParamValueType(ValueType = typeof(double))]
        DentitionLengthReductionByTooth,
        [Display(Name = "Уменьшение общей длины зубного ряда (ретенция) на 2 зуба и более")]
        [ParamDescription(Descriptions = 
            new string[] { "уменьшение общей длины зубного ряда (ретенция) на 2 зуба и более" })]
        [ParamValueType(ValueType = typeof(double))]
        DentitionLengthReductionByTooths,
        [Display(Name = "Дата рождения")]
        [ParamDescription(Descriptions = new string[] { "дата рождения" })]
        [ParamValueType(ValueType = typeof(DateTime))]
        [NoAllowToSelect]
        Birthdate,
        [Display(Name = "фио")]
        [ParamDescription(Descriptions = new string[] { "фио" })]
        [ParamValueType(ValueType = typeof(string))]
        [NoAllowToSelect]
        FullName,
        [Display(Name = "Баллы после лечения")]
        [ParamDescription(Descriptions = new string[] { "balls_after_treatment" })]
        [ParamValueType(ValueType = typeof(double))]
        [NoAllowToSelect]
        ScoreAfterTreatment,
        [Display(Name = "Продолжительность лечения (в месяцах)")]
        [ParamDescription(Descriptions = new string[] { "duration_treatment" })]
        [ParamValueType(ValueType = typeof(double))]
        [NoAllowToSelect]
        TreatmentDuration,
        [Display(Name = "Количество шагов лечения")]
        [ParamDescription(Descriptions = new string[] { "steps" })]
        [ParamValueType(ValueType = typeof(double))]
        [NoAllowToSelect]
        TreatmentSteps,
        [Display(Name = "Apparatues")]
        [ParamDescription(Descriptions = new string[] { "apparatues" })]
        [ParamValueType(ValueType = typeof(double))]
        [NoAllowToSelect]
        TreatmentApparatuesCount
    }
}
