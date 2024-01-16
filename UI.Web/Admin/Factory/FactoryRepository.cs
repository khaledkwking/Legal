using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace UI.Web.Admin.WebPages
{
    public class FactoryRepository : Controller
    {
        private static Controller objRep = null;

        private static object SynLocK = new object();

        public static Controller getRep()
        {
            if (objRep == null)
            {
                lock (SynLocK)
                {
                    if (objRep == null)
                    {
                        objRep = new Controller();
                    }
                }
            }
            return objRep;
        }
    }
}