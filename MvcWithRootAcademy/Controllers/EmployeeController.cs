using AutoMapper;
using Demo.BLL.Repo.Abstract;
using Demo.DAL.Models;
using Demo.pl.ViewModels;
using Drmo.BLL.Interfaces;
using Drmo.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using MvcWithRootAcademy.Helper;
using AutoMapper;
using Demo.pl.ViewModels;
    
namespace Demo.pl.Controllers
{
    public class EmployeeController : Controller
    {
            //private IEmployeeRepository _employeeRepository; // Null
            //private IDepartmentRepository _departmentRepository; // Null
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(/*IEmployeeRepository employeeRepository*/
             /* , IDepartmentRepository departmentRepository*/
                IUnitOfWork unitOfWork ,
                IMapper mapper) //Ask CLR Create Object From DepartmentRepository
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // Get All
            public async Task<IActionResult> Index(string SearchName)
            {
                var employees = Enumerable.Empty<Employee>();
                if (string.IsNullOrEmpty(SearchName)) 
                {
                employees = await _unitOfWork.EmployeeRepository.GetAll();
                }
                else
                {
                ViewData["SearchName"] = SearchName;
                employees =await _unitOfWork.EmployeeRepository.GetByName(SearchName);
                }
                 var employeeViewModels =
                 mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

                     return View(employeeViewModels);
            }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            ViewData["Departments"] =await _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                model.ImageName = DoucmentSettings.UploadFile(model.Image, "images");
                var employee=mapper.Map<Employee>(model);
                //int count = _employeeRepository.Add(model);
                _unitOfWork.EmployeeRepository.Add(employee);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Departments"] =await _unitOfWork.DepartmentRepository.GetAll();

            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string ViewNmae = "Details")
        {
            if (id is null)
                return BadRequest();

            var Employees =await _unitOfWork.EmployeeRepository.Get(id.Value);
            var results = mapper.Map<EmployeeViewModel>(Employees); 
            if (Employees is null)
                return NotFound();

            return View(ViewNmae, results);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Departments"] =await _unitOfWork.DepartmentRepository.GetAll();
            
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model) // Server Side Validation
        {
            if (id != model.Id)
                return BadRequest();
            if(model.ImageName is not null)
            {
                DoucmentSettings.Delete(model.ImageName, "images");
            }
            model.ImageName = DoucmentSettings.UploadFile(model.Image, "images");

            if (ModelState.IsValid)
            {
                //var count = _employeeRepository.Update(model);
                var result = mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Update(result);
                int count =await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Departments"] =await _unitOfWork.DepartmentRepository.GetAll();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return  await Details(id, "Delete");
        }

        //[HttpPost]
        //public IActionResult Delete([FromRoute] int? id, EmployeeViewModel model) // Server Side Validation
        //{
        //    //if (id != model.Id)
        //    //    return BadRequest();

        //    if (ModelState.IsValid)
        //    {
        //        var result = mapper.Map<Employee>(model);

        //        //var count = _employeeRepository.Delete(model);
        //        _unitOfWork.EmployeeRepository.Delete(result );
        //        int count = _unitOfWork.Complete();
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.Get(id);

            if (employee is null)
                return NotFound();

            _unitOfWork.EmployeeRepository.Delete(employee);

            int count =await _unitOfWork.Complete();

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }
    }
}
