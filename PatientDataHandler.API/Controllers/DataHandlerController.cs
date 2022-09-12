using Microsoft.AspNetCore.Mvc;
using PatientDataHandler.API.Models;

namespace PatientDataHandler.API.Controllers
{
    public class DataHandlerController: Controller
    {
        Func<DataParserTypes, IDataProvider> dataParserResolver;
        PatientsDataDbContext patientsDataDbContext;


        public DataHandlerController(Func<DataParserTypes, IDataProvider> dataParserResolver, PatientsDataDbContext patientsDataDbContext)
        {
            this.dataParserResolver = dataParserResolver;
            this.patientsDataDbContext = patientsDataDbContext;
        }
    }
}
