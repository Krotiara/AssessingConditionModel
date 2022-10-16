using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
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
}
