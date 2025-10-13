namespace Application.Models.Constants.Settings
{
    public static class SettingConstant
    {
        public readonly static string[] AllowedImages = [".png", ".jpg", ".jpeg"];
        // public readonly static string[] AllowedFileFormats = [".clx"];
        public static class PaginationSettings
        {
            public const int MaxPageSize = 100;

        }

        public const string PerUserRateLimiting = "per-user";
        public const string RichRateLimiting = "rich";
        public const string AnonymousRateLimiting = "anonymous";
        public const string FixedRateLimiting = "fixed";
        public const string Tool30sOutputCache = "Tool30s";
        public const string Tool1mOutputCache = "Tool1m";
    }
}
