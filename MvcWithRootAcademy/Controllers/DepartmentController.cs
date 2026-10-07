using AutoMapper;
using Demo.BLL.Repo.Abstract;
using Demo.BLL.Repo.Implementation;
using Demo.DAL.Models;
using Demo.pl.ViewModels;
using Drmo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace MvcWithRootAcademy.Controllers;

public class DepartmentController : Controller
{
    //private IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentController(/*IDepartmentRepository departmentRepository*/
                                  IUnitOfWork unitOfWork ,
                                  IMapper mapper)
    {
        _unitOfWork = unitOfWork    ;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var Departments = await _unitOfWork.DepartmentRepository.GetAll();
        var departmentViewModels =
        _mapper.Map<IEnumerable<DepartmentViewModel>>(Departments);
        return View(departmentViewModels);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //public IActionResult Create(Department model)
    //{
    //    if (ModelState.IsValid) // Server Side Validation
    //    {
    //        int count = _departmentRepository.Add(model);
    //        if (count > 0)
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //    }

    //    return View();
    //}
    [HttpPost]

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var department =
        _mapper.Map<Department>(model);

        //int count = _departmentRepository.Add(model);
        _unitOfWork.DepartmentRepository.Add(department);
        int count = await _unitOfWork.Complete();

        if (count > 0)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Could not save department.");
        return View(model);
    }

    public  async Task<IActionResult> Details(int? id, string ViewNmae = "Details")
    {
        if (id is null)
            return BadRequest();

        var department =await  _unitOfWork.DepartmentRepository.Get(id.Value);
        var results = _mapper.Map<DepartmentViewModel>(department);

        if (department is null)
            return NotFound();

        return View(ViewNmae, results);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        return await Details(id, "Edit");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel model) // Server Side Validation
    {
        if (id != model.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            //var count = _departmentRepository.Update(model);
            var department = _mapper.Map<Department>(model); 
            _unitOfWork.DepartmentRepository.Update(department);
            int count = await _unitOfWork.Complete();
            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        return await Details(id, "Delete");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Delete([FromRoute] int? id, Department model) // Server Side Validation
    {
        //if (id != model.Id)
        //    return BadRequest();

        //var count = _departmentRepository.Delete(model);
        var employees = await _unitOfWork.EmployeeRepository.GetAll();

        bool hasEmployees = employees.Any(e => e.DepartmentID == id.Value);
        if (hasEmployees)
        {
            TempData["Error"] =
                "You cannot delete this department because it has employees.";

            return RedirectToAction(nameof(Index));
        }

        _unitOfWork.DepartmentRepository.Delete(model);
            int count = await _unitOfWork.Complete();
            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
               TempData["Success"] = "Department deleted successfully.";
            }


        return RedirectToAction(nameof(Index));
    }
}
