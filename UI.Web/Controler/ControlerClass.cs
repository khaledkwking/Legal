using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure;
using DomainInterface;

namespace UI.Web
{
    public class ControlerClass
    {
        public string GetContent(string strPagePath)
        {
            string strLang = Settings.Language.Name;
            string strContent = "";

            var objPageRepository = IoC.Resolve<INextPagesRepository>();
            var objPage = objPageRepository.GetPageByPath(strPagePath);

            if (objPage != null)
            {
                if (strLang.Equals("ar"))
                {
                    strContent = objPage.AR_Content;
                }
                else
                   
                {
                    strContent = objPage.EN_Content;
                }
            }
            return strContent;
        }

        public void SelectMaster(System.Web.UI.Page Page)
        {
            try
            {
                string strLang = Settings.Language.Name;
                if (strLang.Equals("ar"))
                {
                    Page.MasterPageFile = "~/Masters/AR_Next.Master";
                }
                else
                {
                    Page.MasterPageFile = "~/Masters/EN_Next.Master";
                }
            }
            catch (Exception) 
            {

            }

        }


    }
}