using Domain.Database;
using RepoDb.Attributes;
using System;

namespace Domain.Identity.Data
{
    public class User : BaseModel
    {
        [Primary]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SaltText { get; set; }
        public DateTime PasswordExpiredDate { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public short PasswordErrorCounter { get; set; }
        public bool IsResetPassword { get; set; }
        public bool IsLocked { get; set; }
    }
}
