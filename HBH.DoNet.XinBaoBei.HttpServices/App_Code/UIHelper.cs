using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

/// <summary>
/// UIHelper 的摘要说明
/// </summary>
public class UIHelper
{
	public UIHelper()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


    public static string GetParam(Page page, string keyName)
    {
       
        return page.Request[keyName];
    }

    public static int GetInt(object obj)
    {
        int lResult;
        bool bllTmp;
        if (int.TryParse(GetString(obj), out lResult))
        {
            return lResult;
        }
        else if (bool.TryParse(GetString(obj), out bllTmp))
        {
            lResult = Convert.ToInt32(bllTmp);
            return lResult;
        }
        //return (long)long.MinValue;
        return (int)-1;
    }

    public static long GetLong(object obj)
    {
        long lResult;
        bool bllTmp;
        if (long.TryParse(GetString(obj), out lResult))
        {
            return lResult;
        }
        else if (bool.TryParse(GetString(obj), out bllTmp))
        {
            lResult = Convert.ToInt64(bllTmp);
            return lResult;
        }
        //return (long)long.MinValue;
        return (long)-1;
    }
    public static bool GetBool(object obj)
    {
        bool result = false;

        bool.TryParse(GetString(obj), out result);

        return result;
    }

    public static DateTime GetDate(object obj)
    {
        try
        {
            if (obj != null)
            {
                return Convert.ToDateTime(obj);
            }
        }
        catch (Exception ex)
        {
            //return DateTime.MinValue;
        }
        return DateTime.MinValue;
    }

    public static string GetString(object obj)
    {
        if (obj != null)
        {
            string old = obj.ToString();

            //string strNew = new String(old.getBytes("gbk"), "utf-8");

            string strNew = old;        //       UTF8ToGB2312(old);

            return strNew;
        }

        return string.Empty;
    }

    public static bool IsNull(string str)
    {
        if (str != null
            && str.Length > 0
            )
        {
            return false;   
        }
        return true;
    }

    //private const string Const_CHEncoding = "gb2312";
    private const string Const_CHEncoding = "gbk";

    public static string UTF8ToGB2312(string str)
    {
        try
        {
            Encoding utf8 = Encoding.GetEncoding(65001);
            Encoding gb2312 = Encoding.GetEncoding(Const_CHEncoding);//Encoding.Default ,936
            byte[] temp = utf8.GetBytes(str);
            byte[] temp1 = Encoding.Convert(utf8, gb2312, temp);
            string result = gb2312.GetString(temp1);
            return result;
        }
        catch (Exception ex)//(UnsupportedEncodingException ex)
        {
            // MessageBox.Show(ex.ToString());
            // return null;

            throw ex;
        }
    }

    public static string GB2312ToUTF8(string str)
    {
        try
        {
            Encoding uft8 = Encoding.GetEncoding(65001);
            Encoding gb2312 = Encoding.GetEncoding(Const_CHEncoding);
            byte[] temp = gb2312.GetBytes(str);
            //MessageBox.Show("gb2312的编码的字节个数：" + temp.Length);
            //for (int i = 0; i < temp.Length; i++)
            //{
            //    MessageBox.Show(Convert.ToUInt16(temp[i]).ToString());
            //}
            byte[] temp1 = Encoding.Convert(gb2312, uft8, temp);
            //MessageBox.Show("uft8的编码的字节个数：" + temp1.Length);
            //for (int i = 0; i < temp1.Length; i++)
            //{
            //    MessageBox.Show(Convert.ToUInt16(temp1[i]).ToString());
            //}
            string result = uft8.GetString(temp1);
            return result;
        }
        catch (Exception ex)//(UnsupportedEncodingException ex)
        {
            //MessageBox.Show(ex.ToString());
            //return null;

            throw ex;
        }
    }


    public static string CharsetChanged(string str,string src,string target)
    {
        try
        {
            Encoding targetEncode = Encoding.GetEncoding(src);
            Encoding srcEncode = Encoding.GetEncoding(target);
            byte[] temp = srcEncode.GetBytes(str);
            //MessageBox.Show("gb2312的编码的字节个数：" + temp.Length);
            //for (int i = 0; i < temp.Length; i++)
            //{
            //    MessageBox.Show(Convert.ToUInt16(temp[i]).ToString());
            //}
            byte[] temp1 = Encoding.Convert(srcEncode, targetEncode, temp);
            //MessageBox.Show("uft8的编码的字节个数：" + temp1.Length);
            //for (int i = 0; i < temp1.Length; i++)
            //{
            //    MessageBox.Show(Convert.ToUInt16(temp1[i]).ToString());
            //}
            string result = targetEncode.GetString(temp1);
            return result;
        }
        catch (Exception ex)//(UnsupportedEncodingException ex)
        {
            //MessageBox.Show(ex.ToString());
            //return null;

            throw ex;
        }
    }
}
