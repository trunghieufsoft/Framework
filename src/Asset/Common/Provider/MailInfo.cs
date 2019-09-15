using Asset.Common.Provider.Enumerable;

namespace Asset.Common.Provider
{
    public class MailInfo
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string Username { get; set; }

        public SendType Option { get; set; } = SendType.NewPassword;

        public string Company { get; set; } = "FPT Company";
    }
}
