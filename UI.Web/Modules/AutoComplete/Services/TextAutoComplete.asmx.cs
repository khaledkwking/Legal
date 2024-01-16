using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using UI.Web.Admin.Controller;

namespace UI.Web.Modules.AutoComplete.Services
{
    /// <summary>
    /// Summary description for TextAutoComplete
    /// </summary>
    [WebService(Namespace = "http://cmgs.gov.kw/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TextAutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string[] SubjectAutoCompete(string prefixText, int count)
        {
            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(count);
            ArrayList name = new ArrayList();
            ArrayList value = new ArrayList();

            name.Add("@pre");
            value.Add(prefixText.ToLower());

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.sp_subjectAutoComp(prefixText.ToLower())
                     select obj).ToArray();
                //return result.FirstOrDefault<Pm_Letters>();

                foreach (var item in result)
                {
                    items.Add(item.name);

                }

                return items.ToArray();

            }

            // DataSet ds = ABOBasic.ins.ExecuteSPDs("sp_subjectAutoComp", name, value);
        }

        [WebMethod]
        public string[] PersonsAutoCompete(string prefixText, int count)
        {

            ArrayList name = new ArrayList();
            ArrayList value = new ArrayList();

            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(count);

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.sp_PersonAutoComp(prefixText.ToLower())
                     select obj).ToArray();
                //return result.FirstOrDefault<Pm_Letters>();

                foreach (var item in result)
                {
                    items.Add(item.name);

                }

                return items.ToArray();

            }


        }

   [WebMethod]
        public string[] MedalPersonsAutoCompete(string prefixText, int count)
        {

            ArrayList name = new ArrayList();
            ArrayList value = new ArrayList();

            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(count);

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.sp_MedalPersonAutoComp(prefixText.ToLower())
                     select obj).ToArray();
                //return result.FirstOrDefault<Pm_Letters>();

                foreach (var item in result)
                {
                    items.Add(item.name);

                }

                return items.ToArray();

            }


        }



        //[WebMethod]
        //public string[] OrgAutoCompete(string prefixText, int count)
        //{

        //    ArrayList name = new ArrayList();
        //    ArrayList value = new ArrayList();

        //     name.Add("@pre");
        //    value.Add(prefixText.ToLower());
        //    DataSet ds = ABOBasic.ins.ExecuteSPDs("Parliament_Orgs", name, value);

        //    System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(count);

        //    using (var DC = new CMGS_DBEntities())
        //    {
        //        var result =
        //            (from obj in DC.Parliament_Orgs(prefixText.ToLower())
        //             select obj).ToArray();
        //        //return result.FirstOrDefault<Pm_Letters>();

        //        foreach (var item in result)
        //        {
        //            items.Add(item.name);

        //        }

        //        return items.ToArray();

        //    }


        //}



        [WebMethod]
        public string[] OmaPersonsAutoComplete(string prefixText,  int count ,int contextKey)
        {
 


            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(count);


            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.sp_OmaPersonAutoComp(prefixText.ToLower(),0)
                     select obj).ToArray();
                //return result.FirstOrDefault<Pm_Letters>();

                foreach (var item in result)
                {
                    items.Add(item.name);

                }

                return items.ToArray();

            }


        }


        [WebMethod]
        public string[] ArchOrgAutoCompete(string prefixText, int count)
        {

             

            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>(count);


            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.sp_ArcOrgAutoComp(prefixText.ToLower())
                     select obj).ToArray();
                //return result.FirstOrDefault<Pm_Letters>();

                foreach (var item in result)
                {
                    items.Add(item.name);

                }

                return items.ToArray();

            }


        }

    }
}
