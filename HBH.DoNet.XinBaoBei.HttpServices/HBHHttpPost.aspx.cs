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
using PostHelper;
using System.Text;
using System.Net;
using System.IO;
using Entities;

public partial class HBHHttpPost : System.Web.UI.Page
{
    private const string Const_MethodName = "Method";


    //private const string Const_UserQueryAccount = "select *,birthday as age,-1 as branch_id,-1 as unit_id,'' as branch_name,'' as hintmessage,'' as selfsign from hbh_user where account ='{0}';";
    //private const string Const_UserQueryID = "select *,birthday as age,-1 as branch_id,-1 as unit_id,'' as branch_name,'' as hintmessage,'' as selfsign  from hbh_user where id = {0};";
    //private const string Const_UsersIn = "select *,birthday as age,-1 as branch_id,-1 as unit_id,'' as branch_name,'' as hintmessage,'' as selfsign from hbh_user where  id in ({0}) ;";

    //private const string Const_UserQuerySelect = "select case when birthday < '1950-01-01' then null else concat(convert(birthday,char),'') end as age,ID,Account,Name,Region,Passwd,Sex,concat(convert(birthday,char),'') as birthday,Pic,Address,Tel,Judge_user_account,SysVersion,SelfSign,BabyName,Province,City,County,-1 as branch_id,-1 as unit_id,'' as branch_name,'' as hintmessage from hbh_user";
    // 把数据库的 datetime 类型都改为 VARCHAR(125) 了，那样就不报错了；在程序里转成日期;
    private const string Const_UserQuerySelect = "select *,birthday as age,-1 as branch_id,-1 as unit_id,'' as branch_name,'' as hintmessage,'' as selfsign from hbh_user";
    private const string Const_UserQueryAccount = Const_UserQuerySelect + " where (account='{0}' or tel='{0}');";
    private const string Const_UserQueryID = Const_UserQuerySelect + " where id = {0};";
    private const string Const_UsersIn = Const_UserQuerySelect + " where  id in ({0}) ;";
    private const string Const_UserQueryTel = Const_UserQuerySelect + " where tel = '{0}';";

    // 暂时设置成2000,客户端先不分页吧
    private const int ps = 2000;
    private const int pn = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        string strMethod = UIHelper.GetParam(this, Const_MethodName);

        string result = string.Empty;

        switch (strMethod)
        {
           
            case "GetMessage":
                {
              
                    long userID = Convert.ToInt64(UIHelper.GetParam(this, "userid"));
                    if(userID<=0)
                        result = "错识！没有用户！";
                    result = GetMessage(userID);

                }
                break;
            case "GetUser":
                {
                    // http://localhost:8012/HBHHttpPost.aspx?method=GetUser&Account=18001298823&IdentifyCode=72847
                    string strUserCode = UIHelper.GetParam(this, "Account");
                    string strPwd = UIHelper.GetParam(this, "pwd");
                    string strIdentifyCode = UIHelper.GetParam(this, "IdentifyCode");

                    //result = GetUser(strUserCode, strPwd, strIdentifyCode);

                    PostResult postResult = GetUser(strUserCode, strPwd, strIdentifyCode);

                    result = GetResultJson(postResult);

                }
                break;
            case "GetParentChildCurriculum":
                {

                    long userID = Convert.ToInt64(UIHelper.GetParam(this, "userid"));

                    result = GetParentChildCurriculum(userID);

                }
                break;
            //case "GetInformation":
            //    {

            //        string aboutAge = UIHelper.GetParam(this, "aboutAge");

            //        result = GetInformation(aboutAge);

            //    }
            //    break;
            case "GetAboutUs":
                {
                    result = GetAboutUs();

                }
                break;
            case "GetSubQes_T":
                {
                    long userID = Convert.ToInt64(UIHelper.GetParam(this, "userID"));

                    //int pageSize = ps;
                    //int pageNum = pn;

                    //try
                    //{
                    //    pageNum = Convert.ToInt32(UIHelper.GetParam(this, "pageNum"));
                    //    if (pageNum <= 0)
                    //    { pageNum = 1; }

                    //}
                    //catch (Exception ex)
                    //{
                    //    pageNum = pn;

                    //}
                    //try
                    //{
                    //    pageSize = Convert.ToInt32(UIHelper.GetParam(this, "pageSize"));
                        
                    //}
                    //catch (Exception ex)
                    //{
                    //    pageSize = ps;
                    //}
                    int pageSize;
                    int pageNum;
                    GetPageInfo(out pageSize, out pageNum);


                    result = GetSub_Qes_TByUserID(userID, pageNum, pageSize);

                }
                break;
            case "InsertSubQes_T":
                {

                    result = InsertSubQes_T();

                }
                break;
                

            //case "SaveJudgeResult":
            //    {
            //        string judgeResult = UIHelper.GetParam(this, "JudgeResult");

            //        result = SaveJudgeResult(judgeResult);

            //    }
            //    break;
                
            case "IsAnswered":
                {
                    long user_id = Convert.ToInt64(UIHelper.GetParam(this, "userid"));
                    long t_id = Convert.ToInt64(UIHelper.GetParam(this, "tid"));
                    result = GetSubjectivityQuestionByTID(user_id,t_id);


                }
                break;
            case "GetSubjectivityQuestion":
                {
                  
                    long user_id = Convert.ToInt64(UIHelper.GetParam(this, "userid"));
                    //int pageSize = ps;
                    //int pageNum = pn;
                        
                    //try
                    //{
                    //    pageNum = Convert.ToInt32(UIHelper.GetParam(this, "pageNum"));
                    //    if (pageNum <= 0)
                    //    { pageNum = 1; }
                        

                    //}
                    //catch (Exception ex)
                    //{
                    //    pageNum = pn;

                    //} 
                    //try
                    //{
                    //    pageSize = Convert.ToInt32(UIHelper.GetParam(this, "pageSize"));
                      
                        
                    //}
                    //catch (Exception ex)
                    //{
                    //    pageSize = ps;
                    //}
                    int pageSize;
                    int pageNum;
                    GetPageInfo(out pageSize, out pageNum);

                    string where = "";
                    result = GetSubjectivityQuestion(user_id, pageSize, pageNum,where);

                }
                break;
            // 提交Keywords回答
            case "SumitKeywordsAnswer":
            case "SaveSubjectivityQuestion":
                {
                    

                    string question = "";
                    try
                    {
                        question = UIHelper.GetParam(this, "Question");
                    }
                    catch (Exception ex)
                    {

                    }

                    result = SaveSubjectivityQuestion(question);

                }
                break;
            // localhost:8012/HBHHttpPost.aspx?method=SaveUser&user={"ID":12,"Account":"测试唉下","Name":"","Region":"","Passwd":"33333","Sex":"","Age":"","Birthday":null,"Pic":"nopic.jpg","Address":"","Tel":"18001298823","Judge_user_account":"","SysVersion":6,"Branch_ID":-1,"Branch_Name":"","Unit_id":-1,"HintMessage":"","SelfSign":"","BabyName":"","Province":"河北","City":"邢台","County":"桥西区"}
            case "SaveUser":
                {
                    string user = "";
                    try
                    {
                        user = UIHelper.GetParam(this, "user");
                    }
                    catch (Exception ex)
                    {

                    }

                    result = SaveUser(user);

                }
                break;
            //case "GetBabyAgeGroup":
            //    {

            //        string age = UIHelper.GetParam(this, "age");

            //        result = EntityHelper.GetBabyAgeGroup(age);

            //    }
            //    break;
            //case "GetPic":
            //    {
            //        this.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //        this.Response.ContentType = "text/html;charset=GB2312";

            //       //result =  GetPic();

            //    }
            //    break;

            // 获得Keywords
            case "GetKeywordsQuestion":
                {
                    long userID = PubClass.GetLong(UIHelper.GetParam(this, "userID"));
                    string strUserAccount = PubClass.GetString(UIHelper.GetParam(this, "Account"));

                    //int pageSize;
                    //int pageNum;
                    //GetPageInfo(out pageSize, out pageNum);

                    //result = GetKeywordsQuestion(userID, strUserAccount, pageSize, pageNum);
                    result = GetKeywordsQuestion(userID, strUserAccount);

                }
                break;

            // 获得 你问我答 历史问题、答案
            case "GetKeywordsHistory":
                {
                    long userID = PubClass.GetLong(UIHelper.GetParam(this, "userID"));
                    string strUserAccount = PubClass.GetString(UIHelper.GetParam(this, "Account"));

                    int pageSize;
                    int pageNum;
                    GetPageInfo(out pageSize, out pageNum);


                    result = GetKeywordsHistory(userID, strUserAccount, pageSize, pageNum);
                }
                break;

            // 获得Keywords
            case "GetKeywordsQuestion2":
                {
                    long userID = PubClass.GetLong(UIHelper.GetParam(this, "userID"));
                    string strUserAccount = PubClass.GetString(UIHelper.GetParam(this, "Account"));

                    //int pageSize;
                    //int pageNum;
                    //GetPageInfo(out pageSize, out pageNum);

                    //result = GetKeywordsQuestion(userID, strUserAccount, pageSize, pageNum);
                    result = GetKeywordsQuestion(userID, strUserAccount,2);

                }
                break;
            case "UserRegister":
                {
                    // localhost:8012/HBHHttpPost.aspx?method=UserRegister&user={"pwd":"123456","telno":"18001298823"}&IdentifyCode=71817
                    string strUserInfo = PubClass.GetString(UIHelper.GetParam(this, "user"));
                    string strIdentifyCode = PubClass.GetString(UIHelper.GetParam(this, "IdentifyCode"));

                    PostResult postResult = RegisterUser(strUserInfo, strIdentifyCode);

                    result = GetResultJson(postResult);
                }
                break;
            // 获得当天Message
            case "GetMessageFromDate":
                {
                    DateTime dt = PubClass.GetDateTime(UIHelper.GetParam(this, "Date"), DateTime.Today);

                    result = GetMessageFromDate(dt);

                }
                break;
            // SMS:  short message service
            // http://211.149.198.209:8012/HBHHttpPost.aspx?Method=GetSMS&PhoneNumber=18001298823
            case "GetSMS":
                {
                    // 
                    string strUserInfo = PubClass.GetString(UIHelper.GetParam(this, "PhoneNumber"));

                    PostResult postResult = GetSMS(strUserInfo);

                    result = GetResultJson(postResult);
                }
                break;
            // http://localhost:8012/HBHHttpPost.aspx?Method=ModifyPassword&user={"Passwd":"22222","id":"12"}&OldPassword=123456
            // http://localhost:8012/HBHHttpPost.aspx?Method=ModifyPassword&user={"Passwd":"33333","tel":"18001298823"}&IdentifyCode=35176
            // 修改密码
            case "ModifyPassword":
                {
                    string user = UIHelper.GetParam(this, "user");
                    string strIdentifyCode = UIHelper.GetParam(this, "IdentifyCode");
                    string strOldPwd = UIHelper.GetParam(this, "OldPassword");

                    PostResult postResult = ModifyPassword(user, strIdentifyCode, strOldPwd);

                    result = GetResultJson(postResult);

                }
                break;
            default:
                {
                    result = "无效的方法名!";
                }
                break;
        }

