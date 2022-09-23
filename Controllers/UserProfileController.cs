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
//[Route("[controller]/[action]")]
public class UserProfileController : ControllerBase
{
     
     private readonly IUserRepo _repo;

   public UserProfileController(IUserRepo repo)
   {
       _repo = repo;
   }
   DateTime correctUtc = DateTime.UtcNow;

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
   public ActionResult GetUsers(int id)
   {
      var result = _repo.GetUsersAsync(id);
      return Ok(result);
      
      
   }

   [HttpPost("GenerateOTP")]
   public ActionResult GenerateOTP ()
   {
      
      var correction = new TimeCorrection(correctUtc);
      var totp = new Totp(secretKey(), step: 300, totpSize: 4, timeCorrection: correction); //OTP expires in 5 minutes
      var code = totp.ComputeTotp(DateTime.UtcNow);
      return Ok(code);
      
   }

   [HttpPost("VerifyOTP")]
   public ActionResult VerifyOTP(string code)
   {
        var correction = new TimeCorrection(correctUtc); 
        var window = new VerificationWindow(previous:1, future:1);
        var totp = new Totp(secretKey(), step: 300, totpSize: 4, timeCorrection: correction);
        while (true)
        {
         bool valid = totp.VerifyTotp(DateTime.UtcNow,code, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay); //window = null);  
         string validStr = valid ? "OTP Verified Successfully" : "Invalid OTP or Expired";
         return Ok($" The OTP code : {code} generated is == {validStr}");
        }
        
   }

   private byte[] secretKey()
   {
      string base32Secret = "SECRET";
      var secretBytes = Base32Encoding.ToBytes(base32Secret);
      return secretBytes;
   }

}
