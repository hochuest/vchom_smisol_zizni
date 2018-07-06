using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using testwpf.whiskas;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;
using HtmlAgilityPack;

namespace testwpf
{
   public partial class MainWindow : MetroWindow
   {
      [System.Runtime.InteropServices.DllImport("wininet.dll")]
      private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

      public static Settings cfg = new Settings();
      static Dictionary<string, string> ID = new Dictionary<string, string>(16);
      
      public MainWindow()
      {
         InitializeComponent();

         SaverSettings.Refresh();

         mailSend.Text = cfg.recipientOfLetters;

         // фоновый режим
         BackgroundMode.Start(Purser.Start, UseDB.GetList, UseDB.AddRange, SendMail.Send);

         // иконка в трее
         IconTray.InitializeNotifyIcon(this, "tree.ico", new ToolStripItem[] { });
      }

      async void Find(string path = null)
      {
         int g;
         if ( !InternetGetConnectedState(out g,0) )
         {
            await this.ShowMessageAsync("Ошибка", "Отсутсвует подключение к интернету");
            return;
         }

         List <Product> items;
         ProgressRing.IsActive = true;

         items = await Task.Factory.StartNew(() => {
            List<Product> prod = new List<Product>();

            Request r = Purser.Start(new Request(cfg.findProduct, new List<Product>(), minPrice, maxPrice), path);
            UseDB.Add(r);

            prod = r.ListProduct;
            
            return prod;
         });

         ProgressRing.IsActive = false;

         if (items.Count == 0)
            await this.ShowMessageAsync("Ошибка", "Товары не найдены");

         DataGrid1.ItemsSource = items;
         
         if (Purser.CategoryList.Count > 0)
         {
            CB.IsEnabled = true;
            DefaultNameComboBox.Content = "-- Выберите категорию --";
            foreach (var node in Purser.CategoryList)
               if (node.Key != "")
                  CB.Items.Add(node.Key);
         }
         else
         {
            CB.IsEnabled = false;
            DefaultNameComboBox.Content = "";
            RangeSlider.Minimum = 0;
            RangeSlider.Maximum = 20000;
         }

         RangeSlider.Minimum = Purser.Minimum;
         RangeSlider.Maximum = Purser.Maximum;
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

      bool flag = true;
      private void CB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         if (flag) { flag = false; return; }
         Find(CB.SelectedItem.ToString());
         flag = true;
         CB.Items.Clear();
         CB.IsEnabled = false;
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

      public static int minPrice = 0;
      public static int maxPrice = 200;

      private void RangeSlider_LowerValueChanged(object sender, MahApps.Metro.Controls.RangeParameterChangedEventArgs e)
      {
         minPrice = (int)e.NewValue;
      }

      private void RangeSlider_UpperValueChanged(object sender, MahApps.Metro.Controls.RangeParameterChangedEventArgs e)
      {
         maxPrice = (int)e.NewValue;
      }

      private void Flyout_ClosingFinished(object sender, RoutedEventArgs e)
      {
         cfg.hour = TimePicker?.SelectedTime != null ? (int)(TimePicker?.SelectedTime.Value.Hours) : 0;
         cfg.minute = TimePicker?.SelectedTime != null ? (int)(TimePicker?.SelectedTime.Value.Minutes) : 0;
         
         cfg.recipientOfLetters = mailSend.Text;

         SaverSettings.Save();
      }
   }
}