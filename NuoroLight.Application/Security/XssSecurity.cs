using Ganss.Xss;

namespace NuoroLight.Application.Security
{
    public static class XssSecurity
    {
        // یک شیء HtmlSanitizer ثابت برای جلوگیری از ساخت شیء در هر بار فراخوانی
        private static readonly HtmlSanitizer _htmlSanitizer = new HtmlSanitizer
        {
            KeepChildNodes = true,
            AllowDataAttributes = true
        };

        public static string SanitizeText(this string text)
        {
            // بررسی برای null یا خالی بودن ورودی
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            // انجام عملیات ضد XSS
            return _htmlSanitizer.Sanitize(text);
        }
    }
}