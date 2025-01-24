using System.Security.Claims;
using System.Security.Principal;

namespace NuoroLight.Web.PresentationExtensions
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                if (data != null) return Convert.ToInt32(data.Value);
            }
            return default(int);
        }
        public static int GetUserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;
            return user.GetUserId();
        }
        public static string GetUserMobile(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.MobilePhone);
                if (data != null) return Convert.ToString(data.Value);
            }
            return default(string);
        }
        public static string GetUserMobile(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;
            return user.GetUserMobile();
        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Email);
                if (data != null) return Convert.ToString(data.Value);
            }
            return default(string);
        }
        public static string GetUserEmail(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;
            return user.GetUserEmail();
        }
    }
}
