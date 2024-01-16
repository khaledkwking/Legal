using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Utilities
{
    public class HttpWeb
    {
         
          public StringBuilder GetStrFromPage(string _strUrl, string strPost,string strLoginName,string strPassword ,ref CookieContainer cookies)
      {
            StringBuilder strBuilder = new StringBuilder();
            try
            {
              HttpWebRequest req = default(HttpWebRequest);
            HttpWebResponse resp = default(HttpWebResponse);
            
            byte[] b = null;
            req = (HttpWebRequest)System.Net.WebRequest.Create(_strUrl);
            System.Net.WebProxy myProxy = new System.Net.WebProxy();
            Uri myURI = new Uri(_strUrl);
            myProxy.IsBypassed(myURI);
            req.Proxy = myProxy;
            //time out 5.6 hours
            req.Timeout = 200000000;
            req.CookieContainer = cookies;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
           
              var credCache = new CredentialCache();
              credCache.Add(myURI, "Basic",new NetworkCredential(strLoginName, strPassword));
              req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
              req.Credentials = credCache;
              req.PreAuthenticate = true;
              b = System.Text.Encoding.GetEncoding(1256).GetBytes(strPost);
              req.ContentLength = b.Length;
             //Logger.LogIt("try to post URl:-postdata" + postdata,
             //               GrowthConstant.enumLOGType.INFORMATION.ToString(), ExchangeID);

              using (Stream newStream = req.GetRequestStream())
              { 
                  newStream.Write(b, 0, b.Length);
                  newStream.Close();
              } 

                resp = (HttpWebResponse)req.GetResponse();
                StreamReader webstream = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(1256));
                strBuilder.Append(webstream.ReadToEnd());
                webstream.Close();
            }
            catch (Exception)
            {

                throw;

            }
            return strBuilder;

        }
    }
}
