using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DomainInterface;
using UI.Web.Controler;
using UI.Web.Admin.Controller;
using System.Collections;
using Permission.DAL.Entities;
using System.Globalization;
using Infrastructure.DAL;
using Permission.DAL.Repository;
using Infrastructure;

namespace UI.Web.Admin.Pages
{
    public partial class Home : BaseFormAdmin
    {

        public DashboardRepository objRepository = IoC.Resolve<DashboardRepository>();
        private Security_pr_admin user;

        public string _TicketMonthlyRate = "";
        public string _CasesStatusCount = "";
        public string _TicketTypesCount = "";
        public string _CasesJudgmentresult = "";

        public string _TicketDailyRate = "";


        public string _TotalUnderStudy = "0";
        public string _TotalToMinistery = "0";
        public string _TotalToOmma = "0";
        public string _TotalToLaw = "0";
        public string _Totalmarsoom = "0";
        public string _TotalPublished = "0";
        public string _TotalCMGS = "0";
        public string _TotalOther = "0";


        public string _TotalInitial = "0";
        public string _TotalFinal = "0";
        public string _TotalFinalFromInitial = "0";
        public string _TotalAgreementAll = "0";



        public string _TotalPublished_Law = "0";
        public string _TotalPublished_Marsoom = "0";
        public string _TotalPublished_Unknown = "0";


        public string _TotalMedalCompleteData = "0";
        public string _TotalMedalToMinister = "0";
        public string _TotalMedalToDewan = "0";

        public string _TotalMedalEkhtar = "0";
        public string _TotalMedalSader = "0";

        public string _MedalOrgTypes = "";
        public string _MedalOrgPendingProcedure = "";

        public string _MedalStatusTypes = "";



        //Cases
        public string _TotalCasesDegree1 = "0";
        public string _TotalCasesDegree2 = "0";
        public string _TotalCasesDegree3 = "0";
        public string _TotalCasesDegree4 = "0";
        public string _TotalCasesDegree5 = "0";
        public string _TotalCasesDegree6 = "0";
        public string _TotalCasesDegree7 = "0";
        public string _TotalCasesDegree8 = "0";
        //Law Docs
        public string _LawDocType1 = "0";
        public string _LawDocType2 = "0";
        public string _LawDocType3 = "0";
        public string _LawDocType4 = "0";
        public string _LawDocType5 = "0";
        public string _LawDocType6 = "0";
        public string _LawDocPublished = "0";

        public string _LawDocExpireCount = "0";

        //Questions
        public string _TotalQuestion_Relied = "0";
        public string _TotalQuestion_Delaied = "0";
        public string _TotalQuestion_NoAnswer = "0";
        public string _TotalQuestion_Estifaa = "0";
        public string _TotalQuestion_partial = "0";
        public string ChapterName = "";
        public string Chapterid = "0";
        public int _TotalQuestion_All = 0;

        //Questions2 for last Chapter
        public string _TotalQuestion_Relied2 = "0";
        public string _TotalQuestion_Delaied2 = "0";
        public string _TotalQuestion_NoAnswer2 = "0";
        public string _TotalQuestion_Estifaa2 = "0";
        public string _TotalQuestion_partial2 = "0";
        public string _TotalQuestion_Unlegal = "0";
        public int _TotalQuestion_All2 = 0;



        public string _QuestionStatusCount = "";
        public string _QuestionTargetCount = "";
        public string _QuestionALLCount = "";
        public int _ALLQuestionCount = 0;
        //public string _MedalOrgPendingProcedure = "";

        //public string _MedalStatusTypes = "";

        public string _CasesAgainst = "";
        public string _Casesby = "";
        public string _CasesTransYear = "";
        public string _LawsTransYear = "";

        public string _LegalMemoPerYear = "";
        public int _AllLegalMemo = 0;
        public string _legalMemmoProcedure = "";


