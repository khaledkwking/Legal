using System;
namespace DomainInterface
{
    public interface IMemberShip
    {
        string AliasName { get; set; }
        //Infrastructure.DAL.ObjectChangeTracker ChangeTracker { get; set; }
        string Code { get; set; }
        string Email { get; set; }
        bool? Graduated { get; set; }
        int ID { get; set; }
        DateTime? LastUploadDate { get; set; }
        string Mobile { get; set; }
        string Name { get; set; }
        string Note { get; set; }
        void OnDeserializedMethod(System.Runtime.Serialization.StreamingContext context);
        void OnDeserializingMethod(System.Runtime.Serialization.StreamingContext context);
        string Password { get; set; }
        string Photo { get; set; }
        IRule Rule { get; set; }
        int? RuleID { get; set; }
        string Status { get; set; }
        string Thumbnail { get; set; }
        string UploadCv { get; set; }
    }
}
