namespace AspNetCoreIdentityApp.Service.Services
{
    public interface IEmailServices
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
    }
}
