using Models.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Entities
{
    [Table("Models")]
    public class ModelMeta : IModelMeta
    {

        public ModelMeta() { }

        [Key]
        [Column("StorageId")]
        public string StorageId { get; set; }

        [Column("FileName")]
        public string Name { get; set; }

        [Column("Accuracy")]
        public double Accuracy { get; set; }

        [Column("Version")]
        public double Version { get; set; }

        [Column("InputParamsCount")]
        public int InputParamsCount { get; set; }

        [Column("OutputParamsCount")]
        public int OutputParamsCount { get; set; }

        [Column("ParamsNames")]
        public string[] ParamsNames { get; set; } //Гарантирует ли бд порядок?
    }
}
