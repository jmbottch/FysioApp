using ApplicationCore.Abstractions.Api;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosesController : ControllerBase
    {
        private readonly IDiagnosesRepository _diagnoses;

        public DiagnosesController(IDiagnosesRepository vektis)
        {
            _diagnoses = vektis;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(
                _diagnoses.GetAll()
                );

        [HttpGet("{Code}")]
        public async Task<IActionResult> GetOne(string Code) =>
            Ok(
                _diagnoses.GetOne(Code).FirstOrDefault()
                );




    }
}
