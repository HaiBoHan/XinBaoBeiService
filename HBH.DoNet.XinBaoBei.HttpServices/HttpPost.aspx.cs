using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;

public partial class HttpPost : System.Web.UI.Page
{
    private const string Const_MethodName = "Method";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.ContentEncoding = System.Text.Encoding.GetEncoding("latin1");
        this.Response.ContentType = "text/html;charset=GB2312";


        string strMethod = UIHelper.GetParam(this, Const_MethodName);

        string result = string.Empty;
        switch (strMethod)
        {
            case "Login":
            case "GetUser":
                {

                    string strUserCode = UIHelper.GetParam(this, "Account");
                    string strPwd = UIHelper.GetParam(this, "pwd");
                    result = GetUser(strUserCode, strPwd);
                    //result = GetUser(strUserCode);

                }
                break;
            case "GetMeasure":
                {

                    result = GetMeasure();

                }
                break;
            case "GetBranch":
                {

                    result = GetBranch();

                }
                break;
            case "GetUnit":
                {

                    result = GetUnit();

                }
                break;

            case "SelectUser":
                {

                    string strUserCode = UIHelper.GetParam(this, "Account");
                    //string strPwd = UIHelper.GetParam(this, "pwd");
                    //result = GetUser(strUserCode, strPwd);
                    result = GetUser(strUserCode);

                }
                break;

            default:
                {
                    this.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    this.Response.ContentType = "text/html;charset=GB2312";

                    result = "无效的方法名!";
                }
                break;
        }

        this.Response.Write(result);
        this.Response.End();
    }
    /// <summary>
    /// 通过HTTPPost获取用户,PHP获取用户
    /// </summary>
    /// <param name="strAccount"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private string GetUserByPost(string strAccount, string password)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select u.id,u.account,passwd,u.name,sex, age,pic,u.branch_id,u.unit_id,b.branchname as branch_name from judge_user as u join judge_branch as b on b.id = u.branch_id where u.account ='{0}';";
        // select u.id,u.account,passwd,u.name,sex, age,pic,u.branch_id,u.unit_id,b.branchname as branch_name from judge_user as u join judge_branch as b on b.id = u.branch_id where u.account ='my001'
        if (!UIHelper.IsNull(strAccount))
        {
            // string where = string.Format(" account ='{0}'", strAccount);
            sql = string.Format(sql, strAccount);

        }
        else
        {
            sql = string.Format(sql, "");
        }
        //  string strQueryResult = HttpPostHelper.QueryByHttpPost(sql);


        try
        {
            string strQueryResult = HttpPostHelper.QueryByHttpPost(sql);

            List<Entities.User> list = Entities.User.GetUserFromPost(strQueryResult);
            // List<Entities.User> list = Entities.User.GetUserFromTable(ds);


            bool exist = false;
            if (list != null)
            {
                foreach (Entities.User us in list)
                {
                    if (us.Passwd == password)
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.ResultJson = strJson;
                    result.IsSuccess = true;
                    result.Message = "Login Success ! ";
                }
                else
                {
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.ResultJson = strJson;
                    result.IsSuccess = false;

                    result.Message = "Login failed, the password is not correct ! ";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "User does not exist ! ";
            }


        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;


    }

  
    /// <summary>
    /// 根据传入的编码得到登录用户
    /// </summary>
    /// <param name="strAccount">用户编码</param>
    /// <returns></returns>
    private string GetUser(string strAccount)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select id,account,passwd,name,sex,date_format(age,'%Y-%c-%d %H:%i:%s') as birthday,pic,tel,address,region from judge_user where account ='{0}' ;";
        sql = string.Format(sql, strAccount);


        List<Entities.User> list = EntityHelper.GetUserEntityBySQL(sql,SetOfBookType.Test);
        try
        {
           
           
            result.IsSuccess = true;
            if (list != null)
            {
                foreach (Entities.User us in list)
                {
                    us.Pic = "http://ceping.xinbaobeijiaoyu.com/measure/upload/" + us.Pic;
                    string newAge = EntityHelper.GetAgeByBirthday(Convert.ToDateTime(us.Birthday), DateTime.Now);
                    us.Age = System.Text.Encoding.GetEncoding("latin1").GetString(System.Text.Encoding.Default.GetBytes(newAge));
                }
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.ResultJson = strJson;
                result.IsSuccess = true;
                result.Message = "Login Success!";
            }
            else
            {
                result.Message = "查询结果为空";
            }

          
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }

    
    private string GetUser(string strAccount, string password)
    {

        PostHelper.PostResult result = new PostHelper.PostResult();

        // ,year(now())- year(age) as age
        string sql = "select u.id,u.account,passwd,u.name,sex, age as birthday,date_format(age,'%Y-%c-%d %H:%i:%s') as age ,pic,tel,u.branch_id,u.unit_id,b.branchname as branch_name,region,u.account as judge_user_account,address,-1 as sysversion,null as createdon,'' as createdby,null as modifiedon,'' as modifiedby from judge_user as u join judge_branch as b on b.id = u.branch_id where u.account ='{0}';";
        //string sql = "select id,account,passwd,name,sex,date_format(age,'%Y-%c-%d %H:%i:%s') as age,pic from judge_user where account ='{0}'  ; show variables like 'char%';";
        sql = string.Format(sql, strAccount);
        List<Entities.User> list =EntityHelper.GetUserEntityBySQL(sql,SetOfBookType.Test);

      
        try
        {
           
            bool exist = false;
            if (list != null)
            {
                foreach (Entities.User us in list)
                {
                    if (us.Passwd == password)
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    foreach (Entities.User us in list)
                    {
                        us.Pic = "http://ceping.xinbaobeijiaoyu.com/measure/upload/" + us.Pic;
                        string newAge = EntityHelper.GetAgeByBirthday(Convert.ToDateTime(us.Birthday), DateTime.Now);
                        us.Age = System.Text.Encoding.GetEncoding("latin1").GetString(System.Text.Encoding.Default.GetBytes(newAge));
                    }
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.ResultJson = strJson;
                    result.IsSuccess = true;
                    result.Message = "Login Success!";
                }
                else
                {
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                   // result.ResultJson = strJson;
                    
                    result.IsSuccess = false;

                    result.Message = "Login failed, the password is not correct!";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "User does not exist!";
            }

          
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }

    /// <summary>
    /// 得到所有的测评
    /// </summary>
    /// <returns></returns>
    private string GetMeasure()
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select m.id,m.measure_title,type.measure_typename AS measure_type ,measure_xml_path ,measure_result_xml_path from measure_table  as m join  measure_type as type on type.id = m.measure_type_id; show variables like 'char%';";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.Test);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.Measure> list = Entities.Measure.GetMeasureFromTable(ds);


            if (list != null)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query is Null";
            }

            if (ds.Tables.Count > 1)
            {

                List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

                string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                result.ResultJson2 = strJson2;
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;

    }


    private string GetBranch()
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select id,branchname from judge_branch";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.Test);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.JudgeBranch> list = Entities.JudgeBranch.GetJudgeBranchFromTable(ds);


            if (list != null)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query is Null";
            }

            if (ds.Tables.Count > 1)
            {

                List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

                string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                result.ResultJson2 = strJson2;
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;

    }

    private string GetUnit()
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select id,unitname from judge_unit";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.Test);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.JudgeUnit> list = Entities.JudgeUnit.GetJudgeUnitFromTable(ds);


            if (list != null)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query is Null";
            }

            if (ds.Tables.Count > 1)
            {

                List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

                string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                result.ResultJson2 = strJson2;
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;

    }
}

