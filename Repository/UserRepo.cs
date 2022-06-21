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
      

        public int CreateUserAsync(UserProfile model)
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
           int result = new int();
           try
           {
               if(model != null)
                {                            
                  // await _context.Database.ExecuteSqlRawAsync(sqlCommand,emailAddParm,passwordHashParam,oldParam,oldProvParam,userIdParam);                  
                   //var result2 = Convert.ToInt32(userIdParam.Value); 
                   connection.Open();
                   var res = sqlCommand.ExecuteNonQuery();
                   connection.Close();
            

                   //using(IDataReader dr = sqlCommand.ExecuteReader())
                   result = res;

                }
                else
                {
                    return -1;
                }
           
           }
           catch (SqlException ex)
           {
               _logger.LogInformation($"Error creating User:{ex.Message}");
                
           }
            return result;
          
        }

        public Task<bool> DeletUsersAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserProfile>> GetUsersAsync(int id)
        {
            
            var idParam = new SqlParameter("@id", id);
            //var passwordHashParam = new SqlParameter("@passwordHash", passwordHash);
             
            var users =  _context.UserProfiles.FromSqlRaw(Endpoints.SqlGetUsers, idParam).ToList();
            
             if(users.Count < 0)
              {
                return null;
              }  
              else
              {
                 return Task.FromResult<List<UserProfile>>(users).Result;
              }     
        }

        public Task<UserProfile> UpdateUser(UserProfile model)
        {
            throw new NotImplementedException();
        }
    }











    public interface IUserRepo
    {
        int CreateUserAsync(UserProfile model);
        Task <List<UserProfile>> GetUsersAsync(int id);

        Task <bool> DeletUsersAsync(int id);

        Task<UserProfile> UpdateUser(UserProfile model);
    }

}





    