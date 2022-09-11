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
        Antihypoxic,
        [Display(Name = "Антиоксидантное")]
        Antioxidant,
        [Display(Name = "Противовоспалительное")]
        AntiInflammatory,
        [Display(Name = "Биологически активная добавка")]
        BiologicallyActiveAdditive
    }
}
