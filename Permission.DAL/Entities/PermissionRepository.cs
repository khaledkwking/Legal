using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using System.Web;
using Utilities;
using Permission.DAL.Repository;

namespace Permission.DAL
{
    public class PermissionRepository : IPermissionRepository
    {
        // My Data context From Permission EDM
        //  private CMGS_SECDBEntities DataContext;

        #region ""

        #endregion

        #region "Start Security_pr_admin"

        //################################ Start Security_pr_admin################################

        //Find User By ID
        public IUser FindUserByID(int intID)
        {
            using (var DataContext = new CMGS_SECDBEntities())
            {
                var varUsers = (from objUsers in DataContext.Security_pr_admin
                                    //.Include("PermissionPaths")
                                    //.Include("Groups")
                                where objUsers.id == intID
                                select objUsers).FirstOrDefault();
                return (IUser)varUsers;
            }
        }

        //Find User By Name
        public IUser FindUserByName(string strName)
        {
            using (var DataContext = new CMGS_SECDBEntities())
            {
                var varUsers = (from objUsers in DataContext.Security_pr_admin
                                    //.Include("PermissionPaths")
                                    //.Include("PermissionPaths.Permission")
                             
                                    //.Include("Groups")
                                    //.Include("Groups.PermissionPaths")
                                    //.Include("Groups.PermissionPaths.Permission")
                                where objUsers.username  == strName
                                select objUsers).FirstOrDefault();
                return (IUser)varUsers;
            }
        }

        //Get List Of User
        public IList<IUser> GetListUsers()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varUsers = (from objUsers in DataContext.Security_pr_admin
            //                      .Include("Security_pr_Permission")
            //                     //.Include("Groups")
            //                    select objUsers);
            //    return varUsers.ToList<IUser>();
            //}

            return null;
        }

        public IList<IUser> GetListUsersWithoutSuperAdmin()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varUsers = (from objUsers in DataContext.Security_pr_admin
            //                     //.Include("PermissionPaths")
            //                     //.Include("Groups")
            //                    where objUsers.username  != "superadmin"
            //                    select objUsers);
            //    return varUsers.ToList<Security_pr_admin>();
            //}
            return null;
        }

        public IList<KeyListItem> getListKeyUsers()
        {
            using (var DataContext = new CMGS_SECDBEntities())
            {

                IQueryable<KeyListItem> varUser = (from objUser in DataContext.Security_pr_admin.OfType<Repository.Security_pr_admin>()
                                                   select new KeyListItem { ID = objUser.id, Name = objUser.username  });
                return varUser.AsEnumerable().Cast<KeyListItem>().ToList<KeyListItem>();
            }
        }


        //public IPermissionPath FindPermissionPathByID(int intID)
        //{
        //    using (var DataContext = new CMGS_SECDBEntities())
        //    {
        //        var varUsers = (from obj in DataContext.PermissionPaths
        //                        where obj.ID == intID
        //                        select obj).FirstOrDefault<PermissionPath>();
        //        return (IPermissionPath)varUsers;
        //    }
        //}
        //Get User To Be Update
        //public IUser GetUserToBeUpdate(string name)

        //*********** Get User To Be Update Now Not Working *****************

        public IUser GetUserToBeUpdate(int IntID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    User Obj =
            //        (from User usr in DataContext.Security_pr_admin
            //         //where usr.Name == name
            //         where usr.ID == IntID
            //         select usr).FirstOrDefault<User>();
            //    return (IUser)Obj;
            //}
            return null;
        }

