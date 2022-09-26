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
}
