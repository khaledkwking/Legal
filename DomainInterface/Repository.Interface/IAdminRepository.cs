using System;
using DomainInterface;
using System.Collections.Generic;
 
namespace DomainInterface
{
    public interface IAdminRepository
    {
        //int AddCareer<T>(T item);
        //int DeleteCareer<T>(T item);
        //int UpdateCareer<T>(T item);

        //int AddTender<T>(T item);
        //int DeleteTender<T>(T item);
        //int UpdateTender<T>(T item);

        //int AddDepartment<T>(T item);
        //int DeleteDepartment<T>(T item);
        //int UpdateDepartment<T>(T item);

        //int AddEmail<T>(T item);
        //int DeleteEmail<T>(T item);
        //int UpdateEmail<T>(T item);

        //int AddApplyForm<T>(T item);
        //int DeleteApplyForm<T>(T item);
        //int UpdateApplyForm<T>(T item);

        //int AddPressCenter<T>(T item);
        //int DeletePressCenter<T>(T item);
        //int UpdatePressCenter<T>(T item);

        //int AddUploadPath<T>(T item);
        //int DeleteUploadPath<T>(T item);
        //int UpdateUploadPath<T>(T item);

        //int AddPartner<T>(T item);
        //int DeletePartner<T>(T item);
        //int UpdatePartner<T>(T item);

        //int AddTeamWork<T>(T item);
        //int DeleteTeamWork<T>(T item);
        //int UpdateTeamWork<T>(T item);

        //int AddCourse<T>(T item);
        //int DeleteCourse<T>(T item);
        //int UpdateCourse<T>(T item);

        //int AddTrack<T>(T item);
        //int DeleteTrack<T>(T item);
        //int UpdateTrack<T>(T item);

        //int AddCategory<T>(T item);
        //int DeleteCategory<T>(T item);
        //int UpdateCategory<T>(T item);

        //int AddRule<T>(T item);
        //int DeleteRule<T>(T item);
        //int UpdateRule<T>(T item);

        //int AddMemberShip<T>(T item);
        //int DeleteMemberShip<T>(T item);
        //int UpdateMemberShip<T>(T item);

        //int AddClient<T>(T item);
        //int DeleteClient<T>(T item);
        //int UpdateClient<T>(T item);

        //int AddClientFeedBack<T>(T item);
        //int DeleteClientFeedBack<T>(T item);
        //int UpdateClientFeedBack<T>(T item);



        //IList<IRule> GetRules();
        //IList<IRule> GetRuleForAdmin();
        //IList<IMemberShip> GetMemberShips();
        //IList<IMemberShip> GetMemberShipsForAdmin();
        ISecurity_pr_admin GetMemberShipByName(string Name);

        //IRule GetNewRule();
        //IMemberShip GetNewMemberShip();


        //IList<IKeyListItem> FindCareersKeys();
        //IList<IKeyListItem> FindTendersKeys();
        //IList<IKeyListItem> FindEmailsKeys();
        //IList<IKeyListItem> FindPressCentersKeys();
        //IList<IKeyListItem> FindTeamWorksKeys();
        //IList<IKeyListItem> GetProjectsKeys();
        //IList<IKeyListItem> FindPartnersKeys();
        //IList<IKeyListItem> FindCourseKeys();
        //IList<IKeyListItem> FindTrackKeys();
        //IList<IKeyListItem> FindCategoryKeys();
        //IList<IKeyListItem> FindRuleKeys();
        //IList<IKeyListItem> FindMemberShipKeys();
        //IList<IKeyListItem> FindClientKeys();
        //IList<IKeyListItem> FindClientFeedBacksKeys();



        //IRule GetRuleByID(int ID);
        //IMemberShip GetMemberShipByID(int ID);
        //IList<IMemberShip> GetMemberShipByRuleID(int ID);


        //int AddProject<T>(T item);
        //int DeleteProject<T>(T item);
        //int UpdateProject<T>(T item);


        //int AddProjectCateg<T>(T item);


        //// New Code For White Center

        //// Divisions

        //int AddDivision<T>(T item);
        //int DeleteDivisions<T>(T item);
        //int UpdateDivisions<T>(T item);


        //int UpdateTAW_Access_Level<T>(T item);
    }
}