        protected void Page_Load(object sender, EventArgs e)
        {

            MemberShip_Permission.isAuthenticationCookie();
            user = MemberShipConstantUI.CurrentUser;
            if (user != null)
            {
                ViewState["userid"] = user.id.ToString();
                Session["userid"] = user.id.ToString();

                ViewState["AdminName"] = user.name.ToString();
                Session["AdminName"] = user.name.ToString();
                Session["ViewPrivate"] = user.isOperation.ToString();


                FillPermissions(user.AdminType, user.id);


            }
            else
            {
                Response.Redirect("~/Admin/Pages/Login.aspx");

            }



            if (!IsPostBack)
            {
                //FillGrid();
                Fillanalytics();
            }



        }
        private void FillPermissions(int? adminType, int adminID)
        {
            ArrayList UserPermssionDetails = MemberShip_Permission.ins.SetUserPermission(adminType, adminID);


            Hashtable tbl = new Hashtable();
            Hashtable sys = new Hashtable();

            ViewState["Permission"] = (Hashtable)UserPermssionDetails[0];
            Session["Permission"] = (Hashtable)UserPermssionDetails[0];
            ViewState["System"] = (Hashtable)UserPermssionDetails[1];
            Session["System"] = (Hashtable)UserPermssionDetails[1];
        }
        protected void btnfilter_Click(object sender, EventArgs e)
        {
            Fillanalytics();
        }

        private void Fillanalytics()
        {
            #region "Default Date"
            //DateTime stratDate = new DateTime(DateTime.Now.Year,1, 1);
            DateTime stratDate = new DateTime(1900, 1, 1);
            DateTime Endadate = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));

            //DateTime stratDate = NullDateifEmpty("01/01/2016");
            //DateTime Endadate = NullDateifEmpty("30/01/2016");


            //DateTime stratDate = DateTime.Today.AddDays(-1);
            //DateTime Endadate = DateTime.Today;

            if (hdnDateRange.Value != "" && hdnDateRange.Value != "undefined")
            {
                string[] daterange = hdnDateRange.Value.Split('-');
                stratDate = NullDateifEmptywithoutformat(daterange[0]);
                Endadate = NullDateifEmptywithoutformat(daterange[1]);

            }




            if (stratDate == null && stratDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                return;
            }
            #endregion

            #region "Agreements"

            //Prodesure Types COunt

            var AgreementsLastProceduresRates = objRepository.GetAgreementbyProcedures(stratDate, Endadate, getBool(ReadSession("ViewPrivate")));
            if (AgreementsLastProceduresRates != null)
            {

                foreach (var item in AgreementsLastProceduresRates)
                {
                    if (item != null)
                    {

                        switch (item.statuscode)
                        {

                            case 1://قيد الدراسة
                                _TotalUnderStudy = item.AgreementCount.ToString();
                                break;
                            case 2:// الاحالة للجان الوزارية
                                _TotalToMinistery = item.AgreementCount.ToString();
                                break;
                            case 3:// الاحالة للجان الاختصاص
                                _TotalToLaw = item.AgreementCount.ToString();
                                break;
                            case 16:// الاحالة للجان الوزارية
                                _TotalCMGS = item.AgreementCount.ToString();
                                break;
                            case 6://تم النشر
                                _TotalPublished = (Convert.ToInt32(_TotalPublished) + Convert.ToInt32(item.AgreementCount)).ToString();
                                //Fill published Details
                                var PublishedDetails = objRepository.GetAgreementbyProceduresPublishType(stratDate, Endadate, getBool(ReadSession("ViewPrivate")));
                                if (PublishedDetails != null)
                                {
                                    foreach (var publishType in PublishedDetails)
                                    {
                                        switch (publishType.StatusName)
                                        {
                                            case "1":
                                                _TotalPublished_Law = publishType.AgreementCount.ToString();
                                                break;
                                            case "2":
                                                _TotalPublished_Marsoom = publishType.AgreementCount.ToString();
                                                break;
                                            case "null":
                                                _TotalPublished_Unknown = (Convert.ToInt32(_TotalPublished_Unknown) + publishType.AgreementCount).ToString();
                                                break;

                                            case null:
                                                _TotalPublished_Unknown = (Convert.ToInt32(_TotalPublished_Unknown) + publishType.AgreementCount).ToString();
                                                break;
                                            case "0":
                                                _TotalPublished_Unknown = (Convert.ToInt32(_TotalPublished_Unknown) + publishType.AgreementCount).ToString();
                                                break;
                                            default:
                                                break;
                                        }
                                    }


                                }

                                break;

                            default:// بدون اجراء  
                                _TotalOther = (Convert.ToInt32(_TotalOther) + Convert.ToInt32(item.AgreementCount)).ToString();
                                break;

                        }

                    }


                }


            }

            //// CasesMonthlyRates
            // var AgreementsStatusRates = objRepository.GetAgreemtsCatseStatusRate(stratDate, Endadate);

