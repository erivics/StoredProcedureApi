using DotNet.RateLimiter;
using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using StoredProcedureApi.Models;
using StoredProcedureApi.Repository;
using OtpNet;
using System.Text;

namespace StoredProcedureApi.Controllers;

[ApiController]
[Route("rate-limit-on-action")]
//if set Scope to Controller to rate limit on all actions no matter which actions call
//the default value is Action means this rate limit check for each action separately
//[RateLimit(Limit = 3, PeriodInSec = 60, Scope = RateLimitScope.Controller)]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
     DateTime correctUtc;
     private readonly IUserRepo _repo;

    public UserProfileController(IUserRepo repo)
   {
       _repo = repo;
   }

   [HttpPost ("create")]
   [RateLimit(PeriodInSec = 10, Limit = 3, RouteParams = "create")]
   public ActionResult<ResponseModel> Create([FromBody] UserProfile model)
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
   public IActionResult GetUsers(int id)
   {
      var result = _repo.GetUsersAsync(id);
      return Ok(result);
      
      
   }

   public IActionResult GenerateOTP ()
   {
      var correction = new TimeCorrection(correctUtc);
      var totp = new Totp(secretKey(), totpSize: 4, timeCorrection: correction);
      var code = totp.ComputeTotp();
      return Ok(code);
   }

   public IActionResult VerifyOTP(string code)
   {
        var window = new VerificationWindow(previous:1, future:1);
        var totp = new Totp(secretKey());
        bool valid = totp.VerifyTotp(code, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay); //window = null);  
        string validStr = valid ? "OTP Verified Successfully" : "Invalid OTP or Expired";
        return Ok(validStr);
   }

   private byte[] secretKey()
   {
      string base32Secret = "SECRET";
      var secret = Base32Encoding.ToBytes(base32Secret);
      return secret;
   }

}
