using System.Collections.Generic;
using UnityEngine;

namespace LFramework
{
    public class ResLoder
    {
        public static List<Res> SharedReses = new List<Res>();

        private List<Res> _resRecord = new List<Res>();

        public T LoadAsset<T>(string assetName) where T : Object
        {
            // 查询本地
            var res = _resRecord.Find(loadAsset => loadAsset.Name == assetName);

            if (res != null) return res.Asset as T;

            // 查询全局
            res = SharedReses.Find(loadAsset => loadAsset.Asset);

            if (res != null)
            {
                
                _resRecord.Add(res);
                res.Retain();
                return res.Asset as T;
            }

            // 加载资源
            var asset= Resources.Load<T>(assetName);
            res = new Res(asset);
            res.Retain();

            // 添加本地和全局
            SharedReses.Add(res);
            _resRecord.Add(res);
            return res.Asset as T;
        }

        public void ReleaseAll()
        {
            _resRecord.ForEach(res => res.Release());

            _resRecord.Clear();
            _resRecord = null;
        }
    }
}