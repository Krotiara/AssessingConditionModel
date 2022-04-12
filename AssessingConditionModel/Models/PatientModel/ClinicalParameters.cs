using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("ClinicalParameters")]
    public class ClinicalParameters
    {

        public ClinicalParameters()
        {
            LungTissueDamage = new LungTissueDamage();
            GeneralBloodTest = new GeneralBloodTest();
            GeneralUrineAnalysis = new GeneralUrineAnalysis();
            //suitable constructor for entity type for awoid EF error No suitable constructor found for entity type
        }


        private int patientId;
        [Key]
        [Required]
        public int PatientId
        {
            get => patientId;
            set
            {
                patientId = value;
                LungTissueDamage.Id = value;
                GeneralBloodTest.Id = value;
                GeneralUrineAnalysis.Id = value;
            }
        }

        [Column("Date")]
        [DisplayName("Дата снятия показателей")]
        public DateTime Date { get; set; }

        [Column("Temperature")]
        [DisplayName("Температура")]
        public double Temperature { get; set; }

        [Column("Saturation")]
        [DisplayName("Сатурация")]
        public double Saturation { get; set; }

        [Column("IsCough")]
        [DisplayName("Кашель")]
        public bool IsCough { get; set; }

        //[Column("LungTissueDamage")]
        //[DisplayName("Объем поражения легочной ткани")]
        //public double LungTissueDamage { get; set; }

        [Column("FRM")]
        [DisplayName("Частота дыхательных движений")]
        public double FRM { get; set; }

        [Column("HeartRate")]
        [DisplayName("Частота сердечных сокращений")]
        public double HeartRate { get; set; }

        [Column("CReactiveProtein")]
        [DisplayName("С-реактивный белок")]
        public double CReactiveProtein { get; set; }


        public LungTissueDamage LungTissueDamage { get; set; } //TODO add and init LungDamageModel

        public GeneralBloodTest GeneralBloodTest { get; set; }


        public GeneralUrineAnalysis GeneralUrineAnalysis { get; set; }


        public LungsModel.LungsModel LungsModel { get; set; }


        public void InitLungsModel()
        {
            try
            {
                if (LungTissueDamage != null)
                {
                    LungsModel = new LungsModel.LungsModel();
                    LungsModel.SetData(LungTissueDamage.IsRightHandDamage,
                        LungTissueDamage.IsLeftHandDamage,
                        LungTissueDamage.RightLungDamageDescription,
                        LungTissueDamage.LeftLungDamageDescription,
                        LungTissueDamage.DamageVolumeDescription);
                }
                else
                {
                    //TODO log
                }
            }
            catch(Exception ex)
            {
                //TODO log
            }
        }

    }
}
