using AutoMapper;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Exceptions;
using PatientsResolver.API.Service.Query;
using System.Text;

namespace PatientsResolver.API.Controllers
{
    public class PatientsResolverController : Controller
    {
        private readonly IMediator mediator;
        //private readonly IMapper mapper;

        public PatientsResolverController(IMediator mediator)
        {
            //this.mapper = mapper;
            this.mediator = mediator;
        }


#warning для тестируемых нужд
        [HttpGet("update/{patientId}")]
        public async Task<ActionResult> UpdatePatient(int patientId)
        {
            try
            {
                return Ok(await mediator.Send(new SendUpdatePatientsInfoCommand() { UpdatePatientsInfo = new UpdatePatientsInfo() { UpdatedIds = new HashSet<int> { patientId } } }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("latestPatientParameters/{patientId}")]
        public async Task<ActionResult<List<IPatientParameter>>> GetLatestPatientParameters(int patientId, [FromBody]DateTime[] timeSpan)
        {
            try
            {  
                return Ok(await mediator.Send(new GetLatesPatientParametersQuery() { PatientId = patientId, 
                    StartTimestamp = timeSpan.FirstOrDefault(), 
                    EndTimestamp = timeSpan.LastOrDefault()}));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("influence/{patientId}")]
        public async Task<ActionResult<List<Influence>>> GetPatientInfluences(int patientId,
            DateTime? startTimestamp, DateTime? endTimestamp)
        {
            try
            {
                if (startTimestamp == null)
                    startTimestamp = DateTime.MinValue;
                if (endTimestamp == null)
                    endTimestamp = DateTime.MaxValue;
                return Ok(await mediator.Send(new GetPatientInfluencesQuery(patientId, 
                    (DateTime)startTimestamp, (DateTime)endTimestamp)));
            }
            catch (Exception ex)
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
            #warning Необходимо переделать под массив байт в body
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
                IList<Patient> addedPatients = await mediator.Send(new AddNotExistedPatientsCommand() {Patients = new List<Patient> { patient } });
                if (addedPatients.Count > 0)
                    await mediator.Send(new SendPatientsCommand() { Patients = addedPatients.ToList() });

                return Ok(addedPatients.Count > 0);
            }
            catch(AddPatientException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
