using Interfaces;

namespace WebMVC.Models
{
    public class FileData : IFileData
    {
        public byte[] RawData { get ; set ; }
    }
}
