using ATM.DBContext;
using ATM.Models;
using ATM.Models.LoggerModels;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Services
{
    public static class EmailServices
    {

        public static void SendLimitWarMail(int cardId) 
        {
            ////Адрес SMTP-сервера
            //String smtpHost = "smtp.mail.ru";
            ////Порт SMTP-сервера
            //int smtpPort = 587;
            ////Логин
            //String smtpUserName = "suppanurlan@mail.ru";
            ////Пароль
            ////jBVACh8EvN485qsP2g0c
            //String smtpUserPass = "AAAa025810jBVACh8EvN485qsP2g0c";

            ////Создание подключения
            ////SmtpClient client = new SmtpClient(smtpHost, smtpPort);
            ////client.Credentials = new NetworkCredential(smtpUserName, smtpUserPass);
            ///
            try
            {
                CardRepository cr = new CardRepository();
                var card = cr.FindByIdAsync(cardId).Result;
                UserRepository ur = new UserRepository();
                var user = ur.FindByIdAsync(card.OwnerUserId).Result;

                UserServices ass = new UserServices();
                string cardLast4Number = card.ShowCardLastFourNumber();

                SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.mail.ru",
                    Port = 2525,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("suppanurlan@mail.ru", "jBVACh8EvN485qsP2g0c")
                };


                String msgFrom = "suppanurlan@mail.ru";

                String msgTo = user.Email;
                String msgSubject = "Over Limit Bank Card";
                String msgBody = $"{cardLast4Number} card blocked. Please contact with support";

                MailMessage message = new MailMessage(msgFrom, msgTo, msgSubject, msgBody);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    CardLog.CreateLoggerError(cardId, ex);
                }
            }
            catch (Exception ex)
            {
                CardLog.CreateLoggerError(cardId, ex);
            }
        }
    }
}
