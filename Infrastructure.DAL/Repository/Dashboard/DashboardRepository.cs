using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.ViewModels;

namespace Infrastructure.DAL
{
    public partial class DashboardRepository : BaseRepository
    {
        public DashboardRepository(CMGS_DBEntities _context) : base(_context)
        {
        }
        #region "Portal Dashboard "
        public List<AgreementModel> GetCaseMontlyRateByStatus(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = " select cast(TransDate as date) TransDate,NameEn as StatusNameEn,count(Cases.code) as caseCount from ";
            sequenceMaxQuery += " dbo.Cases inner join dbo.D_StatusCode on CaseStatusCode = D_StatusCode.code";
            sequenceMaxQuery += " group by cast(TransDate as date),NameEn";

           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();

        }
        public List<AgreementModel> GetAgreementbyProcedures(DateTime _fromDate, DateTime _toDate, bool isPrivate)
        {


            string sequenceMaxQuery = "select COALESCE(Agreement_ProcedureTypes.code,0) as statuscode ,COALESCE(Namear,N'بدون اجراء') as StatusName,count(AgreementData.code) as AgreementCount from ";
            sequenceMaxQuery += " dbo.AgreementData left join dbo.Agreement_ProcedureTypes on LastActionID = Agreement_ProcedureTypes.code where parentid=0 ";
            sequenceMaxQuery += " group by  Agreement_ProcedureTypes.code,Namear";
           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();
        }
        public List<AgreementModel> GetAgreementbyProceduresPublishType(DateTime _fromDate, DateTime _toDate, bool isPrivate)
        {

            string sequenceMaxQuery = "  SELECT COUNT(dbo.AgreementData.code) as AgreementCount,convert(nvarchar, publishType)  as StatusName FROM AgreementData left JOIN dbo.Agreement_procedureHistory ";
            sequenceMaxQuery += " ON Agreement_procedureHistory.Code = AgreementData.LastProcedureRefId  WHERE LastActionID = 6 ";
            // sequenceMaxQuery += "  and cast(Agr_ReciveDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += "GROUP BY  dbo.Agreement_procedureHistory.publishType ";

           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();

        }
        public List<MedalModel> GetMedalsbyProcedures(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select Medal_M_ProcedureType.code as statuscode ,Namear as StatusName,count(Medal_Data.code) as MedalCount from ";
            sequenceMaxQuery += " dbo.Medal_Data inner join dbo.Medal_M_ProcedureType on LastActionID = Medal_M_ProcedureType.code ";
            sequenceMaxQuery += "  where cast(Medal_receivedDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  Medal_M_ProcedureType.code,Namear";


           
            var result = DC.Database.SqlQuery<MedalModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<MedalModel>();

        }
        public List<AgreementModel> GetAgreemtsCatseStatusRate(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select Agreement_StatusCodes.code as statuscode ,Namear as StatusName,count(AgreementData.code) as AgreementCount from ";
            sequenceMaxQuery += " dbo.AgreementData inner join dbo.Agreement_StatusCodes on StatusCodeID = Agreement_StatusCodes.code ";
            sequenceMaxQuery += "  where cast(Agr_ReciveDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  Agreement_StatusCodes.code,Namear";


           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();

        }
        public List<AgreementModel> GetAgreementsCatseTypesRate(DateTime _fromDate, DateTime _toDate)
        {


            string sequenceMaxQuery = "select Agreement_TypeCodes.code as statuscode ,Namear as StatusName,count(AgreementData.code) as AgreementCount from ";
            sequenceMaxQuery += " dbo.AgreementData inner join dbo.Agreement_TypeCodes on Agr_TypeCode = Agreement_TypeCodes.code ";
            sequenceMaxQuery += "  where parentid=0 and cast(Agr_ReciveDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  Agreement_TypeCodes.code,Namear";

           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();

        }


