using System.Net;

namespace Client.Models.Errors
{
    public sealed record CustomErrorViewModel
    {
        public CustomErrorViewModel()
        {

        }

        public CustomErrorViewModel(int statusCode, string title, string message, string? redirectPath)
        {
            StatusCode = statusCode;
            Title = title;
            Message = message;
            RedirectPath = redirectPath;
        }

        public int StatusCode { get; init; }
        public string Title { get; init; } = null!;
        public string Message { get; init; } = null!;
        public string? RedirectPath { get; init; }

        internal static CustomErrorViewModel NotFound(string? redirectPath) => new CustomErrorViewModel(
                (int)HttpStatusCode.NotFound,
                "Sayfa Bulunamadı!",
                "Üzgünüz, aradığınız sayfa bulunamadı.",
                redirectPath
            );

        internal static CustomErrorViewModel AccessDenied(string? redirectPath) => new CustomErrorViewModel(
                (int)HttpStatusCode.Forbidden,
                "Erişim Reddedildi!",
                "Bu sayfaya erişim izniniz yok.",
                redirectPath
            );

        internal static CustomErrorViewModel Internal(string? redirectPath) => new CustomErrorViewModel(
                (int)HttpStatusCode.InternalServerError,
                "Beklenmeyen Hata!",
                "Bir şeyler ters gitti. Lütfen daha sonra tekrar deneyin.",
                redirectPath
            );
    }
}