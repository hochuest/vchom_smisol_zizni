using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;

using testwpf.whiskas;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;
using HtmlAgilityPack;

namespace testwpf
{
   public partial class MainWindow
   {
      public static Settings cfg = new Settings();
      static Dictionary<string, string> ID = new Dictionary<string, string>(16);

      public static int minPrice = 0;
      public static int maxPrice = 200;

      public MainWindow()
      {
         InitializeComponent();

         //cfg = (Settings)XmlSaver.Read(Settings.fileName, typeof(Settings));
         // части настроек

         //cfg.findProduct = "холодильник";
         //cfg.categoryId = "90764"; 

         cfg.findProduct = "";
         //cfg.categoryId = "54915";

         cfg.mailLogin = "nim20101@yandex.ru";
         cfg.mailPass = "madama98";

         cfg.recipientOfLetters = "nim20101@yandex.ru";

         cfg.message_header = "заголовок сообщения";
         cfg.letter_subject = "тема сообщения";

         cfg.before_the_message = "Поступили новые товары: \n";
         cfg.after_the_message = " (c) Система оповещений о новых товарах";

         cfg.hour = 2;
         cfg.minute = 22;

         //XmlSaver.Save(Settings.fileName, cfg);


         // пурсер
         //listProduct = new ObservableCollection<Product>(Purser.Start()); // через раз парсит

         // фоновый режим
         BackgroundMode.Start(Purser.Start, UseDB.GetList, UseDB.AddRange, SendMail.Send);

         // иконка в трее
         IconTray.InitializeNotifyIcon(this, "tree.ico", new ToolStripItem[] { });

      }

      async void Find(string path = null)
      {
         DataGrid1.ItemsSource = new List<Product>();
         ProgressRing.IsActive = true;

         DataGrid1.ItemsSource = await Task.Factory.StartNew(() => {
            List<Product> prod = new List<Product>();

            Request r = Purser.Start(new Request(cfg.findProduct, new List<Product>(), minPrice, maxPrice), path);
            UseDB.Add(r);

            prod = r.ListProduct;

            return prod;
         });

         ProgressRing.IsActive = false;

         if (Purser.CategoryList.Count > 0)
         {
            CB.IsEnabled = true;
            foreach (var node in Purser.CategoryList)
               if (node.Key != "")
                  CB.Items.Add(node.Key);

            if (this.IsActive)
               CB.IsDropDownOpen = true;
         }
         else
            CB.IsEnabled = false;

         HtmlNodeCollection Price = Purser.document.DocumentNode.SelectNodes("//div[@class='_16hsbhrgAf']/ul/li/p/input");
         if (Price != null)
         {
            RangeSlider.Minimum = Convert.ToInt32(Price[0].GetAttributeValue("placeholder", "").Replace(" ", ""));
            RangeSlider.Maximum = Convert.ToInt32(Price[1].GetAttributeValue("placeholder", "").Replace(" ", ""));
         }

      }

      // events

      void OnStateChanged(object sender, EventArgs args)
      {
         IconTray.ev(args);
      }

      private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
      {
         Hyperlink link = (Hyperlink)e.OriginalSource;
         Process.Start(link.NavigateUri.AbsoluteUri);
      }

      bool flag;
      private void CB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         if (flag) return;
         Find(CB.SelectedItem.ToString());
         flag = true;
         CB.Items.Clear();
         CB.IsEnabled = false;
         flag = false;
      }

      private void Prod_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
      {
         if (e.Key == Key.Return)
         {
            cfg.findProduct = Prod.Text;
            Find();
         }
      }
      
      private void Button_Click(object sender, RoutedEventArgs e)
      {
         Flyout.IsOpen = true;
      }

      private void RangeSlider_LowerValueChanged(object sender, MahApps.Metro.Controls.RangeParameterChangedEventArgs e)
      {
         minPrice = (int)e.NewValue;
      }

      private void RangeSlider_UpperValueChanged(object sender, MahApps.Metro.Controls.RangeParameterChangedEventArgs e)
      {
         maxPrice = (int)e.NewValue;
      }
   }
}