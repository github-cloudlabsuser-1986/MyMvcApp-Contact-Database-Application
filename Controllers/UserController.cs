using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class UserController : Controller
{
    public static System.Collections.Generic.List<User> userlist = new System.Collections.Generic.List<User>();

    // GET: User
    public ActionResult Index()
    {
        return View(userlist);
    }

    // GET: User/Details/5
    public ActionResult Details(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // GET: User/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: User/Create
    [HttpPost]
    public ActionResult Create(User user)
    {
        if (user == null)
        {
            return BadRequest("User cannot be null.");
        }

        if (ModelState.IsValid)
        {
            userlist.Add(user);
            TempData["Message"] = $"User '{user.Name}' has been successfully created.";
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    // GET: User/Edit/5
    public ActionResult Edit(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: User/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, User user)
    {
        var existingUser = userlist.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            TempData["Message"] = $"User '{user.Name}' has been successfully updated.";
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: User/Delete/5
    public ActionResult Delete(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: User/Delete/5
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        userlist.Remove(user);
        TempData["Message"] = $"User '{user.Name}' has been successfully deleted.";
        return RedirectToAction(nameof(Index));
    }

    // GET: User/Search
    public ActionResult Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            TempData["Message"] = "Showing all users.";
            return View("Index", userlist); // Return all users if no query is provided
        }

        var filteredUsers = userlist
            .Where(u => u.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                        u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        TempData["Message"] = filteredUsers.Any() 
            ? $"Found {filteredUsers.Count} user(s) matching '{query}'."
            : $"No users found matching '{query}'.";

        return View("Index", filteredUsers); // Reuse the Index view to display results
    }
}