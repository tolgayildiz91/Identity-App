namespace AspNetCoreIdentityApp.Web.Services
{
    public interface IEmailServices
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
    }
}
