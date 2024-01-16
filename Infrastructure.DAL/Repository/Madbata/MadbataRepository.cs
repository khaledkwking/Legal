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
    public partial class MadbataRepository : BaseRepository
    {

        public MadbataRepository(CMGS_DBEntities _context):base(_context)
        {

        }

        #region "Madbata master Data"

        #region List
        public List<View_MadbataList> GetList(string M_Serial, DateTime TransactionDatFrom, DateTime TransactionDatTo,
            int ChapterID, int sessionID,bool notinLint=false,int targetQuestionId=0,string SearchKeys="")
        {


                var result =
                    (from obj in DC.View_MadbataList
                     orderby obj.D_Order, obj.M_Date ascending
                  //   orderby obj.D_Order, obj.M_Serial
                     where 1 == 1
                     && (M_Serial !="" ? obj.M_Serial == M_Serial : 1 == 1)

                     && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.M_Date >= TransactionDatFrom : 1 == 1)||(TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.M_Date >= TransactionDatFrom : 1 == 1))
                     && ( (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.M_Date <= TransactionDatFrom : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.M_Date <= TransactionDatFrom : 1 == 1))
                     && (ChapterID != 0 ? obj.ChapterID == ChapterID : 1 == 1)
                     && (sessionID != 0 ? obj.SessionID == sessionID : 1 == 1)
                     && (notinLint == true ?!(from obj2 in DC.Parliament_Questions_Madbata
                                             where obj2.QuestionRefId== targetQuestionId
                                             select obj2.MadbataRefId  ).Contains(obj.code) : 1 == 1)

                     select obj);


            var _out = result.ToList<View_MadbataList>();
            PostResultToAudit((int)SysModulesRef.questions, "MadbataList", "Search Result /GetList", SearchKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;


        }


        public List<View_QuestionsMadbata> GetQuestionMadbata(int QuestionRefId)
        {


                var result =
                    (from obj in DC.View_QuestionsMadbata

                     orderby obj.M_Serial
                     where obj.QuestionRefId == QuestionRefId
                     select obj);

                return result.ToList<View_QuestionsMadbata>();

        }

        public Parliament_madbata GetDetails(int _Code)
        {


                var result =
                    (from obj in DC.Parliament_madbata
                     .Include("Parliament_legislativeChapter")
                     .Include("Parliament_legislativeSession")


                     where obj.code == _Code
                     select obj);
                return result.FirstOrDefault<Parliament_madbata>();

        }

        public bool CheckMadbataExistance(string M_Serial , int ChapterID, int fileRefID)
        {



                var result =
                    (from obj in DC.Parliament_madbata
                     where obj.M_Serial == M_Serial && obj.ChapterID == ChapterID  &&  obj.code != fileRefID
                     select obj).FirstOrDefault<Parliament_madbata>();
                if (result != null)
                {
                    return true;
                }


            return false;
        }



        public Parliament_madbata GetDetailsForEdit(int _Code)
        {


                var result =
                    (from obj in DC.Parliament_madbata
                     where obj.code == _Code
                     select obj);
                return result.FirstOrDefault<Parliament_madbata>();

        }



        #endregion

        #region "Add ,  Update  ,Delete" Madbata Operation

        public int AddMadbata<T>(T item)
        {


                DC.Parliament_madbata.Add(item as Parliament_madbata);
                return DC.SaveChanges();

        }

        public int DeleteMadbata<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Parliament_madbata).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateMadbata<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Parliament_madbata).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Madbata link"


        public List<Parliament_Questions_Madbata> GetQuestionLinkedMadbata(int QuestionRefId,int madbataRefId)
        {


                var result =
                    (from obj in DC.Parliament_Questions_Madbata


                     where obj.QuestionRefId == QuestionRefId && obj.MadbataRefId == madbataRefId
                     select obj);

                return result.ToList<Parliament_Questions_Madbata>();

        }

        #region "Add ,  Update  ,Delete" Madbata link Operation

        public int AddQuestionMadbata<T>(T item)
        {


                DC.Parliament_Questions_Madbata.Add(item as Parliament_Questions_Madbata);
                return DC.SaveChanges();

        }

        public int DeleteQuestionMadbata<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Parliament_Questions_Madbata).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateQuestionMadbata<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Parliament_Questions_Madbata).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion
        #endregion
    }
}
