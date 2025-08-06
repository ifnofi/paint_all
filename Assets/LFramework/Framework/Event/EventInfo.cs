/****************************************************
	文件：EventInfo.cs
	作者：XWL
	邮箱:  <2263007881@qq.com>
	日期：2021/6/26 16:57:35
	功能：Nothing
*****************************************************/

using UnityEngine.Events;

namespace LFramework
{
	public class EventInfo<T>:IEventInfo
	{
		public UnityAction<T> actions;

		public EventInfo(UnityAction<T> action)
		{
			actions += action;
		}
	}
	public class EventInfo<T,X>:IEventInfo
	{
		public UnityAction<T,X> actions;

		public EventInfo(UnityAction<T,X> action)
		{
			actions += action;
		}
	}
	public class EventInfo:IEventInfo
	{
		public UnityAction actions;

		public EventInfo(UnityAction action)
		{
			actions += action;
		}
	}
}
