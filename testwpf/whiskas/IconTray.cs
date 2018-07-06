using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testwpf.whiskas
{
   static class IconTray
   {
      static private NotifyIcon m_notifyIcon;
      static private MainWindow mw;

      static public void InitializeNotifyIcon(MainWindow mwNew, string pathIcoFile, ToolStripItem[] menuList)
      {
         mw = mwNew;
         m_notifyIcon = new NotifyIcon();

         m_notifyIcon.Text = "test3";
         if ( File.Exists(pathIcoFile) )
            m_notifyIcon.Icon = new Icon(pathIcoFile);
         m_notifyIcon.MouseClick += new MouseEventHandler(m_notifyIcon_MouseClick);

         m_notifyIcon.ContextMenuStrip = new ContextMenuStrip();
         m_notifyIcon.ContextMenuStrip.Items.AddRange(menuList);
      }

      // events

      static private WindowState m_storedWindowState = WindowState.Normal;

      static public void ev(EventArgs args)
      {
         if (m_notifyIcon.Icon == null)
            return;

         if (mw.WindowState == WindowState.Minimized)
            mw.Hide();
         else
            m_storedWindowState = mw.WindowState;

         if (m_notifyIcon != null)
            m_notifyIcon.Visible = !mw.IsVisible;
      }

      static void m_notifyIcon_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            mw.Show();
            mw.WindowState = m_storedWindowState;
         }
      }

   }
}
