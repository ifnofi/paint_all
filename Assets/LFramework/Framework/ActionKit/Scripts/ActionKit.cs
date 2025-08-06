/****************************************************************************
 * Copyright (c) 2016 - 2023 liangxiegame UNDER MIT License
 * 
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using System.Collections;
using System.Threading.Tasks;

namespace LFramework
{

    public partial class ActionKit : Architecture<ActionKit>
    {
        public static ulong ID_GENERATOR = 0;
        

        public static IAction Delay(float seconds, Action callback)
        {
            return  LFramework.Delay.Allocate(seconds, callback);
        }


        public static ISequence Sequence()
        {
            return LFramework.Sequence.Allocate();
        }

        public static IAction DelayFrame(int frameCount, Action onDelayFinish)
        {
            return LFramework.DelayFrame.Allocate(frameCount, onDelayFinish);
        }

        public static IAction NextFrame(Action onNextFrame)
        {
            return LFramework.DelayFrame.Allocate(1, onNextFrame);
        }


        public static IAction Lerp(float a,float b,float duration,Action<float> onLerp,Action onLerpFinish = null)
        {
            return LFramework.Lerp.Allocate(a, b, duration, onLerp, onLerpFinish);
        }

        public static IAction Callback(Action callback)
        {
            return LFramework.Callback.Allocate(callback);
        }


        void ConditionAPI()
        {
        }

        public static IRepeat Repeat(int repeatCount = -1)
        {
            return LFramework.Repeat.Allocate(repeatCount);
        }


        public static IParallel Parallel()
        {
            return LFramework.Parallel.Allocate();
        }

        public void ComplexAPI()
        {
        }


        public static IAction Custom(Action<ICustomAPI<object>> customSetting)
        {
            var action = LFramework.Custom.Allocate();
            customSetting(action);
            return action;
        }

        public static IAction Custom<TData>(Action<ICustomAPI<TData>> customSetting)
        {
            var action = LFramework.Custom<TData>.Allocate();
            customSetting(action);
            return action;
        }


        public static IAction Coroutine(Func<IEnumerator> coroutineGetter)
        {
            return CoroutineAction.Allocate(coroutineGetter);
        }
        
        public static IAction Task(Func<Task> taskGetter)
        {
            return TaskAction.Allocate(taskGetter);
        }


        #region Events
        


        public static EasyEvent OnUpdate => ActionKitMonoBehaviourEvents.Instance.OnUpdate;
        

        public static EasyEvent OnFixedUpdate => ActionKitMonoBehaviourEvents.Instance.OnFixedUpdate;
        


        public static EasyEvent OnLateUpdate => ActionKitMonoBehaviourEvents.Instance.OnLateUpdate;
        

        public static EasyEvent OnGUI => ActionKitMonoBehaviourEvents.Instance.OnGUIEvent;
        

        public static EasyEvent OnApplicationQuit => ActionKitMonoBehaviourEvents.Instance.OnApplicationQuitEvent;

        public static EasyEvent<bool> OnApplicationPause =>
            ActionKitMonoBehaviourEvents.Instance.OnApplicationPauseEvent;

        

        public static EasyEvent<bool> OnApplicationFocus =>
            ActionKitMonoBehaviourEvents.Instance.OnApplicationFocusEvent;

        protected override void Init()
        {
        }

        #endregion
    }
}