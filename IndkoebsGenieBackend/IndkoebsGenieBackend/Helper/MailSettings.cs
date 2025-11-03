namespace IndkoebsGenieBackend.Helper
{
    public class MailSettings
    {
        public string Server { get; set; } = default!;
        public int Port { get; set; } = default!;
        public string SenderName { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
