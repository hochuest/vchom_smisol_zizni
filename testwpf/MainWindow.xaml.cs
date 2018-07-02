﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel; // для ObservableCollection

using testwpf.whiskas;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace testwpf
{
   public partial class MainWindow
   {
      static public ObservableCollection<Product> listProduct = new ObservableCollection<Product>();
      static public Settings cfg = new Settings();
      static Dictionary<string, string> ID = new Dictionary<string, string>(16);

      public MainWindow()
      {
         InitializeComponent();

         cfg.findProduct = "Philips";
         cfg.categoryId = "54915";

         cfg.mailLogin = "nim20101@yandex.ru";
         cfg.mailPass = "madama98";

         cfg.recipientOfLetters = "nim20101@yandex.ru";

         cfg.message_header = "заголовок сообщения";
         cfg.letter_subject = "тема сообщения";

         cfg.before_the_message = "Поступили новые товары: \n";
         cfg.after_the_message = " (c) Система оповещений о новых товарах";

         cfg.hour = 15;
         cfg.minute = 09;

         getID.Go(CB, ID);

         // фоновый режим
         BackgroundMode.Start(Purser.Start, UseDB.GetList, UseDB.addDB, SendMail.Send);

         // пурсер
         listProduct = new ObservableCollection<Product>(Purser.Start()); // через раз парсит

         // заполнение таблицы
         DataGrid1.ItemsSource = listProduct;

         // иконка в трее
         IconTray.InitializeNotifyIcon(this, "tree.ico", new ToolStripItem[] { });

         // пример добавления в таблицу: listProduct.Add(new Product("kek1", "kek2", "kek3"));
         // ...
      }

      // events

      void OnStateChanged(object sender, EventArgs args)
      {
         IconTray.ev(args);
      }

      private void CB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         cfg.findProduct = Prod.Text;
         cfg.categoryId = ID[CB.SelectedValue.ToString()];

         listProduct = new ObservableCollection<Product>(Purser.Start());
      }

      private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
      {

      }
   }
}