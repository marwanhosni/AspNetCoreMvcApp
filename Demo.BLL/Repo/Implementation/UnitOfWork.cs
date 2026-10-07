using Demo.BLL.Repo.Abstract;
using Demo.BLL.Repo.Implementation;
using Demo.DAL.Data;
using Drmo.BLL.Interfaces;
using Drmo.BLL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drmo.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEmployeeRepository EmployeeRepository { get; set; }

        public IDepartmentRepository DepartmentRepository { get ; set ; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context; 
            EmployeeRepository = new EmployeeRepository(context);
            DepartmentRepository = new DepartmentRepository(context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
