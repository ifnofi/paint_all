

namespace LFramework
{
    using UnityEngine;

    public interface IBind
    {
        string TypeName { get; }
        
        string Comment { get; }

        Transform Transform { get; }
    }

    public interface IBindGroup
    {
        string TemplateName { get; }
    }
}