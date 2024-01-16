using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using UI.Web.Admin.Controller;

namespace UI.Web.LegalMemos.Forms
{
    public partial class LegalMemoOrg : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LegalMemoRepository objRepository = IoC.Resolve<LegalMemoRepository>();
        public string _PageTitle = " الجهات   ";

 

        #endregion

        #region "Page Events"
         
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");
           // lnkScan.Attributes.Add("onclick", "return chkImage();");

            if (!IsPostBack)
            {
               
                FillDllwithoptional_ALL(objLookup.fill_LegalMemo_OrgCat(), ref lstChapter, "NameAr", "Code", "");
                FillDllwithoptional_ALL(objLookup.fill_LegalMemo_OrgCat(), ref lstFilterChapter, "NameAr", "Code", "الكل");
 

                ViewState["itemID"] = "0";
                FillGrid();
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
        }

        protected void pager_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdData.PageCount))
            {
                currnetPageIndx = (grdData.PageCount - 1);
            }

          //  pager1.CurrentIndex = currnetPageIndx;
            grdData.CurrentPageIndex = (currnetPageIndx - 1);
            this.FillGrid();
        }
        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {

                LegalMemo_Org obj = new LegalMemo_Org();
                for (int i = 0; i <= grdData.Items.Count - 1; i++)
                {

                    if ((grdData.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteLegalMemoOrg((LegalMemo_Org)objRepository.GetOrgDetails(ZeroIntergerIFNull(grdData.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillGrid();

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteMaterDataFailed + ex.InnerException.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

        }

        protected void grdData_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            this.ClearForm();
            ViewState["itemID"] = id;
            this.FillForm();
            tblshow.Visible = false;
            tblAdd.Visible = true;
        }

        protected void grdData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#EFEFEF\';");
            }

        }
        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveSessionInformation();
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.ClearForm();
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.FillGrid();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {

            this.ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;
        }
        #endregion

        #region "Fill Information"
        private int SaveSessionInformation()
        {
            string script = "";

            LegalMemo_Org obj = new LegalMemo_Org();
            try
            {


                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save


                    if (objRepository.checkItemExistance(ZeroIntergerIFNull( lstChapter.SelectedValue),txtNameAr.Text,  0))
                    {

                        script = FormatpopupErrorMSG("الجهة مسجل من قبل", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0  ;
                    }



                    obj.CatId = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    obj.NameAr = txtNameAr.Text;
                    obj.NameEn = txtNameEn.Text;
                     //obj.sessionOrder =ZeroIntergerIFNull( txtOrder.Text);

                    objRepository.AddLegalMemoOrg(obj);
                }
                else
                { //Update 

                    if (objRepository.checkItemExistance(ZeroIntergerIFNull(lstChapter.SelectedValue), txtNameAr.Text, ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {

                        script = FormatpopupErrorMSG("الجهة مسجل من قبل", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }

                    obj = objRepository.GetOrgDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.CatId = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    obj.NameAr = txtNameAr.Text;
                    obj.NameEn = txtNameEn.Text;
                    //obj.sessionOrder = ZeroIntergerIFNull(txtOrder.Text);

                    objRepository.UpdateLegalMemoOrg(obj);

                }

                ClearForm();
                FillGrid();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            return obj.Code;
        }

        //private int SaveSessionInformation_scan()
        //{
        //     string script = "";

        //    Parliament_legislativeSession obj = new Parliament_legislativeSession();
        //    try
        //    {

        //        if (gets(ViewState["itemID"]).Equals("0"))
        //        {//Save

        //            obj.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
        //            obj.Serial = txtRef.Text;
        //            obj.NameAr = txtNameAr.Text;
        //            obj.NameEn = txtNameEn.Text;

        //            objRepository.AddOmaPerson(obj);
        //        }
        //        else
        //        { //Update 
        //            obj = objRepository.getPersonDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
        //            obj.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
        //            obj.Serial = txtRef.Text;
        //            obj.NameAr = txtNameAr.Text;
        //            obj.NameEn = txtNameEn.Text;
        //            objRepository.UpdateOmaPerson(obj);

        //        }

        //    }
        //    catch (Exception ex)
        //    {


        //        script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
        //    }

        //    return obj.Code;
        //}
        private void FillGrid()
        {
            var objList = objRepository.FilterLegalMemo_Org(ZeroIntergerIFNull( lstFilterChapter.SelectedValue));

            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() +  Resources.Utilities.records));
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdData.PageSize));
            
            grdData.DataSource = objList;
            grdData.DataBind();
            int _totalCount = objList.Count;
          //  pager1.ItemCount = _totalCount;

        }
        private string GetTitle(bool isadd)
        {
            if (isadd)
            {
                return "";
            }
            else
            {
                return "";
            }

        }
        private void FillForm()
        {
            var objList = objRepository.GetOrgDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                
                
                txtNameAr.Text = gets(objList.NameAr);
                txtNameEn.Text = gets(objList.NameEn);
                try
                {
                    lstChapter.SelectedValue = gets(objList.CatId);
                }
                catch (Exception)
                {
                }
                
            }

            tblAdd.Visible = true;
            lblSubTitle.Text = this.GetTitle(false);

        }
        private void ClearForm()
        {
           
            txtNameAr.Text = "";
            txtNameEn.Text = "";
             txtOrder.Text = "0";
            ViewState["itemID"] = 0;
            tblAdd.Visible = false;
            tblshow.Visible = true;
            lblSubTitle.Text = this.GetTitle(true);
        }

        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            //btnDelete.Visible = userAccess.Delete;

            


        }


        #endregion



        protected void grdData_PageIndexChanged1(object source, DataGridPageChangedEventArgs e)
        {
            grdData.CurrentPageIndex = e.NewPageIndex;
            this.FillGrid();
        }
    }
}