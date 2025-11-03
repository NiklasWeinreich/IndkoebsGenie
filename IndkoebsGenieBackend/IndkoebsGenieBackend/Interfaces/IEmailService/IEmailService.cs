using IndkoebsGenieBackend.DTO.EmailDTO;

namespace IndkoebsGenieBackend.Interfaces.IEmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailResponse EmailData);
    }
}
