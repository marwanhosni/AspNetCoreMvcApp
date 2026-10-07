

namespace Demo.DAL.Models;
public class Department : BaseEntity
{
    public String Code { get; set; }
    public String Name { get; set; }
    public DateTime DateOfCreation { get; set; }


    public ICollection<Employee>? Employees{ get; set; }
}
