using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading.Tasks;
using System.Reflection;

namespace WebService.REST
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            foreach (var str in Отправители.Produser1C8)
            {
                Task<string> task = Methods1C8.ResponseAsync(str);
            }

            foreach (var str in Получатели.Consumer1С8)
            {
                Task<string> task = Methods1C8.PostObject(str);
            }

            Task<string> task0 = SqlConnector.PostObject();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Methods1C7.Disconnect();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Methods1C7.Disconnect();
        }
    }
}