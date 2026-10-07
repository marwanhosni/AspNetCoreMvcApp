using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace MvcWithRootAcademy.Helper;

public class EmailSettings
{
    public static void sendEmail(Email email) 
    { 
    var client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("marwanhosni785@gmail.com", "apwjhjssisgmxvtw");

        client.Send("marwanhosni785@gmail.com", email.Recipients, email.Subject, email.Body);
    }
}
