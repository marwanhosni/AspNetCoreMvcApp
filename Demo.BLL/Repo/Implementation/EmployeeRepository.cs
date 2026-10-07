using Demo.BLL.Repo.Implementation;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Drmo.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Drmo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Employee>> GetByName(string Name)
        {
            return await _context.Employees.Where(E => E.Name.Contains(Name)).Include(E => E.Department).ToListAsync();
        }
    }
}
