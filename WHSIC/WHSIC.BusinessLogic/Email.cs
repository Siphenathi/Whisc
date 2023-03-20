using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.BusinessLogic
{
    public class Email
    {
        public MailAddress to { get; set; }
        public MailAddress from { get; set; }
        public string sub { get; set; }
        public string body { get; set; }

    //    public string ConfirmUploads()
    //    {
    //        string feed = "";

    //        var b = new MailMessage()
    //        {
    //            Subject = "Confirmation of Uploads",
    //            Body = body,
    //            IsBodyHtml = true,
    //        };
    //        b.From = new MailAddress("21231855@dut4life.ac.za", "Secretary");
    //        b.To.Add(to);
    //        SmtpClient smtp = new SmtpClient
    //        {
    //            Host = "smtp.live.com",
    //            Port = 25,
    //            Credentials = new NetworkCredential("21231855@dut4life.ac.za", "Dut950319"),
    //            EnableSsl = true
    //        };
    //        try
    //        {
    //            smtp.Send(b);
    //            feed = "Email has been sent to Bishop for confirmation";
    //        }
    //        catch (Exception e)
    //        {
    //            feed = "Email not Sent" + e.Message;
    //        }
    //        return feed;
    //    }
    }

    public class MailService
    {
        public static async Task SendMail(string eto, string eSubject, string eBody)
        {
            try
            {
                var _email = "ncnmbatha45@outlook.com";
                var _pass = "Vuyiswa4me";
                // var _disp = "King";

                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(eto);
                mailMessage.From = new MailAddress(_email);
                mailMessage.Subject = eSubject;
                mailMessage.Body = eBody;
                mailMessage.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.live.com", 25))
                {
                    smtp.EnableSsl = true;
                    //smtp.Host = "smtp.live.com";
                    //smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _pass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(mailMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
