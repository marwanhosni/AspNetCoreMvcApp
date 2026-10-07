using Demo.BLL.Repo.Abstract;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace Demo.BLL.Repo.Implementation;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private protected ApplicationDbContext _context;


    public GenericRepository(ApplicationDbContext context) // Ask CLR Create Object From AppDbContext
    {
        // _context = new AppDbContext();
        _context = context;
    }

        
    public async Task<T> Get(int id)
    {
        //var employees = _context.Employees.FirstOrDefault(D => D.Id == id);
        var Result = await  _context.Set<T>().FindAsync(id);
        return Result;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        //var employees = _context.Employees.ToList();
        //var employees = _context.Set<Employee>.ToList();
        //var Result = _context.Set<T>().ToList();
        //return Result;
        if (typeof(T) == typeof(Employee))
        {
            return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
        }

        return await _context.Set<T>().ToListAsync();
    }

    public void Add(T entity)
    {
        if (entity is not null)
        {
            //_context.Employees.Add(entity);
            _context.Add(entity);
        }
        //return -1;
    }

    public void Delete(T entity)
    {
        if (entity is not null)
        {
            //_context.Employees.Remove(entity);
            _context.Remove(entity);
        }
        //return 0;
    }
    public void Update(T entity)
    {
        //_context.Employees.Remove(entity);
        _context.Update(entity);
    }

    //T IEmployeeRepository.Get(int id)
    //{
    //    throw new NotImplementedException();
    //}
}
