using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainInterface.Repository;
using Infrastructure;
using DomainInterface;

namespace UI.Web.Admin.WebPages
{
    public class Controller
    {
        public INextPagesRepository objMetroPagesRepository;
        public IPollRepository objPollRepository;
        public IAdminRepository objAdminRepository;
        public ISurveyRepository objSurveyRepository;
        //public ITravelersCountRepository objTravelersCountRepository; 
         
        public Controller()
        {
            objMetroPagesRepository = IoC.Resolve<INextPagesRepository>();
            objPollRepository = IoC.Resolve<IPollRepository>();
            objAdminRepository = IoC.Resolve<IAdminRepository>();
            objSurveyRepository = IoC.Resolve<ISurveyRepository>();
            //objTravelersCountRepository = IoC.Resolve<ITravelersCountRepository>();
        }


    }
}   