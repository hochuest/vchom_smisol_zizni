using System.Collections.Generic;
using System.Net.Mail;

namespace testwpf.whiskas
{
   class SendMail
   {
      public static void Send(List<Product> newProducts)
      {
         // настройки smtp-сервера, с которого мы и будем отправлять письмо
         SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
         smtp.EnableSsl = true;
         smtp.Credentials = new System.Net.NetworkCredential("nim20101@yandex.ru", "madama98"); // TODO: настройки через конфиг

         // наш email с заголовком письма
         MailAddress from = new MailAddress("nim20101@yandex.ru", "Test"); // TODO: настройки через конфиг
         // кому отправляем
         MailAddress to = new MailAddress("nim20101@yandex.ru"); // TODO: настройки через конфиг
         // создаем объект сообщения
         MailMessage m = new MailMessage(from, to);
         // тема письма
         m.Subject = "theme";
         // текст письма

         string message = "";
         foreach (var pr in newProducts)
            message += $"{pr.name} {pr.price} {pr.url} \n";

         m.Body = "Поступили новые товары: \n" + message; //  TODO: как то тоже настравивать через конфиг
         smtp.Send(m);
      }
   }
}
