using AutoMapper;
using Demo.DAL.Models;
using Demo.pl.ViewModels;

namespace Demo.pl.Helper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<EmployeeViewModel, Employee>();

            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();

        }
    }
}
