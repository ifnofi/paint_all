/****************************************************
	文件：EventCenter.cs
	作者：XWL
	邮箱:  <2263007881@qq.com>
	日期：2021/6/26 15:48:2
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using LFramework;
using UnityEngine.Events;

namespace LFramework
{
    public class EventCenter : MonoSingleton<EventCenter>
    {

        
        
        /// <summary>
        /// 事件的名字    监听这个事件对应的委托函数们  UnityAction没有参数没有返回值
        /// </summary>
        private Dictionary<EventName, IEventInfo> _eventDic =
            new Dictionary<EventName, IEventInfo>();


        /// <summary>
        /// 给某函数添加事件监听  action有参数
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件的委托函数   有参数</param>
        public void AddEventListener<T>(EventName name, UnityAction<T> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T>).actions += action;
            }
            else
            {
                _eventDic.Add(name, new EventInfo<T>(action));
            }
        }
        public void AddEventListener<T,X>(EventName name, UnityAction<T,X> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T,X>).actions += action;
            }
            else
            {
                _eventDic.Add(name, new EventInfo<T,X>(action));
            }
        }
        /// <summary>
        /// 给某函数添加事件监听  action无参数
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件的委托函数   无参数</param>
        public void AddEventListener(EventName name, UnityAction action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo).actions += action;
            }
            else
            {
                _eventDic.Add(name, new EventInfo(action));
            }
        }

        /// <summary>
        /// 移除监听  action有参数
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">哪一个方法 含参数/param>
        public void RemoveEventListener<T>(EventName name, UnityAction<T> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T>).actions -= action;
            }
        }
        public void RemoveEventListener<T,X>(EventName name, UnityAction<T,X> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T,X>).actions -= action;
            }
        }
        /// <summary>
        /// 移除监听  action无参数
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">哪一个方法 无参数</param>
        public void RemoveEventListener(EventName name, UnityAction action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo).actions -= action;
            }
        }

        /// <summary>
        /// 某事件触发  action有参数
        /// </summary>
        /// <param name="name">事件的名字 有参数</param>
        public void EventTrigger<T>(EventName name,T info)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T>)?.actions?.Invoke(info); //_eventDic[name]();
            }
        }
        public void EventTrigger<T,X>(EventName name,T info,X value)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T,X>)?.actions?.Invoke(info,value); //_eventDic[name]();
            }
        }
        /// <summary>
        /// 某事件触发  action无参数
        /// </summary>
        /// <param name="name">事件的名字 无参数</param>
        public void EventTrigger(EventName name)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo)?.actions?.Invoke(); //_eventDic[name]();
            }
        }
        

        /// <summary>
        /// 事件清空,  场景切换使用
        /// </summary>
        public void ClearEvent()
        {
            _eventDic.Clear();
        }
    }
}