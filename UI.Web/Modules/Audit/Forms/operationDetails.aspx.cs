using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using UI.Web.Admin.Controller;
using Newtonsoft.Json;
using Infrastructure.DAL.ViewModels;

namespace UI.Web.Modules.Audit.Forms
{
    public partial class operationDetails : BaseFormAdmin
    {
        #region "Page Members"

        public AuditRepository objRepository = IoC.Resolve<AuditRepository>();
        #endregion
        #region "Page Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "Frm_UserOperaions.aspx";
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (!IsPostBack)
            {

                FillAuditDetails();

            }

        }

        protected void grdresult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }


        }

        #endregion

        #region "Fill Information"

        private void FillAuditDetails()
        {


            var obj = objRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["RecordID"].ToString()));


            switch (obj.moduleCode)
            {

                case (int)SysModulesRef.agreements:
                    {
                        var Result = JsonConvert.DeserializeObject<List<View_AgreeementsList>>(obj.queryResult);

                        lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        if (Result.Count > 0)
                        {
                            tblshow.Visible = true;
                            pager1.Visible = true;
                        }
                        else
                        {
                            tblshow.Visible = false;
                            pager1.Visible = false;
                            string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                        }
                        grdresult.DataSource = Result;
                        grdresult.DataBind();
                        pager1.ItemCount = Result.Count;
                        break; ;
                    }

                case (int)SysModulesRef.medal:
                    {
                        var Result = JsonConvert.DeserializeObject<List<MedalViewModel>>(obj.queryResult);
                        lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        if (Result.Count > 0)
                        {
                            tblshow.Visible = true;
                            pager1.Visible = true;
                        }
                        else
                        {
                            tblshow.Visible = false;
                            pager1.Visible = false;
                            string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                        }
                        grdresult.DataSource = Result;
                        grdresult.DataBind();
                        pager1.ItemCount = Result.Count;
                        break; ;
                    }
                case (int)SysModulesRef.cases:
                    {
                        var Result = JsonConvert.DeserializeObject<List<CasesViewModel>>(obj.queryResult);
                        lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        if (Result.Count > 0)
                        {
                            tblshow.Visible = true;
                            pager1.Visible = true;
                        }
                        else
                        {
                            tblshow.Visible = false;
                            pager1.Visible = false;
                            string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                        }
                        grdresult.DataSource = Result;
                        grdresult.DataBind();
                        pager1.ItemCount = Result.Count;
                        break; ;
                    }
                case (int)SysModulesRef.questions:
                    {

                        switch (obj.userModule)
                        {

                            case "questions":
                                {

                                    var Result = JsonConvert.DeserializeObject<List<View_QuestionsList>>(obj.queryResult);
                                    lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                                    lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                                    if (Result.Count > 0)
                                    {
                                        tblshow.Visible = true;
                                        pager1.Visible = true;
                                    }
                                    else
                                    {
                                        tblshow.Visible = false;
                                        pager1.Visible = false;
                                        string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                    }
                                    grdresult.DataSource = Result;
                                    grdresult.DataBind();
                                    pager1.ItemCount = Result.Count;
                                    break;
                                }

                            case "MadbataList":
                                {

                                    var Result = JsonConvert.DeserializeObject<List<View_MadbataList>>(obj.queryResult);
                                    lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                                    lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                                    if (Result.Count > 0)
                                    {
                                        tblshow.Visible = true;
                                        pager1.Visible = true;
                                    }
                                    else
                                    {
                                        tblshow.Visible = false;
                                        pager1.Visible = false;
                                        string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                    }
                                    grdresult.DataSource = Result;
                                    grdresult.DataBind();
                                    pager1.ItemCount = Result.Count;
                                    break;
                                }
                            case "Complaints":
                                {

                                    var Result = JsonConvert.DeserializeObject<List<View_Complaints>>(obj.queryResult);
                                    lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                                    lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                                    if (Result.Count > 0)
                                    {
                                        tblshow.Visible = true;
                                        pager1.Visible = true;
                                    }
                                    else
                                    {
                                        tblshow.Visible = false;
                                        pager1.Visible = false;
                                        string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                    }
                                    grdresult.DataSource = Result;
                                    grdresult.DataBind();
                                    pager1.ItemCount = Result.Count;

                                    break;
                                }
                        }

                        break;
                    }

                case (int)SysModulesRef.libarary:
                    {
                        var Result = JsonConvert.DeserializeObject<List<view_LibraryDocs>>(obj.queryResult);
                        lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        if (Result.Count > 0)
                        {
                            tblshow.Visible = true;
                            pager1.Visible = true;
                        }
                        else
                        {
                            tblshow.Visible = false;
                            pager1.Visible = false;
                            string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                        }
                        grdresult.DataSource = Result;
                        grdresult.DataBind();
                        pager1.ItemCount = Result.Count;
                        break; ;
                    }
                case (int)SysModulesRef.Legislation:
                    {
                        var Result = JsonConvert.DeserializeObject<List<View_LawsDocs>>(obj.queryResult);
                        lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        if (Result.Count > 0)
                        {
                            tblshow.Visible = true;
                            pager1.Visible = true;
                        }
                        else
                        {
                            tblshow.Visible = false;
                            pager1.Visible = false;
                            string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                        }
                        grdresult.DataSource = Result;
                        grdresult.DataBind();
                        pager1.ItemCount = Result.Count;
                        break; ;
                    }
                case (int)SysModulesRef.pmLetter:
                    {
                        var Result = JsonConvert.DeserializeObject<List<View_PmLetters>>(obj.queryResult);
                        lblcount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (Result.Count.ToString() + Resources.Utilities.records));
                        if (Result.Count > 0)
                        {
                            tblshow.Visible = true;
                            pager1.Visible = true;
                        }
                        else
                        {
                            tblshow.Visible = false;
                            pager1.Visible = false;
                            string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                        }
                        grdresult.DataSource = Result;
                        grdresult.DataBind();
                        pager1.ItemCount = Result.Count;
                        break; ;
                    }

            }
        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdresult.PageCount))
            {
                currnetPageIndx = (grdresult.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdresult.CurrentPageIndex = (currnetPageIndx - 1);
            FillAuditDetails();
        }

        #endregion

    }
}