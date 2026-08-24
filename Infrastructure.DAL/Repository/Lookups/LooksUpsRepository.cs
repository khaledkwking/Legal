using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.DAL.Model;


namespace Infrastructure.DAL
{
    public class LooksUpsRepository : BaseRepository
    {

        public LooksUpsRepository(CMGS_DBEntities _context):base(_context)
        {

        }



        #region "Shared"

        public List<Library_DocsType> FillDocTypes(int CatID)
        {
                var result =
                    (from obj in DC.Library_DocsType
                     where obj.CatId== CatID
                     orderby obj.D_Order
                     select obj);

                return result.ToList<Library_DocsType>();
        }


        public List<Library_DocsCategory> Fill_LibDocCategory()
        {


                var result =
                    (from obj in DC.Library_DocsCategory
                     where obj.isActive.Value
                     orderby obj.D_Order
                     select obj);

                return result.ToList<Library_DocsCategory>();

        }

        public List<Agreement_TypeCodes> FillAgreementTypes()
        {


                var result =
                    (from obj in DC.Agreement_TypeCodes
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_TypeCodes>();

        }


        public List<Agreement_TypeCodes> FillAgreementTypes(int includeid)
        {


                var result =
                    (from obj in DC.Agreement_TypeCodes
                     where obj.Code== includeid
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_TypeCodes>();

        }


        public List<Agreement_TypeCodes> FillAgreementExecludedTypes(int excludeid)
        {


                var result =
                    (from obj in DC.Agreement_TypeCodes
                     where obj.Code != excludeid
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_TypeCodes>();

        }
        public List<Agreement_Category> FillAgreementCategories()
        {


                var result =
                    (from obj in DC.Agreement_Category
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_Category>();

        }
        public List<Agreement_Orgs> FillOrganization()
        {


                var result =
                    (from obj in DC.Agreement_Orgs
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_Orgs>();

        }
        public List<AgreementAttachmentTypes> FillAgreementAttachmentTypes()
        {


                var result =
                    (from obj in DC.AgreementAttachmentTypes
                     orderby obj.NameAr
                     select obj);

                return result.ToList<AgreementAttachmentTypes>();

        }


        public List<Agreement_ProcedureTypes> FillProcedureTypes()
        {


                var result =
                    (from obj in DC.Agreement_ProcedureTypes
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_ProcedureTypes>();

        }
        public List<Agreement_ProcedureTypes> FillInitailProcedureTypes()
        {


                var result =
                    (from obj in DC.Agreement_ProcedureTypes
                     where obj.Code !=6
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_ProcedureTypes>();

        }
        public List<FD_RPTGrouping> FillRptGrouoing(int ModuleID)
        {


                var result =
                    (from obj in DC.FD_RPTGrouping
                     where obj.ModuleID == ModuleID
                     orderby obj.NameAr
                     select obj);

                return result.ToList<FD_RPTGrouping>();

        }
        public List<Agreement_StatusCodes> FillStatusCode()
        {


                var result =
                    (from obj in DC.Agreement_StatusCodes
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_StatusCodes>();

        }


        public List<AgreementAttachmentTypes> FillAttachmentTypes()
        {


                var result =
                    (from obj in DC.AgreementAttachmentTypes
                     orderby obj.NameAr
                     select obj);

                return result.ToList<AgreementAttachmentTypes>();

        }

        public List<Agreement_ProcedureRelatedOrgs> FillAgreementRelatedOrgs()
        {


                var result =
                    (from obj in DC.Agreement_ProcedureRelatedOrgs
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Agreement_ProcedureRelatedOrgs>();

        }
        //public List<D_StatusCode> FullCaseStatusCode()

        //    using (var DC = new CMGS_DBEntities())
        //    {
        //        var result =
        //            (from obj in DC.D_StatusCode
        //             select obj);

        //        return result.ToList<D_StatusCode>();
        //    }



        #endregion

        #region MedalLookups


        public List<Medal_M_AttachmentTypes> FillMedalattachmentTyps()
        {


                var result =
                    (from obj in DC.Medal_M_AttachmentTypes
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Medal_M_AttachmentTypes>();

        }

        public List<Medal_M_ProcedureType> FillMedalProcedureType()
        {


                var result =
                    (from obj in DC.Medal_M_ProcedureType
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Medal_M_ProcedureType>();
          //  }
        }
        public List<Medal_M_Organizations> FillMedalOrgs()
        {


                var result =
                    (from obj in DC.Medal_M_Organizations
                     orderby obj.NameAr
                     select obj);

                return result.ToList<Medal_M_Organizations>();
           // }
        }



        public List<Medal_M_Category> FillMedalsCats()
        {


                var result =
                    (from obj in DC.Medal_M_Category

                     select obj);

                return result.ToList<Medal_M_Category>();
         //   }
        }
        public List<Medal_M_Types> FillMedalsTypes()
        {


                var result =
                    (from obj in DC.Medal_M_Types

                     select obj);

                return result.ToList<Medal_M_Types>();
           // }
        }
        public  Medal_M_Types FillMedalsTypeByName(string NameAr )
        {


            var result =
                (from obj in DC.Medal_M_Types
                 where obj.NameAr == NameAr || obj.NameEn == NameAr
                 select obj);

            return result.FirstOrDefault<Medal_M_Types>();
            // }
        }
        public Medal_M_jobGrade getGradeByName(string NameAr)
        {


            var result =
                (from obj in DC.Medal_M_jobGrade
                 where obj.NameAr == NameAr || obj.NameEn == NameAr
                 select obj);

            return result.FirstOrDefault<Medal_M_jobGrade>();
            // }
        }


        public int AddMedal_M_Types<T>(T item)
        {
            DC.Medal_M_Types.Add(item as Medal_M_Types);
            return DC.SaveChanges();

        }
          public int AddMedalGrade<T>(T item)
        {
            DC.Medal_M_jobGrade.Add(item as Medal_M_jobGrade);
            return DC.SaveChanges();

        }

        public List<medal_M_Grantreasons> FillGrantReason()
        {


                var result =
                    (from obj in DC.medal_M_Grantreasons

                     select obj);

                return result.ToList<medal_M_Grantreasons>();
           // }
        }


        public List<Medal_M_jobGrade> FillJobGrade()
        {


                var result =
                    (from obj in DC.Medal_M_jobGrade
                     orderby obj.Code
                     select obj);

                return result.ToList<Medal_M_jobGrade>();
           // }
        }

        #endregion "MedalLookups"


        #region "Cases"
        
        public List<Cases_M_CaseTypes> FillCasesTypes()
        {


                var result =
                    (from obj in DC.Cases_M_CaseTypes

                     select obj);

                return result.ToList<Cases_M_CaseTypes>();
           // }
        }
        public List<Cases_M_CaseMainTypes> FillCasesMainTypes()
        {


            var result =
                (from obj in DC.Cases_M_CaseMainTypes

                 select obj);

            return result.ToList<Cases_M_CaseMainTypes>();
            // }
        }
        public List<Cases_M_Court> FillCourt()
        {


                var result =
                    (from obj in DC.Cases_M_Court

                     select obj);

                return result.ToList<Cases_M_Court>();

        }
        public List<Cases_decision> Filldession()
        {


                var result =
                    (from obj in DC.Cases_decision

                     select obj);

                return result.ToList<Cases_decision>();

        }
        public List<Cases_M_DecisionResut> FilldessionResult()
        {


            var result =
                (from obj in DC.Cases_M_DecisionResut

                 select obj);

            return result.ToList<Cases_M_DecisionResut>();

        }
        public List<Case_AssignedPersons> FillCaseAssignedPersons()
        {


                var result =
                    (from obj in DC.Case_AssignedPersons

                     select obj);

                return result.ToList<Case_AssignedPersons>();

        }

        public List<Agreement_AssignedPersons> FillAgreementsAssignedPersons()
        {


            var result =
                (from obj in DC.Agreement_AssignedPersons

                 select obj);

            return result.ToList<Agreement_AssignedPersons>();

        }


        public List<string> FillCasePatires()
        {


                var result =
                    (from obj in DC.Case_parties
                     orderby obj.FullName
                     select obj.FullName).Distinct();

                return result.ToList();

        }

        public List<Case_AutoPersons> FillCaseAutopersons()
        {


                var result =
                    (from obj in DC.Case_AutoPersons
                     select obj);

                return result.ToList();

        }


        public List<arc_AttachmentsTypes> FillAttachmentType()
        {


                var result =
                    (from obj in DC.arc_AttachmentsTypes

                     select obj);

                return result.ToList<arc_AttachmentsTypes>();

        }


        public List<Case_LitigationDegree> FillLitigationDegree()
        {


                var result =
                    (from obj in DC.Case_LitigationDegree

                     select obj);

                return result.ToList<Case_LitigationDegree>();

        }


        public List<Cases_M_Status> FillCasesStatus()
        {


                var result =
                    (from obj in DC.Cases_M_Status

                     select obj);

                return result.ToList<Cases_M_Status>();

        }
        #endregion

        #region "Questions"
        public List<Parliament_AssignedPerson> FillParliament_AssignedPerson()
        {


                var result =
                    (from obj in DC.Parliament_AssignedPerson

                     select obj);

                return result.ToList<Parliament_AssignedPerson>();

        }


        public List<Parliament_Persons> FillOMaPerson(int ChapterID)
        {
                var result =
                    (from obj in DC.Parliament_Persons
                     where obj.ChapterID == ChapterID
                     select obj);

                return result.ToList<Parliament_Persons>();
        }
       public List<Parliament_CMGSRelatedOrgs> FillParliament_CMGSRelatedOrgs()
        {
            var result =
                (from obj in DC.Parliament_CMGSRelatedOrgs
                 select obj);

            return result.ToList<Parliament_CMGSRelatedOrgs>();
        }

        public List<Parliament_Orgs> FillParliament_RelatedOrgs()
        {
                var result =
                    (from obj in DC.Parliament_Orgs
                     select obj);

                return result.ToList<Parliament_Orgs>();
        }

        public List<Parliament_legislativeChapter> FillParliament_legislativeChapter()
        {
                var result =
                    (from obj in DC.Parliament_legislativeChapter
                     orderby obj.D_Order ascending
                     select obj);

                return result.ToList<Parliament_legislativeChapter>();
        }

        public List<Pm_Letters_Categories> FillPmLetters_Categories()
        {


                var result =
                    (from obj in DC.Pm_Letters_Categories

                     select obj);

                return result.ToList<Pm_Letters_Categories>();

        }
        public List<Parliament_legislativeSession> FillParliament_legislativeSession(int ChapterID)
        {


                var result =
                    (from obj in DC.Parliament_legislativeSession
                     where obj.ChapterID == ChapterID
                     orderby obj.sessionOrder ascending
                     select obj);

                return result.ToList<Parliament_legislativeSession>();

        }
        public List<Parliament_QuestionStatus> FillParliament_QuestionStatus()
        {


                var result =
                    (from obj in DC.Parliament_QuestionStatus

                     select obj);

                return result.ToList<Parliament_QuestionStatus>();


        }

        public List<Parliament_QuestionResult> FillParliament_QuestionResult()
        {


                var result =
                    (from obj in DC.Parliament_QuestionResult

                     select obj);

                return result.ToList<Parliament_QuestionResult>();


        }

        public List<Parliament_RequestedToPerson> FillParliament_RequestedToPerson()
        {


                var result =
                    (from obj in DC.Parliament_RequestedToPerson

                     select obj);

                return result.ToList<Parliament_RequestedToPerson>();

        }
        #endregion

        #region "Law"
        public List<Law_DocType> FillLaw_DocType()
        {
                var result =
                    (from obj in DC.Law_DocType
                     where obj.Code != 5 && obj.Code != 6 && obj.Code != 7
                     select obj);

                return result.ToList<Law_DocType>();
        }

        public List<Law_DocData_Linked_Status> FillLawDocLinkedStatus()
        {
            var result =
                (from obj in DC.Law_DocData_Linked_Status
                 orderby obj.NameAr
                 select obj);

            return result.ToList<Law_DocData_Linked_Status>();
        }

        public List<Committees_Ministers> fillMinisters()
        {
            var result =
                (from obj in DC.Committees_Ministers
                 
                 select obj);

            return result.ToList<Committees_Ministers>();
        }


        public List<Committees_ProceduresTypes> fillCommittees_ProceduresTypes()
        {
            var result =
                (from obj in DC.Committees_ProceduresTypes

                 select obj);

            return result.ToList<Committees_ProceduresTypes>();
        }


        public List<Committees_RefList> FillCommitteeList()
        {
            var result =
                (from obj in DC.Committees_RefList

                 select obj);

            return result.ToList<Committees_RefList>();
        }

        public List<Law_DocType> FillLaw_DessionType()
        {


                var result =
                    (from obj in DC.Law_DocType
                     where obj.Code==5 || obj.Code == 6
                     select obj);

                return result.ToList<Law_DocType>();

        }

        public List<Law_DocType> FillLaw_project()
        {


            var result =
                (from obj in DC.Law_DocType
                 where obj.Code ==7
                 select obj);

            return result.ToList<Law_DocType>();

        }
        public List<Law_DocCategory> FillLaw_DocCategory()
        {


                var result =
                    (from obj in DC.Law_DocCategory

                     select obj);

                return result.ToList<Law_DocCategory>();

        }

        public List<Law_DocCategory> FillLaw_DocZeroCategory()
        {


                var result =
                    (from obj in DC.Law_DocCategory
                     where obj.TypeID !=3
                     select obj);

                return result.ToList<Law_DocCategory>();

        }


        public List<Law_DocCategory> FillLaw_DocCategory(int TypeID)
        {


                var result =
                    (from obj in DC.Law_DocCategory
                     where obj.TypeID == TypeID
                     select obj);

                return result.ToList<Law_DocCategory>();

        }

    

        public List<law_DocProceduresTypes> Filllaw_DocProceduresTypes()
        {


                var result =
                    (from obj in DC.law_DocProceduresTypes

                     select obj);

                return result.ToList<law_DocProceduresTypes>();

        }
        public List<law_DocProceduresTypes> FillDecissionProcedureType()
        {
            var result =
                (from obj in DC.law_DocProceduresTypes
                 where obj.Code==13
                 select obj);

            return result.ToList<law_DocProceduresTypes>();

        }

        #endregion
        #region "LegalMemo"


        public List<LegalMemo_Status> FillLegalMemoStatus()
        {
            var result =
                (from obj in DC.LegalMemo_Status
                 select obj);

            return result.ToList<LegalMemo_Status>();
        }

        public List<LegalMemo_Category> fillLLegalMemo_Category()
        {
            var result =
                (from obj in DC.LegalMemo_Category
                 select obj);

            return result.ToList<LegalMemo_Category>();
        }

        public List<LegalMemo_OrgCat> fill_LegalMemo_OrgCat()
        {
            var result =
                (from obj in DC.LegalMemo_OrgCat
                 select obj);

            return result.ToList<LegalMemo_OrgCat>();
        }

        public List<LegalMemo_Procedure> fill_LegalMemo_Procedure()
        {
            var result =
                (from obj in DC.LegalMemo_Procedure
                 select obj);

            return result.ToList<LegalMemo_Procedure>();
        }

        public List<LegalMemo_AssignedPersons> fill_LegalMemo_AssignedPersons()
        {
            var result =
                (from obj in DC.LegalMemo_AssignedPersons
                 select obj);

            return result.ToList<LegalMemo_AssignedPersons>();
        }

        public List<LegalMemo_Consultant> fillConsultant()
        {
            var result =
                (from obj in DC.LegalMemo_Consultant
                 select obj);

            return result.ToList<LegalMemo_Consultant>();
        }


        public List<LegalMemo_Org> fill_LegalMemo_Org()
        {
            var result =
                (from obj in DC.LegalMemo_Org
                 select obj);

            return result.ToList<LegalMemo_Org>();
        }


        public List<LegalMemo_Org> fill_LegalMemo_OrgByCat(int catId)
        {
            var result =
                (from obj in DC.LegalMemo_Org
                 where obj.catId == catId
                 select obj);

            return result.ToList<LegalMemo_Org>();
        }


        #endregion

        #region "Compaints"


        public List<Complaints_Committees> FillComplaints_Committees()
        {


                var result =
                    (from obj in DC.Complaints_Committees

                     select obj);

                return result.ToList<Complaints_Committees>();

        }


        public List<Complaints_RelatedOrg> FillComplaints_RelatedOrg()
        {


                var result =
                    (from obj in DC.Complaints_RelatedOrg

                     select obj);

                return result.ToList<Complaints_RelatedOrg>();

        }

        #endregion

    }
}
