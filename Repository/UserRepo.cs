using System.Data;
using System.Dynamic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoredProcedureApi.Models;
using StoredProcedureApi.SPEndpoints;
using System.Data.SqlClient;
using System.Configuration;


namespace StoredProcedureApi.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepo> _logger;
        private string _conn;

        public UserRepo(AppDbContext context, ILogger<UserRepo> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _conn = config.GetConnectionString("DConnection");
        }

        ResponseModel response = new ResponseModel();
      

        public ResponseModel CreateUserAsync(UserProfile model)
        {
           //Declaring Parameters binding
           SqlConnection connection = new SqlConnection(_conn);
           SqlCommand sqlCommand = new SqlCommand(Endpoints.SpCreateUsers, connection);
           sqlCommand.CommandType = CommandType.StoredProcedure;
           sqlCommand.Parameters.AddWithValue("@emailAddress",model.EmailAddress);
           sqlCommand.Parameters.AddWithValue("@passwordHash", model.PasswordHash);
           sqlCommand.Parameters.AddWithValue("@old", model.Old);
           sqlCommand.Parameters.AddWithValue("@oldProvider", string.IsNullOrEmpty(model.OldProvider)? "": model.OldProvider);
           //sqlCommand.Parameters.AddWithValue("@useridout", SqlDbType.Int);


           /*var emailAddParm = new SqlParameter("@emailAddress",model.EmailAddress);
           var passwordHashParam = new SqlParameter("@passwordHash", model.PasswordHash);
           var oldParam = new SqlParameter("@old", model.Old);
           var oldProvParam = new SqlParameter("@oldProvider", string.IsNullOrEmpty(model.OldProvider)? "": model.OldProvider);
           var userIdParam = new SqlParameter("@useridout", SqlDbType.Int);
           userIdParam.Direction = ParameterDirection.Output; */
           //int result = new int();
           try
           {
               if(model != null)
                {                            
                  // await _context.Database.ExecuteSqlRawAsync(sqlCommand,emailAddParm,passwordHashParam,oldParam,oldProvParam,userIdParam);                  
                   //var result2 = Convert.ToInt32(userIdParam.Value); 
                   connection.Open();
                   var res = sqlCommand.ExecuteNonQuery();
                   connection.Close();
            
                   response.Error = res;
                   response.Message = "Record Successfully created";

                

                }
                else
                {
                   
                   response.Error = -1;
                   response.Message = " Create request not successful";
                    
                }
           
           }
           catch (SqlException ex)
           {
               _logger.LogInformation($"Error creating User:{ex.Message}");
                
           }
           
           return response;
          
        }

        public Task<bool> DeletUsersAsync(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel GetUsersAsync(int id)
        {
            
            var idParam = new SqlParameter("@id", id);
            //var passwordHashParam = new SqlParameter("@passwordHash", passwordHash);
             
            var user =  _context.UserProfiles.FromSqlRaw(Endpoints.SqlGetUsers, idParam);
            var userResult =  Task.FromResult(user).Result; 
            //if(userResult.ToString == null) return new ResponseModel {Message = "Not found", Error = 0};
            response.Message = "Success";
            response.Error = 1;
            response.Result = userResult;
                   
            return response;  
               
        }

        public Task<UserProfile> UpdateUser(UserProfile model)
        {
            throw new NotImplementedException();
        }
    }











    public interface IUserRepo
    {
        ResponseModel CreateUserAsync(UserProfile model);
        ResponseModel GetUsersAsync(int id);

        Task <bool> DeletUsersAsync(int id);

        Task<UserProfile> UpdateUser(UserProfile model);
    }

    public class ResponseModel
    {
        public int Error { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Result { get; set; }
    }

}





    