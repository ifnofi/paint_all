/****************************************************
	文件：IEventInfo.cs
	作者：XWL
	邮箱:  <2263007881@qq.com>
	日期：2021/6/26 16:57:57
	功能：事件接口,实现泛型的委托方法
*****************************************************/

namespace LFramework
{
    public interface IEventInfo { }

    public enum EventName
    {
        None,
	    ShowPanel,
	    HidePanel,
	    PanelHide
    }
}