using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

using BOCOM.IVX.Properties;

namespace BOCOM.IVX.Common
{
    public class TextUtil
    {
        private const string STRING_REGEX_IP =
                @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
        private const string PATTERN_NAME = "[\\w\\d\\-\\.]{{{0},{1}}}"; //"[a-zA-Z0-9_\\-\\.]{{{0},{1}}}";

        public static string FormatExceptionMsg(Exception ex)
        {
            string ret = String.Format("{0},\n stack trace:{1}", ex.Message, ex.StackTrace) ;

            if (ex.InnerException != null)
            {
                ret = String.Format("{0}\n Inner Exception:{1} \n Stack Trace: {2}", ret, ex.InnerException, ex.InnerException.StackTrace);
            }
            return ret;
        }

        /// <summary>
        /// Return Non null String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetNonNullString(object obj)
        {
            string sRet = String.Empty;
            if (obj != null)
            {
                sRet = obj.ToString();
            }
            return sRet;
        }

        public static string GetSingleStringItem(StringCollection sc)
        {
            string sRet = null;
            if (sc != null && sc.Count > 0)
            {
                sRet = sc[0];
            }
            return sRet;
        }

        public static bool ValidateIPAddress(string ip)
        {
            bool bRet = false;
            if (!String.IsNullOrEmpty(ip))
            {
                Regex reg = new Regex(STRING_REGEX_IP);
                bRet = reg.IsMatch(ip);
            }
            return bRet;
        }

        public static bool ValidateIfEmptyString(string input, string name, out string msg)
        {
            bool bRet = !String.IsNullOrWhiteSpace(input);
            msg = string.Empty;

            if (!bRet)
            {
                msg = String.Format(Resources.TextUtil_Msg_InputCannotBeNull, name.TrimEnd(": ：".ToCharArray()));
            }

            return bRet;
        }

         static bool ValidateTextLength(string input, bool allowNullorEmpty, string name, int minLength, int maxLength, out string msg)
        {
            bool bRet = false;
            msg = null;
            if (minLength < 1 || maxLength < minLength)
            {
                throw new ArgumentException("Minimum length should be greater than 0, maximum length should not be less then minimu length");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null");
            }
            if (string.IsNullOrEmpty(input))
            {
                if (allowNullorEmpty)
                {
                    bRet = true;
                }
                else
                {
                    msg = String.Format(Resources.TextUtil_Msg_InputCannotBeNull, name.TrimEnd(": ：".ToCharArray()));
                }
            }
            else
            {
                bRet = true;
                int count = System.Text.ASCIIEncoding.Default.GetByteCount(input);
                if (count < minLength || count > maxLength)
                {
                    bRet = false;
                    msg = String.Format("{0} 长度必须介于 {1} - {2}", name, minLength, maxLength);
                }
            }
            return bRet;
        }

        public static bool ValidateNameText(ref string input, bool allowNullorEmpty, string name, int minLength, int maxLength, out string msg)
        {
            bool bRet = false;
            msg = null;
            if (minLength < 0 || maxLength < minLength)
            {
                throw new ArgumentException("Minimum length should be greater than 0, maximum length should not be less then minimu length");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null");
            }
            if (input == null || string.IsNullOrEmpty(input = input.Trim()))
            {
                if (allowNullorEmpty)
                {
                    bRet = true;
                }
                else
                {
                    msg = String.Format(Resources.TextUtil_Msg_InputCannotBeNull, name.TrimEnd(": ：".ToCharArray()));
                }
            }
            else
            {
                if (input.Length == 0 && !allowNullorEmpty)
                {
                    msg = String.Format(Resources.TextUtil_Msg_InputCannotBeNull, name.TrimEnd(": ：".ToCharArray()));
                }
                else
                {
                    int count = System.Text.ASCIIEncoding.Default.GetByteCount(input);
                    if (count < minLength || count > maxLength)
                    {
                        msg = String.Format(Resources.TextUtil_Msg_InvalidInput2, name.TrimEnd(": ：".ToCharArray()), minLength, maxLength);
                    }
                    else
                    { 
                        string pattern = String.Format(PATTERN_NAME, minLength, maxLength);
                        Regex regex = new Regex(pattern);
                        Match match;
                        if ((match = regex.Match(input)) != null &&
                            match.Length == input.Length)
                        {
                            bRet = true;
                        }
                        else
                        {
                            msg = String.Format(Resources.TextUtil_Msg_InvalidInput2, name.TrimEnd(": ：".ToCharArray()), minLength, maxLength);
                        }
                    }
                }
            }
            return bRet;
        }

        public static string GetNameWithIncreaseNO(string prefix, List<string> names, int startIndex)
        {
            if(string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("Prefix should not be null");
            }

            string nameRet = string.Format("{0}{1}", prefix, startIndex);
            if (names != null && names.Count > 0)
            {
                Regex regex = new Regex(string.Format("{0}[0-9]+", prefix));
                MatchCollection matches;
                Match match;
                List<int> ids = new List<int>();
                string number;

                // 过滤全部符合规则的名车， 并取出编号集合
                foreach (string name in names)
                {
                    matches = regex.Matches(name);
                    if (matches != null && matches.Count > 0 && matches[0].Length == name.Length)
                    {
                        number = name.Substring(prefix.Length);
                        // 忽略数字是零开头的
                        match = Regex.Match(number, "[1-9]+[0-9]*", RegexOptions.None);
                        if (match.Success && match.Value.Length == number.Length)
                        {
                            try
                            {
                                ids.Add(int.Parse(match.Value));
                            }
                            catch (OverflowException)
                            { }
                        }
                    }
                }
                                
                if (ids.Count > 0)
                {
                    // 从起始编号开始，递增找到未使用的编号
                    int startNO = 1;
                    int i = startIndex;
                    while (true)
                    {
                        if (!ids.Contains(i))
                        {
                            startNO = i;
                            break;
                        }
                        i++;
                    }
                    
                    nameRet = string.Format("{0}{1}", prefix, startNO);
                }
            }
            return nameRet;
        }

        public static string GetFormatedLastTime(uint time)
        {
            if (time == 0)
                return "已完成";

            TimeSpan ts = new TimeSpan(0, 0, (int)time);
            string strTime = "";
            if (ts.TotalHours > 1)
                strTime = string.Format("剩余：{0}小时{1}分{2}秒", ts.Hours, ts.Minutes, ts.Seconds);
            else if(ts.TotalMinutes >1)
                strTime = string.Format("剩余：{0}分{1}秒", ts.Minutes, ts.Seconds);
            else
                strTime = string.Format("剩余：{0}秒", ts.Seconds);

            return strTime;
        }
    }
}
