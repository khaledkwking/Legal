using Infrastructure;
using Infrastructure.DAL;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using UI.Web.Admin.Controller;
using UI.Web.Helper;

namespace UI.Web.Modules.Laws.Forms
{
    public partial class LawDocSearchDetails : BaseFormAdmin
    {
        private const int PageSize = 2;

        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        private int CurrentPageIndex
        {
            get { return ViewState["LawDocSearchDetailsPageIndex"] != null ? (int)ViewState["LawDocSearchDetailsPageIndex"] : 0; }
            set { ViewState["LawDocSearchDetailsPageIndex"] = value; }
        }

        protected int CurrentPageNumber
        {
            get { return CurrentPageIndex + 1; }
        }

        protected int PageOffset
        {
            get { return CurrentPageIndex * PageSize; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillSourceLaws();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 0)
            {
                CurrentPageIndex--;
            }

            BindPagedResults();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPageIndex++;
            BindPagedResults();
        }

        protected void rptPages_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                var pageIndex = ZeroIntergerIFNull(e.CommandArgument.ToString()) - 1;
                if (pageIndex >= 0)
                {
                    CurrentPageIndex = pageIndex;
                    BindPagedResults();
                }
            }
        }

        private void FillSourceLaws()
        {
            var destDocId = ZeroIntergerIFNull(Request.QueryString["destdocid"]);
            if (destDocId == 0)
            {
                dlSourceLaws.DataSource = null;
                dlSourceLaws.DataBind();
                lblDetailsCount.Text = "0";
                return;
            }
            int DestId = ZeroIntergerIFNull(destDocId.ToString());
            var q = objRepository.DC.Law_DocData.Where(o => o.Code == DestId).ToList();
            if(q.Count>0)
            {
                int DoctypId = q[0].DocTypeID ?? 0;
                string qType= objRepository.DC.Law_DocType.Where(o => o.Code == DoctypId).FirstOrDefault().NameAr;
                lblMainDoc.Text = qType + " —ﬁ„ " + q[0].DocNum + " ·”‰… " + q[0].DocYear;
                lblMainDocSubject.Text = q[0].DocSubject;
                ahrefDoc.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(q[0].Code) + "/&vfileList=[" + gets(q[0].DocFilepath) + ";]";
            }
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DestDocID", GetNullableInt(destDocId.ToString()))
            };
            var result = objRepository.DC.Database.SqlQuery<LawSummaryDTO>(
               "EXEC sp_GetLinkedLawsSummary_HTML_Details @DestDocID",
               parameters.ToArray()).ToList();

            Session["LawDocSearchDetailsResults"] = result;
            lblDetailsCount.Text = result.Count.ToString();
            CurrentPageIndex = 0;
            BindPagedResults();
        }

        private void BindPagedResults()
        {
            var result = Session["LawDocSearchDetailsResults"] as List<LawSummaryDTO> ?? new List<LawSummaryDTO>();
            var paged = new PagedDataSource
            {
                DataSource = result,
                AllowPaging = true,
                PageSize = PageSize,
                CurrentPageIndex = CurrentPageIndex
            };

            dlSourceLaws.DataSource = paged;
            dlSourceLaws.DataBind();

            btnPrev.Enabled = !paged.IsFirstPage;
            btnNext.Enabled = !paged.IsLastPage;

            var pages = Enumerable.Range(1, Math.Max(paged.PageCount, 1)).ToList();
            rptPages.DataSource = pages;
            rptPages.DataBind();
        }

        private object GetNullableInt(string value)
        {
            var parsed = ZeroIntergerIFNull(value);
            return parsed == 0 ? (object)DBNull.Value : parsed;
        }
    }
}
