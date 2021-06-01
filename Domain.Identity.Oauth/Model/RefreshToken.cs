using RepoDb.Attributes;
using System;

namespace Domain.Identity.Oauth
{
    public class RefreshToken
    {
        [Primary]
        public long Id { get; set; }
        public string  Subject { get; set; }
        public char Type { get; set; }
        public string Value { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Revoked { get; set; }
    }
}
