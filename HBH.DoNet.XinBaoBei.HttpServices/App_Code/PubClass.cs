using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Collections.Generic;

/// <summary>
///PubClass 的摘要说明
/// </summary>
public class PubClass
{
    public PubClass()
    {
    }
    #region 字段转化

    //
    //TODO: 在此处添加构造函数逻辑
    //


    //将字符串转换为指定类型
    /// <summary>
    /// 将字符串转换为指定类型;
    /// </summary>
    /// <param name="objValue">字符串的值</param>
    /// <param name="fieldType">类型</param>
    /// <returns>转换结果</returns>
    public static object GetObjFromString(object objValue, string fieldType)
    {
        if (fieldType != null)
        {
            string strValue = objValue.ToString().Trim();
            switch (fieldType.Trim().ToLower())
            {
                case "string":
                    if (objValue != null)
                    {
                        return objValue.ToString().Trim();
                    }
                    return string.Empty;
                    break;
                case "short":
                    short stResult;
                    bool blstTmp;
                    if (short.TryParse(strValue, out stResult))
                    {
                        return stResult;
                    }
                    else if (bool.TryParse(strValue, out blstTmp))
                    {
                        stResult = Convert.ToInt16(blstTmp);
                        return stResult;
                    }
                    return (short)short.MinValue;
                    break;
                case "int":
                    int iResult;
                    bool bliTmp;
                    if (int.TryParse(strValue, out iResult))
                    {
                        return iResult;
                    }
                    else if (bool.TryParse(strValue, out bliTmp))
                    {
                        iResult = Convert.ToInt32(bliTmp);
                        return iResult;
                    }
                    return (int)int.MinValue;
                    break;
                case "long":
                    long lResult;
                    bool bllTmp;
                    if (long.TryParse(strValue, out lResult))
                    {
                        return lResult;
                    }
                    else if (bool.TryParse(strValue, out bllTmp))
                    {
                        lResult = Convert.ToInt64(bllTmp);
                        return lResult;
                    }
                    return (long)long.MinValue;
                    break;
                case "bool":
                    bool blResult;
                    if (bool.TryParse(strValue, out blResult))
                    {
                        return blResult;
                    }
                    return false;
                    break;
                case "char":
                    char crResult;
                    if (char.TryParse(strValue, out crResult))
                    {
                        return crResult;
                    }
                    return char.MinValue;
                    break;
                case "decimal":
                    decimal decResult;
                    if (decimal.TryParse(strValue, out decResult))
                    {
                        return decResult;
                    }
                    return decimal.Zero;
                    break;
                case "double":
                    double douResult;
                    if (double.TryParse(strValue, out douResult))
                    {
                        return douResult;
                    }
                    return (double)0;
                    break;
                case "float":
                    float fltResult;
                    if (float.TryParse(strValue, out fltResult))
                    {
                        return fltResult;
                    }
                    return (float)0;
                    break;
                case "null":
                    return objValue;
                    break;
                default:
                    return objValue;
                    break;
            }
        }
        //没有设置类型，返回传入参数
        return objValue;
    }

    public static string GetString(object obj)
    {
        if (obj != null)
            return obj.ToString().Trim();

        return string.Empty;
    }

    public static short GetShort(object obj)
    {
        short result = short.MinValue;
        bool blstTmp;
        string strValue = GetString(obj);

        if (!short.TryParse(strValue, out result))
        {
            if (bool.TryParse(strValue, out blstTmp))
            {
                result = Convert.ToInt16(blstTmp);
            }
        }

        return result;
    }

    public static int GetInt(object obj)
    {
        //int result = int.MinValue;
        int result = 0;
        bool blstTmp;
        string strValue = GetString(obj);

        if (!int.TryParse(GetString(obj), out result))
        {
            if (bool.TryParse(strValue, out blstTmp))
            {
                result = Convert.ToInt32(blstTmp);
            }
        }

        return result;
    }

