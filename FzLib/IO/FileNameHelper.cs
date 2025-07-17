
namespace FzLib.IO;

using System;
using System.IO;
using System.Text;

public static class FileNameHelper
{
    public static string GenerateUniquePath(string desiredPath, ISet<string> usedPaths,
        string suffixTemplate = " ({0})", int firstCounter = 2)
    {
        if (!usedPaths.Contains(desiredPath))
        {
            return desiredPath;
        }

        string dir = Path.GetDirectoryName(desiredPath);
        string name = Path.GetFileNameWithoutExtension(desiredPath);
        string ext = Path.GetExtension(desiredPath);

        int counter = firstCounter;
        string newPath;
        do
        {
            string suffix = string.Format(suffixTemplate, counter);
            newPath = Path.Combine(dir, $"{name}{suffix}{ext}");
            counter++;
        } while (usedPaths.Contains(newPath));

        return newPath;
    }

    public static string[] GetDirNames(string dirNamesFromFilePicker, bool checkExist = true)
    {
        string[] fileNames = dirNamesFromFilePicker.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        if (checkExist)
        {
            foreach (var fileName in fileNames)
            {
                if (!Directory.Exists(fileName))
                {
                    throw new FileNotFoundException($"目录{fileName}不存在");
                }
            }
        }

        return fileNames;
    }

    public static string[] GetFileNames(string fileNamesFromFilePicker, bool checkExist = true)
    {
        string[] fileNames = fileNamesFromFilePicker.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        if (checkExist)
        {
            foreach (var fileName in fileNames)
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException($"文件{fileName}不存在");
                }
            }
        }

        return fileNames;
    }

    public static string GetValidFileName(string name, string defaultName = "未命名")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var invalidChars = Path.GetInvalidFileNameChars();
        var builder = new StringBuilder(name.Length);
        bool hasInvalidChar = false;

        foreach (var c in name)
        {
            if (Array.IndexOf(invalidChars, c) >= 0)
            {
                builder.Append('_');
                hasInvalidChar = true;
            }
            else
            {
                builder.Append(c);
            }
        }

        if (!hasInvalidChar && name.Length <= 255 && name.Trim() == name && !name.EndsWith("."))
        {
            return name;
        }

        var validName = builder.ToString().Trim().TrimEnd('.');

        if (validName.Length > 255)
        {
            validName = validName[..255];
        }

        return string.IsNullOrWhiteSpace(validName) ? defaultName : validName;
    }
}