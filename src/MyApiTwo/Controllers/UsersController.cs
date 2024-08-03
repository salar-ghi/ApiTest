using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MyApiTwo.Models;

namespace MyApiTwo.Controllers;

[Authorize]
//[Route("api/[controller]")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private static readonly List<User> users = new()
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
            new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" },
            new User { Id = 4, Name = "Alice Williams", Email = "alice@example.com" },
            new User { Id = 5, Name = "Tom Brown", Email = "tom@example.com" }
    };

    [HttpGet]
    public IEnumerable<User> GetUsers()
    {
        return users;
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)  
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPost]
    public ActionResult<User> CreateUser(User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public ActionResult<User> UpdateUser(int id, User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        return user;
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        users.Remove(user);
        return NoContent();
    }
}
