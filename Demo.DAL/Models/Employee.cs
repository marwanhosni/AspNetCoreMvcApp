using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;


namespace Demo.DAL.Models;
public class Employee : BaseEntity
{

    
    public string Name { get; set; }

    public int? Age { get; set; }
    public decimal Salary { get; set; }

    
    public string Address { get; set; } // 123-Street-City-Country

    public string Phone { get; set; }

    public string Email { get; set; }

    public string? ImageName { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime HireDate { get; set; }

    public DateTime DateOfCreation { get; set; }

    public int? DepartmentID { get; set; }
    public  Department? Department { get; set; }
}
