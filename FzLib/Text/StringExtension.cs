namespace FzLib.Text
{
    public static class StringExtension
    {
        /// <summary>
        /// 移除字符串尾部指定后缀
        /// </summary>
        public static string RemoveEnd(this string str, string suffix)
        {
            if (str == null || suffix == null || suffix.Length == 0)
            {
                return str;
            }

            int suffixLen = suffix.Length;
            if (suffixLen > str.Length)
            {
                return str;
            }

            // 计算尾部匹配的起始索引
            int startIndex = str.Length - suffixLen;

            // 逐字符对比尾部
            for (int i = 0; i < suffixLen; i++)
            {
                if (str[startIndex + i] != suffix[i])
                {
                    return str;
                }
            }

            // 确认匹配后，返回移除后的部分
            return str.Remove(startIndex);
        }

        /// <summary>
        /// 移除字符串头部指定前缀
        /// </summary>
        public static string RemoveStart(this string str, string prefix)
        {
            if (str == null || prefix == null || prefix.Length == 0)
            {
                return str;
            }

            if (prefix.Length > str.Length)
            {
                return str;
            }

            // 逐字符对比头部
            for (int i = 0; i < prefix.Length; i++)
            {
                if (str[i] != prefix[i])
                {
                    return str;
                }
            }

            // 确认匹配后，返回剩余部分
            return str.Substring(prefix.Length);
        }

        public static string[] SplitLines(this string str, bool removeEmptyLine = true)
        {
            return str.Split(["\r\n", "\n", "\r"],
                removeEmptyLine ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }
    }
}