            var AgreementsStatusRates = objRepository.GetAgreementbyProcedures(stratDate, Endadate, getBool(ReadSession("ViewPrivate")));
            if (AgreementsStatusRates != null)
            {
                int TotalCasesCount = 0;
                foreach (var item in AgreementsStatusRates)
                {
                    if (item != null)
                    {

                        TotalCasesCount += item.AgreementCount;
                        _TicketMonthlyRate += " {";
                        _TicketMonthlyRate += "StatusName: '" + StringLimit(item.StatusName, 18) + "', ";
                        _TicketMonthlyRate += " CasesCount: " + item.AgreementCount;
                        _TicketMonthlyRate += "},";


                    }


                }



            }





            //// CasesMonthlyRates
            var AgreementsTypesRates = objRepository.GetAgreementsCatseTypesRate(stratDate, Endadate);
            if (AgreementsTypesRates != null)
            {
                foreach (var item in AgreementsTypesRates)
                {
                    if (item != null)
                    {


                        _TicketTypesCount += " {";
                        _TicketTypesCount += "StatusName: '" + StringLimit(item.StatusName, 18) + "', ";
                        _TicketTypesCount += " CasesCount: " + item.AgreementCount;
                        _TicketTypesCount += "},";
                    }
                }
            }



            //// CasesMonthlyRates
            var AgreementsStatus = objRepository.GetAgreementsByStatus(stratDate, Endadate);
            if (AgreementsStatus != null)
            {
                foreach (var item in AgreementsStatus)
                {
                    if (item != null)
                    {

                        if (item.isInitial == true)
                        {//Inial
                            _TotalInitial = item.AgreementCount.ToString();
                            _TotalAgreementAll = (Convert.ToInt32(_TotalAgreementAll) + Convert.ToInt32(item.AgreementCount)).ToString();
                        }
                        else if (item.isInitial == false)
                        {
                            if (item.hasparent == 1)
                            {//final from Initail
                                _TotalFinalFromInitial = item.AgreementCount.ToString();
                            }
                            else
                            {//Final
                                _TotalFinal = item.AgreementCount.ToString();
                                _TotalAgreementAll = (Convert.ToInt32(_TotalAgreementAll) + Convert.ToInt32(item.AgreementCount)).ToString();
                            }
                        }

                    }


                }

            }


            #endregion

            #region "Medal Dashborad"

            //// Meal Org by Types
            var _MedalOrgTypesobj = objRepository.GetOrgMedalTypes(stratDate, Endadate);
            if (_MedalOrgTypesobj != null)
            {

                foreach (var item in _MedalOrgTypesobj)
                {
                    if (item != null)
                    {
                        _MedalOrgTypes += " {";
                        _MedalOrgTypes += "StatusName: '" + item.StatusName + "', ";
                        _MedalOrgTypes += " CasesCount: " + item.MedalCount;
                        _MedalOrgTypes += "},";

                    }


                }
            }



            // Medal Org Procedures Types

            var MedalProceduresStatus = objRepository.GetMedalLastProceduresStatus(stratDate, Endadate);
            if (MedalProceduresStatus != null)
            {
                foreach (var item in MedalProceduresStatus)
                {
                    if (item != null)
                    {
                        _MedalStatusTypes += " {";
                        _MedalStatusTypes += "StatusName: '" + item.StatusName + "', ";
                        _MedalStatusTypes += " CasesCount: " + item.MedalCount;
                        _MedalStatusTypes += "},";

                    }


                }

            }
            var MedalProceduresRates = objRepository.GetMedalsbyProcedures(stratDate, Endadate);
            if (MedalProceduresRates != null)
            {

                foreach (var item in MedalProceduresRates)
                {
                    if (item != null)
                    {

                        switch (item.statuscode)
                        {

                            case 1:
                                _TotalMedalCompleteData = item.MedalCount.ToString();
                                break;
                            case 2:
                                _TotalMedalToMinister = item.MedalCount.ToString();
                                break;
                            case 3:
                                _TotalMedalToDewan = item.MedalCount.ToString();
                                break;
                            case 4:
                                _TotalMedalSader = item.MedalCount.ToString();
                                break;
                            case 5:
                                _TotalMedalEkhtar = item.MedalCount.ToString();
                                break;


                            default:
                                break;
                        }

                    }


                }


            }

            #endregion


