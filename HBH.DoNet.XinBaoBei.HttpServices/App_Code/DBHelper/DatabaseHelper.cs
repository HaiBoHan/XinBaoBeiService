using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;

/// <summary>
///DatabaseHelper 的摘要说明
/// </summary>
public interface DatabaseHelper
{
    //public DatabaseHelper();

    //public DatabaseHelper(string SqlConnStr);


    int ExecuteNonQuery(CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters);

    object ExecuteScalar(CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters);

    void Fill(DataSet ds, CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters);

    void Fill(DataTable dt, CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters);

    int ExecuteNonQuery(CommandType p_CommandType, string p_CommandText, TableTypeParameter[][] p_Parameters);
}


// 帐套
/// <summary>
/// 帐套
/// </summary>
public enum SetOfBookType
{
    
    /// <summary>
    /// HBHBaby
    /// </summary>
    HBHBaby = 0,

    // 测试
    /// <summary>
    /// 测评
    /// </summary>
    Test = 1,

}
