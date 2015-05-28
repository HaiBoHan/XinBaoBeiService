using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

/// <summary>
///HttpPostHelper 的摘要说明
/// </summary>
public class HttpPostHelper
{
    public HttpPostHelper()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }


    public static string QueryByHttpPost(string query)
    {
        // query = "select * from judge_user";
        string strParams = string.Empty;
        // string strParams = "Debug=1&Query=select * from judge_user where Account in ('001','0123')";

        // string strParams = string.Format("Query={0}", query);
        //string url = "http://localhost/PHPTest/QueryPost.php";
        //string url = string.Format("http://localhost/PHPTest/QueryPost.php?Query={0}", query);
        //string url = string.Format("http://localhost/PHPTest/utf8.php");
        //string url = string.Format("http://211.149.198.209/QueryPost.php?Debug=1&Query=select * from judge_user where Account in ('001','0123')");
        // string url = string.Format("http://211.149.198.209/QueryPost.php");


        // string url = string.Format("http://localhost/PHPTest/utf8.php");
        //string url = string.Format("http://211.149.198.209:8092/HttpPost.aspx");
        // string url = "http://localhost/HttpServicesTest/HttpPost.aspx";
        string url = string.Format("http://211.149.198.209/QueryPost.php?Query={0}", query);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        byte[] postDataStr = Encoding.UTF8.GetBytes(strParams);  // Encoding.Default.GetBytes(strParams);
        request.Method = "POST";

        request.ContentType = "text/html;charset=utf-8";  //  charset=GB2312

        request.ContentLength = postDataStr.Length;// Encoding.UTF8.GetByteCount(strParams);


        System.IO.Stream sm = request.GetRequestStream();
        sm.Write(postDataStr, 0, postDataStr.Length);
        sm.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream streamResponse = response.GetResponseStream();
        StreamReader streamRead = new StreamReader(streamResponse, System.Text.Encoding.GetEncoding("GB2312"));
        //Char[] readBuff = new Char[256];
        //int count = streamRead.Read(readBuff, 0, 256);

        ////result为http响应所返回的字符流  
        //String result = "";
        //while (count > 0)
        //{
        //    String outputData = new String(readBuff, 0, count);
        //    result += outputData;
        //    count = streamRead.Read(readBuff, 0, 256);
        //}

        //response.Close();

        //return result;


        string result = streamRead.ReadToEnd();

        return result;
    }
}