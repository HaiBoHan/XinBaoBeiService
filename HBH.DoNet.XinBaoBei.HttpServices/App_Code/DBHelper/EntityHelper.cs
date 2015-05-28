using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// GetEntityHelper 的摘要说明
/// </summary>
public class EntityHelper
{
    public EntityHelper()
    {
    }


    /// <summary>
    /// 根据宝宝年龄获取宝宝属于哪个年龄段
    /// </summary>
    /// <param name="aboutAge">规则为  2-3代表2岁3个月   0-3代表0岁3个月   4-0代表4岁0个月  </param>
    /// <returns></returns>
    public static string GetBabyAgeGroup(string aboutAge)
    {
        aboutAge = aboutAge.Trim();
        string[] ages = aboutAge.Split('-');
        int years = Convert.ToInt32(ages[0].ToString());
        int month = Convert.ToInt32(ages[1].ToString());

        return JudgeOwnAgeGroup(years, month);

    }

    private static string JudgeOwnAgeGroup(int year, int months)
    {

        if (0 <= year && year < 1)
        {
            if (0 <= months && months < 3)
            {
                return "0-3个月";
            }
            if (3 < months && months <= 6)
            {
                return "3-6个月";
            }
            if (6 < months && months <= 9)
            {
                return "6-9个月";
            }
            if (9 < months && months < 12)
            {
                return "9-12个月";
            }
        }
        if (1 <= year && year < 2)
        {
            if (0 <= months && year <= 6)
            {
                return "1-1.5岁";
            }
            if (6 < months && months <= 12)
            {
                return "1.5-2岁";
            }
        }

        if (2 <= year && year < 3)
        {
            return "2-3岁";
        }
        if (3 <= year && year < 5)
        {
            return "3-5岁";
        }
        if (5 <= year && year < 7)
        {
            return "5-7岁";
        }
        if (7 <= year && year <= 12)
        {
            return "7-12岁";
        }
        return "0";
    }

    //public static string WeekOfMonth(DateTime dt)      //第几周
    //{
       
    //    int daysOfWeek = 7;
    //    if (dt.AddDays(0 - daysOfWeek).Month <= dt.Month || dt.AddDays(0 - daysOfWeek).Month == 12)
    //        return "1周";
    //    if (dt.AddDays(0 - 2 * daysOfWeek).Month <= dt.Month)
    //        return "2周";
    //    if (dt.AddDays(0 - 3 * daysOfWeek).Month <= dt.Month)
    //        return "3周";
    //    if (dt.AddDays(0 - 4 * daysOfWeek).Month <= dt.Month)
    //        return "4周";

    //    return "5周";
    //}
    /// <summary>
    /// 得到x岁X个月x周第x天
    /// </summary>
    /// <param name="dtBirthday"></param>
    /// <param name="dtNow"></param>
    /// <returns></returns>
    public static string GetWeekDayByBirthday(DateTime dtBirthday, DateTime dtNow)
    {
        string week = GetWeekByBirthday(dtBirthday, dtNow);
        string intDay = GetWeekDaythByBirthday(dtBirthday, dtNow);
        string weekDay = week + intDay;
        return weekDay;
    }
    /// <summary>
    /// 得天某日期是一周的第几天
    /// </summary>
    /// <param name="dtBirthday"></param>
    /// <param name="dtNow"></param>
    /// <returns></returns>
    public static string GetWeekDaythByBirthday(DateTime dtBirthday, DateTime dtNow)
    {

        int intDay = 0;                                      // 天


        // 计算天数
        intDay = dtNow.Day - dtBirthday.Day;
        if (intDay < 0)
        {
            dtNow = dtNow.AddMonths(-1);
            intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        }
        int week = intDay / 7 + 1;
        int intDaith = 0;
        //目前暂时处理成第五周显示 成第四周的内容  ，则显示第四周第5天-第7天的内容。
        if (week == 5 )
        {
            intDaith = 5;
        }
        else
        {
            intDaith = intDay % 7 + 1;
        }


        return "第" + intDaith.ToString() + "天";
    }

