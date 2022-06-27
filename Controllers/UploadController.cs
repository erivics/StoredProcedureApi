using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoredProcedureApi.Models;
using StoredProcedureApi.Repository;

namespace StoredProcedureApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadRepo _repo;

        public UploadController(IUploadRepo repo)
        {
            _repo = repo;
            
        }

        [HttpPost, Route("uploadfile")]
        public async Task<ActionResult<ResponseModel>> Upload(IFormFile formfile, [FromForm] UploadModel uploadmodel )
        {
           var result = await _repo.UploadImage(formfile, uploadmodel);
           return Ok(result);

        }
        
    }
}