using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Utilities
{
   public   class WebServices
    {



        //Create Channel To Services
        //IP :- String URL ,OP :- Object from Channel according to Interface
       public static IServices getChannel<IServices>(string StrURL)
        {
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            //  set security mode
            basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;// BasicHttpSecurityMode.Transport;
            basicHttpBinding.MaxReceivedMessageSize = 2147483647;

            TimeSpan _TimeMax = TimeSpan.FromHours(99);
            basicHttpBinding.CloseTimeout = _TimeMax;
            basicHttpBinding.OpenTimeout = _TimeMax;
            basicHttpBinding.ReceiveTimeout  = _TimeMax;
            basicHttpBinding.SendTimeout = _TimeMax;
           

            Uri WebServiceUrlPath = new Uri(StrURL);
            //define the end point( the server which host the service)
            EndpointAddress endpointAddress = new EndpointAddress(WebServiceUrlPath);
            IServices Channel = new ChannelFactory<IServices>(basicHttpBinding, endpointAddress).CreateChannel();
            return Channel;
        }
        //+
    }
}
