using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class CompaintsRepository : BaseRepository
    {

        public CompaintsRepository(CMGS_DBEntities _context):base(_context)
        {

        }

        #region "Compaints master Data"

        #region List
        public List<View_Complaints> GetList(int CType,   int Complaints_Num, int C_Year,string ComplainSerial,
            DateTime TransactionDatFrom, DateTime TransactionDatTo,
            int ChapterID, int sessionID,List<int> selectedRequestedFrom,
             string CompaintsSubject, int RelatedOrg,string ComplainfromName,string SearchKeys)
        {
            DC.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var result =
                    (from obj in DC.View_Complaints
                     //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
                     orderby obj.Complaints_Date descending
                     where obj.Complaints_Type == CType

                     && (Complaints_Num != 0 ? obj.Complaints_Num == Complaints_Num : 1 == 1)
                     && (C_Year != 0 ? obj.Complaints_Year == C_Year : 1 == 1)
                     && (ComplainSerial != "" ? obj.Complaints_Serial == ComplainSerial : 1 == 1)
                     && (ComplainfromName != "" ? obj.Complaints_OwnerName == ComplainfromName : 1 == 1)

                     && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Complaints_Date >= TransactionDatFrom : 1 == 1)||(TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Complaints_Date >= TransactionDatFrom : 1 == 1))
                     && ( (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Complaints_Date <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Complaints_Date <= TransactionDatTo : 1 == 1))



                     && (ChapterID != 0 ? obj.ChapterId == ChapterID : 1 == 1)
                     && (sessionID != 0 ? obj.SessionId == sessionID : 1 == 1)
                      && (RelatedOrg != 0 ? obj.RelatedOrgId == RelatedOrg : 1 == 1)


                     && (selectedRequestedFrom.Count > 0 ? selectedRequestedFrom.Contains( obj.OwnerId.Value)  : 1 == 1)
                     && (CompaintsSubject != "" ? obj.Complaints_Subject.Contains(CompaintsSubject) : 1 == 1)


                      // //&& (RelatedOrgs.Count > 0 ? RelatedOrgs.Contains(obj.Q_RequestTo.Value) : 1 == 1)
                      //&& (RelatedOrgs != 0 ?  DC.Parliament_Requestedby
                      //                         .Where(x=>x.PersonID== RelatedOrgs)
                      //                         .Select(x=>x.CompaintsID).Contains(obj.code)
                      //                         : 1 == 1)

                     //    //  && (JObGradeID != 0 ? obj.Parliament_Requestedby.Any(sub => sub.GradeID == JObGradeID) : 1 == 1)
                      //    && (isPrivate != true ? obj.isPrivate == false : 1 == 1)

                     select obj);

            var _out = result.ToList<View_Complaints>();



            PostResultToAudit((int)SysModulesRef.questions, "Complaints", "Search Result /GetList", SearchKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;

        }

        public List<View_Complaints> GetSuggestionList(int CType, int Complaints_Num, int C_Year, string ComplainSerial,
           DateTime TransactionDatFrom, DateTime TransactionDatTo,
           int ChapterID, int sessionID, List<int> selectedRequestedFrom,
            string CompaintsSubject, int RelatedCommitteid, string ComplainfromName, string SearchKeys)
        {
            DC.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var result =
                    (from obj in DC.View_Complaints
                         //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
                     orderby obj.Complaints_Date descending
                     where obj.Complaints_Type == CType

                     && (Complaints_Num != 0 ? obj.Complaints_Num == Complaints_Num : 1 == 1)
                     && (C_Year != 0 ? obj.Complaints_Year == C_Year : 1 == 1)
                     && (ComplainSerial != "" ? obj.Complaints_Serial == ComplainSerial : 1 == 1)
                     && (ComplainfromName != "" ? obj.Complaints_OwnerName == ComplainfromName : 1 == 1)

                     && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Complaints_Date >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Complaints_Date >= TransactionDatFrom : 1 == 1))
                     && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Complaints_Date <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Complaints_Date <= TransactionDatTo : 1 == 1))



                     && (ChapterID != 0 ? obj.ChapterId == ChapterID : 1 == 1)
                     && (sessionID != 0 ? obj.SessionId == sessionID : 1 == 1)
                      && (RelatedCommitteid != 0 ? obj.RelatedCommitteid == RelatedCommitteid : 1 == 1)


                     && (selectedRequestedFrom.Count > 0 ? selectedRequestedFrom.Contains(obj.OwnerId.Value) : 1 == 1)
                     && (CompaintsSubject != "" ? obj.Complaints_Subject.Contains(CompaintsSubject) : 1 == 1)


                     // //&& (RelatedOrgs.Count > 0 ? RelatedOrgs.Contains(obj.Q_RequestTo.Value) : 1 == 1)
                     //&& (RelatedOrgs != 0 ?  DC.Parliament_Requestedby
                     //                         .Where(x=>x.PersonID== RelatedOrgs)
                     //                         .Select(x=>x.CompaintsID).Contains(obj.code)
                     //                         : 1 == 1)

                     //    //  && (JObGradeID != 0 ? obj.Parliament_Requestedby.Any(sub => sub.GradeID == JObGradeID) : 1 == 1)
                     //    && (isPrivate != true ? obj.isPrivate == false : 1 == 1)

                     select obj);

            var _out = result.ToList<View_Complaints>();



            PostResultToAudit((int)SysModulesRef.questions, "Complaints", "Search Result /GetList", SearchKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;

        }



        public Complaints_Data GetDetails(int _Code)
        {


                var result =
                    (from obj in DC.Complaints_Data

                     where obj.Code == _Code
                     select obj);
                return result.FirstOrDefault<Complaints_Data>();

        }

        public View_Complaints GetViewDetails(int _Code)
        {


                var result =
                    (from obj in DC.View_Complaints

                     where obj.Code == _Code
                     select obj);
                return result.FirstOrDefault<View_Complaints>();

        }

        public bool checkComplainExistance(string Q_Serial ,  int fileRefID,int Complaints_Type)
        {



                var result =
                    (from obj in DC.Complaints_Data
                     where obj.Complaints_Serial == Q_Serial    &&  obj.Code  != fileRefID   && obj.Complaints_Type== Complaints_Type
                     select obj).FirstOrDefault<Complaints_Data>();
                if (result != null)
                {
                    return true;
                }


            return false;
        }

        public Complaints_Data GetDetailsForEdit(int _Code)
        {


                var result =
                    (from obj in DC.Complaints_Data
                     where obj.Code == _Code
                     select obj);
                return result.FirstOrDefault<Complaints_Data>();

        }

        #endregion

        #region "Add ,  Update  ,Delete" Compaints Operation

        public int AddCompaints<T>(T item)
        {


                DC.Complaints_Data.Add(item as Complaints_Data);
                return DC.SaveChanges();

        }

        public int DeleteCompaints<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Complaints_Data).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateCompaints<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Complaints_Data).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Compaints Child"


        #region "persons"
         public List<Complaints_Data_proposer> getComplainPersons(int ComplainId)
        {


                var result =
                    (from obj in DC.Complaints_Data_proposer

                     where obj.ComplainId == ComplainId

                     select obj);

                return result.ToList<Complaints_Data_proposer>();

        }
        public int Deleteperson<T>(T item)
        {
            DC.Entry(item as Complaints_Data_proposer).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();
        }

        public int AddPerson<T>(T item)
        {


            DC.Complaints_Data_proposer.Add(item as Complaints_Data_proposer);
            return DC.SaveChanges();

        }
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
        public List<arc_Data> FillArcDocs(int Doc_Type, int RefDocID, int parentid=0)
        {


                var result =
                    (from obj in DC.arc_Data

                     where obj.TargetModule == (int)ArcTargetModules.ComplaintsModules && obj.Doc_Type == Doc_Type && obj.RefDocID == RefDocID
                     && (parentid != 0 ? obj.ParentArcRefID == parentid : 1 == 1)
                     select obj);

                return result.ToList<arc_Data>();

        }

        public int getchildDocs( int parentid )
        {


                var result =
                    (from obj in DC.arc_Data
                     where obj.TargetModule == (int)ArcTargetModules.ComplaintsModules && obj.ParentArcRefID == parentid

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


        public List<Parliament_legislativeSession> FillChapterSession(int ChapterID )
        {


                var result =
                    (from obj in DC.Parliament_legislativeSession
                       .Include("Parliament_legislativeChapter")
                     where 1 == 1
                 && (ChapterID != 0 ? obj.ChapterID == ChapterID : 1 == 1)
                 orderby obj.sessionOrder ascending
                     select obj);

                return result.ToList<Parliament_legislativeSession>();

        }


        public Parliament_legislativeSession FillSessionDetails(int SessionID)
        {


                var result =
                    (from obj in DC.Parliament_legislativeSession
                     .Include("Parliament_legislativeChapter")
                     where obj.Code == SessionID
                     select obj);

                return result.FirstOrDefault<Parliament_legislativeSession>();

        }

        public Parliament_legislativeSession getSessionDetails(int SessionID)
        {


                var result =
                    (from obj in DC.Parliament_legislativeSession
                       .Include("Parliament_legislativeChapter")
                     where obj.Code == SessionID
                     select obj);

                return result.FirstOrDefault<Parliament_legislativeSession>();

        }

        public bool checkItemExistance(int ChapterId,string itemName, int RefId)
        {


                var result =
                    (from obj in DC.Parliament_legislativeSession
                     where obj.ChapterID== ChapterId &&  obj.NameAr == itemName && obj.Code != RefId
                     select obj).FirstOrDefault();

                if (result != null )
                {
                    return true;
                }

            return false;
        }


        #endregion
    }
}
