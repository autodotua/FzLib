using FzLib.Extension;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace FzLib.Program.Notify
{
    public class TrayIcon : IDisposable
    {
        NotifyIcon trayIcon = new NotifyIcon();


        private TrayIcon()
        {
            trayIcon.MouseClick += (p1, p2) =>
              {
                  if (p2.Button == MouseButtons.Left)
                  {
                      MouseLeftClick?.Invoke(p1, p2);
                  }
                  else if (p2.Button == MouseButtons.Right)
                  {
                      MouseRightClick?.Invoke(p1, p2);
                  }
              };
        }

        public TrayIcon(System.Drawing.Icon icon, string text) : this()
        {
            trayIcon.Text = text;
            trayIcon.Icon = icon;
        }

        public TrayIcon(System.Drawing.Icon icon, string text, MouseEventHandler mouseClick) : this(icon, text)
        {
            if (mouseClick != null)
            {
                trayIcon.MouseClick += mouseClick;
            }

        }

        public TrayIcon(System.Drawing.Icon icon, string text, Action mouseLeftClick, Action mouseRightClick) : this(icon, text)
        {
            if (mouseLeftClick != null || mouseRightClick != null)
            {
                trayIcon.MouseClick += (p1, p2) =>
                 {
                     if (p2.Button == MouseButtons.Left)
                     {
                         mouseLeftClick?.Invoke();
                     }
                     else if (p2.Button == MouseButtons.Right)
                     {
                         mouseRightClick?.Invoke();
                     }
                 };
            }
        }

        public TrayIcon(System.Drawing.Icon icon, string text, Action mouseLeftClick, Dictionary<string, Action> mouseRightClickMenu) : this(icon, text)
        {
            if (mouseLeftClick != null)
            {
                trayIcon.MouseClick += (p1, p2) =>
                {
                    if (p2.Button == MouseButtons.Left)
                    {
                        mouseLeftClick?.Invoke();
                    }

                };
            }

            if (mouseRightClickMenu != null && mouseRightClickMenu.Count != 0)
            {
                AddContextMenuItems(mouseRightClickMenu);

            }

        }

        public void AddContextMenuItems(Dictionary<string, Action> mouseRightClickMenu)
        {
            if (mouseRightClickMenu == null)
            {
                throw new ArgumentNullException();
            }
            if (mouseRightClickMenu.Count == 0)
            {
                return;
            }
            if (trayIcon.ContextMenu == null)
            {
                trayIcon.ContextMenu = new ContextMenu();
            }
            foreach (var item in mouseRightClickMenu)
            {
                trayIcon.ContextMenu.MenuItems.Add(item.Key, new EventHandler((p1, p2) => item.Value()));
            }
        }
        public void AddContextMenuItem(string text, Action action)
        {
            if (text == null || action == null)
            {
                throw new ArgumentNullException();
            }
            if (trayIcon.ContextMenu == null)
            {
                trayIcon.ContextMenu = new ContextMenu();
            }
            trayIcon.ContextMenu.MenuItems.Add(text, new EventHandler((p1, p2) => action()));

        }
        public void InsertContextMenuItem(string text, Action action, int index)
        {
            if (text == null || action == null)
            {
                throw new ArgumentNullException();
            }
            if (trayIcon.ContextMenu == null)
            {
                trayIcon.ContextMenu = new ContextMenu();
            }
            trayIcon.ContextMenu.MenuItems.Add(index, new MenuItem(text, new EventHandler((p1, p2) => action())));

        }

        public void DeleteContextMenuItem(string text)
        {
            trayIcon.ContextMenu.MenuItems.RemoveByKey(text);
        }

        public void ClearContextMenuItems()
        {
            if (trayIcon.ContextMenu != null)
            {
                trayIcon.ContextMenu.MenuItems.Clear();
            }
        }

        public ContextMenu ContextMenu => trayIcon.ContextMenu;

        public void ClickToOpenOrHideWindow(Window window)
        {
            MouseLeftClick += (p1, p2) =>
            {
                if (window.Visibility != Visibility.Visible)
                {
                    window.Show();
                    window.Activate();
                }
                else
                {
                    window.Hide();
                }
            };
        }
        public void ClickToOpenOrHideWindow<T>(ISingleObject<T> obj) where T:Window,new()
        {
            MouseLeftClick += (p1, p2) =>
            {
                T window = null;
                if (obj.SingleObject != null)
                {
                    window = obj.SingleObject;
                    var propertyInfo = typeof(Window).GetProperty("IsDisposed", BindingFlags.NonPublic | BindingFlags.Instance);
                    var isDisposed = (bool)propertyInfo.GetValue(obj.SingleObject);
                    if (!isDisposed)
                    {
                        if (window.Visibility != Visibility.Visible)
                        {
                            window.Show();
                            window.Activate();
                        }
                        else
                        {
                            window.Hide();
                        }
                        return;
                    }
                }
                
                window = new T();
                obj.SingleObject= window;
                window.Show();
                
            };
        }

        public event MouseEventHandler MouseLeftClick;
        public event MouseEventHandler MouseRightClick;

        public void ShowMessage(string message, int ms = 2000)
        {
            trayIcon.BalloonTipText = message;
            trayIcon.ShowBalloonTip(ms);
        }
        public void Show()
        {
            trayIcon.Visible = true;
        }

        public void Hide()
        {
            trayIcon.Visible = false;
        }

        public void Dispose()
        {
            trayIcon.Dispose();
        }





        public void AddToManager(string key)
        {

        }
    }
}