    public static long GetLong(object obj)
    {
        long result = long.MinValue;
        bool blstTmp;
        string strValue = GetString(obj);

        if (!long.TryParse(GetString(obj), out result))
        {
            if (bool.TryParse(strValue, out blstTmp))
            {
                result = Convert.ToInt32(blstTmp);
            }
        }

        return result;
    }

    public static bool GetBool(object obj)
    {
        bool result = false;

        string str = GetString(obj);
        // 如果不是bool型的，那么按数字来，大于0 true，小于零 ，false
        if (!bool.TryParse(str, out result))
        {
            int single = -1;
            if (int.TryParse(str, out single))
            {
                result = single > 0 ? true : false;//将字符型转换为布尔型。
            }
        }

        return result;
    }

    public static decimal GetDecimal(object obj)
    {
        //decimal result = decimal.MinValue;
        decimal result = 0;

        string str = GetString(obj);

        // 如果是百分号结尾,那么去掉百分号  再除以100 ;
        if (str.Trim().EndsWith("%"))
        {
            str = str.Trim().TrimEnd('%');
            decimal.TryParse(str, out result);

            result = result / 100;
        }
        // 否则正常转换
        else
        {
            decimal.TryParse(str, out result);
        }

        return result;
    }

    public static double GetDouble(object obj)
    {
        double result = double.MinValue;

        //double.TryParse(GetString(obj), out result);

        string str = GetString(obj);

        // 如果是百分号结尾,那么去掉百分号  再除以100 ;
        if (str.Trim().EndsWith("%"))
        {
            str = str.Trim().TrimEnd('%');
            double.TryParse(str, out result);

            result = result / 100;
        }
        // 否则正常转换
        else
        {
            double.TryParse(str, out result);
        }


        return result;
    }

    public static float GetFloat(object obj)
    {
        float result = float.MinValue;

        //float.TryParse(GetString(obj), out result);

        string str = GetString(obj);

        // 如果是百分号结尾,那么去掉百分号  再除以100 ;
        if (str.Trim().EndsWith("%"))
        {
            str = str.Trim().TrimEnd('%');
            float.TryParse(str, out result);

            result = result / 100;
        }
        // 否则正常转换
        else
        {
            float.TryParse(str, out result);
        }

        return result;
    }

    public static DateTime GetDateTime(object obj, DateTime defaultValue)
    {
        if (obj != null)
        {
            DateTime dt;
            if (DateTime.TryParse(obj.ToString(), out dt)
                )
            {
                return dt;
            }
        }
        return defaultValue;
    }

    public static DateTime? GetDateTime(object obj)
    {
        if (obj != null)
        {
            DateTime dt;
            if (DateTime.TryParse(obj.ToString(), out dt)
                )
            {
                return dt;
            }
        }
        return null;
    }

    // 默认格式的日期
    /// <summary>
    /// 返回默认格式的日期: "yyyy-MM-dd hh:mm:ss"
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string GetStringFromDate(DateTime dt)
    {
        if (dt != null)
        {
            return dt.ToString("yyyy-MM-dd hh:mm:ss");
        }
        return string.Empty;
    }

    // 数字变字符串(去掉无用的0)
    /// <summary>
    ///  数字变字符串(去掉无用的0)
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static string GetStringRemoveZero(decimal d)
    {
        return d.ToString("G0");
    }

    #endregion

