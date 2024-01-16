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
    public partial class PmLettersRepository : BaseRepository
    {
  public PmLettersRepository(CMGS_DBEntities _context):base(_context)
        {
        }


        #region "letter master Data"

        #region List
        public List<View_PmLetters> GetList(int Arc_Num, int Arc_Year, string M_Serial,
            DateTime TransactionDatFrom, DateTime TransactionDatTo,
            int CategoryID,int ChapterId,string SerachKeys)
        {


                var result =
                    (from obj in DC.View_PmLetters

                     orderby obj.D_Order, obj.Arc_Date descending
                     where 1 == 1
                     && (M_Serial !="" ? obj.Arc_Serial == M_Serial : 1 == 1)
                      && (Arc_Num != 0? obj.Arc_Num == Arc_Num : 1 == 1)
                       && (Arc_Year != 0 ? obj.Arc_Year == Arc_Year : 1 == 1)

                     && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Arc_Date >= TransactionDatFrom : 1 == 1)||(TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Arc_Date >= TransactionDatFrom : 1 == 1))
                     && ( (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Arc_Date <= TransactionDatFrom : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Arc_Date <= TransactionDatFrom : 1 == 1))
                     && (CategoryID != 0 ? obj.CategoryID == CategoryID : 1 == 1)
                     && (ChapterId != 0 ? obj.ChapterId == ChapterId : 1 == 1)

                     select obj);

            var _out = result.ToList<View_PmLetters>();
            PostResultToAudit((int)SysModulesRef.pmLetter, nameof(SysModulesRef.pmLetter), "Search Result /GetList", SerachKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;


        }
        public Pm_Letters GetDetails(int _Code)
        {


                var result =
                    (from obj in DC.Pm_Letters
                     .Include("Pm_Letters_Categories")



                     where obj.code == _Code
                     select obj);
                return result.FirstOrDefault<Pm_Letters>();

        }

        public bool CheckletterExistance(string Arc_Serial, int Arc_Year, int CategoryID, int fileRefID)
        {



                var result =
                    (from obj in DC.Pm_Letters
                     where obj.Arc_Serial == Arc_Serial && obj.Arc_Year == Arc_Year && obj.CategoryID == CategoryID  &&  obj.code != fileRefID
                     select obj).FirstOrDefault<Pm_Letters>();
                if (result != null)
                {
                    return true;
                }


            return false;
        }



        public Pm_Letters GetDetailsForEdit(int _Code)
        {


                var result =
                    (from obj in DC.Pm_Letters
                     where obj.code == _Code
                     select obj);
                return result.FirstOrDefault<Pm_Letters>();

        }



        #endregion

        #region "Add ,  Update  ,Delete" letter Operation

        public int Addletter<T>(T item)
        {


                DC.Pm_Letters.Add(item as Pm_Letters);
                return DC.SaveChanges();

        }

        public int Deleteletter<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Pm_Letters).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int Updateletter<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Pm_Letters).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion


    }
}