        public List<AgreementModel> GetAgreementsByStatus(DateTime _fromDate, DateTime _toDate)
        {


            string sequenceMaxQuery = " select isInitial ,(case when parentid!=0 then 1 else 0 end )as hasparent  ,count(code) AgreementCount   from ";
            sequenceMaxQuery += " dbo.AgreementData   ";
            sequenceMaxQuery += "  where  cast(Agr_ReciveDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += "   group by isInitial ,(case when parentid!=0 then 1 else 0 end )";

           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();

        }
        public List<MedalModel> GetOrgMedalTypes(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select Medal_M_Organizations.code as statuscode ,Namear as StatusName,count(Medal_Data.code) as MedalCount from ";
            sequenceMaxQuery += " dbo.Medal_Data inner join dbo.Medal_M_Organizations on Medal_OrgID = Medal_M_Organizations.code ";
            sequenceMaxQuery += "  where cast(Medal_receivedDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  Medal_M_Organizations.code,Namear";

           
            var result = DC.Database.SqlQuery<MedalModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<MedalModel>();

        }
        public List<View_OrgMedalbyProcedures> GetOrgMedalLastProcedures(DateTime _fromDate, DateTime _toDate)
        {

            var result =
                (from obj in DC.View_OrgMedalbyProcedures
                     // where obj.re
                 select obj);

            return result.ToList<View_OrgMedalbyProcedures>();

        }
        public List<MedalModel> GetMedalLastProceduresStatus(DateTime _fromDate, DateTime _toDate)
        {


            string sequenceMaxQuery = " select ";
            sequenceMaxQuery += "(";
            sequenceMaxQuery += "case when Medal_M_ProcedureType.code <= 3 then N'قيد الإصدار' else N'تم الإصدار' end";
            sequenceMaxQuery += ") as StatusName";
            sequenceMaxQuery += ",count(Medal_Data.code) as MedalCount from";
            sequenceMaxQuery += "  dbo.Medal_Data";
            sequenceMaxQuery += "  inner join Medal_M_ProcedureType on Medal_Data.LastActionID = dbo.Medal_M_ProcedureType.code";
            sequenceMaxQuery += "  where cast(Medal_receivedDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by(";
            sequenceMaxQuery += "case when Medal_M_ProcedureType.code <= 3 then N'قيد الإصدار' else N'تم الإصدار' end";
            sequenceMaxQuery += " )";



           
            var result = DC.Database.SqlQuery<MedalModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<MedalModel>();

        }
        public List<AgreementModel> GetDailyRate(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select  cast(TransDate as date) TransDate,count(Cases.code) as CasesCount from ";
            sequenceMaxQuery += " dbo.Cases inner join dbo.D_StatusCode on CaseStatusCode = D_StatusCode.code ";
            sequenceMaxQuery += " group by  cast(TransDate as date)";

           
            var result = DC.Database.SqlQuery<AgreementModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<AgreementModel>();

        }
        #endregion

        #region "Questions Methods"
        public List<QuestionsModel> GetQuestionLastChapterStstus(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = @"  select Parliament_QuestionStatus.code as statuscode ,Parliament_QuestionStatus.Namear as StatusName,Parliament_legislativeChapter.Namear as ChapterName,Parliament_legislativeChapter.code as Chaptercode,
                                             count(Parliament_Questions.code) as QuestionCount 
                                             from      Parliament_Questions
                                             inner join dbo.Parliament_QuestionStatus on StatusID = Parliament_QuestionStatus.code  
                                              INNER JOIN dbo.Parliament_legislativeChapter ON Parliament_legislativeChapter.Code = Parliament_Questions.ChapterID     
                                               where  qtype=1 and  ChapterID=(select top 1 ChapterID from Parliament_Questions order by Q_Date desc) and cast(Q_Date as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) group by  Parliament_QuestionStatus.code,Parliament_QuestionStatus.Namear,Parliament_legislativeChapter.Namear,Parliament_legislativeChapter.code ";

           
            var result = DC.Database.SqlQuery<QuestionsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<QuestionsModel>();

        }
        public List<QuestionsModel> GetQuestionStstus(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = @"  select Parliament_QuestionStatus.code as statuscode ,Namear as StatusName,
                                             count(Parliament_Questions.code) as QuestionCount 
                                             from      Parliament_Questions
                                             inner join dbo.Parliament_QuestionStatus on StatusID = Parliament_QuestionStatus.code  
                                             where QType=1 and   ChapterID=(select top 1 ChapterID from Parliament_Questions order by Q_Date desc) and  cast(Q_Date as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) group by  Parliament_QuestionStatus.code,Namear ";

           
            var result = DC.Database.SqlQuery<QuestionsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<QuestionsModel>();

        }