        this.Response.Write(result);
        this.Response.End();
    }

    // 获得验证码
    private PostResult GetSMS(string PhoneNumber)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        if (!string.IsNullOrEmpty(PhoneNumber))
        {
            string smsContent = Config.SmsContent;
            Random rm = new Random();
            int IdentifyCode = rm.Next(Config.SmsIdentifyMin, Config.SmsIdentifyMax);

            smsContent = string.Format(smsContent, IdentifyCode);

            string smsResult = sendSMSMessage(PhoneNumber, smsContent,IdentifyCode.ToString());

            result.IsSuccess = true;
            result.Message = smsResult;
        }
        else
        {
            result.IsSuccess = false;
            result.Message = "参数不可为空!";
        }
        //string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        //return strResult;

        return result;
    }

    public static string sendSMSMessage(string PhoneNumber, string smsContent, string IdentifyCode)
    {
        string tmp = "";
        if (PhoneNumber != "")
        {
            //string accountname = "sdktest";
            //string accountpwd = "";
            string url = Config.SmsUrl;
            string accountname = Config.SmsAccount;
            string accountpwd = Config.SmsPassword;

            Encoding encoding = Encoding.GetEncoding("GBK");
            string postData = "accountname=" + accountname;
            postData += ("&accountpwd=" + accountpwd);
            postData += ("&mobilecodes=" + PhoneNumber);
            //postData += ("&msgcontent=" + SMSContent);
            postData += ("&msgcontent=" + smsContent);
            byte[] data = encoding.GetBytes(postData);
            // Prepare web request
            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://csdk.zzwhxx.com:8002/submitsms.aspx");
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.Timeout = 10000;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
            StreamReader sreader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            tmp = sreader.ReadToEnd();


            //这里有问题，是否可以让同一用户多次提交同一个问题。或者修改
            MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);
            // 保存到数据库中
            string insertSQL = string.Format("insert into hbh_SMS ( PhoneNumber,IdentifyCode,SMSUrl,SMSMessage,PostData,Result,ModifiedOn,CreatedOn) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{6}');  select @@IDENTITY ;SELECT LAST_INSERT_ID();"
                                , PhoneNumber, IdentifyCode, url, smsContent, postData, tmp, DateTime.Now
                            );

            DataTable dt = new DataTable();
            long id = UIHelper.GetLong(mysqlHelper.ExecuteScalar(CommandType.Text, insertSQL, new MySql.Data.MySqlClient.MySqlParameter()));
            
        }
        return tmp;
    }

    private string GetMessageFromDate(DateTime dt)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string strToday = dt.ToString("yyyy-MM-dd");

        //        string sql = @"
        //select distinct
        //	user.birthday as UserBirthday
        //    ,msg.*
        //from
        //	hbh_user user
        //    inner join hbh_Message msg
        //    on 
        //		(year('{0}') - year(user.birthday)) * 12 * 31 + (month('{0}') - month(user.birthday)) * 31 + (day('{0}') - day(user.birthday))
        //		 = (cast(msg.aboutAgeEnd_age as SIGNED) * 12 * 31 + cast(msg.aboutAgeBegin_month as SIGNED) * 31 
        //		 	+  + cast(msg.aboutAgeBegin_week as SIGNED) * 7 +  + cast(msg.aboutAgeBegin_day as SIGNED))
        //where
        //	user.birthday is not null and user.birthday > '1950-01-01'";
        string sql = @"
select distinct
	user.birthday as UserBirthday
    ,msg.*
from
	hbh_user user
    inner join hbh_Message msg
    on 
		(year(@Date) - year(user.birthday)) * 12 * 31 + (month(@Date) - month(user.birthday)) * 31 + (day(@Date) - day(user.birthday))
		 = (cast(msg.aboutAgeEnd_age as SIGNED) * 12 * 31 + cast(msg.aboutAgeBegin_month as SIGNED) * 31 
		 	+  + cast(msg.aboutAgeBegin_week as SIGNED) * 7 +  + cast(msg.aboutAgeBegin_day as SIGNED))
