using CodeChallenge.Models;
using System.Collections.Generic;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation CreateCompensation(string employeeId, Compensation compensation);
        List<Compensation> GetAllCompensations(string employeeId);
        Compensation GetCurrentCompensation(string employeeId);
    }
}
