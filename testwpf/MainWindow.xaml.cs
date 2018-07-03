using System;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel; // для ObservableCollection

using testwpf.whiskas;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace testwpf
{
   public partial class MainWindow
   {
      static public List<Product> listProduct = new List<Product>();
      static public Settings cfg = new Settings();
      static Dictionary<string, string> ID = new Dictionary<string, string>(16);

      public MainWindow()
      {
         InitializeComponent();

         //cfg.findProduct = "холодильник";
         //cfg.categoryId = "90764"; 

         cfg.findProduct = "Утюг";
         //cfg.categoryId = "54915";

         cfg.mailLogin = "nim20101@yandex.ru";
         cfg.mailPass = "madama98";

         cfg.recipientOfLetters = "nim20101@yandex.ru";

         cfg.message_header = "заголовок сообщения";
         cfg.letter_subject = "тема сообщения";

         cfg.before_the_message = "Поступили новые товары: \n";
         cfg.after_the_message = " (c) Система оповещений о новых товарах";

         cfg.hour = 22;
         cfg.minute = 44;

         // getID.Go(CB, ID);

         // пурсер
         //listProduct = new ObservableCollection<Product>(Purser.Start()); // через раз парсит

         // фоновый режим
         BackgroundMode.Start(Purser.Start, UseDB.GetList, UseDB.addDB, SendMail.Send);

         // заполнение таблицы
         DataGrid1.ItemsSource = listProduct;

         // иконка в трее
         IconTray.InitializeNotifyIcon(this, "tree.ico", new ToolStripItem[] { });

         // пример добавления в таблицу: listProduct.Add(new Product("kek1", "kek2", "kek3"));
         // ...
      }

      async void Find()
      {
         DataGrid1.ItemsSource = new List<Product>();
         ProgressRing.IsActive = true;

         DataGrid1.ItemsSource = await Task.Factory.StartNew(() => {
            List<Product> prod = new List<Product>();

            prod = Purser.Start();
            if (prod.Count == 0)
               listProduct.Clear();

            return prod;
         });

         ProgressRing.IsActive = false;
      }

      // events

      void OnStateChanged(object sender, EventArgs args)
      {
         IconTray.ev(args);
      }

      private void CB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         //Find();
      }
      
      private void Prod_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
      {
         if (e.Key == Key.Return)
         {
            cfg.findProduct = Prod.Text;
            Find();
         }
      }
   }
}