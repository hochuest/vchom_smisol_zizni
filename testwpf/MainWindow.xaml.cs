using System;
using System.Windows.Forms; // !!!!! Лиза, wpf 
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel; // для ObservableCollection

using testwpf.whiskas;

namespace testwpf
{
   public partial class MainWindow
   {
      static public ObservableCollection<Product> listProduct = new ObservableCollection<Product>();

      public MainWindow()
      {
         InitializeComponent();

         // заполнение таблицы
         DataGrid1.ItemsSource = listProduct;

         // пурсер
         Purser.Start(); // через раз парсит, хз почему

         // иконка в трее
         IconTray.InitializeNotifyIcon(this, "tree.ico", new ToolStripItem[] { });

         // пример добавления в таблицу 
         // listProduct.Add(new Product("kek1", "kek2", "kek3"));

         //BackgroundMode.Start( ... , SendMail.Send);

         //...
      }



      // events

      void OnStateChanged(object sender, EventArgs args)
      {
         IconTray.ev(args);
      }
   }
}
