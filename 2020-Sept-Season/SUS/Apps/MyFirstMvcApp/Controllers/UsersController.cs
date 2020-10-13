using SUS.HTTP;
using SUS.MvcFramework;

namespace BattleCards.Controllers
{
    public class UsersController : Controller
    {
        // GET /users/login
        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin()
        {
            return this.Redirect("/");
        }

        // GET /users/register
        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister()
        {
            // TODO: read data
            // TODO: check user
            // TODO: log user
            return this.Redirect("/");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Error("Only logged-in users can logout.");
            }

            this.SignOut();
            return this.Redirect("/");
        }
    }
}
