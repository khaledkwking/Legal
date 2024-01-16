using System;
namespace DomainInterface
{
    public interface IApplicationPath
    {
        string Path { get; set; }
        int ID { get; set; }
        string Title { get; set; }
    }
}
