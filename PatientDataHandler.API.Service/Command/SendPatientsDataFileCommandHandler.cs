using Interfaces;
using MediatR;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API_Messaging.Send.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Service.Command
{
    public class SendPatientsDataFileCommandHandler :
        IRequestHandler<SendPatientsDataFileCommand, Unit>
    {
        Func<DataParserTypes, IDataProvider> dataParserResolver;
        IPatientsDataSender patientsDataSender;

        public SendPatientsDataFileCommandHandler(Func<DataParserTypes, IDataProvider> dataParserResolver,
            IPatientsDataSender patientsDataSender)
        {
            this.dataParserResolver = dataParserResolver;
            this.patientsDataSender = patientsDataSender;
        }

        public Task<Unit> Handle(SendPatientsDataFileCommand request, CancellationToken cancellationToken)
        {
            FileData fileData = request.Data; //TODO по FileData определение DataParserTypes
            IDataProvider dataProvider = dataParserResolver.Invoke(DataParserTypes.TestVahitova);

            using (Stream stream = GenerateStreamFromString(fileData.RawData))
            {
                IList<IPatientData> patientDatas = dataProvider.ParseData(stream);
                patientsDataSender.SendPatientsData(patientDatas);
                return Unit.Task;
            }
        }


        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
