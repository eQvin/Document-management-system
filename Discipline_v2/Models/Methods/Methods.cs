using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Discipline_v2.Models
{
    public class Method : Controller
    {
        public void sendEmail(string emailTo, string subject, string body)
        {
            SmtpClient client = new SmtpClient("smtp.yandex.ru", 25);
            // вкл Ssl
            client.EnableSsl = true;
            // обрабатываем исходящее сообщение
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // отключаем запросы
            client.UseDefaultCredentials = false;
            // проверка отправителя
            client.Credentials = new NetworkCredential("disciplineweb@yandex.ru", "dis12345");
            // отправление
            MailMessage msg = new MailMessage();
            try
            {
                msg.Subject = subject;
                msg.Body = body;
                msg.From = new MailAddress("disciplineweb@yandex.ru");
                msg.To.Add(emailTo);
                msg.IsBodyHtml = true;
                client.Send(msg);
                //string userState = "test message1";
                //client.SendAsync(msg, userState);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }

        }

    }
}