﻿using Interfaces;
using MediatR;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Service.Services;
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
            IList<IPatientData> patientDatas = dataProvider.ParseData(fileData.RawData);
            patientsDataSender.SendPatientsData(patientDatas);
            return Unit.Task;

        }
    }
}
