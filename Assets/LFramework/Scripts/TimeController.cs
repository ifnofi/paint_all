using System;
using DG.Tweening;

public static class TimeController
{
    public static void Call(float deltaTime, Action action, string id = default)
    {
        var a = 0f;
        DOTween.To(() => a, x => a = x, 10f, deltaTime)
            .OnComplete(() => { action?.Invoke(); }).SetId(id).SetEase(Ease.Linear);
    }
    public static void Call(float deltaTime, Action action,Action<float> update, string id)
    {
        var a = 0f;
        DOTween.To(() => a, x => a = x, deltaTime, deltaTime)
            .OnComplete(() => { action?.Invoke(); }).SetId(id).SetEase(Ease.Linear).OnUpdate(() =>
            {
                update?.Invoke(a);
            });
    }

    public static void Call(float deltaTime, Action action, long id = default)
    {
        Call(deltaTime, action, id.ToString());
    }

    public static void Kill(string id, bool complete = false)
    {
        DOTween.Kill(id, complete);
    }

    public static void Kill(long id, bool complete = false)
    {
        DOTween.Kill(id.ToString(), complete);
    }
}