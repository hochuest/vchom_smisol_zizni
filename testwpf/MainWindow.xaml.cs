using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;

using testwpf.whiskas;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;

namespace testwpf
{
   public partial class MainWindow
   {
      static public Settings cfg = new Settings();
      static Dictionary<string, string> ID = new Dictionary<string, string>(16);

      public MainWindow()
      {
         InitializeComponent();


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

         cfg.hour = 22;
         cfg.minute = 44;

         // getID.Go(CB, ID);

         // пурсер
         //listProduct = new ObservableCollection<Product>(Purser.Start()); // через раз парсит

         // фоновый режим
         BackgroundMode.Start(Purser.Start, UseDB.GetList, UseDB.addRange, SendMail.Send);

         // иконка в трее
         IconTray.InitializeNotifyIcon(this, "tree.ico", new ToolStripItem[] { });


         
      }

      async void Find(string path = null)
      {
         DataGrid1.ItemsSource = new List<Product>();
         ProgressRing.IsActive = true;

         DataGrid1.ItemsSource = await Task.Factory.StartNew(() => {
            List<Product> prod = new List<Product>();

            prod = Purser.Start(null, path);

            return prod;
         });

         ProgressRing.IsActive = false;

         if (Purser.CategoryList.Count > 0)
         {
            CB.IsEnabled = true;
            foreach (var node in Purser.CategoryList)
               CB.Items.Add(node.Key);

            if (this.IsActive)
               CB.IsDropDownOpen = true;
         }
         else
            CB.IsEnabled = false;
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

      private void RangeSlider_LowerValueChanged(object sender, MahApps.Metro.Controls.RangeParameterChangedEventArgs e)
      {

      }

      private void RangeSlider_UpperValueChanged(object sender, MahApps.Metro.Controls.RangeParameterChangedEventArgs e)
      {

      }

      private void RangeSlider_LowerThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
      {

      }

      private void RangeSlider_LowerThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
      {

      }

      private void RangeSlider_UpperThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
      {

      }

      private void RangeSlider_UpperThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
      {

      }
      
      private void Button_Click(object sender, RoutedEventArgs e)
      {
         Flyout.IsOpen = true;
      }
   }
}