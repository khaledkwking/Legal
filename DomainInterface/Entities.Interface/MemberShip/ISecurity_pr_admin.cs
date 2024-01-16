using System;
namespace DomainInterface
{
    public interface ISecurity_pr_admin
    {
         int id { get; set; }
         string name { get; set; }
         string username { get; set; }
         string password { get; set; }
         Nullable<int> AdminType { get; set; }
         Nullable<int> CompanyID { get; set; }
         Nullable<int> BranchID { get; set; }
         Nullable<bool> IsActive { get; set; }
         string mobile { get; set; }
         string Email { get; set; }
         string Address { get; set; }
         Nullable<System.DateTime> RegisterDate { get; set; }
         string AdminPhoto { get; set; }
         Nullable<bool> isOperation { get; set; }
         string AccessCompnayGroup { get; set; }
 
    }
}
