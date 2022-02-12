﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public enum ParamsNames
    {
        [Display(Name = "Температура")]
        Temperature,
        [Display(Name = "Нижнее значение нормы температуры")]
        LowNormalTemperature,
        [Display(Name = "Верхнее значение нормы температуры")]
        UpNormalTemperature,
        [Display(Name = "Нижний предел нормы температуры")]
        LowCriticalTemperature,
        [Display(Name = "Верхний предел нормы температуры")]
        UpCriticalTemperature,
        [Display(Name = "Сатурация")]
        Saturation,
        [Display(Name = "Нижнее значение нормы сатурации")]
        LowNormalSaturation,
        [Display(Name = "Нижний предел нормы сатурации")]
        LowCriticalSaturation,
        [Display(Name = "Кашель")]
        Cough,
        [Display(Name = "Частота дыхательных движений")]
        FRM,
        [Display(Name = "Частота сердечных сокращений")]
        HeartRate,
        [Display(Name = "Объем поражения легочной ткани")]
        LungTissueDamage,
        [Display(Name = "Критический объем поражения легочной ткани")]
        UpCriticalLungTissueDamage,
        [Display(Name = "С-реактивный белок")]
        CReactiveProtein,
        [Display(Name = "Верхний предел нормы С-реактивного белка")]
        UpCriticalCReactiveProtein,
        [Display(Name = "Верхнее значение нормы  С-реактивного белка")]
        UpNormCReactiveProtein
    }
}
