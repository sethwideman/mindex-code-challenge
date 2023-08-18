using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost("{id}/compensation")]
        public IActionResult CreateCompensation(string id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation creation request for employee '{id}'");

            compensation = _compensationService.CreateCompensation(id, compensation);

            if (compensation == null)
                return NotFound();

            return CreatedAtRoute("getCompensationByEmployeeId", new { id }, compensation);
        }

        [HttpGet("{id}/compensation/current", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCurrentCompensationByEmployeeId(string id)
        {
            _logger.LogDebug($"Get current compensation request received for employee '{id}'");

            var compensation = _compensationService.GetCurrentCompensation(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

        [HttpGet("{id}/compensation/")]
        public IActionResult GetAllCompensationByEmployeeId(string id)
        {
            _logger.LogDebug($"Get all compensation request received for employee '{id}'");

            var compensationList = _compensationService.GetAllCompensations(id);

            if (compensationList == null)
                return NotFound();

            return Ok(compensationList);
        }
    }
}