where
	user.birthday is not null and user.birthday > '1950-01-01'";

        //sql = string.Format(sql, strToday);
        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql, new TableTypeParameter("Date", strToday));

            List<Entities.PushMessage> list = Entities.PushMessage.GetFromTable(ds);

            if (list != null
                && list.Count > 0
                )
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                string reslut = JsonHelper.JsonSerializer(strJson);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;
            }
            else
            {
                result.IsSuccess = true;
                result.Message = "Query is Null!";
            }

            //if (ds.Tables.Count > 1)
            //{

            //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

            //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
            //    result.ResultJson2 = strJson2;
            //}
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;


    }

    private void GetPageInfo(out int pageSize, out int pageNum)
    {
        pageSize = ps;
        pageNum = pn;

        try
        {
            int curPageNum = Convert.ToInt32(UIHelper.GetParam(this, "pageNum"));

            if (curPageNum > 0)
            {
                pageNum = curPageNum;
            }
        }
        catch { }

        try
        {
            int curPageSize = Convert.ToInt32(UIHelper.GetParam(this, "pageSize"));

            if (curPageSize > 0)
            {
                pageSize = curPageSize;
            }
        }
        catch { }

    }


    /// <summary>
    /// 得到用户
    /// </summary>
    /// <param name="strAccount">用户编码</param>
    /// <param name="password">用户密码</param>
    /// <returns></returns>
    private PostResult GetUser(string strAccount, string password, string strIdentifyCode)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        //string sql = "select id,account,passwd,name,sex,date_format(age,'%Y-%c-%d %H:%i:%s') as age,pic from judge_user where account ='{0}'  ; show variables like 'char%';";
        //string sql = "select *,birthday as age,-1 as branch_id,-1 as unit_id,'' as branch_name from hbh_user where account ='{0}';";
        string sql = Const_UserQueryAccount;
        sql = string.Format(sql, strAccount);
        List<Entities.User> list = EntityHelper.GetUserEntityBySQL(sql,SetOfBookType.HBHBaby);

        

        try
        {

            bool exist = false;
            if (list != null
                && list.Count > 0
                )
            {
                //foreach (Entities.User us in list)
                for(int i = list.Count - 1 ; i >= 0 ; i --)
                {
                    Entities.User us = list[i];
                    // 密码正确 或 验证码非空(在上面已验证成功)
                    if (!PubClass.IsNull(strIdentifyCode))
                    {
                        int overDueMinute = Config.SmsOverDueMinute;
                        bool bl = IdentifyCodeValidate(strIdentifyCode, strAccount, overDueMinute);

                        if (!bl)
                        {
                            result.IsSuccess = false;
                            result.Message = string.Format("验证码不正确或已过期(过期时间{0}分钟)!", overDueMinute);
                            return result;
                        }
                        else
                        {
                            exist = true;
                        }
                    }
                    else if (us.Passwd == password
                        // && !PubClass.IsNull(strIdentifyCode)
                        )
                    {
                        exist = true;
                    }
                    else
                    {
                        list.RemoveAt(i);
                    }
                }
                if (exist)
                {
                    foreach (Entities.User us in list)
                    {
                        //us.Pic = "http://ceping.xinbaobeijiaoyu.com/measure/upload/" + us.Pic;
                        if (!PubClass.IsNull(us.Birthday))
                        {
                            DateTime? birthday = PubClass.GetDateTime(us.Birthday);
                            if (birthday != null)
                            {
                                string newAge = EntityHelper.GetAgeByBirthday(birthday.Value, DateTime.Now);
                                us.Age = newAge;//System.Text.Encoding.GetEncoding("latin1").GetString(System.Text.Encoding.Default.GetBytes(newAge));
                            }
                        }
                    }
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.ResultJson = strJson;
                    result.IsSuccess = true;
                    result.Message = "登陆成功!";
                }
                else
                {
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    // result.ResultJson = strJson;

                    result.IsSuccess = false;

                    result.Message = "账号或密码错误!";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "用户不存在!";
            }


        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        //string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        //return strResult;
        return result;
    }


    /// <summary>
    /// 得到用户所有的消息
    /// </summary>
    /// <returns></returns>
    private string GetMessage(long userID)
    {
        if (string.IsNullOrEmpty(userID.ToString()) || userID <= 0)
        {
            return "没有用户，无法获取问题列表！";
        }

        //string getUserByIDSql = string.Format("select * from hbh_user  where id ='{0}';", userID);
        string getUserByIDSql = string.Format(Const_UserQueryID, userID);
        List<Entities.User> lstUsers = EntityHelper.GetUserEntityBySQL(getUserByIDSql,SetOfBookType.HBHBaby);
        Entities.User user = null;
        if (lstUsers != null && lstUsers.Count > 0)
        {
            user = lstUsers[0];
        }
        else
        {
            return "没有此用户！无法获取问题列表";
        }
        // string aboutAge = EntityHelper.GetAgeByBirthday(Convert.ToDateTime(user.Birthday),DateTime.Now);
        //if (!string.IsNullOrEmpty(aboutAge))
        //{
        // string ageGroup = EntityHelper.GetBabyAgeGroup(aboutAge);
        string ageGroup = EntityHelper.GetWeekDayByBirthday(Convert.ToDateTime(user.Birthday), DateTime.Now);


        PostHelper.PostResult result = new PostHelper.PostResult();

        //string sql = "select id,user_id,message_Content,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate ,message_Title,isRead,poster_id,subhead from hbh_Message where aboutAgeEnd >= '{0}' and aboutAgeBegin <= '{0}' Order by isRead ,messDate desc ;";
        //string sql = "select id,user_id,message_Content,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate ,message_Title,isRead,poster_id,subhead from hbh_Message where aboutAgeBegin <= '{0}' Order by isRead ,messDate desc ,aboutAgeBegin desc limit 0 , 6 ;";
        //sql = string.Format(sql, ageGroup);

        // 增加日期判断逻辑(注册当天只给欢迎消息) ，2015-06-08 wf 
        string sql = @"
select msg.id,msg.user_id,msg.message_Content
	,date_format(msg.messDate,'%Y-%c-%d %H:%i:%s') as messDate 
    ,msg.message_Title,msg.isRead,msg.poster_id
    ,msg.subhead 
    ,msg.aboutAgeBegin
    ,usr.birthday
    ,usr.CreatedOn
    ,date_format(date_add(date_add(date_add(date_add(usr.Birthday,interval ifnull(msg.aboutAgeBegin_age,0) Year),interval ifnull(msg.aboutAgeBegin_month,0) Month),interval ifnull(msg.aboutAgeBegin_week - 1,0) Week),interval ifnull(msg.aboutAgeBegin_day,0) Day),'%Y-%c-%d %H:%i:%s') as AboutBegin
    ,date_format(date_add(date_add(date_add(date_add(usr.Birthday,interval ifnull(msg.aboutAgeEnd_age,0) Year),interval ifnull(msg.aboutAgeEnd_month,0) Month),interval ifnull(msg.aboutAgeEnd_week - 1,0) Week),interval ifnull(msg.aboutAgeEnd_day,0) Day),'%Y-%c-%d %H:%i:%s') as AboutEnd
    ,msg.*
from hbh_Message msg
	inner join hbh_user usr
    on  1=1
		and ((aboutAgeEnd = '0岁0个月0周第0天')
			or (now() >= 
					-- date_add(date_add(date_add(date_add(usr.Birthday,interval ifnull(msg.aboutAgeEnd_age,0) Year),interval ifnull(msg.aboutAgeEnd_month,0) Month),interval ifnull(msg.aboutAgeEnd_week - 1,0) Week),interval ifnull(msg.aboutAgeEnd_day,0) Day)
                    date_add(date_add(date_add(date_add(usr.Birthday,interval ifnull(msg.aboutAgeBegin_age,0) Year),interval ifnull(msg.aboutAgeBegin_month,0) Month),interval ifnull(msg.aboutAgeBegin_week - 1,0) Week),interval ifnull(msg.aboutAgeBegin_day,0) Day)
				and usr.CreatedOn <=
					date_add(date_add(date_add(date_add(usr.Birthday,interval ifnull(msg.aboutAgeBegin_age,0) Year),interval ifnull(msg.aboutAgeBegin_month,0) Month),interval ifnull(msg.aboutAgeBegin_week - 1,0) Week),interval ifnull(msg.aboutAgeBegin_day,0) Day)
                and curdate() > usr.CreatedOn
				)
			)
where 1=1	
	and usr.ID = {0}
	-- and aboutAgeEnd <= '{0}' 
Order by msg.isRead
    ,aboutAgeBegin_age desc,aboutAgeBegin_month desc,aboutAgeBegin_week desc,aboutAgeBegin_day desc
    ,msg.aboutAgeBegin desc ,msg.messDate desc 

-- limit 0 , 6 ;
";
        sql = string.Format(sql, userID );

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.Message> list = Entities.Message.GetMessageFromTable(ds);

            
            if (list != null && list.Count>0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Entities.Message entity = list[i];
                    
                    if (entity.Message_Title.Contains("\n"))
                    {
                        list[i].Message_Title = list[i].Message_Title.Replace("\n", "");
                    }
                    try
                    {
                        list[i].MessDate = Convert.ToDateTime(entity.MessDate).ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        list[i].MessDate = "2015-01-01";
                    }
                }
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                string reslut = JsonHelper.JsonSerializer(strJson);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query is Null!";
            }

            //if (ds.Tables.Count > 1)
            //{

            //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

            //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
            //    result.ResultJson2 = strJson2;
            //}
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
    /// 亲子课程
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    private string GetParentChildCurriculum(long userID)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select id,stage,Content,aboutAge from hbh_ParentChildCurriculum  Order by sequence;";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.ParentChildCurriculum> list = Entities.ParentChildCurriculum.GetParentChildCurriculumFromTable(ds);


            if (list != null && list.Count > 0)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                string reslut = JsonHelper.JsonSerializer(strJson);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query is Null!";
            }

            //if (ds.Tables.Count > 1)
            //{

            //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

            //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
            //    result.ResultJson2 = strJson2;
            //}
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
    /// 根据用户年龄提供用户应该得到的信息
    /// </summary>
    /// <param name="aboutAge">宝宝年龄，规则：：x岁x个月/x岁/x个月</param>
    /// <returns></returns>
    private string GetInformation(string aboutAge)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string AgeGroup = "";
       
        AgeGroup = EntityHelper.GetBabyAgeGroup(aboutAge);
       

        
        string sql = "select id,information_type,information_Content,aboutAge from hbh_Information where aboutAge ='{0}' ;";
        sql = string.Format(sql, AgeGroup);

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.Message> list = Entities.Message.GetMessageFromTable(ds);


            if (list != null && list.Count>0)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query is Null!";
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
   
    /// <summary>
    /// 关于我们
    /// </summary>
    /// <returns></returns>
    private string GetAboutUs()
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        string sql = "select Replace(Replace(Replace(Replace(Replace(aboutUs_Content,'，',','),'“','\"'),'”','\"'),'。','.'),'、','、') as aboutUs_Content from hbh_AboutUs ;";
        //string sql = "select aboutUs_Content from AboutUs ; show variables like 'char%';";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.AboutUs> list = Entities.AboutUs.GetAboutUsFromTable(ds);

           
            if (list != null && list.Count>0)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query Result is null!";
            }

            //if (ds.Tables.Count > 1)
            //{

            //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

            //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
            //    result.ResultJson2 = strJson2;
            //}
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }

    private string InsertSubQes_T()
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);
        try
        {

            Entities.Subjectivity_Question_T que = new Entities.Subjectivity_Question_T();
            que.KeyWords = "认知，体征，婴儿，发育，牙";
            que.AboutAge = "3-5岁";
            que.Question_Title = "对于孩子的体重、身高、头发有_问题想了解吗？";
           // que.Question_Title = "您对于孩子的发展情况还有_问题需要我们解答吗？";
            //que.Question_Title3 = "对于孩子的体重、身高、头发有_问题想了解吗？";

            string insertSQL = string.Format("insert into hbh_sub_qes_t (question_title,keyWords, aboutAge) values('{0}','{1}','{2}');  select @@IDENTITY ;SELECT LAST_INSERT_ID();"
            , que.Question_Title,que.KeyWords,  que.AboutAge);


            DataTable dt = new DataTable();
            int sum = mysqlHelper.ExecuteNonQuery(CommandType.Text, insertSQL, new MySql.Data.MySqlClient.MySqlParameter());
            if (sum > 0)
            {
                result.IsSuccess = true;
                result.Message = "成功" + sum.ToString();
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "失败";
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = "失败"+ex.Message;
        }
                    
                    string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    return strResult;
    }

    /// <summary>
    /// 通过用户id，得到用户age查询出主观问题题目
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    private string GetSub_Qes_TByUserID(long userID,int pageNum,int pageSize)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();
        SetOfBookType setOfBookType = SetOfBookType.HBHBaby;
        if (string.IsNullOrEmpty(userID.ToString()) || userID <= 0)
        {
            return "没有用户，无法获取问题列表！";
        }

        //string getUserByIDSql = string.Format("select * from hbh_user where id ='{0}';", userID);
        string getUserByIDSql = string.Format(Const_UserQueryID, userID);
        List<Entities.User> lstUsers = EntityHelper.GetUserEntityBySQL(getUserByIDSql,SetOfBookType.HBHBaby);
        Entities.User user = null;
        if (lstUsers != null && lstUsers.Count > 0)
        {
            user = lstUsers[0];
        }
        else 
        {
            return "没有此用户！无法获取问题列表";
        }
       // string aboutAge = EntityHelper.GetAgeByBirthday(Convert.ToDateTime(user.Birthday),DateTime.Now);
        //if (!string.IsNullOrEmpty(aboutAge))
        //{
           // string ageGroup = EntityHelper.GetBabyAgeGroup(aboutAge);
            string ageGroup = EntityHelper.GetWeekByBirthday(Convert.ToDateTime(user.Birthday), DateTime.Now);

            string sql = "";
            string where = " where  aboutAgeEnd >= '{0}' and aboutAgeBegin <= '{0}'";
            where = string.Format(where, ageGroup);

            if (pageSize <= 0)
            {
                sql = "select t.id,t.question_Title,t.aboutage,t.keywords,t.aboutAgeEnd,t.aboutAgeBegin,qes.keywords as answer from hbh_Sub_Qes_T t left join hbh_subquestion qes on qes.sub_qes_t=t.id and qes.questioner={0} {1} ;";
                sql = string.Format(sql,userID, where);

            }
            else
            {
                int start = (pageNum - 1) * pageSize;
                sql = "select t.id,t.question_Title,t.aboutage,t.keywords,t.aboutAgeEnd,t.aboutAgeBegin,qes.keywords as answer from hbh_Sub_Qes_T t left join hbh_subquestion qes on qes.sub_qes_t=t.id and qes.questioner={0} {1} limit {2},{3};";
                sql = string.Format(sql,userID, where, start, pageSize);


            }
           // string sql = string.Format("select id,question_Title,aboutage,keywords from hbh_Sub_Qes_T where aboutage = '{0}';", ageGroup);
            //string sql = "select aboutUs_Content from AboutUs ; show variables like 'char%';";

            MySqlHelper mysqlHelper = new MySqlHelper(setOfBookType);

            DataSet ds = new DataSet();
            try
            {
                mysqlHelper.Fill(ds, CommandType.Text, sql);

                List<Entities.Subjectivity_Question_T> list = Entities.Subjectivity_Question_T.GetSubjectivity_Question_TFromTable(ds);


                if (list != null && list.Count > 0)
                {
                    for (int i=0;i<list.Count;i++)
                    {
                        Entities.Subjectivity_Question_T entity = list[i];
                        if (entity.KeyWords.Contains("、"))
                        {
                            list[i].KeyWords = list[i].KeyWords.Replace("、", "，");
                        }
                        if (entity.Question_Title.Contains("\n"))
                        {
                            list[i].Question_Title = list[i].Question_Title.Replace("\n", "");
                        }
                    }
                    //if (row_content.Contains("、"))
                    //{
                    //    row_content = row_content.Replace("、", "，");
                    //}
                    //if (row_title.Contains("\n"))
                    //{
                    //    row_title = row_title.Replace("\n", "");
                    //}
                    result.IsSuccess = true;
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.Message = "Query Successfully!";
                    result.ResultJson = strJson;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Query Result is null!";
                }
                string totalNumSql = "select t.id,t.question_Title,t.aboutage,t.keywords,t.aboutAgeEnd,t.aboutAgeBegin,qes.keywords as answer from hbh_Sub_Qes_T t left join hbh_subquestion qes on qes.sub_qes_t=t.id and qes.questioner={0} {1} ;";
                totalNumSql = string.Format(sql, userID, where);

                int totalNum = mysqlHelper.GetRowsCount(totalNumSql, setOfBookType);

                Entities.CommonPageMessage pageMessage = SetPageMessage(pageSize, pageNum, totalNum);


                result.ResultJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(pageMessage);

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

        //}
      

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }


    
   /// <summary>
    ///  获得主观提问的问题
   /// </summary>
   /// <param name="aboutAge"></param>
   /// <returns></returns>
    private string GetSub_Qes_T(string aboutAge)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        if (string.IsNullOrEmpty(aboutAge))
        {
            return "没有得到宝宝年龄！无法获取问题！";
        }
        string age = EntityHelper.GetBabyAgeGroup(aboutAge);
       
        string sql = string.Format("select id,question_Title,aboutage,keywords from hbh_Sub_Qes_T where aboutage = '{0}';",age);
        //string sql = "select aboutUs_Content from AboutUs ; show variables like 'char%';";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.Subjectivity_Question_T> list = Entities.Subjectivity_Question_T.GetSubjectivity_Question_TFromTable(ds);


            if (list != null && list.Count>0)
            {
                foreach (Entities.Subjectivity_Question_T entity in list)
                {
                    string title = entity.Question_Title;
                    string[] titleParts = title.Split('_');
                    if (titleParts.GetLength(0) >= 2)
                    {
                        entity.Question_Title_Part1 = titleParts[0];
                        entity.Question_Title_Part2 = titleParts[1];
                    }
                }
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query Result is null!";
            }

            //if (ds.Tables.Count > 1)
            //{

            //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

            //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
            //    result.ResultJson2 = strJson2;
            //}
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
    /// 将提交的结果写入数据库
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private string SaveJudgeResult(string result)
    {
        PostHelper.PostResult postResult = new PostHelper.PostResult();

        List<Entities.Judge_Result> lstResult = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as List<Entities.Judge_Result>;
        if (lstResult != null && lstResult.Count > 0)
        {
            string insertSQL = "insert into hbh_judge_result (measureID,userID,result) values(";

            for (int i = 0; i < lstResult.Count; i++)
            {
                Entities.Judge_Result rst = lstResult[i];

                if (i == lstResult.Count - 1)
                {
                    insertSQL += rst.MeasureID + "," + rst.UserID + "," + rst.Result + ")";
                }
                else
                {
                    insertSQL += rst.MeasureID + "," + rst.UserID + "," + rst.Result + "),";
                }
            }
            insertSQL += ";   select @@IDENTITY ;";

            MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);
            try
            {

                int succNum = mysqlHelper.ExecuteNonQuery(CommandType.Text, insertSQL, new MySql.Data.MySqlClient.MySqlParameter());
                postResult.IsSuccess = true;
                postResult.Message = "Successfully saved " +succNum.ToString() +" data!";
            }
            catch (Exception ex)
            {
                postResult.IsSuccess = false;
                postResult.Message = ex.Message;
            }
        }
        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }

    /// <summary>
    /// 将提交的主观问题写入数据库
    /// </summary>
    /// <param name="subQuestion"></param>
    /// <returns></returns>
    private string SaveSubjectivityQuestion(string subQuestion)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        Entities.Subjectivity_Question SubQues = null;

        if (!string.IsNullOrEmpty(subQuestion))
        {
            Entities.SubmitSubQuestion submitEntity = new Entities.SubmitSubQuestion();

            submitEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.SubmitSubQuestion>(subQuestion) as Entities.SubmitSubQuestion;

            if (submitEntity != null)
            {
                SubQues = new Entities.Subjectivity_Question();

                SubQues.AboutAge = submitEntity.Age;
                SubQues.MessDate = DateTime.Now;
                SubQues.Questioner = submitEntity.UserID;
                SubQues.Sub_Qes_T = submitEntity.QuestionID;
                SubQues.Sub_Qes_title = submitEntity.QuestionTitle;
                string[] keywords = submitEntity.Keyword;
                string key = "";
                if (keywords != null && keywords.GetLength(0) > 0)
                {
                    for (int i=0;i<keywords.GetLength(0);i++)
                    {
                        string keyword = keywords[i].ToString();
                        key += keyword + "、";
                    }
                   key = key.Substring(0, key.Length - 1);
                }
                SubQues.KeyWords = key;
                
               // SubQues.KeyWords = 
            }

        }

       
        try
        {
            Entities.Subjectivity_Question que = SubQues;

            if (que != null)
            {
                //这里有问题，是否可以让同一用户多次提交同一个问题。或者修改
                MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

                List<long> ids = new List<long>();
                if (que.Sub_Qes_T <= 0)
                {
                    throw new Exception("提交问题不明！请重新选择回答问题！");
                }
                if (que.Questioner <= 0)
                {
                    throw new Exception("提交者不明！");
                }
                string selectSQL = string.Format("select id from HBH_SubQuestion where questioner = {0} and sub_qes_t = {1}",que.Questioner,que.Sub_Qes_T);
                long selectId = UIHelper.GetLong(mysqlHelper.ExecuteScalar(CommandType.Text, selectSQL, new MySql.Data.MySqlClient.MySqlParameter()));
                if (selectId > 0)
                {

                    string updateSQL = string.Format("update hbh_subQuestion set aboutAge='{0}',messDate ='{1}',Sub_Qes_title ='{2}',keywords='{3}'  where sub_qes_t='{4}' and questioner='{5}';",que.AboutAge,que.MessDate,que.Sub_Qes_title,que.KeyWords,que.Sub_Qes_T,que.Questioner);
                    try
                    {
                        mysqlHelper.ExecuteNonQuery(CommandType.Text, updateSQL, new MySql.Data.MySqlClient.MySqlParameter());

                        result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(que);
                        result.IsSuccess = true;
                        result.Message = "更新成功！";
                    }
                    catch(Exception ex)
                    {
                        result.IsSuccess = false;
                        result.Message = ex.Message;
                    }
                }
                else
                {


                    string insertSQL = string.Format("insert into HBH_SubQuestion (sub_Qes_title,question_Content,keyWords, messDate,questioner,aboutAge,sub_Qes_T) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');  select @@IDENTITY ;SELECT LAST_INSERT_ID();"
                    , que.Sub_Qes_title, que.Question_Content, que.KeyWords, DateTime.Now, que.Questioner, que.AboutAge, que.Sub_Qes_T);


                    DataTable dt = new DataTable();
                    long id = UIHelper.GetLong(mysqlHelper.ExecuteScalar(CommandType.Text, insertSQL, new MySql.Data.MySqlClient.MySqlParameter()));
                    //string selectSql = "";
                    //long id = UIHelper.GetLong(mysqlHelper.ExecuteScalar(CommandType.Text, selectSql, new MySql.Data.MySqlClient.MySqlParameter()));
                    if (id > 0)
                    {
                        ids.Add(id);
                    }


                    if (ids.Count > 0)
                    {
                        string where = "";
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (i == ids.Count - 1)
                            {
                                where += ids[i].ToString();
                            }
                            else
                            {
                                where += ids[i].ToString() + ",";
                            }


                        }
                        string sql = string.Format("select * from HBH_SubQuestion where id in({0}) ;", where);

                        result = GetSubQuestionResult(sql);

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "";
                        result.ResultJson = null;
                    }
                }

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "反序列化失败！没有得到实体List";
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
    /// 保存用户
    /// </summary>
    /// <param name="user">user实体的Json字符串</param>
    /// <returns></returns>
    private string SaveUser(string userJson)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        Entities.User user = null;

        if (!string.IsNullOrEmpty(userJson))
        {
            user = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.User>(userJson) as Entities.User;

            //Entities.User submitEntity = new Entities.User();

            //submitEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.User>(userJson) as Entities.User;

            //if (submitEntity != null)
            //{
            //    user = new Entities.User();

            //    user.ID = submitEntity.ID;
            //    user.Name = submitEntity.Name;
            //    user.Account = submitEntity.Account;
            //    user.Address = submitEntity.Address;
            //    user.Birthday = submitEntity.Birthday;
            //    user.Passwd = submitEntity.Passwd;
            //    user.Region = submitEntity.Region;
            //    user.Sex = submitEntity.Sex;
            //    user.Tel = submitEntity.Tel;
            //    user.Pic=submitEntity.Pic;
            //    //user.CreatedBy = submitEntity.CreatedBy;
            //    //user.CreatedOn = submitEntity.CreatedOn;
            //    //user.ModifiedBy = submitEntity.ModifiedBy;
            //    //user.ModifiedOn = submitEntity.ModifiedOn;
            //    user.SysVersion = submitEntity.SysVersion;
                
            //}
        }


        try
        {
            Entities.User saveUser = user;

            if (saveUser != null)
            {
                MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

                List<long> ids = new List<long>();


                long saveUserId = saveUser.ID;
                //有ID更新
                if (saveUserId > 0)
                {
                    //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
                    string selectSQL = string.Format(Const_UserQueryID, saveUser.ID);
                    List<Entities.User> list = EntityHelper.GetUserEntityBySQL(selectSQL,SetOfBookType.HBHBaby);
                    //有ID传入且  数据库里有此ID的数据
                    if (list != null && list.Count > 0)
                    {
                        //ID大于0、版本小于0时，强制更新
                        if (saveUser.SysVersion <= 0)
                        {
                            result = UpdateUserByNewEntity(saveUser);
                        }
                        else
                        {
                            foreach (Entities.User entity in list)
                            {
                                result.ResultJson = "";
                                //判断版本大于0 并且 与数据库这个用户的版本比较，如果app端版本大
                                if (entity.SysVersion <= saveUser.SysVersion
                                    )
                                {
                                    result = UpdateUserByNewEntity(saveUser);
                                }
                                else //否则不更新,返回重新查出最新的用户数据
                                {
                                   
                                    result.IsSuccess = false;
                                    result.Message = "已被他人修改,更新不成功!";
                                }
                            }
                            if (string.IsNullOrEmpty(result.ResultJson))
                            {
                                result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                            }
                        }
                    }
                    else//有ID传入，但数据库里没有查到此ID的数据
                    {
                        result.IsSuccess = false;
                        result.Message = "用户不存在！不执行更新！";
                    }

                }
                else //没有ID新增
                {
                    /*
alter table hbh_user
add BabyName varchar(125) 

commit;
                     */
                    // ,CreatedOn,CreatedBy,ModifiedBy,ModifiedOn
                    // ,'{10}','{11}','{12}','{13}'
                    string strNow = PubClass.GetStringFromDate(DateTime.Now);
                    string insertSQL = string.Format("insert into HBH_User (address,region,account, name,sex,tel,pic,passwd,birthday,BabyName,SelfSign,SysVersion,CreatedOn,CreatedBy,ModifiedBy,ModifiedOn) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');  select @@IDENTITY ;SELECT LAST_INSERT_ID();"
                    , saveUser.Address, saveUser.Region, saveUser.Account, saveUser.Name, saveUser.Sex, saveUser.Tel, saveUser.Pic, saveUser.Passwd, saveUser.Birthday, saveUser.BabyName, saveUser.SelfSign
                    , 0, strNow, string.Empty, strNow, string.Empty
                        // ,saveUser.CreatedOn,saveUser.CreatedBy,saveUser.ModifiedBy,saveUser.ModifiedOn
                    );


                    DataTable dt = new DataTable();
                    long id = UIHelper.GetLong(mysqlHelper.ExecuteScalar(CommandType.Text, insertSQL, new MySql.Data.MySqlClient.MySqlParameter()));
                    if (id > 0)
                    {
                        ids.Add(id);
                    }


                    if (ids.Count > 0)
                    {
                        string where = "";
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (i == ids.Count - 1)
                            {
                                where += ids[i].ToString();
                            }
                            else
                            {
                                where += ids[i].ToString() + ",";
                            }


                        }
                        //  string sql = string.Format("select * from HBH_User where id in({0}) ;", where);
                        //string selectSQL = string.Format("select * from hbh_user where id in ({0}) ;", where);
                        string selectSQL = string.Format(Const_UsersIn, where);
                        List<Entities.User> list = EntityHelper.GetUserEntityBySQL(selectSQL,SetOfBookType.HBHBaby);

                        //string hintMessage = "欢迎你加入鑫宝贝慧爱家族!";

                        if (list != null && list.Count > 0)
                        {
                            result.IsSuccess = true;
                            result.Message = where;
                            result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "获取新增数据失败！";
                            result.ResultJson = null;
                        }
                    }

                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "反序列化失败！没有得到实体List";
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

    private static PostHelper.PostResult UpdateUserByNewEntity(Entities.User saveUser)
    {

        PostHelper.PostResult result = new PostHelper.PostResult();
        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
        string selectSQL = string.Format(Const_UserQueryID, saveUser.ID);
        // // ,CreatedBy='{10}',CreatedOn='{11}',ModifiedBy='{12}',ModifiedOn='{13}'
        //string updateSQL = string.Format("update hbh_user set address='{0}',region ='{1}',account ='{2}',name='{3}' ,sex='{4}',tel='{5}',pic='{6}',passwd='{7}',birthday='{8}',SysVersion=SysVersion+1,BabyName='{9}',SelfSign='{10}',Province='{11}',City='{12}',County='{13}' where id='{14}';",
        //saveUser.Address, saveUser.Region, saveUser.Account, saveUser.Name, saveUser.Sex, saveUser.Tel, saveUser.Pic, saveUser.Passwd, saveUser.Birthday
        //    , saveUser.BabyName, saveUser.SelfSign
        //    // , saveUser.CreatedBy, saveUser.CreatedOn, saveUser.ModifiedBy, saveUser.ModifiedOn
        //    , saveUser.Province, saveUser.City, saveUser.County
        //    , saveUser.ID
        //);


        // 改成传什么，就Update什么
        StringBuilder sbSet = new StringBuilder();

        if (saveUser != null)
        {
            if (saveUser.Address != null)
            { 
                sbSet.Append(string.Format("address='{0}',",saveUser.Address));

                // 改成 省、市、区 了
                //// 校验
                //if (PubClass.IsNull(saveUser.Address))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "地址不可为空 !";
                //    return result;
                //}
            }
            if (saveUser.Region != null)
            {
                sbSet.Append(string.Format("region='{0}',", saveUser.Region));

                // 校验
                if (PubClass.IsNull(saveUser.Region))
                {
                    result.IsSuccess = false;
                    result.Message = "地址不可为空 !";
                    return result;
                }
            }
            if (saveUser.Account != null)
            {
                sbSet.Append(string.Format("account='{0}',", saveUser.Account));

                // 已取消了账号
                //// 校验
                //if (PubClass.IsNull(saveUser.Account))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "账号不可为空 !";
                //    return result;
                //}
            }
            if (saveUser.Name != null)
            {
                sbSet.Append(string.Format("name='{0}',", saveUser.Name));

                // 校验
                if (PubClass.IsNull(saveUser.Name))
                {
                    result.IsSuccess = false;
                    result.Message = "用户名不可为空 !";
                    return result;
                }
            }
            if (saveUser.Sex != null)
            {
                sbSet.Append(string.Format("sex='{0}',", saveUser.Sex));

                // 校验
                if (PubClass.IsNull(saveUser.Sex))
                {
                    result.IsSuccess = false;
                    result.Message = "性别不可为空 !";
                    return result;
                }
            }
            if (saveUser.Tel != null)
            {
                sbSet.Append(string.Format("tel='{0}',", saveUser.Tel));

                // 校验
                if (PubClass.IsNull(saveUser.Tel))
                {
                    result.IsSuccess = false;
                    result.Message = "电话不可为空 !";
                    return result;
                }
            }
            if (saveUser.Pic != null)
            {
                sbSet.Append(string.Format("pic='{0}',", saveUser.Pic));
            }
            if (saveUser.Passwd != null)
            {
                sbSet.Append(string.Format("passwd='{0}',", saveUser.Passwd));

                // 校验
                if (PubClass.IsNull(saveUser.Passwd))
                {
                    result.IsSuccess = false;
                    result.Message = "密码不可为空 !";
                    return result;
                }
            }
            if (saveUser.Birthday != null)
            {
                sbSet.Append(string.Format("birthday='{0}',", saveUser.Birthday));

                // 校验
                if (PubClass.IsNull(saveUser.Birthday))
                {
                    result.IsSuccess = false;
                    result.Message = "生日不可为空 !";
                    return result;
                }
            }
            if (saveUser.BabyName != null)
            {
                sbSet.Append(string.Format("BabyName='{0}',", saveUser.BabyName));

                // 校验
                if (PubClass.IsNull(saveUser.BabyName))
                {
                    result.IsSuccess = false;
                    result.Message = "宝宝名称不可为空 !";
                    return result;
                }
            }
            if (saveUser.SelfSign != null)
            {
                sbSet.Append(string.Format("SelfSign='{0}',", saveUser.SelfSign));
            }
            if (saveUser.Province != null)
            {
                sbSet.Append(string.Format("Province='{0}',", saveUser.Province));

                // 校验
                if (PubClass.IsNull(saveUser.Province))
                {
                    result.IsSuccess = false;
                    result.Message = "省市不可为空 !";
                    return result;
                }
            }
            if (saveUser.City != null)
            {
                sbSet.Append(string.Format("City='{0}',", saveUser.City));

                // 校验
                if (PubClass.IsNull(saveUser.City))
                {
                    result.IsSuccess = false;
                    result.Message = "省市不可为空 !";
                    return result;
                }
            }
            if (saveUser.County != null)
            {
                sbSet.Append(string.Format("County='{0}',", saveUser.County));
                
            }
        }

        if (sbSet.Length == 0)
        {
            result.IsSuccess = false;
            result.Message = "没有传入更新的字段";
            return result;
        }
        
        string strNow = PubClass.GetStringFromDate(DateTime.Now);
        string updateSQL = string.Format("update hbh_user set {1} SysVersion=SysVersion+1,ModifiedBy='{2}',ModifiedOn='{3}'  where id='{0}';"
            , saveUser.ID
            , sbSet.ToString(),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            , string.Empty
            , strNow
            );

        mysqlHelper.ExecuteNonQuery(CommandType.Text, updateSQL, new MySql.Data.MySqlClient.MySqlParameter());
        try
        {
            List<Entities.User> updatedList = EntityHelper.GetUserEntityBySQL(selectSQL, SetOfBookType.HBHBaby);//

            result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(updatedList);
            result.IsSuccess = true;
            result.Message = "更新成功！";
        }
        catch (Exception ex)
        {

            result.IsSuccess = false;
            result.Message = ex.Message;
        }
        return result;
    }

    private string GetSubjectivityQuestionByTID(long userid, long tid)
    {
        int pageSize = 0;
        int pageNum = 0;
        string where = "";
        where = "where questioner ='{0}' and sub_Qes_T='{1}'";
        where = string.Format(where, userid, tid);
        string result = GetSubjectivityQuestion(userid, pageSize, pageNum, where);
        return result;
    }


    /// <summary>
/// 根据用户id查出该用户提出的所有的主观问题
/// </summary>
/// <param name="user_id"></param>
/// <returns></returns>
    private string GetSubjectivityQuestion(long user_id,int pageSize,int pageNum,string where)
    {
        string sql = "";
        string whereTJ = "where questioner ='{0}'";
        if (string.IsNullOrEmpty(where))
        {
            whereTJ = string.Format(whereTJ, user_id);
            where = whereTJ; 
        }
       
        SetOfBookType setOfBookType = SetOfBookType.HBHBaby;
        if (pageSize <= 0)
        {
            //sql = "select id,keyWords,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate,questioner,subQue_Result,aboutAge from HBH_SubQuestion {0};";
            sql = "select id,Sub_Qes_title,question_Content,keyWords,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate,questioner,subQue_Result,aboutAge,sub_Qes_T from HBH_SubQuestion {0};";
            sql = string.Format(sql, where);

        }
        else
        {
            int start = (pageNum - 1) * pageSize;
            sql = "select id,Sub_Qes_title,question_Content,keyWords,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate,questioner,subQue_Result,aboutAge,sub_Qes_T from HBH_SubQuestion {0} limit {1},{2};";
            //sql = "select id,keyWords,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate,questioner,subQue_Result,aboutAge from HBH_SubQuestion {0} limit {1},{2};";
            sql = string.Format(sql, where, start, pageSize);


        }
        PostHelper.PostResult result = new PostHelper.PostResult();

        MySqlHelper mysqlHelper = new MySqlHelper(setOfBookType);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.Subjectivity_Question> list = Entities.Subjectivity_Question.GetSubjectivityQuestionFromTable(ds);


            if (list != null && list.Count>0)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query Result is null!";
            }

        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }
        string totalNumSql = "select id,Sub_Qes_title,question_Content,keyWords,date_format(messDate,'%Y-%c-%d %H:%i:%s') as messDate,questioner,subQue_Result,aboutAge,sub_Qes_T from HBH_SubQuestion where questioner ='{0}';";
        totalNumSql = string.Format(totalNumSql, user_id);
        int totalNum = mysqlHelper.GetRowsCount(totalNumSql, setOfBookType);

        Entities.CommonPageMessage pageMessage = SetPageMessage(pageSize, pageNum, totalNum);

        result.ResultJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(pageMessage);

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }

    private static Entities.CommonPageMessage SetPageMessage(int pageSize, int pageNum, int totalNum)
    {
        Entities.CommonPageMessage pageMessage = new Entities.CommonPageMessage();

        if (pageSize <= 0)
        {
            pageMessage.PageNum = 1;
            pageMessage.PageSize = totalNum;
            pageMessage.TotalNum = totalNum;
            pageMessage.TotalPage = 1;
        }
        else
        {
            pageMessage.PageNum = pageNum;
            pageMessage.PageSize = pageSize;
            pageMessage.TotalNum = totalNum;
            pageMessage.TotalPage = (totalNum + pageSize - 1) / pageSize;
        }
        return pageMessage;
    }

    private static PostHelper.PostResult GetSubQuestionResult(string sql)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.Subjectivity_Question> list = Entities.Subjectivity_Question.GetSubjectivityQuestionFromTable(ds);


            if (list != null && list.Count>0)
            {
                result.IsSuccess = true;
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                result.Message = "Query Successfully!";
                result.ResultJson = strJson;

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Query Result is null!";
            }

        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }
        return result;
    }




    private string  GetPic()
    {
        List<Entities.Subjectivity_Question> lst = new List<Entities.Subjectivity_Question>();
        for (int i = 0; i < 3; i++)
        {
            Entities.Subjectivity_Question entity = new Entities.Subjectivity_Question();
            entity.AboutAge = "1-4岁";
            entity.KeyWords = "k"+i.ToString();
            entity.MessDate = DateTime.Now;
            entity.Question_Content = "内容测试"+i.ToString();
            entity.Sub_Qes_title = "标题测试" + i.ToString();
            entity.Questioner = 1;
            entity.SubQue_Result = "";

            lst.Add(entity);

        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
    }


    private string GetKeywordsQuestion(long userID, string strUserAccount)  //, int pageSize, int pageNum)
    {
        return GetKeywordsQuestion(userID, strUserAccount,3);
    }

    private string GetKeywordsQuestion(long userID, string strUserAccount,int level)  //, int pageSize, int pageNum)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();
        /*
        select 
	age.AgeBegin
    ,age.AgeEnd
	,age.MonthBegin
    ,age.MonthEnd
    ,user.name
    ,menu.*
from t_menu menu
	inner join t_agegroup age
    on menu.AgeGroupName = age.Name
    inner join hbh_user user
    on (year(now())- year(user.birthday)) * 12 + (month(now())- month(user.birthday))
		between (ifnull(age.AgeBegin,0) * 12 + ifnull(age.MonthBegin,0)) 
			and (ifnull(age.AgeEnd,0) * 12 + ifnull(age.MonthEnd,0))
where
	menu.Sequence not like '%.%'
	-- and 
         */
        /*
select 
	child.*
from t_menu menu
	inner join t_agegroup age
    on menu.AgeGroupName = age.Name
    inner join hbh_user user
    on (year(now())- year(user.birthday)) * 12 + (month(now())- month(user.birthday))
		between (ifnull(age.AgeBegin,0) * 12 + ifnull(age.MonthBegin,0)) 
			and (ifnull(age.AgeEnd,0) * 12 + ifnull(age.MonthEnd,0))
	inner join t_menu child
    on child.ParentMenu = menu.ID
where
	menu.Sequence not like '%.%'
         */
        /*
select 
	child.ID as ID
    ,child.Code as KeyWords
	,secondChild.ID as SecondID
    ,secondChild.Code as SecondKeyWords
    ,question.ID as ThirdID
    ,question.KeyWords as ThirdKeyWords
    -- ,menu.Sequence
from t_menu menu
	inner join t_agegroup age
    on menu.AgeGroupName = age.Name
    inner join hbh_user user
    on (year(now())- year(user.birthday)) * 12 + (month(now())- month(user.birthday))
		between (ifnull(age.AgeBegin,0) * 12 + ifnull(age.MonthBegin,0)) 
			and (ifnull(age.AgeEnd,0) * 12 + ifnull(age.MonthEnd,0))
	inner join t_menu child
    on child.ParentMenu = menu.ID
	inner join t_menu secondChild
    on secondChild.ParentMenu = child.ID
    inner join t_question question
    on question.ParentMenu = secondChild.ID
--    inner join t_solution solution
--    on solution.Question = question.ID
where
	menu.Sequence not like '%.%' 
         */
        //string selSql = "select * from hbh_Sub_Qes_T";
        string selSql = @"
select 
	child.ID as ID
    ,child.Code as KeyWords
	,secondChild.ID as SecondID
    ,secondChild.Code as SecondKeyWords
    ,question.ID as ThirdID
    ,question.KeyWords as ThirdKeyWords
from t_menu menu
	inner join t_agegroup age
    on menu.AgeGroupName = age.Name
    inner join hbh_user user
    on (year(now())- year(user.birthday)) * 12 + (month(now())- month(user.birthday))
		between (ifnull(age.AgeBegin,0) * 12 + ifnull(age.MonthBegin,0)) 
			and (ifnull(age.AgeEnd,0) * 12 + ifnull(age.MonthEnd,0))
	inner join t_menu child
    on child.ParentMenu = menu.ID
	inner join t_menu secondChild
    on secondChild.ParentMenu = child.ID
    inner join t_question question
    on question.ParentMenu = secondChild.ID
--    inner join t_solution solution
--    on solution.Question = question.ID
where
	menu.Sequence not like '%.%' ";
        string sql = string.Empty;
        if (userID > 0)
        {
            sql = string.Format("{0} and user.id = {1}", selSql, userID);
        }
        else if (!PubClass.IsNull(strUserAccount))
        {
            sql = string.Format("{0} and user.account = '{1}'", selSql, strUserAccount);
        }
        else
        {
            result.IsSuccess = false;
            result.Message = "传入用户参数异常!";

            //string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //return strResult;
        }

        //int start = (pageNum - 1) * pageSize;
        //sql = string.Format("{0} limit {1},{2};", sql, start, pageSize);

        // 参数有效,则执行查询
        if (!PubClass.IsNull(sql))
        {
            ////if (string.IsNullOrEmpty(aboutAge))
            ////{
            ////    return "没有得到宝宝年龄！无法获取问题！";
            ////}
            //string age = EntityHelper.GetBabyAgeGroup(aboutAge);

            //string sql = string.Format("select id,question_Title,aboutage,keywords from hbh_Sub_Qes_T where aboutage = '{0}';", age);
            ////string sql = "select aboutUs_Content from AboutUs ; show variables like 'char%';";

            MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

            DataSet ds = new DataSet();

            try
            {
                mysqlHelper.Fill(ds, CommandType.Text, sql);

                List<Entities.Keywords_Question> list;
                if (level == 3)
                {
                    list = Entities.Keywords_Question.GetFromDataSet(ds);
                }
                else if (level == 2)
                {
                    list = Entities.Keywords_Question.GetFromDataSet2(ds);
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "暂不支持2,3以外其他层次查询关键字!";
                    string rtnString = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    return rtnString;
                }
               

                if (list != null && list.Count > 0)
                {
                    //foreach (Entities.Keywords_Question entity in list)
                    //{
                    //    string title = entity.Question_Title;
                    //    string[] titleParts = title.Split('_');
                    //    if (titleParts.GetLength(0) >= 2)
                    //    {
                    //        entity.Question_Title_Part1 = titleParts[0];
                    //        entity.Question_Title_Part2 = titleParts[1];
                    //    }
                    //}
                    result.IsSuccess = true;
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.Message = "Query Successfully!";
                    result.ResultJson = strJson;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Query Result is null!";
                }

                //if (ds.Tables.Count > 1)
                //{

                //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

                //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                //    result.ResultJson2 = strJson2;
                //}
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }

    private string GetKeywordsHistory(long userID ,string strUserAccount,int pageSize, int pageNum)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();
        /*
select 
	messDate as CreatedTime
    ,aboutAge as Age
    ,keywords as KeyWords
    ,solution.SText as Solution
from HBH_SubQuestion answer
	inner join hbh_user user
    on answer.questioner = user.id
    left join t_solution solution
    on answer.sub_qes_t = solution.Question
where
	1=1
	-- and 
         */
        // 2015-06-05 wf  ； 客户提bug ，要显示录入的关键字，所以拼接出来；
        // 2015-06-05 wf  ； 客户提bug ，显示答案，第一行显示关键字；
        string selSql = @"
select 
	messDate as CreatedTime
    ,answer.aboutAge as Age
    ,answer.sub_qes_t as QuestionID
    ,concat(parentMenu.Code,':',answer.keywords) as KeyWords
    ,concat(parentMenu.Code,':',answer.keywords,' \r\n \r\n ',solution.SText) as Solution
    ,user.id as UserID
    ,user.Account as UserAccount
    ,concat(SubString(Trim(Replace(Replace(Replace(Replace(solution.SText,'原因分析：',''),'原因分析:',''),char(10),''),char(13),'')),1,20),'...') as SolutionSummary
from HBH_SubQuestion answer
	inner join hbh_user user
    on answer.questioner = user.id
    left join t_solution solution
    on answer.sub_qes_t = solution.Question
    left join t_question parentQuestion
    on parentQuestion.ID = solution.Question
    left join t_menu parentMenu
    on parentMenu.ID = parentQuestion.ParentMenu
where
	1=1";
        string sql = string.Empty;
        if (userID > 0)
        {
            sql = string.Format("{0} and user.id = {1}", selSql, userID);
        }
        else if (!PubClass.IsNull(strUserAccount))
        {
            sql = string.Format("{0} and user.account = '{1}'", selSql, strUserAccount);
        }
        else
        {
            result.IsSuccess = false;
            result.Message = "传入用户参数异常!";

            //string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //return strResult;
        }
        
        int start = (pageNum - 1) * pageSize;
        sql = string.Format("{0} limit {1},{2};", sql, start, pageSize);

        // 参数有效,则执行查询
        if (!PubClass.IsNull(sql))
        {
            ////if (string.IsNullOrEmpty(aboutAge))
            ////{
            ////    return "没有得到宝宝年龄！无法获取问题！";
            ////}
            //string age = EntityHelper.GetBabyAgeGroup(aboutAge);

            //string sql = string.Format("select id,question_Title,aboutage,keywords from hbh_Sub_Qes_T where aboutage = '{0}';", age);
            ////string sql = "select aboutUs_Content from AboutUs ; show variables like 'char%';";

            MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

            DataSet ds = new DataSet();

            try
            {
                mysqlHelper.Fill(ds, CommandType.Text, sql);

                List<Entities.Keywords_QuestionHistory> list = Entities.Keywords_QuestionHistory.GetFromDataSet(ds);


                if (list != null && list.Count > 0)
                {
                    //foreach (Entities.Keywords_Question entity in list)
                    //{
                    //    string title = entity.Question_Title;
                    //    string[] titleParts = title.Split('_');
                    //    if (titleParts.GetLength(0) >= 2)
                    //    {
                    //        entity.Question_Title_Part1 = titleParts[0];
                    //        entity.Question_Title_Part2 = titleParts[1];
                    //    }
                    //}
                    result.IsSuccess = true;
                    string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    result.Message = "Query Successfully!";
                    result.ResultJson = strJson;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Query Result is null!";
                }

                //if (ds.Tables.Count > 1)
                //{

                //    List<Entities.Characters> list2 = Entities.Characters.GetCharactersFromTable(ds.Tables[1]);

                //    string strJson2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                //    result.ResultJson2 = strJson2;
                //}
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
        }

        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }




    // 用户注册
    private PostResult RegisterUser(string userJson, string strIdentifyCode)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        if (!string.IsNullOrEmpty(userJson))
        {
            Entities.UserRegister register = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.UserRegister>(userJson);

            // 校验，用户名、密码、验证码
            string errMsg = Validate(register , strIdentifyCode);

            if (!PubClass.IsNull(errMsg))
            {
                result.IsSuccess = false;
                result.Message = string.Format("{0} !", errMsg);
                return result;
            }
            
            string phoneNumber = register.TelNo;
            // 校验码非空，检查校验码是否有效、过期
            if (!PubClass.IsNull(strIdentifyCode))
            {
                int overDueMinute = Config.SmsOverDueMinute;

                // 是否验证通过
                bool isIdentified = IdentifyCodeValidate(strIdentifyCode, phoneNumber, overDueMinute);

                if (!isIdentified)
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("验证码不正确或已过期(过期时间{0}分钟) !", overDueMinute);
                    return result;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "必须输入验证码 !";
                return result;
            }

            // 用户账号 默认 = 手机号
            string strAccount = register.Account;
            if (PubClass.IsNull(strAccount))
            {
                //result.IsSuccess = false;
                //result.Message = "账号不可为空!";

                //return result;
                strAccount = phoneNumber;
            }

            // 验证码通过，保存用户信息
            try
            {
                if (register != null)
                {
                    MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

                    List<long> ids = new List<long>();


                    //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
                    string existsSQL = string.Format(Const_UserQueryAccount, strAccount);
                    List<Entities.User> existsList = EntityHelper.GetUserEntityBySQL(existsSQL, SetOfBookType.HBHBaby);
                    //有ID传入且  数据库里有此ID的数据
                    if (existsList != null && existsList.Count > 0)
                    {
                        result.IsSuccess = false;
                        result.Message = "该账号或手机已被注册,请选择忘记密码 !";
                        return result;
                    }
                    else
                    {
                        //string insertSQL = string.Format("insert into HBH_User (address,region,account, name,sex,tel,pic,passwd,birthday,SysVersion) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');  select @@IDENTITY ;SELECT LAST_INSERT_ID();"
                        //, saveUser.Address, saveUser.Region, saveUser.Account, saveUser.Name, saveUser.Sex, saveUser.Tel, saveUser.Pic, saveUser.Passwd, saveUser.Birthday, 0
                        //    // ,saveUser.CreatedOn,saveUser.CreatedBy,saveUser.ModifiedBy,saveUser.ModifiedOn
                        //);
                        string strNow = PubClass.GetStringFromDate(DateTime.Now);
                        string insertSQL = string.Format("insert into HBH_User ( account,tel,passwd,SysVersion,CreatedOn,CreatedBy,ModifiedBy,ModifiedOn) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');  select @@IDENTITY ;SELECT LAST_INSERT_ID();"
                        , register.Account, register.TelNo, register.Pwd
                        , 0, strNow, string.Empty, strNow, string.Empty
                        );

                        DataTable dt = new DataTable();
                        long id = UIHelper.GetLong(mysqlHelper.ExecuteScalar(CommandType.Text, insertSQL, new MySql.Data.MySqlClient.MySqlParameter()));
                        if (id > 0)
                        {
                            string selectSQL = string.Format(Const_UsersIn, id.ToString());
                            List<Entities.User> list = EntityHelper.GetUserEntityBySQL(selectSQL, SetOfBookType.HBHBaby);

                            //string hintMessage = "欢迎你加入鑫宝贝慧爱家族!";

                            if (list != null && list.Count > 0)
                            {
                                result.IsSuccess = true;
                                result.Message = string.Empty;
                                result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                                return result;

                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = "根据新增ID获取用户失败 !";
                                result.ResultJson = null;
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "新增数据失败 !";
                            result.ResultJson = null;
                        }

                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "反序列化失败 ! 没有得到实体List.";
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
        }
        //string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        //return strResult;

        return result;
    }

    private string Validate(UserRegister register, string strIdentifyCode)
    {
        if (PubClass.IsNull(register.TelNo))
        {
            return "手机号不可为空";
        }
        if (PubClass.IsNull(register.Pwd))
        {
            return "密码不可为空";
        }
        if (PubClass.IsNull(strIdentifyCode))
        {
            return "验证码不可为空";
        }
        return string.Empty;
    }

    private static bool IdentifyCodeValidate(string strIdentifyCode, string phoneNumber, int overDueMinute)
    {
        bool isIdentified = false;

        string sql = string.Format("select * from hbh_SMS where PhoneNumber = '{0}' and IdentifyCode = '{1}' order by CreatedOn desc limit 0,1;"
            , phoneNumber, strIdentifyCode);
        ////string sql = "select aboutUs_Content from AboutUs ; show variables like 'char%';";

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            DataTable dt = PubClass.GetTable(ds, 0);

            if (dt != null
                && dt.Rows != null
                && dt.Rows.Count > 0
                )
            {
                object objTime = dt.Rows[0]["CreatedOn"];

                DateTime createdTime = PubClass.GetDateTime(objTime, new DateTime(1999, 1, 1));

                TimeSpan span = DateTime.Now - createdTime;

                // 小于10分钟认为是对的
                if (span.TotalMinutes <= overDueMinute)
                {
                    isIdentified = true;
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
        return isIdentified;
    }

    public static string GetResultJson(PostResult result)
    {
        string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        return strResult;
    }



    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="user">user实体的Json字符串</param>
    /// <returns></returns>
    private PostResult ModifyPassword(string userJson, string strIdentifyCode, string strOldPwd)
    {
        PostHelper.PostResult result = new PostHelper.PostResult();

        Entities.User user = null;

        if (!string.IsNullOrEmpty(userJson))
        {
            user = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.User>(userJson) as Entities.User;
        }

        try
        {
            Entities.User saveUser = user;

            if (saveUser != null)
            {
                List<Entities.User> list = null;

                // 未登录，通过验证码 改 密码
                if (saveUser.ID <= 0)
                {
                    if (!PubClass.IsNull(saveUser.Tel))
                    {
                        // 验证码非空，验证校验码
                        if (!PubClass.IsNull(strIdentifyCode))
                        {
                            int overDueMinute = Config.SmsOverDueMinute;
                            bool bl = IdentifyCodeValidate(strIdentifyCode, saveUser.Tel, overDueMinute);

                            if (!bl)
                            {
                                result.IsSuccess = false;
                                result.Message = string.Format("验证码不正确或已过期(过期时间{0}分钟)!", overDueMinute);
                                return result;
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = string.Format("验证码不可为空!");
                            return result;
                        }
                    }

                    //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
                    string selectSQL = string.Format(Const_UserQueryTel, saveUser.Tel);
                    list = EntityHelper.GetUserEntityBySQL(selectSQL, SetOfBookType.HBHBaby);
                }
                // 登陆后，通过ID 更新 密码
                else
                {
                    //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
                    string selectSQL = string.Format(Const_UserQueryID, saveUser.ID);
                    list = EntityHelper.GetUserEntityBySQL(selectSQL, SetOfBookType.HBHBaby);
                }

                MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

                List<long> ids = new List<long>();


                //long saveUserId = saveUser.ID;
                ////有ID更新
                //if (saveUserId > 0)
                //{
                //    //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
                //    string selectSQL = string.Format(Const_UserQueryID, saveUser.ID);
                //    list = EntityHelper.GetUserEntityBySQL(selectSQL, SetOfBookType.HBHBaby);


                //if(list != null
                //    && list.Count > 0
                //    )
                {

                    //有ID传入且  数据库里有此ID的数据
                    if (list != null && list.Count > 0)
                    {
                        //saveUser.Account = list[0].Account;
                        //saveUser.Tel = list[0].Tel;

                        ////ID大于0、版本小于0时，强制更新
                        //if (saveUser.SysVersion <= 0)
                        //{
                        //    result = UpdatePassword(saveUser);
                        //}
                        //else
                        {
                            //foreach (Entities.User entity in list)
                            Entities.User entity = list[0];
                            {
                                result.ResultJson = "";

                                saveUser.ID = entity.ID;

                                // 旧密码非空，且旧密码不一致，报错；
                                if (!PubClass.IsNull(strOldPwd)
                                    && strOldPwd != entity.Passwd
                                    )
                                {
                                    result.IsSuccess = false;
                                    result.Message = "旧密码不正确!";
                                    return result;
                                }
                                else
                                {
                                    //ID大于0、版本小于0时，强制更新
                                    if (saveUser.SysVersion <= 0)
                                    {
                                        result = UpdatePassword(saveUser);
                                    }
                                    //判断版本大于0 并且 与数据库这个用户的版本比较，如果app端版本大
                                    else if (entity.SysVersion <= saveUser.SysVersion)
                                    {
                                        result = UpdatePassword(saveUser);
                                    }
                                    else //否则不更新,返回重新查出最新的用户数据
                                    {

                                        result.IsSuccess = false;
                                        result.Message = "已被他人修改,更新不成功!";
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(result.ResultJson))
                            {
                                result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                            }
                        }
                    }
                    else//有ID传入，但数据库里没有查到此ID的数据
                    {
                        result.IsSuccess = false;
                        result.Message = "用户不存在！不执行更新！";
                    }

                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "反序列化失败！没有得到实体List";
            }

        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
        }
        //string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        //return strResult;

        return result;
    }



    private static PostHelper.PostResult UpdatePassword(Entities.User saveUser)
    {

        PostHelper.PostResult result = new PostHelper.PostResult();
        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.HBHBaby);

        //string selectSQL = string.Format("select * from hbh_user where id = {0} ", saveUser.ID);
        string selectSQL = string.Format(Const_UserQueryID, saveUser.ID);
        // ,CreatedBy='{10}',CreatedOn='{11}',ModifiedBy='{12}',ModifiedOn='{13}'
        string strNow = PubClass.GetStringFromDate(DateTime.Now);
        string updateSQL = string.Format("update hbh_user set passwd='{1}',SysVersion=SysVersion+1,ModifiedBy='{2}',ModifiedOn='{3}' where id='{0}';"
            , saveUser.ID
            , saveUser.Passwd
            , string.Empty
            , strNow
       );
        mysqlHelper.ExecuteNonQuery(CommandType.Text, updateSQL, new MySql.Data.MySqlClient.MySqlParameter());
        try
        {
            List<Entities.User> updatedList = EntityHelper.GetUserEntityBySQL(selectSQL, SetOfBookType.HBHBaby);//

            result.ResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(updatedList);
            result.IsSuccess = true;
            result.Message = "密码更新成功！";
        }
        catch (Exception ex)
        {

            result.IsSuccess = false;
            result.Message = ex.Message;
        }
        return result;
    }


}
