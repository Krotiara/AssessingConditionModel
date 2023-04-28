using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
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


#warning to remove
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
}