        public List<QuestionsModel> GetQuestionTarget(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = @"  select Parliament_RequestedToPerson.code as statuscode ,Namear as StatusName,
                                             count(Parliament_Questions.code) as QuestionCount 
                                             from      Parliament_Questions
                                             inner join dbo.Parliament_RequestedToPerson on Q_RequestTo = Parliament_RequestedToPerson.code  
                                             where QType=1 and   ChapterID=(select top 1 ChapterID from Parliament_Questions order by Q_Date desc) and  cast(Q_Date as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) group by  Parliament_RequestedToPerson.code,Namear ";

           
            var result = DC.Database.SqlQuery<QuestionsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<QuestionsModel>();

        }
        public List<QuestionsModel> getQuestionsAll(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = @"  select Parliament_legislativeChapter.NameAr as ChapterName,(select count(*)  from Parliament_Questions   where QType=1   and ChapterID=Parliament_legislativeChapter.code) as QuestionCount from Parliament_legislativeChapter order by D_Order desc";

           
            var result = DC.Database.SqlQuery<QuestionsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<QuestionsModel>();

        }
        #endregion

        #region "Cases Methods"

        public List<CasesModel> GetCasesCountByDegress(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select count(*) as CasesCount, Case_LitigationDegree.code degreecode,NameEn,NameAr from dbo.Cases_Data ";
            sequenceMaxQuery += "  inner join Case_LitigationDegree on Case_LitigationDegree.code = DegreeID ";
            sequenceMaxQuery += "  where cast(CreationDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += "  group by  Case_LitigationDegree.code,NameEn,NameAr ";

           
            var result = DC.Database.SqlQuery<CasesModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<CasesModel>();

        }

        public List<LawsModel> GetLawDocsTypes(DateTime _fromDate, DateTime _toDate)
        {


            string sequenceMaxQuery = "SELECT DocTypeID, NameEn Law_Doctypenameen, NameAr Law_Doctypenamear,COUNT(Law_DocData.code) AS DocCount ";
            sequenceMaxQuery += " FROM dbo.Law_DocData  left JOIN dbo.Law_DocType ON dbo.Law_DocData.DocTypeID = dbo.Law_DocType.Code ";
            sequenceMaxQuery += "  where cast(DocDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " GROUP BY DocTypeID,NameEn, NameAr ";
           
            var result = DC.Database.SqlQuery<LawsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<LawsModel>();

        }
        public List<LawsModel> GetLawDocsPublished(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "SELECT Count(code) AS DocCount ";
            sequenceMaxQuery += " FROM dbo.View_LawsDocs ";
            sequenceMaxQuery += "  where isPublished=1 and cast(DocDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
           
            var result = DC.Database.SqlQuery<LawsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<LawsModel>();

        }

        public List<LawsModel> getNearExpireDocs()
        {

            string sequenceMaxQuery = "select code from [Law_DocData] where ExpireDate>GETDATE() and ExpireDate<=DATEADD(MONTH,6,GETDATE()) ";

            var result = DC.Database.SqlQuery<LawsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<LawsModel>();

        }
        //public List<CasesModel> GetCasesCountByJudgmentresult(DateTime _fromDate, DateTime _toDate)
        //{

        //        string sequenceMaxQuery = "select count(*) as CasesCount,(CASE WHEN Judgmentresult = 0 THEN N'غير معرف'     WHEN  Judgmentresult=1  THEN N'القضايا التي ربحها المجلس' WHEN Judgmentresult=2 then N'القضايا التي خسرها المجلس' WHEN  Judgmentresult=3  then N'لم يصدر حكم' END) as StatusName    from dbo.Cases_Data ";

        //        sequenceMaxQuery += "  where cast(CreationDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
        //        sequenceMaxQuery += "  group by  Judgmentresult ";

        //       
        //        var result = DC.Database.SqlQuery<CasesModel>(sequenceMaxQuery).DefaultIfEmpty();
        //        return result.ToList<CasesModel>();

        //}


