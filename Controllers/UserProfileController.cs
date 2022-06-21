using DotNet.RateLimiter;
using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using StoredProcedureApi.Models;
using StoredProcedureApi.Repository;

namespace StoredProcedureApi.Controllers;

[ApiController]
[Route("rate-limit-on-action")]
//if set Scope to Controller to rate limit on all actions no matter which actions call
//the default value is Action means this rate limit check for each action separately
//[RateLimit(Limit = 3, PeriodInSec = 60, Scope = RateLimitScope.Controller)]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
     private readonly IUserRepo _repo;

    public UserProfileController(IUserRepo repo)
   {
       _repo = repo;
   }

   [HttpPost ("create")]
   [RateLimit(PeriodInSec = 10, Limit = 3, RouteParams = "create")]
   public IActionResult Create([FromBody] UserProfile model)
   {
      var result =  _repo.CreateUserAsync(model);
      return Ok(result);
   }

   ///summary
   ///Rate Limit:control the frequency of user requests to prevent malicious attacks
   ///summary
   ///<return></returns>

   [HttpGet("{id}")]
   [RateLimit(PeriodInSec = 10, Limit = 3, RouteParams = "{id}")]
   public async Task<IActionResult> GetUsers(int id)
   {
      var result = await _repo.GetUsersAsync(id);
      return Ok(result);
   }

}
