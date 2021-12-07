using ApplicationCore.Abstractions.Api;
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
    public class FillingController : ControllerBase
    {
        private readonly IFillingService _fillingService;

        public FillingController(IFillingService fillingService)
        {
            _fillingService = fillingService;
        }
        // GET: Filling
        [HttpGet("Diagnoses")]
        public async Task<IActionResult> FillDiagnoses() =>
            Ok(
                _fillingService.FillDiagnoses()
            );
        [HttpGet("Operations")]
        public async Task<IActionResult> FillOperations() =>
            Ok(
                _fillingService.FillOperations()
                );
    }
}