        //*********** Save User ***********
        public void SaveUser(IUser objUser)
        {
            #region "Save User"
            ////  IList<int> MyCheckedGroupsIDs = ((User)objUser).GroupsIDs;

            ////User objFromsql = (User) GetUserToBeUpdate(objUser.Name);
            //IUser objFromsql = GetUserToBeUpdate(objUser.ID);

            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    if (objFromsql != null)
            //    {
            //        //User objuser = (User)objUser;
            //        //objFromsql = (User)objUser;
            //        //User objToUpdate = (User)objFromsql;

            //        #region "test update old"
            //        //objFromsql.Name = objUser.Name;
            //        //objFromsql.Email = objUser.Email;
            //        //objFromsql.Password = objUser.Password;
            //        //objFromsql.Email = objUser.Email;
            //        //objFromsql.MobilePIN = objUser.MobilePIN;
            //        //objFromsql.Comment = objUser.Comment;
            //        //objFromsql.Groups = objUser.Groups;
            //        //objFromsql.IsLockedOut = objUser.IsLockedOut;

            //        //             DataContext.ConfigurationSetting.Attach(objUser);
            //        //ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objUser);
            //        //entry.ObjectStateManager
            //        //  .ChangeObjectState(configurationSettingToUpdate,
            //        //                     System.Data.EntityState.Modified);
            //        //entities.SaveChanges();;


            //        //               DataContext.ApplyCurrentValues<User>("Security_pr_admin", objUser as User );
            //        //                DataContext.AttachTo("Security_pr_admin", objUser as User);
            //        //              DataContext.Security_pr_admin.se
            //        //                DataContext.SetModifiedProp<User>(objUser);

            //        //ObjectStateEntry entry;
            //        //while (!DataContext.ObjectStateManager.TryGetObjectStateEntry(objUser, out entry)) ;
            //        //DataContext.Security_pr_admin.Attach((User)objUser);

            //        //var objObs = DC.Orders.Where(obs => obs.ShipAddress.Equals(strAdd));
            //        //Order objObss = objObs.FirstOrDefault<Order>();
            //        //objObss.RequiredDate = DateTime.Now.AddDays(5);

            //        //DC.ApplyCurrentValues<Order>("Orders", objObss);
            //        //DC.SaveChanges();
            //        #endregion

            //        var objgrob =
            //                     from gr in DataContext.Groups
            //                     where objUser.GroupsIDs.Contains(gr.ID)
            //                     select gr;

            //        foreach (var item in objgrob)
            //        {
            //            objUser.Groups.Add(item);
            //        }

            //        //User objToUpdate = (User)objFromsql;


            //        //DataContext.Security_pr_admin.Attach(objToUpdate);

            //        //DataContext.Security_pr_admin.MergeOption = MergeOption.NoTracking;

            //        DataContext.AttachTo("Security_pr_admin", objUser);
            //        ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objUser);
            //        entry.ObjectStateManager.ChangeObjectState(objUser, System.Data.EntityState.Modified);
            //        DataContext.ApplyCurrentValues<User>("Security_pr_admin", objUser as User);

            //        //ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objToUpdate);
            //        //entry.ObjectStateManager.ChangeObjectState(objToUpdate, System.Data.EntityState.Modified);
            //        #region "try update"
            //        //ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objToUpdate);
            //        //entry.ObjectStateManager.ChangeObjectState(objToUpdate, System.Data.EntityState.Modified);

            //        //DataContext.Refresh(RefreshMode.ClientWins, objToUpdate);
            //        //using (var DC = new MetroDBEntities())

            //        //   DataContext.ApplyChanges<T>(entitySetName, item);




            //        //var stateEntry = DataContext.ObjectStateManager.GetObjectStateEntry(objToUpdate);

            //        //foreach (var propertyName in stateEntry.CurrentValues
            //        //                             .DataRecordInfo.FieldMetadata
            //        //                             .Select(fm => fm.FieldType.Name))

            //        //You can dig into the context itself to mark the  properties as changed. 
            //        //    stateEntry.SetModifiedProperty(propertyName);


            //        //Irequests_reports _requests_reports = objRequestReport;
            //        //objContext.Update<requests_reports>("requests_reports", _requests_reports as requests_reports);

            //        //DataContext.SaveChanges();
            //        //DataContext.Dispose();
            //        #endregion

            //        DataContext.SaveChanges();
            //    }
            //    else
            //    {
            //        User objuser = (User)objUser;

            //        //User objNEWuser = new User();

            //        //objNEWuser.Name = "jjjjj";

            //        //PermissionPath objNewPer = new PermissionPath();
            //        //objNewPer.ObjectID = 1;
            //        //objNewPer.ObjectType = 0;
            //        //objNewPer.User = objNEWuser;
            //        //objNewPer.PermissionID  = 1;// (Permission)findPermissionByID();

            //        var objgrob =
            //                      from gr in DataContext.Groups
            //                      where objUser.GroupsIDs.Contains(gr.ID)
            //                      select gr;
            //        foreach (var item in objgrob)
            //        {
            //            objuser.Groups.Add(item);
            //        }

            //        #region "Test Add group"


            //        // objNEWuser.PermissionPaths.Add( (PermissionPath )   FindPermissionPathByID(1));   

            //        //   List<Group> listGroup = new List<Group>();

            //        //listGroup.Add(objgrob);
            //        //  objgrob.Security_pr_admin.Add(objNEWuser);
            //        //      objNEWuser.Groups.Add( objgrob);
            //        //objgrob.Security_pr_admin.Add(objNEWuser);
            //        //// objNEWuser.Groups.Add(objgrob);// = listGroup;
            //        #endregion

            //        DataContext.Security_pr_admin.AddObject(objuser);
            //        DataContext.SaveChanges();

            //        #region "Test1"
            //        //    User objSave = (User)GetUserNew();
            //        //    objSave.ID = objuser.ID;
            //        //    objSave.Name = objuser.Name;
            //        //    objSave.Email = objuser.Email;
            //        //    objSave.Password = objuser.Password;
            //        //    objSave.MobilePIN = objuser.MobilePIN;
            //        //    objSave.Comment = objuser.Comment;
            //        //    objSave.Groups = objuser.Groups;
            //        //    //objSave.IsLockedOut = objuser.IsLockedOut;

            //        //DataContext.AddObject("Security_pr_admin", objUser);


            //        //ObjectStateEntry entry;
            //        //while (DataContext.ObjectStateManager.TryGetObjectStateEntry(objUser, out entry)) ;
            //        //DataContext.Security_pr_admin.Attach((User)objUser);

            //        //    ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objUser);
            //        //    entry.ObjectStateManager.ChangeObjectState(objUser, System.Data.EntityState.Modified);

            //        // DataContext.SaveChanges();
            //        #endregion
            //    }
            //}
            #endregion

            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    User objUsr = (User)objUser;
            //    var objgrob = from gr in DataContext.Groups
            //                  where objUser.GroupsIDs.Contains(gr.ID)
            //                  select gr;
            //    foreach (var item in objgrob)
            //    {
            //        objUsr.Groups.Add(item);
            //    }
            //    DataContext.Security_pr_admin.AddObject(objUsr);
            //    DataContext.SaveChanges();
            //}

            

        }

