using System.Collections.Generic;
using System.Net.Mail;
using testwpf;

namespace testwpf.whiskas
{
   class SendMail
   {
      public static void Send(List<Request> newRequests)
      {
         // настройки smtp-сервера, с которого мы и будем отправлять письмо
         SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
         smtp.EnableSsl = true;
         smtp.Credentials = new System.Net.NetworkCredential(MainWindow.cfg.mailLogin, MainWindow.cfg.mailPass);

         // наш email с заголовком письма
         MailAddress from = new MailAddress( MainWindow.cfg.mailLogin, MainWindow.cfg.message_header );
         // кому отправляем
         MailAddress to = new MailAddress( MainWindow.cfg.recipientOfLetters);
         // создаем объект сообщения
         MailMessage m = new MailMessage( from, to );
         // тема письма
         m.Subject = "theme";
         // текст письма

         string message = "";
         foreach (var nR in newRequests)
         {
            message += $"По запросу: {nR.requestName}\n";

            foreach (var nP in nR.ListProduct)
               message += $"   {nP.name}\n{nP.price}\n{nP.url}\n\n";
         }
            

         m.Body = MainWindow.cfg.before_the_message + message + MainWindow.cfg.after_the_message;
         smtp.Send(m);
      }
   }
}
