using System;
using Serilog;
using System.Net.Mail;
using System.Threading;
using Asset.Common.Provider;
using System.Threading.Tasks;
using Asset.Common.Provider.Enumerable;
using BusinessAccess.Service.Interface;
using Microsoft.Extensions.Configuration;

namespace BusinessAccess.Service
{
    public class EmailProvider : BaseService, IEmailProvider
    {
        private readonly IConfiguration _config;
        // title
        private string resetPasswordTitle = "Password Reset";
        private string resetCompanyInfoTitle = "{company}: Your password has been reset";
        private string newTitle = "Your account has been successfully created";

        // template
        private string resetPasswordTemplate = 
            "Dear {fullName}" +
            "<br><br><br>" +
            "Your password has been successfully reset. " +
            "<br>" +
            "The new password is: {password}" +
            "<br>" +
            "Please change your password after the first log in." +
            "<br><br><br>" +
            "Sincerely,<br>Administrator<br><i>Note: This is an auto-generated email, please do not reply.</i>";
        private string resetCompanyInfoTemplate = 
            "Dear {fullName}" +
            "<br><br><br>" +
            "Your password has been successfully reset.  " +
            "<br>" +
            "The new password is: {password}" +
            "<br><br><br>" +
            "Sincerely,<br>{company}<br><i>Note: This is an auto-generated email, please do not reply.</i>";
        private string newTemplate = 
            "Dear {fullName}" +
            "<br><br><br>" +
            "Your account has been successfully created. Please use detailed information below to log in." +
            "<br> Your username is: {username}" +
            "<br> Your password is: {password}" +
            "<br><br><br>" +
            "Sincerely,<br> {company} <br><i>Note: This is an auto-generated email, please do not reply.</i>";
        
        public EmailProvider(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMail(MailInfo info, CancellationToken cancellationToken = default)
        {
            Log.Information($"Send Mail {info.Option.ToString()} To {info.Email}");
            await SelectSendMail(info);
        }

        private async Task SendMailAsync(string title, string content, string email, string fileUlr)
        {
            try
            {
                string sender = _config["mailServer:Email"];
                string password = _config["mailServer:Password"];
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_config["mailServer:Host"]);

                mail.From = new MailAddress(sender);
                mail.To.Add(email);
                mail.Subject = title;
                mail.Body = content;
                mail.IsBodyHtml = true;
                SmtpServer.UseDefaultCredentials = false;

                if (fileUlr != null)
                {
                    Attachment attachment;
                    attachment = new Attachment(fileUlr);
                    mail.Attachments.Add(attachment);
                }
                SmtpServer.Port = int.Parse(_config["mailServer:Post"]);
                SmtpServer.Credentials = new System.Net.NetworkCredential(sender, password);
                SmtpServer.EnableSsl = bool.Parse(_config["mailServer:EnableSsl"]);

                await SmtpServer.SendMailAsync(mail);
            }
            catch (SmtpException e)
            {
                Log.Error($"Cant send mail, error: {e.Message}");
                throw;
            }
        }

        private async Task SelectSendMail(MailInfo info)
        {
            switch (info.Option)
            {
                case SendType.NewPassword:
                    await SendMailAsync(newTitle, newTemplate.Replace("{password}", info.Password).Replace("{fullName}", info.Fullname).Replace("{username}", info.Username).Replace("{company}", info.Company), info.Email, null);
                    break;
                case SendType.ResetPassword:
                    await SendMailAsync(resetPasswordTitle, resetPasswordTemplate.Replace("{password}", info.Password).Replace("{fullName}", info.Fullname), info.Email, null);
                    break;
                case SendType.ResetCompanyInfo:
                    await SendMailAsync(resetCompanyInfoTitle, resetCompanyInfoTemplate.Replace("{password}", info.Password).Replace("{fullName}", info.Fullname), info.Email, null);
                    break;
                // TODO:

                default:
                    throw new ArgumentException(message: "invalid enum value", paramName: nameof(info.Option));
            }
        }
    }
}