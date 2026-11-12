using System;
using SFA.DAS.Assessor.Domain.Models;

namespace SFA.DAS.Assessor.Api.Models
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string DfeSignInUserId { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLocked { get; set; }

        public static implicit operator GetUserResponse?(User? source)
        {
            if (source == null) return null;

            return new GetUserResponse
            {
                Id = source.Id,
                DfeSignInUserId = source.DfeSignInUserId,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                LastLoginAt = source.LastLoginAt,
                CreatedAt = source.CreatedAt,
                IsLocked = source.IsLocked
            };
        }
    }
}
