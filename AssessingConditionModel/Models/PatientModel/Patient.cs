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
