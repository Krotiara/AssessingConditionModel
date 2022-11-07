using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IAgingDynamicsSaveService
    {
        public string SaveToExcelFile(CommonAgingDynamics dynamics);
    }
}
