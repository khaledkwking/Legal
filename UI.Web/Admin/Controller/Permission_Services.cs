using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
using Infrastructure;
using DomainInterface;
using System.Collections.ObjectModel;
using Utilities;

namespace UI.Web.Admin.WebPages
{
    public class Permission_Services
    {
        public Permission_Services()
        {
        }

        private IUser _CurrentUser;

        private IUser propCurrentUser
        {
            get
            {
                return _CurrentUser;
            }
            set
            {
                _CurrentUser = value;
            }

        }

        public IUser FindUserByName(string strUserName)
        {
            //if (propCurrentUser == null || propCurrentUser.Name != strUserName)
            {
                propCurrentUser = PermissionFactory.GetObject().FindUserByName(strUserName);
            }
            return propCurrentUser;
        }

        public IUser FindUserByID(int intID)
        {
            return PermissionFactory.GetObject().FindUserByID(intID);
        }

        public bool _IsValidUser(string strUserName, string strPassword)
        {
            bool blnResult = false;

            IUser user = FindUserByName(strUserName);

            if (user != null)
            {
                if (user.Password == strPassword)
                {
                    blnResult = true;
                }
            }

            return blnResult;
        }

        //The priority to get Permission 
        //1-Get permission apply to this user exactly 
        //2-Get permission apply to Group that user belong to it 
        public IPermissionPath IsUserHavePermissionAtPage(IUser objUser, string strPage)
        {

            List<IPermissionPath> listPermission;
            IApplicationPath objApplicationPath =
                PermissionFactory.GetObject().getApplicationPath(strPage);

            if (objApplicationPath != null)
            {
                //  //Get list if the record repeat more than one
                //listPermission = objUser.Permissions.
                // Where(per => per.ObjectID == objApplicationPath.ID && 
                // per.ObjectType == (int)OSS.DomainObjects.PermissionObjectType.Page
                //).ToList<IPermission>();

                listPermission = GetPermissionUser(objUser, objApplicationPath.ID, (int)Utilities.PermissionObjectType.Page);

                //if the user haven't permission at the object see the group contain user 
                if (listPermission == null || listPermission.Count < 1)
                    listPermission = GetPermissionGroup(objUser, objApplicationPath.ID, (int)Utilities.PermissionObjectType.Page);

                //TODo the permission in permission paths not permission and check the type of permission

                IPermissionPath objPermission = listPermission.Where(per => per.Permission.Type == "Read").FirstOrDefault<IPermissionPath>();
                return objPermission;
            }


            return null;
        }

        private List<IPermissionPath> GetPermissionUser(IUser objUser, int objID, byte Type)
        {
            List<IPermissionPath> resultListPermission = new List<IPermissionPath>();
            List<IPermissionPath> listPermission = FindPermission(objUser.PermissionPaths, objID, Type);
            foreach (IPermissionPath item in listPermission)
            {
                //if the permission contain more than record of premission to same object
                resultListPermission.Add(item);
            }

            return resultListPermission;
        }

        private List<IPermissionPath> GetPermissionGroup(IUser objUser, int objID, byte Type)
        {
            ICollection<IGroup> listGroups = objUser.Groups;
            List<IPermissionPath> resultListPermission = new List<IPermissionPath>();
            foreach (IGroup objGroup in listGroups)
            {
                List<IPermissionPath> listPermission = FindPermission(objGroup.PermissionPaths, objID, Type);
                foreach (IPermissionPath item in listPermission)
                {
                    //if the permission contain more than record of premission to same object
                    resultListPermission.Add(item);
                }
            }
            return resultListPermission;
        }

        private List<IPermissionPath> FindPermission(ICollection<IPermissionPath> listPermission, int objID, byte Type)
        {
            List<IPermissionPath> listObjPermission =
                // if there is a type inc this >>    && per.ObjectType == Type
                listPermission.Where<IPermissionPath>(per => per.ObjectID == objID).ToList<IPermissionPath>();
            return listObjPermission;
        }

        public IList<IUser> getListUsers()
        {
            IList<IUser> listUsers = PermissionFactory.GetObject().GetListUsers();
            return listUsers;
        }

        public IList<KeyListItem> getListKeyUsers()
        {
            IList<KeyListItem> listUsers = PermissionFactory.GetObject().getListKeyUsers();
            return listUsers;
        }

        //#region "Groups "
        //public IList<IGroup> getListGroups()
        //{
        //    return PermissionFactory.GetObject().GetListGroups();
        //}

        //public IList<KeyListItem> getListKeyGroups()
        //{
        //    return PermissionFactory.GetObject().getListKeyGroups();
        //}

        //public IGroup FindGroupByID(int intID)
        //{
        //    return PermissionFactory.GetObject().FindGroupByID(intID);
        //}
        //#endregion
    }
}