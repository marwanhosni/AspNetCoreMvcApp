using AutoMapper;
using Demo.DAL.Models;
using Demo.pl.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcWithRootAcademy.Helper;
using MvcWithRootAcademy.ViewModels;
using System.Data;

namespace MvcWithRootAcademy.Controllers;
[Authorize(Roles="Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    //Get , GetAll ,Add , Update , Delete
    //Index ,Create, Details ,Edit , Delete
    public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    // Get All
    public async Task<IActionResult> Index(string SearchInput)
    {
        var roles = Enumerable.Empty<RoleViewModel>();
        if (string.IsNullOrEmpty(SearchInput))
        {
            roles = await _roleManager.Roles.Select(R => new RoleViewModel()
            {
                Id = R.Id,
                Name = R.Name
            }).ToListAsync();
        }
        else
        {
            roles = await _roleManager.Roles.Where(R => R.Name
            .ToLower()
            .Contains(SearchInput.ToLower()))
            .Select(R => new RoleViewModel
            {
                Id = R.Id,
                Name = R.Name

            }).ToListAsync();
        }

        return View(roles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new IdentityRole()
            {
                Name = model.Name
            };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));
            else
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    public async Task<IActionResult> Details(string? id, string ViewNmae = "Details")
    {
        if (id is null)
            return BadRequest();

        var roleFromDb = await _roleManager.FindByIdAsync(id);
        if (User is null)
            return NotFound();

        var role = new RoleViewModel()
        {
            Id = roleFromDb.Id,
            Name = roleFromDb.Name
        };


        return View(ViewNmae, role);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {

        return await Details(id, "Edit");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model) // Server Side Validation
    {
        if (id != model.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            //var count = _employeeRepository.Update(model);
            var roleFromDb = await _roleManager.FindByIdAsync(model.Id);
            if (roleFromDb is null)
                return NotFound();

            roleFromDb.Name = model.Name;


            await _roleManager.UpdateAsync(roleFromDb);

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        return await Details(id, "Delete");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model) // Server Side Validation
    {
        if (id != model.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role is null)
                return NotFound();

            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> AddOrRemoveRole(string? Id)
    {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role is null)
                return NotFound();
        ViewData["RoleId"] = role.Id;

        var userInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRoleVM = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.FirstName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                userInRole.Add(userInRoleVM); 
            }
        return View(userInRole);

    }
    [HttpPost]
    public async Task<IActionResult> AddOrRemoveRole(string? Id, List<UserInRoleViewModel> users)
    {
        var role = await _roleManager.FindByIdAsync(Id);
        if ( role is  null)
            return NotFound();

        if (ModelState.IsValid) 
        {
            foreach (var user in users)
            {
                
                var userFromDb = await _userManager.FindByIdAsync(user.UserId);
                if (userFromDb is not null) 
                { 
                    if (user.IsSelected & ! await _userManager.IsInRoleAsync(userFromDb,role.Name)) 
                    {
                        await _userManager.AddToRoleAsync(userFromDb, role.Name);
                    }
                    else if (! user.IsSelected & await _userManager.IsInRoleAsync(userFromDb, role.Name))

                    {
                        await _userManager.RemoveFromRoleAsync(userFromDb, role.Name);
                    }
                }
            }
            return RedirectToAction(nameof(Edit), new {id = Id});
        }
        return View(users);
    }

   

}

