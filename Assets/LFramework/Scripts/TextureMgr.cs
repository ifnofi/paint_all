using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LFramework;


public class TextureMgr : MonoSingleton<TextureMgr>
{
    public Queue<T2dData> queue = new Queue<T2dData>();
    public List<T2dData> cache = new List<T2dData>();

    private IEnumerator Start()
    {
        yield return (LoadQueue());
    }

    private IEnumerator LoadQueue()
    {
        while (true)
        {
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    var t2dData = queue.Dequeue();
                    yield return t2dData.Load();
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }

    public bool QueueEmpty()
    {
        lock (queue)
            return queue.Count == 0;
    }

    public void AppendT2d(T2dData t2dData, bool isCache = false)
    {
        lock (queue)
        {
            queue.Enqueue(t2dData);
        }

        if (isCache)
        {
            cache.Add(t2dData);
        }
    }
}