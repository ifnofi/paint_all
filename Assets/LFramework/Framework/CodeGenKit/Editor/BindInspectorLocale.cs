

#if UNITY_EDITOR
namespace LFramework
{
    public class BindInspectorLocale
    {
        public bool CN = true;
        
        public string Type => CN ? " 类型:" : " Type:";
        public string Comment => CN ? " 注释" : " Comment";
        public string BelongsTo => CN ? " 属于:" : " Belongs 2:";
        public string Select => CN ? "选择" : "Select";
        public string Generate => CN ? " 生成代码" : " Generate Code";

        public string Bind => CN ? " 绑定设置" : " Bind Setting";
        public string ClassName => CN ? "类名" : " Class Name";
    }
}
#endif