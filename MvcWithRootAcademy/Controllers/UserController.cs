using AutoMapper;
using Demo.DAL.Models;
using Demo.pl.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcWithRootAcademy.Helper;
using MvcWithRootAcademy.ViewModels;

namespace MvcWithRootAcademy.Controllers;
[Authorize(Roles = "Admin")]

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManger;

    //Get , GetAll ,Add , Update , Delete
    //Index , Details ,Edit , Delete
    public UserController(UserManager<ApplicationUser> userManger  )
    {
        _userManger = userManger;
    }
    // Get All
    public async Task<IActionResult> Index(string SearchInput)
    {
        var users = Enumerable.Empty<UserViewModel>();
        if (string.IsNullOrEmpty(SearchInput))
        {
            users = await _userManger.Users.Select(U => new UserViewModel()
            {
                Id = U.Id,
                FirstName = U.FirstName,
                LastName = U.LastName,
                Email = U.Email,
            }).ToListAsync();
        }
        else
        {
            users = await _userManger.Users.Where(U => U.Email
            .ToLower()
            .Contains(SearchInput.ToLower()))
            .Select(U => new UserViewModel
            {
                Id = U.Id,
                FirstName = U.FirstName,
                LastName = U.LastName,
                Email = U.Email,
            }).ToListAsync();
        }
        foreach(var user in users)
        {
            var ApplicationUsers = await _userManger.FindByIdAsync(user.Id);

            if(ApplicationUsers is not null)
            {
                user.Roles = await _userManger.GetRolesAsync(ApplicationUsers);
            }
        }
        return View(users);
    }



    public async Task<IActionResult> Details(string? id, string ViewNmae = "Details")
    {
        if (id is null)
            return BadRequest();

        var UserFromDb = await _userManger.FindByIdAsync(id);
        if(User is null)
            return NotFound();
        var user = new UserViewModel()
        {
            Id = UserFromDb.Id,
            FirstName = UserFromDb.FirstName,
            LastName = UserFromDb.LastName,
            Email = UserFromDb.Email,
            Roles = await _userManger.GetRolesAsync(UserFromDb),
        };


        return View(ViewNmae, user);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {

        return await Details(id, "Edit");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model) // Server Side Validation
    {
        if (id != model.Id)
            return BadRequest();
        
        if (ModelState.IsValid)
        {
            //var count = _employeeRepository.Update(model);
            var userFromDb = await _userManger.FindByIdAsync(model.Id);
            if (userFromDb is null)
                return NotFound();

            userFromDb.FirstName = model.FirstName;
            userFromDb.LastName = model.LastName;
            userFromDb.Email = model.Email;

             await _userManger.UpdateAsync(userFromDb);
            
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
    public async Task<IActionResult>Delete([FromRoute] string? id, UserViewModel model) // Server Side Validation
    {
        if (id != model.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            var user = await _userManger.FindByIdAsync(model.Id);
            if (user is null)
                return NotFound();
            await _userManger.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

}

