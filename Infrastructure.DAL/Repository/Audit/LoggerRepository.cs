using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using Infrastructure.DAL.ViewModels;

namespace Infrastructure.DAL
{
    public partial class LoggerRepository : BaseRepository
    {
  public LoggerRepository(CMGS_DBEntities _context):base(_context)
        {
        }


        #region "LoggerRepository master Data"

        #region List


        public int AddLoggere<T>(T item)
        {


            DC.LoggerException.Add(item as LoggerException);
            return DC.SaveChanges();

        }

        #endregion

        #endregion


    }
}
