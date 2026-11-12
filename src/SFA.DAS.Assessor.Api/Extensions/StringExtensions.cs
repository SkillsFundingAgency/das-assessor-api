namespace SFA.DAS.Assessor.Api.Extensions
{
    public static class StringExtensions
    {
        public static string? SanitizeLogData(this string data)
        {
            if (data != null)
            {
                return data.Replace('\n', '_').Replace('\r', '_');
            }

            return null;
        }
    }
}