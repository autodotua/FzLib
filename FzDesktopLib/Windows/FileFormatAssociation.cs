using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using FzLib.Program;

namespace FzLib.Windows
{
    //public class FileFotmatAssociation
    //{
    //    public static void Associate(string extension, string programName, string description, string iconFilePath)
    //    {
    //        Associate(extension, programName, description, iconFilePath, WpfCodes.Program.Information.ProgramFilePath);
    //    }
    //    public static void Associate(string extension, string programName, string description, string iconFilePath, string programPath)
    //    {

    //        if (string.IsNullOrEmpty(extension))
    //        {
    //            throw new ArgumentNullException(nameof(extension));
    //        }
    //        if (string.IsNullOrEmpty(programName))
    //        {
    //            throw new ArgumentNullException(nameof(programName));
    //        }
    //        if (string.IsNullOrEmpty(programPath))
    //        {
    //            throw new ArgumentNullException(nameof(programPath));
    //        }
    //        Registry.ClassesRoot.CreateSubKey(extension).SetValue("", programName);
    //        using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(programName))
    //        {
    //            if (description != null)
    //            {
    //                key.SetValue("", description);
    //            }
    //            if (iconFilePath != null)
    //            {
    //                key.CreateSubKey("DefaultIcon").SetValue("", GetShortPathName(iconFilePath));
    //            }
    //            key.CreateSubKey(@"Shell\Open\Command").SetValue("", GetShortPathName(programPath) + " \"%1\"");
    //        }
    //    }

    //    // Return true if extension already associated in registry
    //    public static bool IsAssociated(string extension)
    //    {
    //        return Registry.ClassesRoot.OpenSubKey(extension, false) != null;
    //    }


    //}


    //public class FileAssociationParas
    //{
    //    public string Extension { get; set; }
    //    public string ProgId { get; set; }
    //    public string FileTypeDescription { get; set; }
    //    public string ExecutableFilePath { get; set; }
    //}

    public class FileFormatAssociation
    {
        // needed so that Explorer windows get refreshed after the registry is updated
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;


        public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string iconPath)
        {
            return SetAssociation(extension, progId, fileTypeDescription, iconPath, Information.ProgramFilePath);
        }
        public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string iconPath = null, string applicationFilePath = null)
        {
            if (applicationFilePath == null)
            {
                applicationFilePath = Information.ProgramFilePath;
            }
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            bool madeChanges = false;
            madeChanges |= SetDefaultValue(@"Software\Classes\" + extension, progId);
            madeChanges |= SetDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            madeChanges |= SetDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            if (iconPath != null)
            {
                madeChanges |= SetDefaultValue(@"Software\Classes\" + progId + "\\DefaultIcon", iconPath);
            }
            SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            return madeChanges;
        }

        public static void DeleteAssociation(string extension, string progId)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Classes"))
            {
                if (key.OpenSubKey(extension) != null)
                {
                    key.OpenSubKey(extension, true).DeleteValue(null);
                }
                if (key.OpenSubKey(progId) != null)
                {
                    key.DeleteSubKeyTree(progId);
                }
            }
            //    madeChanges |= SetDefaultValue(@"Software\Classes\" + extension, progId);
            //madeChanges |= SetDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            //madeChanges |= SetDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            //madeChanges |= SetDefaultValue(@"Software\Classes\" + progId + "\\DefaultIcon", iconPath);
            SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool IsAssociated(string extension, string progId)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            bool yes = true;
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Classes"))
            {
                if (!(key.OpenSubKey(extension) != null && key.OpenSubKey(extension) != null && key.OpenSubKey(extension).GetValue(null) as string == progId))
                {
                    yes = false;
                }
                if (key.OpenSubKey(progId) == null)
                {
                    yes = false;
                }
            }
            return yes;
        }

        private static bool SetDefaultValue(string keyPath, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }
    }
}
