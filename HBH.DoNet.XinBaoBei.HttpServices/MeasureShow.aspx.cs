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
using System.Text;

public partial class MeasureShow : System.Web.UI.Page
{
    string strUserType = "judge_admin";//judge_user/judge_admin/super_admin

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = UIHelper.GetParam(this, "UserCode");
        string pwd = UIHelper.GetParam(this, "pwd");
     
       // strUserCode = "TEST";
            Login(strUserCode,pwd);
       
        

    }
    protected void Login (string account,string pwd)
    {
        

        PutSession(account,pwd);

        Response.Redirect("http://ceping.xinbaobeijiaoyu.com/judge_online.php", false);
       // Response.Redirect("http://ceping.xinbaobeijiaoyu.com/index.php", false);
    }

    private void PutSession(string strAccount,string pwd)
    {
        /*
        
if($usertype=='高级管理员'){
$tablename='super_admin';
$_SESSION['usertype'] = 'super_admin';
$_SESSION['id'] = $tablename->fields['id']; #超级管理员ID
$_SESSION['j_name']='超级管理员'; 
$_SESSION['j_account']=$tablename->fields['account']; #超级管理员编号
$_SESSION['unit_id'] = $tablename->fields['unit_id']; #超级管理员所属单位
$_SESSION['ck']=0; #拥有用户所有权限	
echo "<script language='javascript'>alert('登陆成功.');window.parent.href='index_ok.php';</script>";
exit;
}elseif($usertype=='普通管理员'){
$tablename='judge_admin';
#模拟登陆代码开始
#模拟登陆代码...
#模拟登陆代码结束
$_SESSION['usertype'] = 'judge_admin';  #用户类型
$_SESSION['id'] = $tablename->fields['id']; #管理员ID
$_SESSION['admin_id'] = $tablename->fields['id']; #管理员ID
$_SESSION['j_name']=$tablename->fields['name']; #管理员姓名
$_SESSION['j_account']=$tablename->fields['account']; #管理员编号
$_SESSION['unit_id'] = $tablename->fields['unit_id']; #管理员所属单位
$_SESSION['branch_id']= $tablename->fields['branch_id']; #管理员所属部门

$_SESSION['ck']=intval($tablename->fields['ck']); #管理员权限范围
echo "<script language='javascript'>alert('登陆成功.');window.parent.href='judge_user.php';</script>";
exit;	
}elseif($usertype=='测试者'){
$tablename='judge_user';
#模拟登陆代码开始
#模拟登陆代码...
#模拟登陆代码结束
$_SESSION['usertype'] = 'judge_user';  #用户类型
$_SESSION['id'] = $tablename->fields['id']; #用户ID
$_SESSION['j_name']=$tablename->fields['name']; #用户姓名
$_SESSION['j_account']=$tablename->fields['account']; #用户编号
$_SESSION['unit_id'] = $tablename->fields['unit_id']; #用户所属单位
$_SESSION['branch_id']= $tablename->fields['branch_id']; #用户所属部门
echo "<script language='javascript'>alert('登陆成功.');window.parent.href='judge_online.php';</script>";
exit;	
}else{
echo "<script language='javascript'>alert('请选择用户类型');</script>";
exit;
}
         */
        string sql = "";
        if (strUserType.Equals("judge_user"))
        {
            sql = "select id,account,passwd,unit_id from super_admin where 1=1 {0}; ";
        }
        else if(strUserType.Equals("judge_admin"))
        {
            sql = "select id,name,account,passwd,unit_id,branch_id,ck from judge_admin where 1=1 {0}; ";
        }
        else if (strUserType.Equals("super_admin"))
        {
            sql="select id,account,passwd,unit_id from super_admin where 1=1 {0}; ";
        }

       
        if (!UIHelper.IsNull(strAccount))
        {
            string where = string.Format(" and account ='{0}'", strAccount);
            sql = string.Format(sql, where);
        }
        else
        {
            // sql = string.Format(sql, "");

            Response.Write("javascript'>alert('请录入用户名.');window.parent.href='judge_online.php';</script>");
            Response.End();
        }

        MySqlHelper mysqlHelper = new MySqlHelper(SetOfBookType.Test);

        DataSet ds = new DataSet();

        try
        {
            mysqlHelper.Fill(ds, CommandType.Text, sql);

            if (ds != null
                && ds.Tables != null

                && ds.Tables.Count > 0
                && ds.Tables[0] != null
                && ds.Tables[0].Rows != null
                && ds.Tables[0].Rows.Count > 0
                )
            {
                DataRow row = ds.Tables[0].Rows[0];
                string password = row["password"].ToString();
                if (!password.Equals(pwd))
                {
                    string errorMsg = "<javascript'>alert('用户密码不正确');window.parent.href='judge_online.php';</script>";
                    Response.Write(errorMsg);
                    Response.End();
                }
                if (strUserType.Equals("super_admin"))
                {
                    this.Session["usertype"] = "super_admin";  // 用户类型
                    this.Session["id"] = row["id"]; // 管理员ID
                    this.Session["password"] = row["password"];
                    this.Session["j_name"] = "超级管理员"; // 管理员姓名
                    this.Session["j_account"] = row["account"]; // 管理员编号
                    this.Session["unit_id"] = row["unit_id"]; // 管理员所属单位
                    this.Session["ck"] = 0; // 权限
                }
                else if (strUserType.Equals("judge_admin"))
                {

                    this.Session["usertype"] = "judge_admin";  // 用户类型
                    this.Session["id"] = row["id"]; // 管理员ID
                    this.Session["admin_id"] = row["id"];
                    this.Session["j_name"] = row["name"]; // 管理员姓名
                    this.Session["j_account"] = row["account"]; // 管理员编号
                    this.Session["unit_id"] = row["unit_id"]; // 管理员所属单位
                    this.Session["branch_id"] = row["branch_id"]; // 管理员所属部门
                    this.Session["ck"]= row["ck"];//权限


                  //  this.Session["password"] = row["password"];
                }

                else if(strUserType.Equals("judge_user"))
                {

                    this.Session["usertype"] = "judge_user";  // 用户类型
                    this.Session["id"] = row["id"]; // 用户ID
                    this.Session["j_name"] = row["name"]; // 用户姓名
                    this.Session["j_account"] = row["account"]; // 用户编号
                    this.Session["unit_id"] = row["unit_id"]; // 用户所属单位
                    this.Session["branch_id"] = row["branch_id"]; // 用户所属部门


                   // this.Session["password"] = row["password"];
                }

                
            }
        }
        catch (Exception ex)
        {
            string errorMsg = string.Format("<javascript'>alert('用户查询异常,{0}.');window.parent.href='judge_online.php';</script>", ex.Message);
            Response.Write(errorMsg);
            Response.End();
        }
        finally
        {

        }
    }
}