        //*********** Find Permision By ID ***********
        private static Security_pr_Permission findPermissionByID()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var a =
            //           (from per in DataContext.Permissions
            //            where per.ID == 1
            //            select per).FirstOrDefault();

            //    return a;
            //}

            return null;
        }

        //*********** Get New User ***********
        public IUser GetUserNew()
        {
            ////  using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    User Obj = new User();
            //    return (IUser)Obj;
            //}

            return null;
        }

        //*********** Delete User ***********
        public void DeleteUser(IUser objUser)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    DataContext.Security_pr_admin.Attach((User)objUser);
            //    DataContext.DeleteObject(objUser);
            //    DataContext.SaveChanges();

            //}
        }

        //*********** Update User ***********
        public void UpdateUser(IUser objUserFromUI)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    User objToSave = (from u in DataContext.Security_pr_admin
            //                      where u.ID == objUserFromUI.ID
            //                      select u).FirstOrDefault();
            //    //var objUsrGrp = from UsrGrp in DataContext.Security_pr_admin
            //    List<IGroup> objUsersGroups = (from objUsrGrp in objToSave.Groups
            //                                   select objUsrGrp).ToList<IGroup>();

            //    foreach (var item in objUsersGroups)
            //    {
            //        objToSave.Groups.Remove((Group)item);

            //    }

            //    var objgrob = from gr in DataContext.Groups
            //                  where objUserFromUI.GroupsIDs.Contains(gr.ID)
            //                  select gr;

            //    objToSave.Name = objUserFromUI.Name;
            //    objToSave.Email = objUserFromUI.Email;
            //    objToSave.Password = objUserFromUI.Password;
            //    objToSave.MobilePIN = objUserFromUI.MobilePIN;
            //    objToSave.Comment = objUserFromUI.Comment;
            //    //objToSave.Groups = null;

            //    //if (objgrob != null && objgrob.Count() > 1)
            //    //{
            //        foreach (var item in objgrob)
            //        {
            //            objToSave.Groups.Add(item);
            //        }
            //    //}


            //    // ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objToSave);
            //    //  entry.ObjectStateManager.ChangeObjectState(objToSave, System.Data.EntityState.Modified);

            //    // DataContext.AttachTo("Security_pr_admin", objToSave);
            //    DataContext.ApplyCurrentValues<User>("Security_pr_admin", objToSave as User);

            //    DataContext.SaveChanges();

            //}
        }

        //################################ End Security_pr_admin ################################

        #endregion

        #region "Start Group"

        // ################################ Start Group ################################

        //*********** Save Group ***********
        public void SaveGroup(IGroup objGroup)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    Group objGrp = (Group)objGroup;

            //    var objuser =
            //                  from ur in DataContext.Security_pr_admin
            //                  where objGroup.UsersIDs.Contains(ur.ID)
            //                  select ur;
            //    foreach (var item in objuser)
            //    {
            //        objGrp.Security_pr_admin.Add(item);
            //    }

            //    DataContext.Groups.AddObject(objGrp);
            //    DataContext.SaveChanges();
            //}
        }

        //*********** Update Group ***********
        public void UpdateGroup(IGroup objGroupFromUI)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    Group objToUpdate = (from g in DataContext.Groups
            //                         where g.ID == objGroupFromUI.ID
            //                         select g).FirstOrDefault();

            //    List<IUser> objGroupsUsers = (from objGrpUsr in objToUpdate.Security_pr_admin
            //                                  select objGrpUsr).ToList<IUser>();
            //    foreach (var item in objGroupsUsers)
            //    {
            //        objToUpdate.Security_pr_admin.Remove((User)item);
            //    }

            //    var objuser =
            //                 from ur in DataContext.Security_pr_admin
            //                 where objGroupFromUI.UsersIDs.Contains(ur.ID)
            //                 select ur;

            //    objToUpdate.Name = objGroupFromUI.Name;
            //    objToUpdate.Description = objGroupFromUI.Description;

            //    foreach (var item in objuser)
            //    {
            //        objToUpdate.Security_pr_admin.Add(item);
            //    }

            //    //User objToUpdate = (User)objFromsql;


            //    //DataContext.Security_pr_admin.Attach(objToUpdate);

            //    //DataContext.Security_pr_admin.MergeOption = MergeOption.NoTracking;
            //    //ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objGroup);
            //    //entry.ObjectStateManager.ChangeObjectState(objGroup, System.Data.EntityState.Modified);

            //    //DataContext.AttachTo("Groups", objToUpdate);
            //    DataContext.ApplyCurrentValues<Group>("Groups", objToUpdate as Group);

            //    //ObjectStateEntry entry = DataContext.ObjectStateManager.GetObjectStateEntry(objToUpdate);
            //    //entry.ObjectStateManager.ChangeObjectState(objToUpdate, System.Data.EntityState.Modified);

            //    DataContext.SaveChanges();

            //}
        }

        //*********** Delete Group ***********
        public void DeleteGroup(IGroup objGroup)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    DataContext.Groups.Attach((Group)objGroup);
            //    DataContext.DeleteObject(objGroup);
            //    DataContext.SaveChanges();

            //}
        }

        //public  GetGroupsUsers (int intID)
        //{
        //    using (var DataContext = new CMGS_SECDBEntities())
        //    {
        //        var varGroups = (from objGroup in DataContext.Groups
        //                            .Include("PermissionPaths")
        //                             .Include("Security_pr_admin")
        //                         where objGroup.ID == intID
        //                         select objGroup).FirstOrDefault<Group>();
        //        return (IGroup)varGroups;
        //    }
        //}

        //*********** Get New Group ***********
        public IGroup GetGroupNew()
        {
            ////  using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    Group Obj = new Group();
            //    return (IGroup)Obj;
            //}

            return null;
        }

        //*********** Get List Of Group ***********
        public IList<IGroup> GetListGroups()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varGroups = (from objGroups in DataContext.Groups
            //                     .Include("PermissionPaths")
            //                     .Include("Security_pr_admin")
            //                     select objGroups);
            //    return varGroups.ToList<IGroup>();
            //}

            return null;
        }

        public IList<KeyListItem> getListKeyGroups()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    IQueryable<KeyListItem> varUser = (from objGroup in DataContext.Groups.OfType<Group>()
            //                                       select new KeyListItem { ID = objGroup.ID, Name = objGroup.Name });
            //    return varUser.AsEnumerable().Cast<KeyListItem>().ToList<KeyListItem>();
            //}

            return null;
        }

        //*********** Find Group By ID ***********
        public IGroup FindGroupByID(int intID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varGroups = (from objGroup in DataContext.Groups
            //                        .Include("PermissionPaths")
            //                         .Include("Security_pr_admin")
            //                     where objGroup.ID == intID
            //                     select objGroup).FirstOrDefault<Group>();
            //    return (IGroup)varGroups;
            //}

            return null;
        }

        // ################################ End Group ################################

        #endregion

        //public IList<IUser> getUsers()
        //{
        //    using (var context = new CMGS_SECDBEntities())
        //    {
        //        var objUsers = from u in context.Security_pr_admin
        //                       select u;
        //        return objUsers.ToList<IUser>();
        //    }
        //}

        #region "Start Application Paths"

        // ***** Start Application Paths*********

        //*********** Get List Of Application Path ***********
        public IList<IApplicationPath> GetListApplicationPath()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varApplicationPath = (from objApplicationPath in DataContext.ApplicationPaths
            //                              select objApplicationPath);
            //    return varApplicationPath.ToList<IApplicationPath>();
            //}
            return null;
        }

        public IList<IApplicationPath> GetListApplicationPathWithoutSuperAdmin()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varApplicationPath = (from objApplicationPath in DataContext.ApplicationPaths
            //                              where objApplicationPath.Path != "GroupPermissions.aspx" && objApplicationPath.Path != "GroupsAdmin.aspx" && objApplicationPath.Path != "UserPermissions.aspx" && objApplicationPath.Path != "UsersAdmin.aspx" && objApplicationPath.Path != "ApplicationPathAdmin.aspx"
            //                              select objApplicationPath);
            //    return varApplicationPath.ToList<IApplicationPath>();
            //}

            return null;

        }

        // *********** Find Application Path By Title ***********
        public IApplicationPath FindApplicationPathByTitle(string name)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varApplicationPaths = (from objApplicationPaths in DataContext.ApplicationPaths
            //                               where objApplicationPaths.Title == name
            //                               select objApplicationPaths).FirstOrDefault<ApplicationPath>();
            //    return (IApplicationPath)varApplicationPaths;
            //}

            return null;
        }

        // *********** Find Application Path By Path ***********
        public IApplicationPath getApplicationPath(string strPage)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varApplicationPaths = (from objApplicationPaths in DataContext.ApplicationPaths
            //                               where objApplicationPaths.Path == strPage
            //                               select objApplicationPaths).FirstOrDefault<ApplicationPath>();
            //    return (ApplicationPath)varApplicationPaths;
            //}

            return null;

        }


        //*********** Find Application Path By ID ***********
        public IList<IApplicationPath> FindApplicationPathByID(int AppPathID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varApplicationPath = (from objApplicationPath in DataContext.ApplicationPaths
            //                              where objApplicationPath.ID == AppPathID
            //                              select objApplicationPath);
            //    return varApplicationPath.ToList<IApplicationPath>();
            //}

            return null;
        }

        public IApplicationPath FindApplicationPathBySingleID(int AppPathID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varApplicationPath = (from objApplicationPath in DataContext.ApplicationPaths
            //                              where objApplicationPath.ID == AppPathID
            //                              select objApplicationPath).FirstOrDefault<ApplicationPath>();
            //    return varApplicationPath as ApplicationPath;
            //}

            return null;
        }

        public IApplicationPath GetNewApplicationPath()
        {
            //return new ApplicationPath();

            return null;
        }

        public void SaveApplicationPath(IApplicationPath objApplicationPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    DataContext.ApplicationPaths.AddObject(objApplicationPath as ApplicationPath);
            //    DataContext.SaveChanges();
            //}

        }

        public void UpdateApplicationPath(IApplicationPath objApplicationPathFromUI)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    ApplicationPath objToSave = (from u in DataContext.ApplicationPaths
            //                                 where u.ID == objApplicationPathFromUI.ID
            //                      select u).FirstOrDefault();

            //    objToSave.Title = objApplicationPathFromUI.Title;
            //    objToSave.Path = objApplicationPathFromUI.Path;

            //    DataContext.ApplyCurrentValues<ApplicationPath>("ApplicationPaths", objToSave as ApplicationPath);

            //    DataContext.SaveChanges();

            //}
        }

        public void DeleteApplicationPath(IApplicationPath objApplicationPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    DataContext.ApplicationPaths.Attach((ApplicationPath)objApplicationPath);
            //    DataContext.DeleteObject(objApplicationPath);
            //    DataContext.SaveChanges();
            //}
        }

        #endregion

        #region "Start Permissions"

        // ***** Start Permissions *********

        //*********** Get List Of Permission ***********
        public IList<IPermission> GetListPermission()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varPermission = (from objPermission in DataContext.Permissions
            //                         .Include("PermissionPaths")
            //                         select objPermission);
            //    return varPermission.ToList<IPermission>();
            //}

            return null;
        }

        //*********** Find Permission By Permission ID ***********
        public IList<IPermission> FindPermissionByPermissionID(int PermID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varPermission = (from objPermission in DataContext.Permissions
            //                        .Include("PermissionPaths")
            //                         where objPermission.ID == PermID
            //                         select objPermission);
            //    return varPermission.ToList<IPermission>();
            //}

            return null;
        }

        #endregion

        #region "Start Permission Paths"

        // ***** Start Permission Paths*********

        //Order objOrder = new Order();
        //        objOrder.ShipAddress = strAdd;

        //        DC.Orders.AddObject(objOrder);
        //        DC.SaveChanges();

        //*********** Save Permission Path For Security_pr_admin ***********
        public void SavePermissionPathNewForUser(IPermissionPath objPermissionPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var objToSave = (from objNewperm in DataContext.PermissionPaths
            //                     where objPermissionPath.ObjectID == objNewperm.ObjectID &&
            //                       objPermissionPath.PermissionID == objNewperm.PermissionID &&
            //                       objPermissionPath.UserID == objNewperm.UserID
            //                     select objNewperm);
            //    if (objToSave != null && objToSave.Count() > 0)
            //    {

            //    }
            //    else
            //    {

            //        IPermissionPath _objPermissionPath = objPermissionPath;
            //        DataContext.PermissionPaths.AddObject(objPermissionPath as PermissionPath);
            //        DataContext.SaveChanges();
            //    }
            //}
        }

        //*********** Save Permission Path  For Groups ***********
        public void SavePermissionPathNewForGroup(IPermissionPath objPermissionPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var objToSave = (from objNewperm in DataContext.PermissionPaths
            //                     where objPermissionPath.ObjectID == objNewperm.ObjectID &&
            //                       objPermissionPath.PermissionID == objNewperm.PermissionID &&
            //                       objPermissionPath.GroupID == objNewperm.GroupID
            //                     select objNewperm);
            //    if (objToSave != null && objToSave.Count() > 0)
            //    {

            //    }
            //    else
            //    {

            //        IPermissionPath _objPermissionPath = objPermissionPath;
            //        DataContext.PermissionPaths.AddObject(objPermissionPath as PermissionPath);
            //        DataContext.SaveChanges();
            //    }
            //}
        }

        //*********** Get Permission Path To Get New Object ***********
        public IPermissionPath GetPermissionPathNewobj()
        {
            ////  using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    PermissionPath Obj = new PermissionPath();
            //    return (IPermissionPath)Obj;
            //}

            return null;
        }

        //public IList<IPermissionPath> GetPermissionPathNewList()
        //{
        //    //  using (var DataContext = new CMGS_SECDBEntities())
        //    {
        //        List<PermissionPath> Lst = new List<PermissionPath>();
        //        return (IList<IPermissionPath>)Lst;
        //    }
        //}

        //*********** Delete Permission Path For Security_pr_admin ***********
        public void DeletePermissionPathForUser(IPermissionPath objPermissionPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var objToDelete = (from objperm in DataContext.PermissionPaths
            //                       where objPermissionPath.ObjectID == objperm.ObjectID &&
            //                       objPermissionPath.PermissionID == objperm.PermissionID &&
            //                       objPermissionPath.UserID == objperm.UserID
            //                       select objperm).FirstOrDefault();

            //    //PermissionPath obj = (PermissionPath)objToDelete;
            //    if (objToDelete != null)//&& objToDelete.Count() > 0)
            //    {
            //        DataContext.PermissionPaths.Attach(objToDelete);
            //        DataContext.DeleteObject(objToDelete);
            //        DataContext.SaveChanges();
            //    }
            //}
        }

        //*********** Delete Permission Path For Groups ***********
        public void DeletePermissionPathForGroup(IPermissionPath objPermissionPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var objToDelete = (from objperm in DataContext.PermissionPaths
            //                       where objPermissionPath.ObjectID == objperm.ObjectID &&
            //                       objPermissionPath.PermissionID == objperm.PermissionID &&
            //                       objPermissionPath.GroupID == objperm.GroupID
            //                       select objperm).FirstOrDefault();

            //    //PermissionPath obj = (PermissionPath)objToDelete;
            //    if (objToDelete != null)//&& objToDelete.Count() > 0)
            //    {
            //        DataContext.PermissionPaths.Attach(objToDelete);
            //        DataContext.DeleteObject(objToDelete);
            //        DataContext.SaveChanges();
            //    }
            //}
        }

        //*********** Delete Permission Path For ObjectID ***********
        public void DeletePermissionPathForObjectID(IList<IPermissionPath> ListPermissionPath)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    foreach (var objPermissionPath in ListPermissionPath)
            //    {
            //        var objToDelete = (from objperm in DataContext.PermissionPaths
            //                           where objPermissionPath.ID == objperm.ID
            //                           select objperm).FirstOrDefault();

            //        //PermissionPath obj = (PermissionPath)objToDelete;
            //        if (objToDelete != null)//&& objToDelete.Count() > 0)
            //        {
            //            DataContext.PermissionPaths.Attach(objToDelete);
            //            DataContext.DeleteObject(objToDelete);
            //            DataContext.SaveChanges();
            //        }
            //    }
                
            //}
        }

        //*********** Find Permission Path By User ID ***********
        public IList<IPermissionPath> FindPermissionPathByUserID(int UserID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varPermissionPath = (from objPermissionPath in DataContext.PermissionPaths
            //                        .Include("Group")
            //                        .Include("Permission")
            //                        .Include("User")
            //                             where objPermissionPath.UserID == UserID
            //                             select objPermissionPath);
            //    return varPermissionPath.ToList<IPermissionPath>();
            //}

            return null;

        }

        //*********** Find Permission Path By Object ID ***********
        public IList<IPermissionPath> FindPermissionPathByObjectID(int ObjectID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varPermissionPath = (from objPermissionPath in DataContext.PermissionPaths
            //                        .Include("Group")
            //                        .Include("Permission")
            //                        .Include("User")
            //                             where objPermissionPath.ObjectID == ObjectID
            //                             select objPermissionPath);
            //    return varPermissionPath.ToList<IPermissionPath>();
            //}

            return null;
        }

        //*********** Find Permission Path By Group ID ***********
        public IList<IPermissionPath> FindPermissionPathByGroupID(int GroupID)
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varPermissionPath = (from objPermissionPath in DataContext.PermissionPaths
            //                        .Include("Group")
            //                        .Include("Permission")
            //                        .Include("User")
            //                             where objPermissionPath.GroupID == GroupID
            //                             select objPermissionPath);
            //    return varPermissionPath.ToList<IPermissionPath>();
            //}


            return null;
            
        }

        //*********** Get List Of Permission Paths ***********
        public IList<IPermissionPath> GetListPermissionPath()
        {
            //using (var DataContext = new CMGS_SECDBEntities())
            //{
            //    var varPermissionPaths = (from objPermissionPaths in DataContext.PermissionPaths
            //                     .Include("Group")
            //                     .Include("Permission")
            //                     .Include("User")
            //                              select objPermissionPaths);
            //    return varPermissionPaths.ToList<IPermissionPath>();
            //}


            return null;

        }
        #endregion
    }
}