            #region "Cases Dashborad"

            var CasesJudgmentresult = objRepository.GetCasesCountByJudgmentresult(stratDate, Endadate);
            if (CasesJudgmentresult != null)
            {

                foreach (var item in CasesJudgmentresult)
                {
                    if (item != null)
                    {


                        _CasesJudgmentresult += " {";
                        _CasesJudgmentresult += "StatusName: '" + (item.StatusName) + "', ";
                        _CasesJudgmentresult += " CasesCount: " + item.CasesCount;
                        _CasesJudgmentresult += "},";


                    }


                }



            }


            var CasesDegreeCount = objRepository.GetCasesCountByDegress(stratDate, Endadate);
            if (CasesDegreeCount != null)
            {

                foreach (var item in CasesDegreeCount)
                {
                    if (item != null)
                    {

                        switch (item.degreecode)
                        {

                            case 1://'اول درجه'
                                _TotalCasesDegree1 = item.CasesCount.ToString();
                                break;
                            case 2://اول درجه مستعجل
                                _TotalCasesDegree2 = item.CasesCount.ToString();
                                break;
                            case 3://استئناف (ثاني درجة)
                                _TotalCasesDegree3 = item.CasesCount.ToString();
                                break;
                            case 4://استئناف مستعجل
                                _TotalCasesDegree4 = item.CasesCount.ToString();
                                break;
                            case 5://تمييز
                                _TotalCasesDegree5 = item.CasesCount.ToString();
                                break;
                            case 6://تمييز مستعجل
                                _TotalCasesDegree6 = item.CasesCount.ToString();
                                break;
                            case 7://إستشكال  
                                _TotalCasesDegree7 = item.CasesCount.ToString();
                                break;
                            case 8://استئناف  
                                _TotalCasesDegree8 = item.CasesCount.ToString();
                                break;

                            default:
                                break;
                        }

                    }


                }


            }

            var CasepartiesAgenestCount = objRepository.GetCaseparitesCount(stratDate, Endadate, "1");
            if (CasepartiesAgenestCount != null)
            {
                foreach (var item in CasepartiesAgenestCount)
                {
                    if (item != null)
                    {
                        _CasesAgainst += " {";
                        _CasesAgainst += "DegreeDesc:'" + item.DegreeDesc + "',";
                        _CasesAgainst += "partiesName:'" + item.partiesName + "',";
                        _CasesAgainst += "CasesCount:" + item.CasesCount;
                        _CasesAgainst += "},";

                    }


                }

            }

            var CasepartiesByCount = objRepository.GetCaseparitesCount(stratDate, Endadate, "2");
            if (CasepartiesByCount != null)
            {
                foreach (var item in CasepartiesByCount)
                {
                    if (item != null)
                    {
                        _Casesby += " {";
                        _Casesby += "DegreeDesc:'" + item.DegreeDesc + "',";
                        _Casesby += "partiesName:'" + item.partiesName + "',";
                        _Casesby += "CasesCount:" + item.CasesCount;
                        _Casesby += "},";

                    }


                }

            }

            var CaseYearCount = objRepository.GetCaseYearCount(stratDate, Endadate);
            if (CaseYearCount != null)
            {
                foreach (var item in CaseYearCount)
                {
                    if (item != null)
                    {
                        _CasesTransYear += " {";
                        _CasesTransYear += "DegreeDesc:'" + item.DegreeDesc + "',";
                        _CasesTransYear += "TransYear:'" + item.TransYear + "',";
                        _CasesTransYear += "CasesCount:" + item.CasesCount;
                        _CasesTransYear += "},";

                    }


                }

            }

            #endregion


            #region "Question Dashborad"

            //// Question
            var _QuestionCountsobj = objRepository.GetQuestionStstus(stratDate, Endadate);
            if (_QuestionCountsobj != null)
            {

                foreach (var item in _QuestionCountsobj)
                {
                    if (item != null)
                    {
                        _QuestionStatusCount += " {";
                        _QuestionStatusCount += "StatusName: '" + item.StatusName + "', ";
                        _QuestionStatusCount += " CasesCount: " + item.QuestionCount;
                        _QuestionStatusCount += "},";

                        _TotalQuestion_All += item.QuestionCount;

                        switch (item.statuscode)
                        {

                            case 1://'تمت الاجابة'
                                _TotalQuestion_Relied = item.QuestionCount.ToString();
                                break;
                            case 2://لم تتم الاجابة
                                _TotalQuestion_Delaied = item.QuestionCount.ToString();
                                break;
                            case 3://لعدم الاختصاص
                                _TotalQuestion_NoAnswer = item.QuestionCount.ToString();
                                break;
                            case 4://لعدم الاختصاص
                                _TotalQuestion_Estifaa = item.QuestionCount.ToString();
                                break;
                            case 5://لعدم الاختصاص
                                _TotalQuestion_partial = item.QuestionCount.ToString();
                                break;

                            default:
                                break;
                        }
                    }


                }


            }


