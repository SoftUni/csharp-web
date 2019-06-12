namespace Musaca.App.BindingModels.Users
{
    using SIS.MvcFramework.Attributes.Validation;

    public class UserRegisterBindingModel
    {
        private const string UsernameErrorMessage = "Invalid username length! Must be between 5 and 20 symbols!";

        private const string EmailErrorMessage = "Invalid username length! Must be between 5 and 50 symbols!";

        private const string PasswordErrorMessage = "Invalid password length!";

        [RequiredSis]
        [StringLengthSis(5, 20, UsernameErrorMessage)]
        public string Username { get; set; }

        [RequiredSis]
        [PasswordSis(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [RequiredSis]
        public string ConfirmPassword { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 50, EmailErrorMessage)]
        [EmailSis]
        public string Email { get; set; }
    }
}
