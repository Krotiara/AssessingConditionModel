﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Entities.Requests;
using PatientsResolver.API.Models.Requests;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;
using PatientsResolver.API.Service.Services;
using System;

namespace PatientsResolver.API.Controllers
{
    [Route("patientsApi/[controller]")]
    public class InfluencesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly InfluencesDataService _influencesDataService;

        public InfluencesController(IMediator mediator, InfluencesDataService influencesDataService)
        {
            this.mediator = mediator;
            _influencesDataService = influencesDataService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetInfluenceById(string id)
        {
            var inf = await _influencesDataService.Get(id);
            if (inf == null)
                return Ok();
            return Ok(inf);
        }


        [HttpPost("add")]
        public async Task<ActionResult> AddPatientInfluence([FromBody] Influence influence)
        {
            await _influencesDataService.Insert(influence);
            return Ok();
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdatePatientInfluence([FromBody] Influence influence)
        {
            await _influencesDataService.Update(influence.Id, influence);
            return Ok();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteInfluence(string id)
        {
            await _influencesDataService.Delete(id);
            return Ok();
        }


        [HttpPost("influences")]
        public async Task<ActionResult> GetInfluences([FromBody] GetInfluencesRequest request)
        {
            DateTime start = request.StartTimestamp == null ? DateTime.MinValue : (DateTime)request.StartTimestamp;
            DateTime end = request.EndTimestamp == null ? DateTime.MaxValue : (DateTime)request.EndTimestamp;
            var influences = await _influencesDataService.Query(request.PatientId, request.Affiliation, start, end);
            return Ok(influences);
        }


        [HttpPost("addInfluenceData")]
        public async Task<ActionResult> AddData([FromBody] AddInfluencesRequest request)
        {
            //TODO - заменить на сервис
            bool isSuccessSendRequest = await mediator.Send(new SendPatientDataFileSourceCommand() { Request = request });
            return Ok(isSuccessSendRequest);
        }
    }
}
