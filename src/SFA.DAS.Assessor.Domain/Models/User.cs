using System;

namespace SFA.DAS.Assessor.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string DfeSignInUserId { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLocked { get; set; }

        public static implicit operator User?(Entities.User? source)
        {
            if (source == null)
            {
                return null;
            }

            return new User
            {
                Id = source.Id,
                DfeSignInUserId = source.DfeSignInUserId,
                EmailAddress = source.EmailAddress,
                CreatedAt = source.CreatedAt,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                IsLocked = source.IsLocked
            };
        }
    }
}