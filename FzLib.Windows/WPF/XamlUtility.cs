using System.IO;
using System.Windows.Markup;
using System.Xml;

//using Windows.UI.ViewManagement;

namespace FzLib.WPF
{
    public static class XamlUtility
    {
        public static T CloneXaml<T>(T source)
        {
            string xaml = XamlWriter.Save(source);
            StringReader sr = new StringReader(xaml);
            XmlReader xr = XmlReader.Create(sr);
            return (T)XamlReader.Load(xr);
        }
    }
}