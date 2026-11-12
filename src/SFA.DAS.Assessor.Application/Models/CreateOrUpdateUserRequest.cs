namespace SFA.DAS.Assessor.Application.Models
{
    public class CreateOrUpdateUserRequest
    {
        public required string DfeSignInUserId { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
