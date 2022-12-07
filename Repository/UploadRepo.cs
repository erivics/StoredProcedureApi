using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using StoredProcedureApi.Models;
//using System.Data.SqlClient;
using System.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using StoredProcedureApi.SPEndpoints;
using Dapper;

namespace StoredProcedureApi.Repository
{
    public class UploadRepo : IUploadRepo
    {

    //private readonly AppDbContext _context;
        private readonly ILogger<UserRepo> _logger;
        private readonly IOptions<SConnection> _options;
        public UploadRepo(ILogger<UserRepo> logger, IOptions<SConnection> options)
        {
          _logger = logger;
          _options = options;
        }
    
        public async Task<ResponseModel> UploadImage(IFormFile formFile, UploadModel uploadmodel)
        {
            _logger.LogInformation("Default Logger: Trying to do a file upload");
            try
            {
                 int result = 0;
                 string myFilename = "image1";
                 
                 string filePath = Path.GetTempFileName();
                 //FileInfo fileinfo = new FileInfo(formFile.FileName);
                 //string fileName = myFilename + fileinfo.Extension;
                 //string fileNameWithPath = Path.Combine(filePath,fileName);
                 using(var stream = File.Create(filePath))
                 {
                    await formFile.CopyToAsync(stream);
                 }
                 // converts image file into byte
                 byte[] imageData = await File.ReadAllBytesAsync(filePath);
                 string imageDataToString = Convert.ToBase64String(imageData);

                using(var connection = new SqlConnection(_options.Value.DConnection))
                {                  
                    //connection.ExecuteAsync(); This works using dapper.net
                    SqlCommand sqlCommand = new SqlCommand(Endpoints.SpUpload, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@filename" ,myFilename); //uploadmodel.FileName);
                    sqlCommand.Parameters.AddWithValue("@filetype", 1);//uploadmodel.FileType); 
                    sqlCommand.Parameters.AddWithValue("@imagedata", imageDataToString);
                    await connection.OpenAsync();
                    int imageuploadedResult = await sqlCommand.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                    connection.Dispose();

                     result = imageuploadedResult;//return result;

                }
                if(result < 1)
                {
                    return new ResponseModel{ Message = "Unable to upload your file", ErrorStatus = result, Result = null};
                }
                return new ResponseModel {Message = "Image uploaded successfully", ErrorStatus = result, Result = null};
            }
            catch (SqlException ex)
            {
                
                _logger.LogError($"Error uploading file:{ex.Message}");
                return new ResponseModel {Message = ex.Message, ErrorStatus = -1};
            }
            
        }
    }




    public interface IUploadRepo
    {
    Task<ResponseModel> UploadImage (IFormFile formFile, UploadModel uploadmodel);
    }

}