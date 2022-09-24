using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesRatingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        this._userService = userService;
    }

    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return _userService.Get();
    }
    
    [HttpGet("{id}")]
    public ActionResult<User> Get(string id)
    {
        var user = _userService.Get(id);
        
        if (user == null)
        {
            return NotFound($"User with id = {id} not found");
        }

        return user;
    }

    [HttpPost]
    public ActionResult<User> Post([FromBody]User user)
    {
        _userService.Create(user);

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody]User user)
    {
        var existingUser = _userService.Get(id);

        if (existingUser == null)
        {
            return NotFound($"User with id = {id} not found");
        }
        
        _userService.Update(id, user);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var existingUser = _userService.Get(id);

        if (existingUser == null)
        {
            return NotFound($"User with id = {id} not found");
        }
        
        _userService.Remove(existingUser.Id);

        return Ok($"User with id = {id} deleted");
    }
}