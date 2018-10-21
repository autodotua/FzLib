using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FzLib.Wpf.Program.Config
{
    public static class DotNetConfigHelper
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="name"></param>
        /// <param name="func">转换函数</param>
        /// <returns></returns>
        public static T Get<T>(this Configuration config, string name, Func<string, T> func)
        {
            return func(config.AppSettings.Settings[name].Value);
        }

        public static int GetInt(this Configuration config, string name)
        {
            return Get(config, name, p => int.Parse(p));
            //  return int.Parse(config.AppSettings.Settings[name].Value);
        }

        public static int GetInt(this Configuration config, string name, int defautValue)
        {
            try
            {
                return config.GetInt(name);
            }
            catch
            {
                return defautValue;
            }
        }
        public static long GetLong(this Configuration config, string name)
        {
            return Get(config, name, p => int.Parse(p));
            //  return int.Parse(config.AppSettings.Settings[name].Value);
        }

        public static long GetLong(this Configuration config, string name, long defautValue)
        {
            try
            {
                return config.GetLong(name);
            }
            catch
            {
                return defautValue;
            }
        }

        public static Double GetDouble(this Configuration config, string name)
        {
            return Get(config, name, p => double.Parse(p));
            //  return Double.Parse(config.AppSettings.Settings[name].Value);
        }

        public static Double GetDouble(this Configuration config, string name, double defautValue)
        {
            try
            {
                return config.GetDouble(name);
            }
            catch
            {
                return defautValue;
            }
        }


        public static string GetString(this Configuration config, string name)
        {
            return config.AppSettings.Settings[name].Value;
        }

        public static string GetString(this Configuration config, string name, string defautValue)
        {
            try
            {
                return config.GetString(name);
            }
            catch
            {
                return defautValue;
            }
        }

        public static bool GetBool(this Configuration config, string name)
        {
            return Get(config, name, p => bool.Parse(p));
        }

        public static bool GetBool(this Configuration config, string name, bool defautValue)
        {
            try
            {
                return config.GetBool(name);
            }
            catch
            {
                return defautValue;
            }
        }


        public static string GetTextBoxText(this Configuration config, TextBox txt, bool returnRawIfException = true, bool setToTextBoxDirectly = true)
        {
            string value;
            if (returnRawIfException)
            {
                try
                {
                    value= Get(config, txt.Name, p => p);
                    if(setToTextBoxDirectly)
                    {
                        txt.Text = value;
                    }
                    return value;
                }
                catch
                {
                    return txt.Text;
                }
            }
            else
            {
                value = Get(config, txt.Name, p => p);
                if (setToTextBoxDirectly)
                {
                    txt.Text = value;
                }
                return value;
            }
        }

        public static int GetComboBoxSelectedIndex(this Configuration config, ComboBox cbb, bool returnRawIfException = true, bool setToComboBoxDirectly = true)
        {
            int value;
            if (returnRawIfException)
            {
                try
                {
                    value = Get(config, cbb.Name, p =>int.Parse( p));
                    if (setToComboBoxDirectly)
                    {
                        cbb.SelectedIndex = value;
                    }
                    return value;
                }
                catch
                {
                    return cbb.SelectedIndex;
                }
            }
            else
            {
                value = Get(config, cbb.Name, p => int.Parse(p));
                if (setToComboBoxDirectly)
                {
                    cbb.SelectedIndex = value;
                }
                return value;
            }
        }





        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="func">转换函数</param>
        public static bool Set<T>(this Configuration config, string name, T value, Func<T, string> func, bool showException = false)
        {
            if (showException)
            {
                if (config.AppSettings.Settings[name] != null)
                {
                    config.AppSettings.Settings[name].Value = func(value);
                }
                else
                {
                    config.AppSettings.Settings.Add(name, func(value));
                }
                return true;
            }
            else
            {
                try
                {
                    config.Set(name, value, func, true);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool SetInt(this Configuration config, string name, int value)
        {
            return config.Set(name, value, p => p.ToString());
        }

        public static bool SetLong(this Configuration config, string name, long value)
        {
            return config.Set(name, value, p => p.ToString());
        }

        public static bool SetDouble(this Configuration config, string name, double value)
        {
            return config.Set(name, value, p => p.ToString());
        }

        public static bool SetBool(this Configuration config, string name, bool value)
        {
            return config.Set(name, value, p => p.ToString());
        }

        public static bool SetString(this Configuration config, string name, string value)
        {
            return config.Set(name, value, p => p);
        }

        public static bool SetTextBoxText(this Configuration config, TextBox txt)
        {
            return config.Set(txt.Name, txt.Text, p => p);
        }

        public static bool SetComboBoxSelectedIndex(this Configuration config, ComboBox cbb)
        {
            return config.Set(cbb.Name,cbb.SelectedIndex, p => p.ToString());
        }


        public static void SaveForce(this Configuration config)
        {
            string tempFile = Path.GetTempFileName();
            string path = config.FilePath;
            config.SaveAs(tempFile);
            File.Delete(path);

            File.Move(tempFile, path);
        }
    }
}
