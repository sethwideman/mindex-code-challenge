using CodeChallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation);
        Compensation GetCurrentByEmployeeId(string id);
        List<Compensation> GetAllByEmployeeId(string id);
        Task SaveAsync();
    }
}
