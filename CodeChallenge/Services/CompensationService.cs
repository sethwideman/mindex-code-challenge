using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeService employeeService) {
            _compensationRepository = compensationRepository;
            _employeeService = employeeService;
            _logger = logger;
        }
        public Compensation CreateCompensation(string employeeId, Compensation compensation)
        {
            var employee = _employeeService.GetById(employeeId);
            if (employee == null)
                return null;

            compensation.EmployeeId = employeeId;
            compensation.Employee = employee;

            _compensationRepository.Add(compensation);
            _compensationRepository.SaveAsync().Wait();

            return compensation;
        }
        public List<Compensation> GetAllCompensations(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
                return null;

            return _compensationRepository.GetAllByEmployeeId(employeeId);
        }
        public Compensation GetCurrentCompensation(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
                return null;

            return _compensationRepository.GetCurrentByEmployeeId(employeeId);
        }
    }
}
