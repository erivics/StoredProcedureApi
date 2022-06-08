using Microsoft.AspNetCore.Mvc;
using StoredProcedureApi.Models;
using StoredProcedureApi.Repository;

namespace StoredProcedureApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
     private readonly IUserRepo _repo;

    public UserProfileController(IUserRepo repo)
   {
       _repo = repo;
   }

   [HttpPost ("create")]
   public async Task<IActionResult> Create([FromBody] UserProfile model)
   {
      var result = await _repo.CreateUserAsync(model);
      return Ok(result);
   }

   [HttpGet("{email}:{passwordHash}")]
   public async Task<IActionResult> GetUsers(string email, string passwordHash)
   {
      var result = await _repo.GetUsersAsync(email, passwordHash);
      return Ok(result);
   }

}
