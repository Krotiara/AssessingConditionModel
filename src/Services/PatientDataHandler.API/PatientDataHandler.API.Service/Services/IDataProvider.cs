using Interfaces;
using PatientDataHandler.API.Entities;

namespace PatientDataHandler.API.Service.Services
{
    public interface IDataProvider
    {
        public IList<Influence> ParseData(byte[] bytesData);
    }
}
