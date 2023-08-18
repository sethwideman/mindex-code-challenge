using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;

        public CompensationRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public Compensation Add(Compensation compensation)
        {
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetCurrentByEmployeeId(string id)
        {
            return GetAllCompensationsDateDesc(id).FirstOrDefault();
        }

        public List<Compensation> GetAllByEmployeeId(string id)
        {
            return GetAllCompensationsDateDesc(id).ToList();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        private IOrderedQueryable<Compensation> GetAllCompensationsDateDesc(string id)
        {
            return _employeeContext.Compensations.Include(c => c.Employee).Where(c => c.Employee.EmployeeId == id)
                            .OrderByDescending(c => c.EffectiveDate);
        }
    }
}
