using System;
namespace DomainInterface
{
    public interface IRule
    {
        //.DAL.ObjectChangeTracker ChangeTracker { get; set; }
        string Description { get; set; }
        int ID { get; set; }
        IEntityCollection<ISecurity_pr_admin> MemberShips { get; set; }
        string Note { get; set; }
        void OnDeserializedMethod(System.Runtime.Serialization.StreamingContext context);
        void OnDeserializingMethod(System.Runtime.Serialization.StreamingContext context);
        string RuleName { get; set; }
    }
}
