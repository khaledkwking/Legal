using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Infrastructure;
 

namespace UI.Web
{
    public static class Constant
    {

        private static string strAssemblyLanguage = "UI.Web.Language.Lang";

        private static System.Resources.ResourceManager LangResources = new System.Resources.ResourceManager(strAssemblyLanguage, Assembly.GetExecutingAssembly());

        public static string GetText(string Key)
        {
            try
            {
                return GetText(Key, Settings.Language);
            }
            catch (System.Exception)
            {
                //ToDO 
                //Log   word doesn't exist at resource
                return Key;
            }
        }

        private static string GetText(string Key, System.Globalization.CultureInfo Language)
        {
            try
            {
                return LangResources.GetString(Key, Language);

            }
            catch (Exception)
            {

                return Key;
            }

        }

  

        //public static string GetContentByName(string strPagePath, string strAdvName)
        //{
        //    string strLang = Settings.Language.Name;
        //    string strContent = "";
        //    var objPageRepository = IoC.Resolve<INextPagesRepository>();
        //    var objPage = objPageRepository.GetPageByPath(strPagePath);

        //    var objAdvName = objPageRepository.GetPageByName(strAdvName);

        //    if (objPage != null && objAdvName != null)
        //    {
        //        if (strLang.Equals("ar"))
        //        {
        //            //strContent = objPage.AR_Content;
        //            strContent = objAdvName.EN_Content;
        //        }
        //        else
        //        {
        //            strContent = objAdvName.EN_Content;
        //        }
        //    }
        //    return strContent;
        //}

        public static void SelectMaster(System.Web.UI.Page Page)
        {
            try
            {
                string strLang = Settings.Language.Name;
                if (!strLang.Equals("ar"))
                {
                    Page.MasterPageFile = "~/Masters/Site.Master";
                }
                else
                {
                    Page.MasterPageFile = "~/Masters/Site.Master";
                }
            }
            catch (Exception)
            {

            }



        }

        public static void SelectMasterForAdmin(System.Web.UI.Page Page)
        {
            try
            {
                string strLang = Settings.Language.Name;
                if (!strLang.Equals("ar"))
                {
                    Page.MasterPageFile = "~/Admin/Masters/Admin.Master";
                }
                else
                {
                    Page.MasterPageFile = "~/Admin/Masters/Admin.Master";
                }
            }
            catch (Exception)
            {

            }

        }

    }
}