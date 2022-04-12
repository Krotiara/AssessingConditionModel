using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("Patients")]
    public class Patient
    {
        
        [Column("Id")]
        public long Id { get; set; }

        [DisplayName("ФИО")]
        public string Name { get; set; } = "";


        private int medicalHistoryNumber;

        [Key]
        [Column("MedicalHistoryNumber")]
        [DisplayName("Номер медицинской карты")]
        [Required]
        public int MedicalHistoryNumber
        {
            get => medicalHistoryNumber;
            set
            {
                medicalHistoryNumber = value;
                ClinicalParameters.PatientId = value;
                FunctionalParameters.PatientId = value;
                InstrumentalParameters.PatientId = value;
            }
        }


        public ClinicalParameters ClinicalParameters { get; set; }

        public FunctionalParameters FunctionalParameters { get; set; }

        public InstrumentalParameters InstrumentalParameters { get; set; }

        [NotMapped]
        public ParametersNorms ParametersNorms { get; set; }


        public void InitParameters()
        {
            if (ClinicalParameters != null)
                ClinicalParameters.InitLungsModel();
            InitParametersNorms();
        }


        private void InitParametersNorms()
        {
            if (ParametersNorms == null)
                ParametersNorms = new ParametersNorms();

            //TODO продумать зависимости норма относительно параметров пациентов
            //TODO надежные источники и значения

            //https://ikb1.ru/about/news/1443/
            ParametersNorms.LowNormalSaturation = 95.0;
            ParametersNorms.LowCriticalSaturation = 93.0;

            //https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D0%BC%D0%BF%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0_%D1%82%D0%B5%D0%BB%D0%B0
            ParametersNorms.LowNormalTemperature = 35.0;
            ParametersNorms.LowCriticalTemperature = 32.0;
            ParametersNorms.UpNormalTemperature = 37.0;
            ParametersNorms.UpCriticalTemperature = 39.0;

            // > 75 % соответсвует КТ4 
            ParametersNorms.UpCriticalLungDamage = 75.0;

            //https://helix.ru/kb/item/06-050
            ParametersNorms.UpNormCReactiveProtein = 1.0;
            ParametersNorms.UpCriticalCReactiveProtein = 10.0;
        }
       
        public Patient(string name): this()
        {
            Name = name;
        }

        public Patient()
        {
            ClinicalParameters = new ClinicalParameters();
            FunctionalParameters = new FunctionalParameters();
            InstrumentalParameters = new InstrumentalParameters();
            ParametersNorms = new ParametersNorms();
            //suitable constructor for entity type for awoid EF error No suitable constructor found for entity type
        }


        

    }
}
