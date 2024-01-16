using System;
using System.Collections.Generic;
using DomainInterface;
using Utilities;

namespace DomainInterface
{
    public interface IPermissionRepository
    {
        //IList<IUser> getUsers();
        IList<IUser> GetListUsers();
        IList<IGroup> GetListGroups();
        IUser FindUserByID(int intID);

        //IUser GetUserToBeUpdate(string name);
        IUser GetUserToBeUpdate(int IntID);

        void SaveUser(IUser objUser);
        IUser GetUserNew();
        IGroup FindGroupByID(int intID);
        void DeleteUser(IUser objUser);

        IList<IApplicationPath> GetListApplicationPath();
        IList<IPermission> GetListPermission();

        void SavePermissionPathNewForUser(IPermissionPath objPermissionPath);
        IApplicationPath FindApplicationPathByTitle(string name);
        IPermissionPath GetPermissionPathNewobj();
        void DeletePermissionPathForUser(IPermissionPath objPermissionPath);
        IList<IPermissionPath> FindPermissionPathByUserID(int UserID);
       
        IGroup GetGroupNew();
        void SaveGroup(IGroup objGroup);
        void DeleteGroup(IGroup objGroup);

        IList<IPermissionPath> FindPermissionPathByGroupID(int GroupID);
        void DeletePermissionPathForGroup(IPermissionPath objPermissionPath);
        void SavePermissionPathNewForGroup(IPermissionPath objPermissionPath);

        IList<IPermission> FindPermissionByPermissionID(int PermID);

        IList<IApplicationPath> FindApplicationPathByID(int AppPathID);

        void UpdateUser(IUser objUser);
        IUser FindUserByName(string strName);
        IApplicationPath getApplicationPath(string strPage);
        IList<KeyListItem> getListKeyUsers();
        IList<KeyListItem> getListKeyGroups();
        IApplicationPath FindApplicationPathBySingleID(int AppPathID);
        IApplicationPath GetNewApplicationPath();
        void SaveApplicationPath(IApplicationPath objApplicationPath);
        void UpdateApplicationPath(IApplicationPath objApplicationPathFromUI);
        void DeleteApplicationPath(IApplicationPath objApplicationPath);

        IList<IPermissionPath> FindPermissionPathByObjectID(int ObjectID);
        void DeletePermissionPathForObjectID(IList<IPermissionPath> ListPermissionPath);
        IList<IUser> GetListUsersWithoutSuperAdmin();
        
        IList<IApplicationPath> GetListApplicationPathWithoutSuperAdmin();
        
    }
}
