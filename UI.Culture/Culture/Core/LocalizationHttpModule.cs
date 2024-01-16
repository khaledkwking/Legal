namespace Culture.Core
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;

    public class LocalizationHttpModule : IHttpModule
    {
        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpRequest request = ((HttpApplication) sender).Request;
            HttpContext context = ((HttpApplication) sender).Context;
            string applicationPath = request.ApplicationPath;
            if (applicationPath == "/")
            {
                applicationPath = string.Empty;
            }
            string path = request.Url.AbsolutePath.Substring(applicationPath.Length);
            if (path.ToLower().Contains(".aspx"))
            {
                this.LoadCulture(ref path);
                if (path.IndexOf(".") == -1)
                {
                }
                context.RewritePath(applicationPath + path);
            }
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.context_BeginRequest);
        }

        private void LoadCulture(ref string path)
        {
            string[] strArray = path.Trim(new char[] { '/' }).Split(new char[] { '/' });
            string name = "ar-KW";
            if ((strArray.Length > 1) && (strArray[0].Length > 0))
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(strArray[0]);
                    path = path.Remove(0, strArray[0].Length + 1);
                }
                catch (Exception exception)
                {
                    if (!((exception is ArgumentNullException) || (exception is ArgumentException)))
                    {
                        throw;
                    }
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
                }
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
            }
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}

