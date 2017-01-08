﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace SSMM.Helper
{
    public class FormatHelper
    {
        #region Html 编码 解码
        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return System.Web.HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// html编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return System.Web.HttpUtility.HtmlEncode(str);
        }
        #endregion

        #region URL 编码 解码
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string URLDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return System.Web.HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string URLEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return System.Web.HttpUtility.UrlEncode(str);
        }
        #endregion

        #region Json 序列化 反序列化
        /// <summary>
        /// Json 序列化
        /// </summary>
        public static string JsonSerializer(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json 反序列化
        /// </summary>
        public static T JsonDeserializer<T>(string jsonstr)
        {
            if (string.IsNullOrEmpty(jsonstr))
            {
                return default(T);
            }
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(jsonstr, jSetting);
        }
        #endregion

        #region 时间戳转换
        /// <summary>
        /// 时间戳 转 换时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// <summary>
        /// 时间 转换 时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion

        #region Base64

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            var result = "";
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64ByUTF8(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64ByUTF8(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        #endregion

        #region MD5
        /// <summary>
        /// 计算字符串MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5ByString(string str)
        {
            byte[] bs = Encoding.UTF8.GetBytes(str);
            using (MD5 md5 = MD5.Create())
            {
                bs = md5.ComputeHash(bs);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                sb.Append(bs[i].ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetMD5ByStream(Stream stream)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            arrbytHashValue = oMD5Hasher.ComputeHash(stream); //计算指定Stream 对象的哈希值
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            strHashData = System.BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;
            return strResult;
        }
        #endregion

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString("n");
        }

    }
}
