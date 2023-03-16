using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Specialized;

namespace WebService.REST
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            NameValueCollection prod = Отправители.Produser1C8;
            for (int i = 0; i < prod.AllKeys.Length; i++)
            {
                Task<string> task = Methods1C8.ResponseAsync(prod[i], i + 1);
            }

            NameValueCollection cons = Получатели.Consumer1С8;
            for (int i = 0; i < cons.AllKeys.Length; i++)
            {
                Task<string> task = Methods1C8.PostObject(cons[i], i + 1);
            }

            //Task<string> task0 = SqlConnector.PostObject();
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