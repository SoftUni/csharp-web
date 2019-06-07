namespace IRunes.App.ViewModels.Users
{
    using SIS.MvcFramework.Attributes.Validation;

    public class UserLoginInputModel
    {
        private const string ErrorMessage = "Invalid username or password!";

        [RequiredSis(ErrorMessage)]
        public string Username { get; set; }

        [RequiredSis(ErrorMessage)]
        public string Password { get; set; }
    }
}
