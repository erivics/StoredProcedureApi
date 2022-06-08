using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoredProcedureApi.Models;

namespace StoredProcedureApi.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepo> _logger;

        public UserRepo(AppDbContext context, ILogger<UserRepo> logger)
        {
            _context = context;
            _logger = logger;
            
        }

        public async Task<int> CreateUserAsync(UserProfile model)
        {
           //Declaring Parameters binding
           var emailAddParm = new SqlParameter("@emailAddress", model.EmailAddress);
           var passwordHashParam = new SqlParameter("@passwordHash", model.PasswordHash);
           var oldParam = new SqlParameter("@old", model.Old);
           var oldProvParam = new SqlParameter("@oldProvider", string.IsNullOrEmpty(model.OldProvider)? "": model.OldProvider);
           var userIdParam = new SqlParameter("@id int out", SqlDbType.Int);
           userIdParam.Direction = ParameterDirection.InputOutput;
           int result = new int();
            
           try
           {
               if(model != null)
                {       
                   await _context.Database.ExecuteSqlRawAsync("exec sp_CreateUser @emailAddress, @passwordHash, @old, @oldProvider, @id int out",emailAddParm,passwordHashParam,oldParam,oldProvParam,userIdParam);
                   var result2 = Convert.ToInt32(userIdParam.SqlValue); 
                   result = result2;

                }
                return -1;
           
           }
           catch (SqlException ex)
           {
               
               _logger.LogInformation($"Error while creating user :{ex.Message}");
           }
            return result;
          
        }

        public async Task<List<UserProfile>> GetUsersAsync(string email, string passwordHash)
        {
            var emailAddParm = new SqlParameter("@emailAddress", email);
            var passwordHashParam = new SqlParameter("@passwordHash", passwordHash);
             
            var users =  _context.UserProfiles.FromSqlRaw("exec sp_GetUsers @emailAddress,@passwordHash", emailAddParm, passwordHashParam).ToList();
            
             if(users.Count < 0)
              {
                return null;
              }  
              else
              {
                 return Task.FromResult<List<UserProfile>>(users).Result;
              }     
        }
    }
}
