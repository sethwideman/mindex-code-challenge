using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if (employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee GetEmployeeWithDetailsById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetEmployeeWithDetailsById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if (originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public ReportingStructure GetReportingStructureById(string employeeId)
        {
            var employee = GetEmployeeWithDetailsById(employeeId);

            if (employee == null)
                return null;

            return new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = GetNumberOfReports(employee, new HashSet<Employee>())
            };
        }

        // Recursively calculates total count of subordinates below the given employee. This function assumes a fully hydrated employee hierarchy
        private int GetNumberOfReports(Employee employee, HashSet<Employee> visitedEmployees)
        {
            if (employee == null || employee.DirectReports == null)
            {
                return 0;
            }

            if (visitedEmployees.Contains(employee))
            {
                _logger.LogWarning($"Circular reporting detected for {employee.EmployeeId}. Skipping further counts of this branch.");
                return 0;
            }

            visitedEmployees.Add(employee);

            int count = employee.DirectReports.Count;

            foreach (var subordinate in employee.DirectReports)
            {
                count += GetNumberOfReports(subordinate, visitedEmployees);
            }

            return count;
        }
    }
}
