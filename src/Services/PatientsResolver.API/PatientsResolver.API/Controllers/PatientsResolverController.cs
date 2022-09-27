using AutoMapper;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Exceptions;
using PatientsResolver.API.Service.Query;
using System.Text;

namespace PatientsResolver.API.Controllers
{
    public class PatientsResolverController: Controller
    {
        private readonly IMediator mediator;
        //private readonly IMapper mapper;

        public PatientsResolverController(IMediator mediator)
        {
            //this.mapper = mapper;
            this.mediator = mediator;
        }


        [HttpGet("patientsData/{patientId}")]
        public async Task<ActionResult<List<PatientData>>> GetPatientsDataAsync(int patientId)
        {
            try
            {
                return Ok(await mediator.Send(new GetPatientDataQuery() { PatientId = patientId}));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("patients/{patientId}")]
        public async Task<ActionResult<Patient>> GetPatient(int patientId)
        {
            try
            {
                return Ok(await mediator.Send(new GetPatientQuery() { PatientId = patientId }));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("addData/{pathToDataFile}")]
        public async Task<ActionResult> AddData(string pathToDataFile)
        {
            try
            {
                pathToDataFile = pathToDataFile.Replace("%2F", "/"); //TODO вынести в отдельный метод
                using (Stream stream = System.IO.File.Open(pathToDataFile, FileMode.Open, FileAccess.Read))
                {
                    Func<Stream, byte[]> getStreamData = (stream) =>
                    {
                        stream.Position = 0;
                        //StreamReader streamReader = new StreamReader(stream, encoding: Encoding.UTF8);
                        //return streamReader.ReadToEnd();
                        using(MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            return ms.ToArray();
                        }
                    };
                    byte[] data = getStreamData(stream);
                    FileData fileData = new FileData() { RawData = data };
                    await mediator.Send(new SendPatientDataFileSourceCommand() { Data = fileData });
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("addPatient")]
        public async Task<ActionResult<bool>> AddPatient(Patient patient)
        {
            try
            {
                bool status = await mediator.Send(new AddPatientCommand() { Patient = patient });
                return Ok(status);
            }
            catch(AddPatientException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("addInfluence")]
        public async Task<ActionResult<bool>> AddInfluence(Influence influence)
        {
            try
            {
                bool status = await mediator.Send(new AddInfluenceCommand() { Influence = influence });
                return Ok(status);
            }
            catch(AddInfluenceException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
