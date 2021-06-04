using RepoDb.Attributes;
using System;

namespace Domain.Identity.Oauth
{
    public class RefreshToken
    {
        [Primary]
        public long Id { get; set; }
        public string Token { get; set; }
        public long ClientId { get; set; }
        public string UserId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Revoked { get; set; }
    }
}
