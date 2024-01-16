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
    public partial class operationKeysDetails : BaseFormAdmin
    {
        #region "Page Members"

        public AuditRepository objRepository = IoC.Resolve<AuditRepository>();


        public string SystemName;
        public string userName;
        public string TransDate;
  public string SearchKeys;
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

            List<searchkeys> _outList = new List<searchkeys>();
            var obj = objRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["RecordID"].ToString()));


            if (obj != null)
            {
                lblSearchResultCount.Text ="نتيجة البحث " +  obj.resultCount.ToString();
                userName = obj.name;
                SystemName = obj.namear;
                TransDate = obj.TransDate.ToString() ;


                if (obj.tages != null && obj.tages != "{}" && obj.tages != "")
                {
                    SearchKeys = "";
                    var Result = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj.tages);

                    foreach (KeyValuePair<string, string> item in Result)
                    {
                       // Console.WriteLine(string.Format("Key: {0} Value: {1}", item.Key, item.Value));
                        _outList.Add(new searchkeys() { refRecordId= obj.Code, key = item.Key, value = item.Value });
                    }
                    grdresult.DataSource = _outList;
                    grdresult.DataBind();
                }
                else
                { SearchKeys = "الكل"; }
            }
        }



        #endregion

    }
}