using System.ComponentModel.DataAnnotations;

namespace StoredProcedureApi.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string  EmailAddress { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Old { get; set; } = string.Empty;
        public string OldProvider { get; set; } = string.Empty;

    }

   

}