            var _QuestionCountsobj2 = objRepository.GetQuestionLastChapterStstus(stratDate, Endadate);
            if (_QuestionCountsobj2 != null && _QuestionCountsobj2.Count > 0)
            {
                if (_QuestionCountsobj2[0] != null)
                {
                    ChapterName = _QuestionCountsobj2[0].ChapterName;
                    Chapterid = _QuestionCountsobj2[0].Chaptercode.ToString();
                }
                else { ChapterName = "الحالي"; }

                foreach (var item in _QuestionCountsobj2)
                {
                    if (item != null)
                    {


                        _TotalQuestion_All2 += item.QuestionCount;

                        switch (item.statuscode)
                        {

                            case 1://'تمت الاجابة'
                                _TotalQuestion_Relied2 = item.QuestionCount.ToString();
                                break;
                            case 2://لم تتم الاجابة
                                _TotalQuestion_Delaied2 = item.QuestionCount.ToString();
                                break;
                            case 3://لعدم الاختصاص
                                _TotalQuestion_NoAnswer2 = item.QuestionCount.ToString();
                                break;
                            case 4://لعدم الاختصاص
                                _TotalQuestion_Estifaa2 = item.QuestionCount.ToString();
                                break;
                            case 5://لعدم الاختصاص
                                _TotalQuestion_partial2 = item.QuestionCount.ToString();
                                break;
                            case 6://لعدم الاختصاص
                                _TotalQuestion_Unlegal = item.QuestionCount.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else { ChapterName = "الحالي"; }

            var _QuestionTargetCountsobj = objRepository.GetQuestionTarget(stratDate, Endadate);
            if (_QuestionTargetCountsobj != null)
            {
                foreach (var item in _QuestionTargetCountsobj)
                {
                    if (item != null)
                    {
                        _QuestionTargetCount += " {";
                        _QuestionTargetCount += "StatusName: '" + item.StatusName + "', ";
                        _QuestionTargetCount += " CasesCount: " + item.QuestionCount;
                        _QuestionTargetCount += "},";


                    }


                }

            }


            var _QuestionAll = objRepository.getQuestionsAll(stratDate, Endadate);
            if (_QuestionAll != null)
            {
                _ALLQuestionCount = 0;
                foreach (var item in _QuestionAll)
                {
                    if (item != null)
                    {
                        _QuestionALLCount += " {";
                        _QuestionALLCount += "ChapterName: '" + item.ChapterName + "', ";
                        _QuestionALLCount += " QuestionCount: " + item.QuestionCount;
                        _QuestionALLCount += "},";

                        _ALLQuestionCount += item.QuestionCount;
                    }


                }

            }





            //// Medal Org Procedures Types

            //var MedalProceduresStatus = objRepository.GetMedalLastProceduresStatus(stratDate, Endadate);
            //if (MedalProceduresStatus != null)
            //{
            //    foreach (var item in MedalProceduresStatus)
            //    {
            //        if (item != null)
            //        {
            //            _MedalStatusTypes += " {";
            //            _MedalStatusTypes += "StatusName: '" + item.StatusName + "', ";
            //            _MedalStatusTypes += " CasesCount: " + item.MedalCount;
            //            _MedalStatusTypes += "},";

            //        }


            //    }

            //}
            //var MedalProceduresRates = objRepository.GetMedalsbyProcedures(stratDate, Endadate);
            //if (MedalProceduresRates != null)
            //{
            //    int TotalCasesCount = 0;
            //    foreach (var item in MedalProceduresRates)
            //    {
            //        if (item != null)
            //        {

            //            switch (item.statuscode)
            //            {

            //                case 1:
            //                    _TotalMedalCompleteData = item.MedalCount.ToString();
            //                    break;
            //                case 2:
            //                    _TotalMedalToMinister = item.MedalCount.ToString();
            //                    break;
            //                case 3:
            //                    _TotalMedalToDewan = item.MedalCount.ToString();
            //                    break;
            //                case 4:
            //                    _TotalMedalSader = item.MedalCount.ToString();
            //                    break;
            //                case 5:
            //                    _TotalMedalEkhtar = item.MedalCount.ToString();
            //                    break;


            //                default:
            //                    break;
            //            }

            //        }


            //    }


            //}

            #endregion

            #region "Legal Memo Statistics"

            var LegalMemoList = objRepository.GetLegalMemoPerYear(stratDate, Endadate);
            if (LegalMemoList != null)
            {
                foreach (var item in LegalMemoList)
                {
                    if (item != null)
                    {
                        _LegalMemoPerYear += " {";
                        _LegalMemoPerYear += "DocStatus:'" + item.DocStatus + "',";
                        _LegalMemoPerYear += "TransYear:'" + item.DocYear + "',";
                        _LegalMemoPerYear += "RecordCount:" + item.RecordCount;
                        _LegalMemoPerYear += "},";

                        _AllLegalMemo += item.RecordCount;

                    }


                }

            }

            var LegalMemoList_PerProcedure = objRepository.GetLegalMemoPerProcedure(stratDate, Endadate);
            if (LegalMemoList_PerProcedure != null)
            {
                foreach (var item in LegalMemoList_PerProcedure)
                {
                    if (item != null)
                    {
                        _legalMemmoProcedure += " {";
                        _legalMemmoProcedure += "DocStatus:'" + item.DocStatus + "',";
                        _legalMemmoProcedure += "RecordCount:" + item.RecordCount;
                        _legalMemmoProcedure += "},";

                    }


                }

            }
            #endregion

            #region "Law Docs"


            var LawDocsTypes = objRepository.GetLawDocsTypes(stratDate, Endadate);
            if (LawDocsTypes != null)
            {
                foreach (var item in LawDocsTypes)
                {
                    if (item != null)
                    {

                        switch (item.DocTypeID)
                        {

                            case 1://'قانون'
                                _LawDocType1 = item.DocCount.ToString();
                                break;
                            case 2://مرسوم بقانون
                                _LawDocType2 = item.DocCount.ToString();
                                break;
                            case 3://المراسيم الاميرية)
                                _LawDocType3 = item.DocCount.ToString();
                                break;
                            case 4://الاوامر الاميرية
                                _LawDocType4 = item.DocCount.ToString();
                                break;
                            case 5://قرار مجلس الوزراء
                                _LawDocType5 = item.DocCount.ToString();
                                break;
                            case 6://  قرار رئيس مجلس الوزراء
                                _LawDocType6 = item.DocCount.ToString();
                                break;

                            default:
                                break;
                        }

                    }


                }
            }
            //Is Published

            var ispublished = objRepository.GetLawDocsPublished(stratDate, Endadate);
            if (ispublished != null)
            {
                _LawDocPublished = ispublished[0].DocCount.ToString();
            }
            var isExpire = objRepository.getNearExpireDocs();
            if (isExpire != null)
            {
                _LawDocExpireCount = isExpire.Count.ToString();
            }



            var LawsYearCount = objRepository.GetLawsYearCount(stratDate, Endadate);
            if (LawsYearCount != null)
            {
                foreach (var item in LawsYearCount)
                {
                    if (item != null)
                    {
                        _LawsTransYear += " {";
                        _LawsTransYear += "DegreeDesc:'',";
                        _LawsTransYear += "TransYear:'" + item.TransYear + "',";
                        _LawsTransYear += "CasesCount:" + item.DocCount;
                        _LawsTransYear += "},";

                    }


                }

            }
            #endregion


        }

        public string activeTab(int tabindex)
        {
            string _out = "";
            if (hdnactivetab.Value == "" && tabindex == 1)
            {
                _out = "active";
            }
            else if (ZeroIntergerIFNull(hdnactivetab.Value) == tabindex)
            {
                _out = "active";
            }
            //if (tabindex == 3)
            //{
            //    _out = "active";
            //}

            return _out;

        }

        public bool ShowSystem(string systemid)
        {
            Hashtable tbl = ((Hashtable)(ViewState["System"]));
            if ((tbl[systemid] == null))
            {
                return false;
            }

            return bool.Parse(tbl[systemid].ToString());
        }
    }
}