using System.Globalization;

namespace Helpers
{
    public static class UIToolHelper
    {
        public static string RenderTL(decimal price) => price.ToString("C", new CultureInfo("tr-TR"));
        public static string RenderDateTime(DateTime date) => date.ToString("dd-MM-yyyy HH:mm");
        public static string RenderDate(DateTime date) => date.ToString("dd-MM-yyyy");
        public static string RenderDate(DateOnly date) => date.ToString("dd-MM-yyyy");
    } 
}