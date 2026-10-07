using Demo.BLL.Repo.Abstract;
using Demo.DAL.Models;
using Drmo.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drmo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
       Task<IEnumerable<Employee>> GetByName(string Name);

    }

}
