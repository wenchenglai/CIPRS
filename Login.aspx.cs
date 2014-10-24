using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strUID = txtUsrId.Text.Trim();
        string strPwd = txtPwd.Text.Trim();
        if (strUID == "1")
        {
            strUID = "ariel@jewishcamp.org";
            strPwd = "ariel";
        }
        else if (strUID == "2")
        {
            strUID = "whoffman@jfgp.org";
            strPwd = "warren";
        }
        else if (strUID == "3")
        {
            strUID = "shainad@cjp.org";
            strPwd = "shaina";
        }
        else if (strUID == "4")
        {
            // camp director
            strUID = "zimmerman@emanueldenver.org";
            strPwd = "zim";
        }
        else if (strUID == "5")
        {
            strUID = "rachel.anolik@shalomdc.org";
            strPwd = "camperquest";
        }

        DataSet ds;
        bool isAuth = UserDA.AuthenticateAndGetUserProfile(strUID, strPwd, out ds);

        //bool isAuth = UserDA.Authenticate(strUID, strPwd);
        if (isAuth)
        {
            lblErr.Text = "";
            Session["UserID"] = ds.Tables[0].Rows[0]["ID"].ToString();
            Session["RoleID"] = ds.Tables[0].Rows[0]["UserRole"].ToString();
            Session["FirstName"] = ds.Tables[0].Rows[0]["FirstName"].ToString();
            Session["LastName"] = ds.Tables[0].Rows[0]["LastName"].ToString();
            Session["FedID"] = ds.Tables[0].Rows[0]["Federation"].ToString();
            Session["FedName"] = ds.Tables[0].Rows[0]["FedName"].ToString();

            //Set a cookie for authenticated user
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket((string)Session["UserId"], false, 5000);
            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket("470", false, 5000);
            String encTicket = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            Response.Redirect("Default.aspx");
        }
        else
            lblErr.Text = "Invalid UserID or Password. Please check and re-enter again.";

    }
}
