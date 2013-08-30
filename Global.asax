<%@ Application Language="C#" %>

<script runat="server">
    
    void Application_Start(object sender, EventArgs e) 
    {
        // Set the global environment variables
        
        // We need to get the login url when user is not authenticated
        string envir = ConfigurationManager.AppSettings["ServerEnvironment"].ToString();
        if (envir == "Production")
        {
            Application["RedirectLogin"] = "https://www.onehappycamper.org/CIPMS/Default.aspx";
        }
        else if (envir == "Development")
        {
            Application["RedirectLogin"] = "http://localhost/CIPRS/Login.aspx";
        }

        AppData data = AppDA.GetAppLevelData();
        Application["CampYearID"] = data.CampYearID;
        Application["CampYear"] = data.CampYear;
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

        // Get current user's data
        UserBL.SetUserDetailsInSession(Context.User.Identity.Name);
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        //if (Request.IsAuthenticated)
        //{
        //    FormsIdentity myIdent = (FormsIdentity)User.Identity;
        //    ////string[] aryRoles = myIdent.Ticket.UserData.Split(new Char[] { '|' });
        //    Context.User = new System.Security.Principal.GenericPrincipal(myIdent, null);
            
        //}
    }    
       
</script>
