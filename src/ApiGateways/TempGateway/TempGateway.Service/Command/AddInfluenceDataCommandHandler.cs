using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempGateway.Entities;

namespace TempGateway.Service.Command
{
    public class AddInfluenceDataCommandHandler : IRequestHandler<AddInfluenceDataCommand, Unit>
    {
        private readonly IWebRequester webRequester;

        public AddInfluenceDataCommandHandler(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public async Task<Unit> Handle(AddInfluenceDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
               // FileData fileData = GetFileDataFrom(request.FilePath);
                string url = $"{Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL")}/patientsApi/addInfluenceData/";
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(request.Data);
                _ = await webRequester.GetResponse<bool>(url, "POST", body);
                return await Unit.Task;
            }
            catch(Exception ex)
            {
                throw new AddInfluenceDataException($"", ex);
            }
        }


        //private FileData GetFileDataFrom(string filePath)
        //{
        //    filePath = filePath.Replace("%2F", "/");
        //    using (Stream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        Func<Stream, byte[]> getStreamData = (stream) =>
        //        {
        //            stream.Position = 0;
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                stream.CopyTo(ms);
        //                return ms.ToArray();
        //            }
        //        };
        //        byte[] data = getStreamData(stream);
        //        FileData fileData = new FileData() { RawData = data };
        //        return fileData;
        //    }
        //}
    }
}
