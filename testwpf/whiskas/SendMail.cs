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

         m.IsBodyHtml = true;

         string message = @"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
               <title>Bootstrap Example</title>
               <meta charset=""utf-8"">
               <meta name=""viewport"" content=""width =device-width, initial-scale=1"">
               <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css"">
               <script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js""></script>
               <script src=""https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js""></script>
               <script src=""https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js""></script>
            </head>
            <body>
            <div class=""container"">
         ";


         foreach (var nR in newRequests)
         {
            message += $@"
               <h2>По запросу: {nR.requestName}</h2>    
               <p>Найдено {nR.ListProduct.Count} новых товаров</p>     
               <table class=""table table-striped"">
                  <thead class=""thead-dark"">
                     <tr>
                        <th style=""width: 150px; "">Цена</th>
                        <th>Товар</th>
                     </tr>
                  </thead>
               <tbody> 
            ";

            foreach (var nP in nR.ListProduct)
               message += $@" 
                  <tr>
                     <td>{nP.price}</td>
                     <td><a href="" {nP.url}"">{nP.name}</a></td>
                  </tr>
               ";

            message += @"
               </tbody>
             </table>
            ";
         }

         message += @"
            </div>
            </body>
            </html>
         ";

         m.Body = message;
         smtp.Send(m);
      }
   }
}
