using System;
using System.Collections.Generic;
namespace DomainInterface
{
    public interface IUser
    {
        string Comment { get; set; }
        DateTime? CreateDate { get; set; }
        string Email { get; set; }
        int? FailedPasswordAnswerAttemptCount { get; set; }
        DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
        int? FailedPasswordAttemptCount { get; set; }
        DateTime? FailedPasswordAttemptWindowStart { get; set; }
        ICollection<IGroup> Groups { get; set; }
        int ID { get; set; }
        bool? IsApproved { get; set; }
        bool? IsLockedOut { get; set; }
        DateTime? LastActivityDate { get; set; }
        DateTime? LastLockoutDate { get; set; }
        DateTime? LastLoginDate { get; set; }
        DateTime? LastPasswordChangedDate { get; set; }
        string MobilePIN { get; set; }
        string Name { get; set; }
        string NameActiveDirectory { get; set; }
        string Password { get; set; }
        string PasswordAnswer { get; set; }
        string PasswordQuestion { get; set; }
        ICollection<IPermissionPath> PermissionPaths { get; set; }
        IList<int> GroupsIDs { get; set; }

    }
}
