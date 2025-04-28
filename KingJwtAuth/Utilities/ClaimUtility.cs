using System.Security.Claims;

namespace KingJwtAuth.Utilities
{
    public class ClaimUtility
    {
        public static Guid GetUserId(ClaimsPrincipal User)
        {
            try
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                Guid userId = Guid.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

                return userId;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }
        public static string GetUserEmail(ClaimsPrincipal User)
        {
            try
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                return claimIdentity.FindFirst(ClaimTypes.Email).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string GetUserFullname(ClaimsPrincipal User)
        {
            try
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                return claimIdentity.FindFirst(ClaimTypes.Name).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<string> GetUserRoles(ClaimsPrincipal User)
        {
            try
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                List<string> roles = new List<string>();

                foreach (var item in claimIdentity.Claims.Where(x => x.Type.EndsWith("role")))
                {
                    roles.Add(item.Value);
                }
                return roles;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static bool DoesUserExist(ClaimsPrincipal User)
        {
            try
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                if (claimIdentity.IsAuthenticated)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
