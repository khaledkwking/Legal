using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using Newtonsoft.Json;
using Infrastructure.DAL.ViewModels;

namespace Infrastructure.DAL
{
    public partial class CasesRepository : BaseRepository
    {

        public CasesRepository(CMGS_DBEntities _context) : base(_context)
        {
        }

        #region "Case master Data"

        #region List
        public List<View_CasesList> GetList(string fileAutoNumber, int FileInternalSerial, int fileSerialyear, DateTime TransactionDatFrom,
                        DateTime TransactionDatTo, int CaseType, int StatusID, string CaseSubject, Boolean isPrivate,
                                                      string SuitType, string SuitNum, string SuitYear, int DegreeID, int CaseDecisionID,
                                                      string partofNames, string Caseparty1, int assignedPersonID, int Judgmentresult)
        {


            var result =
                (from obj in DC.View_CasesList

                 orderby obj.CreationDate descending
                 where 1 == 1
               && (fileAutoNumber != "" ? obj.file_Serial == fileAutoNumber : 1 == 1)
               && (FileInternalSerial != 0 ? obj.FileInternalSerial == FileInternalSerial : 1 == 1)
               && (fileSerialyear != 0 ? obj.fileSerialyear == fileSerialyear : 1 == 1)
               && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.CreationDate >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.CaseCreationDate >= TransactionDatFrom : 1 == 1))
               && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.CreationDate <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.CaseCreationDate <= TransactionDatTo : 1 == 1))
               && (CaseType != 0 ? obj.CaseType == CaseType : 1 == 1)
               && (StatusID != 0 ? obj.CaseStatus == StatusID : 1 == 1)
               && (CaseSubject != "" ? obj.CaseSubject.Contains(CaseSubject) : 1 == 1)
               && (isPrivate != true ? obj.isPrivate == false : 1 == 1)
               && (SuitType != "" ? obj.SuitType == SuitType : 1 == 1)
               && (SuitNum != "" ? obj.SuitNum == SuitNum : 1 == 1)
               && (SuitYear != "" ? obj.SuitYear == SuitYear : 1 == 1)
               && (DegreeID != 0 ? obj.DegreeID == DegreeID : 1 == 1)
               && (CaseDecisionID != 0 ? obj.CaseDecisionID == CaseDecisionID : 1 == 1)
               && (Judgmentresult != 0 ? obj.JudgmentresultId == Judgmentresult : 1 == 1)
               //partofNames
               //Caseparty1
               && (partofNames != "" && partofNames != "0" ? (from cparties in DC.Case_parties
                                                              where cparties.FullName.Contains(partofNames) && cparties.PartyType == 2
                                                              select cparties.CaseID.Value).ToList().Contains(obj.Code) : 1 == 1)

               && (Caseparty1 != "" ? (from cparties in DC.Case_parties
                                       where cparties.FullName.Contains(Caseparty1) && cparties.PartyType == 1
                                       select cparties.CaseID.Value).ToList().Contains(obj.Code) : 1 == 1)


                   && (assignedPersonID != 0 ? obj.assignedPersonID == assignedPersonID : 1 == 1)
                 select obj);

            return result.ToList<View_CasesList>();

        }

        public List<View_CasesList> getCasesResult(int DegreeID)
        {


            var result =
                (from obj in DC.View_CasesList

                 orderby obj.CreationDate descending
                 where 1 == 1

               && (DegreeID != 0 ? obj.DegreeID == DegreeID : 1 == 1)

                 select obj);

            return result.ToList<View_CasesList>();

        }

        public Case_FileInformation FillFileDetails(int _Code)
        {


            var result =
                (from obj in DC.Case_FileInformation
                 .Include("Cases_Data")
                 .Include("Cases_Data.Case_parties")
                 .Include("Cases_Data.Case_LitigationDegree")
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Case_FileInformation>();

        }

        public List<View_CasesList> GetFileCases(int FileID)
        {


            var result =
                (from obj in DC.View_CasesList

                 orderby obj.CaseCreationDate descending
                 where obj.FileNumID == FileID
                 select obj);

            return result.ToList<View_CasesList>();

        }
        public List<CasesViewModel> GetFileList(string fileAutoNumber, int FileInternalSerial, int fileSerialyear, DateTime TransactionDatFrom, DateTime TransactionDatTo,
                                                      int CaseType, int StatusID, string CaseSubject, Boolean isPrivate,
                                                      string SuitType, string SuitNum, string SuitYear, int DegreeID, int CaseDecisionID,
                                                      string partofNames, string Caseparty1, int assignedPerson, string searchkeys, int Judgmentresult,int MainTypeId)
        {


            var result =
                (from obj in DC.Case_FileInformation

                 orderby obj.FileInternalSerial ascending, obj.fileSerialyear ascending
                 where 1 == 1
             && (fileAutoNumber != "" ? obj.FileAutoNumber == fileAutoNumber : 1 == 1)
                 && (FileInternalSerial != 0 ? obj.FileInternalSerial == FileInternalSerial : 1 == 1)
                 && (fileSerialyear != 0 ? obj.fileSerialyear == fileSerialyear : 1 == 1)
             //&& ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.CreationDate >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Cases_Data.FirstOrDefault().CreationDate >= TransactionDatFrom : 1 == 1))
             //&& ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.CreationDate <= TransactionDatFrom : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Cases_Data.FirstOrDefault().CreationDate <= TransactionDatFrom : 1 == 1))


             && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Cases_Data.Any(a => a.TransDate >= TransactionDatFrom) : 1 == 1))
             && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Cases_Data.Any(a => a.TransDate <= TransactionDatTo) : 1 == 1))

             && (MainTypeId != 0 ? obj.Cases_Data.Any(a => a.CaseMainType== MainTypeId) : 1 == 1)
             && (CaseType != 0 ? obj.Cases_Data.Any(a => a.CaseType == CaseType) : 1 == 1)
             && (SuitType != "" ? obj.Cases_Data.Any(a => a.SuitType == SuitType) : 1 == 1)
             && (SuitNum != "" ? obj.Cases_Data.Any(a => a.SuitNum == SuitNum) : 1 == 1)
             && (SuitYear != "" ? obj.Cases_Data.Any(a => a.SuitYear == SuitYear) : 1 == 1)
             && (assignedPerson != 0 ? obj.assignedPersonID == assignedPerson : 1 == 1)
             && (StatusID != 0 ? obj.Cases_Data.Any(a => a.CaseStatus == StatusID) : 1 == 1)
             && (CaseSubject != "" ? obj.FileNote.Contains(CaseSubject) || obj.Cases_Data.Any(a => a.CaseSubject.Contains(CaseSubject)) : 1 == 1)
             && (partofNames != "" && partofNames != "0" ? obj.Cases_Data.Any(c => c.Case_parties.Any(d => d.FullName.Contains(partofNames) && d.PartyType == 2)) : 1 == 1)
             && (DegreeID != 0 ? obj.Cases_Data.Any(a => a.DegreeID == DegreeID) : 1 == 1)
             && (CaseDecisionID != 0 ? obj.Cases_Data.Any(a => a.CaseDecisionID == CaseDecisionID) : 1 == 1)
             && (Judgmentresult != 0 ? obj.Cases_Data.Any(a => a.Judgmentresult == Judgmentresult) : 1 == 1)
             && (Caseparty1 != "" && Caseparty1 != "0" ? obj.Cases_Data.Any(c => c.Case_parties.Any(d => d.FullName.Contains(Caseparty1) && d.PartyType == 1)) : 1 == 1)
              //  && (CaseSubject != "" ? obj.Case_parties.Any(sub => sub.Person_NameAr.Contains(CaseSubject)) : 1 == 1)
              //  && (JObGradeID != 0 ? obj.Case_parties.Any(sub => sub.GradeID == JObGradeID) : 1 == 1)
              && (isPrivate != true ? obj.isPrivate == false : 1 == 1)
                 select obj).Select(c => new CasesViewModel {
                     Code = c.Code,
                     file_Serial = c.file_Serial,
                     FileNote = c.FileNote,
                     FileInternalSerial = c.FileInternalSerial,
                     fileSerialyear = c.fileSerialyear,
                     FileAutoNumber = c.FileAutoNumber,
                     TransDate = c.TransDate,
                     LastTransctionDate = c.LastTransctionDate,
                     isPrivate = c.isPrivate.Value,

                 });


            var _out = result.ToList<CasesViewModel>();
            PostResultToAudit((int)SysModulesRef.cases, nameof(SysModulesRef.cases), "Search Result /GetList", searchkeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;


        }



        public List<Cases_M_CaseTypes> FillCaseTypes(int CatId)
        {


            var result =
                (from obj in DC.Cases_M_CaseTypes
                   .Include("Cases_M_CaseMainTypes")
                 where 1 == 1
             && (CatId != 0 ? obj.CatId == CatId : 1 == 1)
                 select obj);

            return result.ToList<Cases_M_CaseTypes>();

        }

        public Cases_M_CaseTypes FillCaseTypeDetails(int SessionID)
        {


            var result =
                (from obj in DC.Cases_M_CaseTypes
                 .Include("Cases_M_CaseMainTypes")
                 where obj.Code == SessionID
                 select obj);

            return result.FirstOrDefault<Cases_M_CaseTypes>();

        }

        public bool checkItemExistance(int ChapterId, string itemName, int RefId)
        {


            var result =
                (from obj in DC.Cases_M_CaseTypes
                 where obj.CatId == ChapterId && obj.NameAr == itemName && obj.Code != RefId
                 select obj).FirstOrDefault();

            if (result != null)
            {
                return true;
            }

            return false;
        }


        public Cases_Data GetCaseDetails(int _Code)
        {


            var result =
                (from obj in DC.Cases_Data
                  .Include("Case_parties")
                 .Include("Case_LitigationDegree")
                  .Include("Cases_decision")
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Cases_Data>();

        }
        public Cases_Data GetFileLastCase(int _FileCode)
        {


            var result =
                (from obj in DC.Cases_Data
                  .Include("Case_parties")
                 .Include("Case_LitigationDegree")
                  .Include("Cases_decision")
                 where obj.FileNumID == _FileCode
                 orderby obj.Code descending
                 select obj).Take(1);

            return result.FirstOrDefault<Cases_Data>();

        }
        public Cases_Data GetDetails(int _Code)
        {


            var result =
                (from obj in DC.Cases_Data
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Cases_Data>();

        }

        public List<Medal_ProcedureHistory> FillMedalProcedures(int _Code)
        {


            var result =
                (from obj in DC.Medal_ProcedureHistory
                 .Include("Medal_M_ProcedureType")
                 orderby obj.ProcedureDate descending
                 where obj.MedalMasterID == _Code
                 select obj);

            return result.ToList<Medal_ProcedureHistory>();

        }


        public List<View_MedalData> FillMedalMater(int _Code)
        {


            var result =
                (from obj in DC.View_MedalData
                 where obj.Code == _Code
                 select obj);

            return result.ToList<View_MedalData>();

        }

        #endregion

        #region "Add ,  Update  ,Delete" Case Operation

        public int AddCase<T>(T item)
        {


            DC.Cases_Data.Add(item as Cases_Data);
            return DC.SaveChanges();

        }

        public int DeleteCase<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Cases_Data).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateCase<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Cases_Data).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion


        #region "Add ,  Update  ,Delete" Case File Operation

        public int AddFile<T>(T item)
        {


            DC.Case_FileInformation.Add(item as Case_FileInformation);
            return DC.SaveChanges();

        }

        public int Deletefile<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Case_FileInformation).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int Updatefile<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Case_FileInformation).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Case File master Data"
        public bool CheckCaseFileExistance(int fileInternalserial, int fileSerialyear, int fileRefID)
        {



            var result =
                (from obj in DC.Case_FileInformation
                 where obj.FileInternalSerial == fileInternalserial && obj.fileSerialyear == fileSerialyear && obj.Code != fileRefID
                 select obj).FirstOrDefault<Case_FileInformation>();
            if (result != null)
            {
                return true;
            }


            return false;
        }


        public bool CheckFileAutoNumberExistance(string FileAutoNumber, int fileRefID)
        {



            var result =
                (from obj in DC.Case_FileInformation
                 where obj.FileAutoNumber == FileAutoNumber & obj.Code != fileRefID
                 select obj).FirstOrDefault<Case_FileInformation>();
            if (result != null)
            {
                return true;
            }


            return false;
        }
        public Case_FileInformation GetFileDetails(int _Code)
        {


            var result =
                (from obj in DC.Case_FileInformation
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Case_FileInformation>();

        }
        #endregion


        #region "Case Child"

        #region"Persons"
        public List<Case_parties> FillPersons(int _Code, int PartyType)
        {


            var result =
                (from obj in DC.Case_parties

                 where obj.CaseID == _Code && obj.PartyType == PartyType
                 select obj);

            return result.ToList<Case_parties>();

        }
        public Case_parties getcasePartyDetails(int _Code)
        {

            var result =
                (from obj in DC.Case_parties
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Case_parties>();

        }



        #region "Add ,  Update  ,Delete" Medal Persons

        public int AddPerson<T>(T item)
        {


            DC.Case_parties.Add(item as Case_parties);
            return DC.SaveChanges();

        }

        public int DeletePersons<T>(T item)
        {

            DC.Entry(item as Case_parties).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();
        }

        public int UpdatePersons<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Case_parties).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion
        #endregion

        #region"Coming"
        public arc_Data GetArcDocDetails(int _Code)
        {


            var result =
                (from obj in DC.arc_Data
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<arc_Data>();

        }
        public List<arc_Data> FillArcDocs(int Doc_Type, int RefDocID, int parentid = 0)
        {


            var result =
                (from obj in DC.arc_Data

                 where obj.TargetModule == (int)ArcTargetModules.CasesModules && obj.Doc_Type == Doc_Type && obj.RefDocID == RefDocID
                 && (parentid != 0 ? obj.ParentArcRefID == parentid : 1 == 1)
                 select obj);

            return result.ToList<arc_Data>();

        }

        public int getchildDocs(int parentid)
        {


            var result =
                (from obj in DC.arc_Data
                 where obj.TargetModule == (int)ArcTargetModules.CasesModules && obj.ParentArcRefID == parentid

                 select obj).Count();

            return result;

        }
        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddArcData<T>(T item)
        {


            DC.arc_Data.Add(item as arc_Data);
            return DC.SaveChanges();

        }

        public int DeleteArcData<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as arc_Data).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateArcData<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as arc_Data).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion


        #region"Attachemnt"


        public arc_Attachments GetAttachemtnDetails(int _Code)
        {


            var result =
                (from obj in DC.arc_Attachments
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<arc_Attachments>();

        }

        public List<arc_Attachments> FillCaseAttachemnt(int _Code)
        {


            var result =
                (from obj in DC.arc_Attachments
                 .Include("arc_AttachmentsTypes")
                 where obj.CaseID == _Code
                 select obj);

            return result.ToList<arc_Attachments>();

        }
        public List<arc_Attachments> FillFileAttachemnt(int _FileCode)
        {


            var result =
                (from obj in DC.arc_Attachments
                 .Include("arc_AttachmentsTypes")
                 where obj.FileID == _FileCode
                 select obj);

            return result.ToList<arc_Attachments>();

        }
        public List<arc_Attachments> FillCaseDocsAttachemnt(int CaseID, int DocID, int fileID)
        {


            var result =
                (from obj in DC.arc_Attachments
                 .Include("arc_AttachmentsTypes")
                 where 1 == 1
           && (DocID != 0 ? obj.DocCode == DocID : 1 == 1)
           && (CaseID != 0 ? obj.CaseID == CaseID : 1 == 1)
           && (fileID != 0 ? obj.FileID == fileID : 1 == 1)
                 select obj);

            return result.ToList<arc_Attachments>();

        }



        public int FillCaseDocsAttachemntCount(int CaseID, int DocID, int fileID)
        {


            var result =
                (from obj in DC.arc_Attachments
                 where 1 == 1
           && (DocID != 0 ? obj.DocCode == DocID : 1 == 1)
           && (CaseID != 0 ? obj.CaseID == CaseID : 1 == 1)
           && (fileID != 0 ? obj.FileID == fileID : 1 == 1)
                 select obj);

            return result.ToList<arc_Attachments>().Count;

        }
        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddAttachement<T>(T item)
        {


            DC.arc_Attachments.Add(item as arc_Attachments);
            return DC.SaveChanges();

        }

        public int DeleteAttachment<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as arc_Attachments).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateAttachment<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as arc_Attachments).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion


        #region"Attachemnt Main"


        public Medalmain_Attachments GetAttachemtnMainDetails(int _Code)
        {


            var result =
                (from obj in DC.Medalmain_Attachments
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Medalmain_Attachments>();

        }

        public List<Medalmain_Attachments> FillMedalMainAttachemnt(int _Code)
        {


            var result =
                (from obj in DC.Medalmain_Attachments
                 .Include("Medal_M_AttachmentTypes")
                 where obj.MedalMasterCode == _Code
                 select obj);

            return result.ToList<Medalmain_Attachments>();

        }

        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddAttachementmain<T>(T item)
        {


            DC.Medalmain_Attachments.Add(item as Medalmain_Attachments);
            return DC.SaveChanges();

        }

        public int DeleteAttacjmentmain<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Medalmain_Attachments).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateAttachmentMain<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Medalmain_Attachments).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region"Hearing"
        public Cases_H_Hearing GetHearingDetails(int? _Code)
        {


            var result =
                (from obj in DC.Cases_H_Hearing
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Cases_H_Hearing>();

        }
        public List<Cases_H_Hearing> FillCaseHearing(int RefDocID)
        {


            var result =
                (from obj in DC.Cases_H_Hearing

                 where obj.CaseID == RefDocID
                 select obj);

            return result.ToList<Cases_H_Hearing>();

        }
        #region "Add ,  Update  ,Delete" Hearing proceduee

        public int AddHearing<T>(T item)
        {


            DC.Cases_H_Hearing.Add(item as Cases_H_Hearing);
            return DC.SaveChanges();

        }

        public int DeleteHearing<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Cases_H_Hearing).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateHearing<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Cases_H_Hearing).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion
        #endregion

        public int AddCaseTypes<T>(T item)
        {


            DC.Cases_M_CaseTypes.Add(item as Cases_M_CaseTypes);
            return DC.SaveChanges();

        }

        public int DeleteCaseTypes<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Cases_M_CaseTypes).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateCaseTypes<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Cases_M_CaseTypes).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


    }
}