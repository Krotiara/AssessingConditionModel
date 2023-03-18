using Interfaces;

namespace WebMVC.Models
{
    public class FileData : IFileData
    {
        public string MedicalOrganization { get; set; }

        public byte[] RawData { get ; set ; }
    }
}
