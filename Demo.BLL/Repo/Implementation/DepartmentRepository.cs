using Demo.BLL.Repo.Abstract;
using Demo.DAL.Models;
using Demo.DAL.Data;
using System;

namespace Demo.BLL.Repo.Implementation;
public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {

	}
}
