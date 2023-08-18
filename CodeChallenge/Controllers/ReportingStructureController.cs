using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}/reporting-structure")]
        public IActionResult GetEmployeeReportingStructure(string id)
        {
            _logger.LogDebug($"Recieved employee reporting structure request for '{id}'");

            var reportingStructure = _employeeService.GetReportingStructureById(id);

            if (reportingStructure == null)
                return NotFound("Employee not found");

            return Ok(reportingStructure);
        }
    }
}
