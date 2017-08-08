using System.Web;
using JWT_ODATA_WEB_API.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.DataAccess
{
    public class UserAccess
    {
        public static ApplicationUser GetCurrentUser()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                return userManager.FindById(userId);
            }
            return null;
        }

        public static ApplicationUser GetUser(string userId)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            return userManager.FindById(userId);
        }

        public static string GetCurrentUserFullName()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = userManager.FindById(userId);
            return user.FirstName + " " + user.LastName;
        }

        public static string GetCurrentUserName()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null)
                {
                    var userId = HttpContext.Current.User.Identity.GetUserId();
                    var user = userManager.FindById(userId);
                    if (user?.UserName != null) return user.UserName;
                }
            }
            return "NoUser";
        }
    }
}