    public static bool IsNull(string value)
    {
        //if (str == null || str.Trim() == string.Empty)
        //    return true;

        //return false;

        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool IsNullDate2010(DateTime dt)
    {
        if (dt != null
            && dt.Year > 2010
            )
        {
            return false;
        }
        return true;
    }

    public static Enum TryParse(string value, Enum defaultValue)
    {
        try
        {
            Enum em = (Enum)Enum.Parse(defaultValue.GetType(), value);
            return em;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static int DayDiff(DateTime dt1, DateTime dt2)
    {
        TimeSpan span = dt1 - dt2;

        return span.Days;
    }

    public static string GetOpathFromList(List<long> list)
    {
        StringBuilder sbIDs = new StringBuilder();

        if (list != null
            && list.Count > 0
            )
        {
            foreach (long id in list)
            {
                sbIDs.Append(id).Append(",");
            }
        }

        if (sbIDs.Length > 0)
        {
            sbIDs.Remove(sbIDs.Length - 1, 1);
        }
        return sbIDs.ToString();
    }

    public static string GetOpathFromList(long[] list)
    {
        StringBuilder sbIDs = new StringBuilder();

        if (list != null
            && list.Length > 0
            )
        {
            foreach (long id in list)
            {
                sbIDs.Append(id).Append(",");
            }
        }

        if (sbIDs.Length > 0)
        {
            sbIDs.Remove(sbIDs.Length - 1, 1);
        }
        return sbIDs.ToString();
    }

    public static string GetOpathFromList(List<string> list, string prevSeparator, string suffixSeparator, string split)
    {
        StringBuilder sbOpath = new StringBuilder();

        if (list != null
            && list.Count > 0
            )
        {
            foreach (string str in list)
            {
                //sbIDs.Append(str).Append(",");
                sbOpath.Append(prevSeparator).Append(str).Append(suffixSeparator).Append(split);
            }
        }

        if (sbOpath.Length > 0
            && !IsNull(split)
            )
        {
            sbOpath.Remove(sbOpath.Length - split.Length, 1);
        }
        return sbOpath.ToString();
    }

    public static string GetOpathFromList(long[] list, string prevSeparator, string suffixSeparator, string split)
    {
        StringBuilder sbOpath = new StringBuilder();

        if (list != null
            && list.Length > 0
            )
        {
            foreach (long str in list)
            {
                //sbIDs.Append(str).Append(",");
                sbOpath.Append(prevSeparator).Append(str).Append(suffixSeparator).Append(split);
            }
        }

        if (sbOpath.Length > 0
            && !IsNull(split)
            )
        {
            sbOpath.Remove(sbOpath.Length - split.Length, 1);
        }
        return sbOpath.ToString();
    }

    public static string GetOpathFromList(string[] list, string prevSeparator, string suffixSeparator, string split)
    {
        StringBuilder sbOpath = new StringBuilder();

        if (list != null
            && list.Length > 0
            )
        {
            foreach (string str in list)
            {
                //sbIDs.Append(str).Append(",");
                sbOpath.Append(prevSeparator).Append(str).Append(suffixSeparator).Append(split);
            }
        }

        if (sbOpath.Length > 0
            && !IsNull(split)
            )
        {
            sbOpath.Remove(sbOpath.Length - split.Length, 1);
        }
        return sbOpath.ToString();
    }


    public static string GetStringRemoveLastSplit(StringBuilder sbList)
    {
        if (sbList != null
            && sbList.Length > 0
            )
        {
            sbList.Remove(sbList.Length - 1, 1);

            return sbList.ToString();
        }
        return string.Empty;
    }


    public static DateTime GetWeekUpOfDate(DateTime dt, DayOfWeek weekday, int Number)
    {
        int wd1 = (int)weekday;
        int wd2 = (int)dt.DayOfWeek;
        return wd2 == wd1 ? dt.AddDays(7 * Number) : dt.AddDays(7 * Number - wd2 + wd1);
    }

    public static DataTable GetTableOfFirst(DataSet ds)
    {
        if (ds != null
            && ds.Tables != null
            && ds.Tables.Count > 0
            )
        {
            return ds.Tables[0];
        }
        return null;
    }

    public static DataTable GetTable(DataSet ds, int index)
    {
        if (ds != null
            && ds.Tables != null
            && ds.Tables.Count >= index + 1
            )
        {
            return ds.Tables[index];
        }
        return null;
    }


}

public class Config
{
    // SMS短信验证码 URL
    private static string _smsUrl;
    /// <summary>
    /// SMS短信验证码 URL
    /// </summary>
    public static string SmsUrl
    {
        get
        {
            if (PubClass.IsNull(_smsUrl))
            {
                _smsUrl = UIHelper.GetString(ConfigurationManager.AppSettings["smsUrl"]);
            }
            return _smsUrl;
        }
        set
        {
            _smsUrl = value;
        }
    }

    // SMS短信验证内容
    private static string _smsContent;
    /// <summary>
    /// SMS短信验证内容
    /// </summary>
    public static string SmsContent
    {
        get
        {
            if (PubClass.IsNull(_smsContent))
            {
                _smsContent = UIHelper.GetString(ConfigurationManager.AppSettings["smsContent"]);
            }
            return _smsContent;
        }
        set
        {
            _smsContent = value;
        }
    }


    // SMS短信验证账号
    private static string _smsAccount;
    /// <summary>
    /// SMS短信验证账号
    /// </summary>
    public static string SmsAccount
    {
        get
        {
            if (PubClass.IsNull(_smsAccount))
            {
                _smsAccount = UIHelper.GetString(ConfigurationManager.AppSettings["smsAccount"]);
            }
            return _smsAccount;
        }
        set
        {
            _smsAccount = value;
        }
    }


    // SMS短信验证密码
    private static string _smsPassword;
    /// <summary>
    /// SMS短信验证密码
    /// </summary>
    public static string SmsPassword
    {
        get
        {
            if (PubClass.IsNull(_smsPassword))
            {
                _smsPassword = UIHelper.GetString(ConfigurationManager.AppSettings["smsPassword"]);
            }
            return _smsPassword;
        }
        set
        {
            _smsPassword = value;
        }
    }


    // SMS短信验证码从(最小数字)
    private static int _smsIdentifyMin;
    /// <summary>
    /// SMS短信验证码从(最小数字)
    /// </summary>
    public static int SmsIdentifyMin
    {
        get
        {
            if (_smsIdentifyMin <= 0)
            {
                int num = UIHelper.GetInt(ConfigurationManager.AppSettings["smsIdentifyMin"]);

                if (num > 0)
                {
                    _smsIdentifyMin = num;
                }
                else
                {
                    _smsIdentifyMin = 1000;
                }
            }
            return _smsIdentifyMin;
        }
        set
        {
            _smsIdentifyMin = value;
        }
    }


    // SMS短信验证码到(最大数字)
    private static int _smsIdentifyMax;
    /// <summary>
    /// SMS短信验证码到(最大数字)
    /// </summary>
    public static int SmsIdentifyMax
    {
        get
        {
            if (_smsIdentifyMax <= 0)
            {
                int num = UIHelper.GetInt(ConfigurationManager.AppSettings["smsIdentifyMax"]);

                if (num > 0)
                {
                    _smsIdentifyMax = num;
                }
                else
                {
                    _smsIdentifyMax = 9999;
                }
            }
            return _smsIdentifyMax;
        }
        set
        {
            _smsIdentifyMax = value;
        }
    }


    // SMS短信验证码 过期时间(分钟) , 10分钟
    private static int _smsOverDueMinute;
    /// <summary>
    /// SMS短信验证码 过期时间(分钟) , 10分钟
    /// </summary>
    public static int SmsOverDueMinute
    {
        get
        {
            if (_smsOverDueMinute <= 0)
            {
                int num = UIHelper.GetInt(ConfigurationManager.AppSettings["smsOverDueMinute"]);

                if (num > 0)
                {
                    _smsOverDueMinute = num;
                }
                else
                {
                    _smsOverDueMinute = 10;
                }
            }
            return _smsOverDueMinute;
        }
        set
        {
            _smsOverDueMinute = value;
        }
    }
}