        public List<CasesModel> GetCasesCountByJudgmentresult(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select count(*) as CasesCount,(CASE WHEN Judgmentresult = 0 THEN N'غير معرف'     WHEN  Judgmentresult=1  THEN N'القضايا التي ربحها المجلس' WHEN Judgmentresult=2 then N'القضايا التي خسرها المجلس' WHEN  Judgmentresult=3  then N'لم يصدر حكم' END) as StatusName    from dbo.Cases_Data ";

            sequenceMaxQuery += "  where cast(CreationDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += "  group by  Judgmentresult ";

           
            var result = DC.Database.SqlQuery<CasesModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<CasesModel>();

        }


        public List<CasesModel> GetCaseparitesCount(DateTime _fromDate, DateTime _toDate, string PartyType)
        {

            string sequenceMaxQuery = "select count(*) as CasesCount,  ";
            sequenceMaxQuery += " Case_LitigationDegree.code degreecode, NameEn DegreeDesc ,FullName as partiesName ";
            sequenceMaxQuery += " from dbo.Cases_Data inner join Case_LitigationDegree on Case_LitigationDegree.code = DegreeID ";
            sequenceMaxQuery += " inner join Case_parties on CaseID = Cases_Data.code where PartyType =  " + PartyType;
            sequenceMaxQuery += " and  cast(CreationDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  Case_LitigationDegree.code, NameEn, NameAr, FullName";


            var result = DC.Database.SqlQuery<CasesModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<CasesModel>();

        }
        public List<CasesModel> GetCaseYearCount(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select count(*) as CasesCount,  ";
            sequenceMaxQuery += " Case_LitigationDegree.code degreecode, Namear DegreeDesc ,COALESCE(YEAR(TransDate),0) as TransYear ";
            sequenceMaxQuery += " from dbo.Cases_Data inner join Case_LitigationDegree on Case_LitigationDegree.code = DegreeID ";
            sequenceMaxQuery += " where  cast(TransDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  Case_LitigationDegree.code,   NameAr, YEAR(TransDate) ORDER BY YEAR(TransDate)";


            var result = DC.Database.SqlQuery<CasesModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<CasesModel>();

        }

        public List<LegalMemoModel> GetLegalMemoPerYear(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select count(*) as RecordCount,DocYear,LegalMemo_Status.NameAr as DocStatus from LegalMemo";
            sequenceMaxQuery += " inner join[dbo].[LegalMemo_Status] on DocStatusId = LegalMemo_Status.Code";
            sequenceMaxQuery += "  where cast(DocDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by DocYear,LegalMemo_Status.NameAr ";


            var result = DC.Database.SqlQuery<LegalMemoModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<LegalMemoModel>();

        }

        public List<LegalMemoModel> GetLegalMemoPerProcedure(DateTime _fromDate, DateTime _toDate)
        {

            string sequenceMaxQuery = "select count(*) as RecordCount,LegalMemo_Procedure.NameAr as DocStatus from LegalMemo";
            sequenceMaxQuery += " inner join LegalMemo_Procedure on ProcedureId = LegalMemo_Procedure.Code";
            sequenceMaxQuery += "  where cast(DocDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by  LegalMemo_Procedure.NameAr ";


            var result = DC.Database.SqlQuery<LegalMemoModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<LegalMemoModel>();

        }

        public List<LawsModel> GetLawsYearCount(DateTime _fromDate, DateTime _toDate)
        {


            string sequenceMaxQuery = "select  count(*) as DocCount,  ";
            sequenceMaxQuery += "   COALESCE(YEAR(DocDate),0) as TransYear ";
            sequenceMaxQuery += " from View_LawsDocs";
            sequenceMaxQuery += " where  cast(DocDate as date) between cast('" + _fromDate.ToString("MM/dd/yyyy") + "' as date)  and cast('" + _toDate.ToString("MM/dd/yyyy") + "' as date) ";
            sequenceMaxQuery += " group by     YEAR(DocDate) ORDER BY YEAR(DocDate)";


           
            var result = DC.Database.SqlQuery<LawsModel>(sequenceMaxQuery).DefaultIfEmpty();
            return result.ToList<LawsModel>();

        }

        #endregion


    }
}
