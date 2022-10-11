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

    public enum Parameters
    {
        [Display(Name = "Id")]
        Id,
        [Display(Name = "возраст")]
        Age,
        [Display(Name = "пол")]
        Gender,
        [Display(Name = "дата вступления")]
        HospitalizationDate,
        [Display(Name = "анамнез")]
        Anamnesis,
        [Display(Name = "артериальная гипертензия")]
        ArterialHypertension,
        [Display(Name = "фп пароксизмальная")]
        ParoxysmalAtrialFibrillation,
        [Display(Name = "желудочковые нр")]
        VentricularArrhythmias,
        [Display(Name = "жэс")]
        VentricularExtrasystole,
        [Display(Name = "пробежки/пароксизмы жт")]
        VentricularTachycardia,
        [Display(Name = "инфаркт миокарда")]
        MyocardialInfarction,
        [Display(Name = "стенокардия")]
        Stenocardia,
        [Display(Name = "фк стенокардии")]
        AnginaPectorisFunctionalClass,
        [Display(Name = "cахарный диабет")]
        DiabetesMellitus,
        [Display(Name = "курение")]
        Smoking,
        [Display(Name = "привержен к лечению")]
        TreatmentAdherence,
        [Display(Name = "баллы по мориски")]
        Moriski,
        [Display(Name = "баллы по шокс")]
        Shock,
        [Display(Name = "сад")]
        SystolicPressure,
        [Display(Name = "дад")]
        DiastolicPressure,
        [Display(Name = "чсс")]
        HeartRate,
        [Display(Name = "гемоглобин")]
        Hemoglobin,
        [Display(Name = "мочевина")]
        Urea,
        [Display(Name = "креатинин")]
        Creatinine,
        [Display(Name = "калий")]
        Potassium,
        [Display(Name = "глюкоза")]
        Glucose,
        [Display(Name = "общий билирубин")]
        TotalBilirubin,
        [Display(Name = "охс")]
        TotalCholesterol,
        [Display(Name = "хбп")]
        StageOfChronicKidneyDisease,
        [Display(Name = "вес")]
        Weight,
        [Display(Name = "рост")]
        Height,
        [Display(Name = "имт")]
        BodyMassIndex,
        [Display(Name = "bnp")]
        BNP,
        [Display(Name = "качество жизни")]
        LifeQuality,
        [Display(Name = "кср")]
        CSR,
        [Display(Name = "кдр")]
        EchocardiographicAssessment,
        [Display(Name = "ксо")]
        FinalSystolicVolume,
        [Display(Name = "кдо")]
        FinalDiastolicVolume,
        [Display(Name = "фв")]
        EjectionFraction
    }
}
