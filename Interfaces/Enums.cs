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

    public enum BioAgeCalculationType
    {
        ByFunctionalParameters = 0
    }  

    public enum InputFileType
    {
        Test
    }
}
