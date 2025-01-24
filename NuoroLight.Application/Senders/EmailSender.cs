using System.Net;
using System.Net.Mail;
namespace NuoroLight.Application.Senders;
public interface ISender
{
    bool SendEmail(string to, string subject, string body);
}
public class EmailSender : ISender
{
    public bool SendEmail(string to, string subject, string body)
    {
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.mail.yahoo.com");
            mail.From = new MailAddress("Samannarimani666@yahoo.com", "نورو لایت");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;

            SmtpServer.Credentials = new NetworkCredential("Samannarimani666@yahoo.com", "nbeleeafnffaeejc");
            SmtpServer.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}