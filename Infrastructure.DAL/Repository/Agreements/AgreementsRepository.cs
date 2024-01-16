using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Enum;
using Infrastructure.DAL.Model;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class AgreementsRepository : BaseRepository
    {
        public AgreementsRepository(CMGS_DBEntities _context) : base(_context)
        {
        }

        #region "Agreements master Data"
        #region "Validation"

        public bool CheckAgreementExistance(int SerialNUm, int serialYear,bool isInitial, int fileRefID)
        {



                var result =
                    (from obj in DC.AgreementData
                     where obj.Agr_SerialNum == SerialNUm && obj.isInitial == isInitial && obj.Agr_SerialYear == serialYear  && obj.Code != fileRefID
                     select obj).FirstOrDefault<AgreementData>();
                if (result != null)
                {
                    return true;
                }
           // }

            return false;
        }
        #endregion
        #region List
        public List<View_AgreeementsList> GetList(int Agr_SerialNum, int Agr_SerialYear, DateTime TransactionDatFrom,
            DateTime TransactionDatTo, int AgreementType, int Agr_CatID, int orgCodeID, int StatusCode,
            int LastActionID,string partOfSubject ,Boolean isPrivate,List<int> notInList,string StatusID ,
            int ProcedureRelatedOrg,int AgrRelatedOrgId,string SelectedFilterKeys ,int assignedPersonID)
        {
                var result =
                    (from obj in DC.View_AgreeementsList
                     orderby obj.Agr_SerialYear ascending, obj.Agr_SerialNum ascending
                     where 1 == 1

                   && (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Agr_ReciveDate >= TransactionDatFrom || obj.Agr_PublishDate >= TransactionDatFrom : 1 == 1)
                   && (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Agr_ReciveDate <= TransactionDatTo || obj.Agr_PublishDate <= TransactionDatTo : 1 == 1)
                   && (AgreementType != 0 ? obj.Agr_TypeCode == AgreementType : 1 == 1)
                   && (Agr_CatID != 0 ? obj.Agr_CatID == Agr_CatID : 1 == 1)
                   && (orgCodeID != 0 ? obj.Agr_OrgID == orgCodeID : 1 == 1)
                   && (assignedPersonID != 0 ? obj.assignedPersonID == assignedPersonID : 1 == 1)
                   && (StatusCode != 0 ? obj.StatusCodeID == StatusCode : 1 == 1)
                   && (AgrRelatedOrgId != 0 ? obj.Agr_RelatedOrgId == AgrRelatedOrgId : 1 == 1)
                   && (LastActionID != 0 ? obj.LastActionID == LastActionID : 1 == 1)
                   && (ProcedureRelatedOrg != 0 ? DC.Agreement_procedureHistory.Where(c => c.RelatedOrgID == ProcedureRelatedOrg).Select(c => c.AgreementCode).ToList().Contains(obj.Code) : 1 == 1)
                   && (partOfSubject != "" ? obj.Agr_Subject.Contains(partOfSubject) : 1 == 1)
                   && (isPrivate != true ? obj.isPrivate == false : 1 == 1)
                   && (StatusID != "" ? 
                       StatusID == "1" ? obj.isInitial == true 
                       :StatusID == "0"? obj.isInitial == false
                       :StatusID == "2"? (from obj2 in DC.AgreementData
                                          where obj2.ParentID!=0
                                          select obj2.ParentID).ToList()
                                          .Contains(obj.Code)
                       : 1 == 1 
                       : 1 == 1)
                   && (Agr_SerialNum != 0 ? obj.Agr_SerialNum == Agr_SerialNum : 1 == 1)
                   && (Agr_SerialYear != 0 ? obj.Agr_SerialYear == Agr_SerialYear ||
                   (DC.View_AgreeementsList.Where(c => c.Agr_PublishDate.Value.Year == Agr_SerialYear).Select(c => c.ParentID).ToList().Contains(obj.Code)) : 1 == 1)
                   && (notInList.Count > 0 ? !notInList.Contains(obj.LastActionID.Value)  : 1 == 1)

                     select obj);

            //Post Result To Operation Audit
            var _out = result.ToList<View_AgreeementsList>();

            //PostResultToAudit((int)SysModulesRef.agreements, nameof(SysModulesRef.agreements), "Search Result /GetList", SelectedFilterKeys, JsonConvert.SerializeObject(_out), _out.Count);
            PostResultToAudit((int)SysModulesRef.agreements, nameof(SysModulesRef.agreements), "Search Result /GetList",
                SelectedFilterKeys, "", _out.Count);

            return _out;

        }
             public List<View_AgreeementsList> getRelatedAgreement(int ParentID)
        {


                var result =
                    (from obj in DC.View_AgreeementsList
                     orderby obj.Agr_SerialYear ascending, obj.Agr_SerialNum ascending
                     where obj.ParentID == ParentID

                     select obj);

                return result.ToList<View_AgreeementsList>();

        }



        public List<Agreement_procedureHistory> FillRelatedAgreementProcedures(int _Code)
        {


                var result =
                    (from obj in DC.Agreement_procedureHistory
                     .Include("Agreement_ProcedureTypes")
                     .Include("AgreementData")
                     orderby obj.ProcedureDate descending
                     where obj.AgreementData.ParentID == _Code
                     select obj);

                return result.ToList<Agreement_procedureHistory>();
           // }
        }

        public List<View_AgreeementsList> GetCurrentAgreementList()
        {


                var result =
                    (from obj in DC.View_AgreeementsList
                     orderby obj.Agr_ReciveDate descending
                     where obj.StatusCodeID==3
                     && obj.Agr_StartDate !=null
                     && (obj.Agr_EndDate ==null || obj.Agr_EndDate>=DateTime.Now)

                     select obj);

                return result.ToList<View_AgreeementsList>();
           // }
        }
        //public View_AgreeementsList FillDetails(int _Code)
        //{


        //        var result =
        //            (from obj in DC.View_AgreeementsList
        //             where obj.Code == _Code
        //             select obj);

        //        return result.FirstOrDefault<View_AgreeementsList>();

        //}

        public AgreementData FillDetails(int _Code)
        {


            var result =
                (from obj in DC.AgreementData
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<AgreementData>();

        }


        public AgreementData GetDetails(int _Code)
        {


                var result =
                    (from obj in DC.AgreementData
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<AgreementData>();
           // }
        }


        //public int GetAgreementLastProcedure(int _AgreementCode)

        //    using (var DC = new CMGS_DBEntities())
        //    {
        //        var result =
        //            (from obj in DC.Agreement_procedureHistory
        //             where obj.AgreementCode == _AgreementCode
        //             orderby obj.ProcedureDate descending
        //             select obj).FirstOrDefault<Agreement_procedureHistory>();

        //        if (result!=null)
        //        {
        //            return result.ProcedureTypeCode.Value;
        //        }

        //        return 0;
        //    }




        public Agreement_procedureHistory GetAgreementLastProcedureobj(int _AgreementCode)
        {


                var result =
                    (from obj in DC.Agreement_procedureHistory
                     where obj.AgreementCode == _AgreementCode
                     orderby obj.ProcedureDate descending
                     select obj).FirstOrDefault<Agreement_procedureHistory>();


                return result;
           // }


        }

        public List<Agreement_procedureHistory> FillAgreementProcedures(int _Code)
        {


                var result =
                    (from obj in DC.Agreement_procedureHistory
                     .Include("Agreement_ProcedureTypes")
                     orderby obj.ProcedureDate descending
                     where obj.AgreementCode == _Code
                     select obj);

                return result.ToList<Agreement_procedureHistory>();
           // }
        }


        public List<View_AgreeementsList> FillAgreementMater(int _Code)
        {


                var result =
                    (from obj in DC.View_AgreeementsList
                     where obj.Code == _Code
                     select obj);

                return result.ToList<View_AgreeementsList>();
           // }
        }

        #endregion

        #region "Add ,  Update  ,Delete" Agreement Operation

        public int AddAgreement<T>(T item)
        {


                DC.AgreementData.Add(item as AgreementData);
                return DC.SaveChanges();
           // }
        }

        public int DeleteAgreement<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as AgreementData).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateAgreement<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as AgreementData).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();
           //
        }


        #endregion


        #endregion


        #region "Agreement Child"


        #region"Procedure"
                public Agreement_procedureHistory GetprOCEDUREDetails(int _Code)
        {


                var result =
                    (from obj in DC.Agreement_procedureHistory
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Agreement_procedureHistory>();
           // }
        }

        #region "Add ,  Update  ,Delete" Agreement proceduee

        public int AddProcedure<T>(T item)
        {


                DC.Agreement_procedureHistory.Add(item as Agreement_procedureHistory);
                return DC.SaveChanges();

        }

        public int DeleteProceduret<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Agreement_procedureHistory).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateProcedure<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Agreement_procedureHistory).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region"Attachemnt"


        public Agreement_Attachments GetAttachemtnDetails(int _Code)
        {


                var result =
                    (from obj in DC.Agreement_Attachments
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Agreement_Attachments>();

        }

        public List<Agreement_Attachments> FillAgreementAttachemnt(int _Code)
        {


                var result =
                    (from obj in DC.Agreement_Attachments
                     //.Include("AgreementAttachmentTypes")
                     where obj.ProcedureID == _Code
                     select obj);

                return result.ToList<Agreement_Attachments>();

        }

        public AgreementMain_Attachments checkAgreementAttachmentExistance(int agreementId, int attchType)
        {
            var result =
                   (from obj in DC.AgreementMain_Attachments
                    .Include("AgreementAttachmentTypes")
                    where obj.AgreementCode == agreementId && obj.AttachmenttypeCode== attchType
                    orderby obj.Code descending
                    select obj);


            return result.FirstOrDefault<AgreementMain_Attachments>();
        }

        #region "Add ,  Update  ,Delete" Agreement proceduee

        public int AddAttachement<T>(T item)
        {

                DC.Agreement_Attachments.Add(item as Agreement_Attachments);
                return DC.SaveChanges();

        }

        public int DeleteAttacjment<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Agreement_Attachments).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateAttachment<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Agreement_Attachments).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region"Attachemnt Master"


        public AgreementMain_Attachments GetAttachemtnMainDetails(int _Code)
        {


                var result =
                    (from obj in DC.AgreementMain_Attachments
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<AgreementMain_Attachments>();

        }

        public List<AgreementMain_Attachments> FillAgreementMainAttachemnt(int _Code)
        {


                var result =
                    (from obj in DC.AgreementMain_Attachments
                     .Include("AgreementAttachmentTypes")
                     where obj.AgreementCode == _Code
                     select obj);

                return result.ToList<AgreementMain_Attachments>();

        }

        #region "Add ,  Update  ,Delete" Agreement proceduee

        public int AddAttachementMain<T>(T item)
        {


                DC.AgreementMain_Attachments.Add(item as AgreementMain_Attachments);
                return DC.SaveChanges();

        }

        public int DeleteAttacjmentMain<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as AgreementMain_Attachments).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateAttachmentmain<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as AgreementMain_Attachments).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion

        #endregion
    }
}
