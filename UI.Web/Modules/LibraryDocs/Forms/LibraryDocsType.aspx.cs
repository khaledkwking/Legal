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

namespace UI.Web.LibraryDocs.Forms
{
    public partial class LibraryDocsType : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LibraryDocsRepository objRepository = IoC.Resolve<LibraryDocsRepository>();
        public string _PageTitle = " أنواع الوثائق   ";



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

                FillDllwithoptional_ALL(objLookup.Fill_LibDocCategory(), ref lstCategory, "NameAr", "Code", "");
                FillDllwithoptional_ALL(objLookup.Fill_LibDocCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");


                ViewState["itemID"] = "0";
                FillGrid();
            }

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

                Library_DocsType obj = new Library_DocsType();
                for (int i = 0; i <= grdData.Items.Count - 1; i++)
                {

                    if ((grdData.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteLibraryType((Library_DocsType)objRepository.GetTypeDetails(ZeroIntergerIFNull(grdData.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillGrid();

            }
            catch (Exception)
            {
                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteMaterDataFailed, "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                //string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteMaterDataFailed + ex.InnerException.ToString(), "1");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
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
            SaveAttachmentInformation();
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
        private int SaveAttachmentInformation()
        {
            string script = "";
            string _img = UploadFileoServer(txtImage, Server.MapPath("/Modules/LibraryDocs/Forms/lib_img/"));
            Library_DocsType obj = new Library_DocsType();
            try
            {

                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save


                    obj.CatId = ZeroIntergerIFNull(lstCategory.SelectedValue);
                    obj.NameAr = txtNameAr.Text;
                    obj.NameEn = txtNameEn.Text;
                    if (!_img.Equals(""))
                    {
                        obj.img = _img;

                    }


                    objRepository.AddLibraryDocType(obj);
                }
                else
                { //Update
                    obj = objRepository.GetTypeDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.CatId = ZeroIntergerIFNull(lstCategory.SelectedValue);
                    obj.NameAr = txtNameAr.Text;
                    obj.NameEn = txtNameEn.Text;
                    if (!_img.Equals(""))
                    {
                        obj.img = _img;

                    }


                    objRepository.UpdateLibrarytype(obj);

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

        //private int SaveAttachmentInformation_scan()
        //{
        //     string script = "";

        //    Library_DocsType obj = new Library_DocsType();
        //    try
        //    {

        //        if (gets(ViewState["itemID"]).Equals("0"))
        //        {//Save

        //            obj.CatId = ZeroIntergerIFNull(lstCategory.SelectedValue);
        //            obj.Serial = txtRef.Text;
        //            obj.NameAr = txtNameAr.Text;
        //            obj.NameEn = txtNameEn.Text;

        //            objRepository.AddOmaPerson(obj);
        //        }
        //        else
        //        { //Update
        //            obj = objRepository.getPersonDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
        //            obj.CatId = ZeroIntergerIFNull(lstCategory.SelectedValue);
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
            var objList = objRepository.GetTypeList(ZeroIntergerIFNull( lstFilterCategory.SelectedValue));

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
            var objList = objRepository.GetTypeDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {


                lstCategory.SelectedValue = gets(objList.CatId);
                txtNameAr.Text = gets(objList.NameAr);
                txtNameEn.Text = gets(objList.NameEn);
            }

            tblAdd.Visible = true;
            lblSubTitle.Text = this.GetTitle(false);

        }
        private void ClearForm()
        {

            txtNameAr.Text = "";
            txtNameEn.Text = "";

            ViewState["itemID"] = 0;
            tblAdd.Visible = false;
            tblshow.Visible = true;
            lblSubTitle.Text = this.GetTitle(true);
        }




        #endregion



        protected void grdData_PageIndexChanged1(object source, DataGridPageChangedEventArgs e)
        {
            grdData.CurrentPageIndex = e.NewPageIndex;
            this.FillGrid();
        }
    }
}