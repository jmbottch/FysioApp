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
    public class OperationsController : ControllerBase
    {
        private readonly IOperationsRepository _operations;

        public OperationsController(IOperationsRepository operations)
        {
            _operations = operations;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(
                _operations.GetAll()
                );

        [HttpGet("{Code}")]
        public async Task<IActionResult> GetOne(string Code) =>
            Ok(
                _operations.GetOne(Code).FirstOrDefault()
                );

    }
}
