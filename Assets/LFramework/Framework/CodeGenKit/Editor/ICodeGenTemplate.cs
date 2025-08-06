

#if UNITY_EDITOR
namespace LFramework
{
    public interface ICodeGenTemplate
    {
        CodeGenTask CreateTask(IBindGroup bindGroup);
    }
}
#endif