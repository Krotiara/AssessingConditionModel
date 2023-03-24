using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.API.Entities
{
    public class ModelMetaRequest : IModelMetaRequest
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