    /// <summary>
    /// 得到x岁x个月x周
    /// </summary>
    /// <param name="dtBirthday"></param>
    /// <param name="dtNow"></param>
    /// <returns></returns>
    public static string GetWeekByBirthday(DateTime dtBirthday, DateTime dtNow)
    {
        string strAge = "";
        int intYear = 0;                                     // 岁
        int intMonth = 0;                                    // 月
        int intDay = 0;                                      // 天

        //// 如果没有设定出生日期, 返回空
        //if (DataType.DateTime_IsNull(ref dtBirthday) == true)
        //{
        //    return string.Empty;
        //}

        // 计算天数
        intDay = dtNow.Day - dtBirthday.Day;
        if (intDay < 0)
        {
            dtNow = dtNow.AddMonths(-1);
            intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        }
        int week = intDay / 7 +1;
        if (week == 5)//目前暂时处理成第五周显示成第四周的内容。
            week = week - 1;
       

        // 计算月数
        intMonth = dtNow.Month - dtBirthday.Month;
        if (intMonth < 0)
        {
            intMonth += 12;
            dtNow = dtNow.AddYears(-1);
        }

        // 计算年数
        intYear = dtNow.Year - dtBirthday.Year;


        strAge = intYear.ToString() + "岁" + intMonth.ToString() + "个月" +week.ToString()+"周";

        return strAge;

    }

    /// <summary>
    /// 出生日期转换成年龄  岁月  得到x岁x个月xx天
    /// </summary>
    /// <param name="dtBirthday"></param>
    /// <param name="dtNow"></param>
    /// <returns></returns>
    public static string GetAgeByBirthday(DateTime dtBirthday, DateTime dtNow)
    {
        string strAge = string.Empty;                        // 年龄的字符串表示
        int intYear = 0;                                     // 岁
        int intMonth = 0;                                    // 月
        int intDay = 0;                                      // 天

        //// 如果没有设定出生日期, 返回空
        //if (DataType.DateTime_IsNull(ref dtBirthday) == true)
        //{
        //    return string.Empty;
        //}

        // 计算天数
        intDay = dtNow.Day - dtBirthday.Day;
        if (intDay < 0)
        {
            dtNow = dtNow.AddMonths(-1);
            intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        }

        // 计算月数
        intMonth = dtNow.Month - dtBirthday.Month;
        if (intMonth < 0)
        {
            intMonth += 12;
            dtNow = dtNow.AddYears(-1);
        }

        // 计算年数
        intYear = dtNow.Year - dtBirthday.Year;

        // 格式化年龄输出
        if (intYear >= 1)                                            
        {
            strAge = intYear.ToString() + "岁";
        }

        if (intMonth > 0)                          
        {
            strAge += intMonth.ToString() + "个月";
        }

        if (intDay >= 0)                             
        {
            strAge += intDay.ToString() +"天";
           
        }

        return strAge;
    }

    public static List<Entities.User> GetUserEntityBySQL(string sql,SetOfBookType setofType)
    {

        MySqlHelper mysqlHelper = new MySqlHelper(setofType);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            List<Entities.User> list = Entities.User.GetUserFromTable(ds);

            if (list != null && list.Count > 0)
            {
                return list;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        return null;
    }


    public delegate T GetFromRow<T>(DataRow row);

    public static List<T> GetFromTable<T>(DataTable table, GetFromRow<T> getRow)
    {
        if (table.Rows != null
            && table.Rows.Count > 0
            )
        {
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T question = getRow(row);

                if (question != null)
                {
                    list.Add(question);
                }
            }

            return list;
        }

        return null;
    }


    public static List<T> GetFromDataSet<T>(DataSet ds, GetFromRow<T> getRow)
    {
        if (ds != null
            && ds.Tables != null
            && ds.Tables.Count > 0
            )
        {
            return GetFromTable<T>(ds.Tables[0], getRow);
        }

        return null;
    }
}
