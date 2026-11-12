using System;

namespace SFA.DAS.Assessor.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string DfeSignInUserId { get; set; }
        public required string EmailAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
    }
}
