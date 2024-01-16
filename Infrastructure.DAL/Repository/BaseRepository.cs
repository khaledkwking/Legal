using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using DomainInterface;
using Infrastructure.DAL.Model;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class BaseRepository
    {

        public readonly CMGS_DBEntities DC;
        HttpContext context = HttpContext.Current;
        AuditLog_UserOperations _Auditobj;

        public JsonSerializerSettings _settings = new JsonSerializerSettings
        {
          //  NullValueHandling = NullValueHandling.Ignore
        };


        public BaseRepository(CMGS_DBEntities _context)
        {

                DC = _context;

        }

        //Save User Functions Aduit
        public void PostResultToAudit(int moduleCode, string userModule,string userFunction,string tages,string queryResult,int resultCount)
        {
            try
            {//Save User FunctionResult


                _Auditobj = new AuditLog_UserOperations();

                _Auditobj.resultCount =resultCount;
             //   _Auditobj.queryResult = (queryResult);
                _Auditobj.tages = (tages);
                _Auditobj.userFunction = userFunction;
                _Auditobj.userModule = userModule;
                _Auditobj.moduleCode = moduleCode;
                _Auditobj.UserID=Convert.ToInt32( context.Session["userid"]??0);
                _Auditobj.url = context.Request.Url.ToString();
                _Auditobj.TransDate = DateTime.Now;
              //  _Auditobj.UserHostName = Dns.GetHostEntry(context.Request.ServerVariables["REMOTE_ADDR"]).HostName;
               // _Auditobj.UserHostAddress = context.Request.ServerVariables["REMOTE_ADDR"];
              //  _Auditobj.UserAgent = context.Request.UserAgent;

                DC.AuditLog_UserOperations.Add(_Auditobj);
                DC.SaveChanges();

            }
            catch (Exception)
            {

                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //throw;


            }

        }
    }
}
