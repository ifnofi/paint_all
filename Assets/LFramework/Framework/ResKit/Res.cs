using UnityEngine;

namespace LFramework
{
    public class Res
    {
        public Res(Object asset)
        {
            Asset = asset;
        }

        public Object Asset { get; private set; }

        public string Name
        {
            get { return Asset.name; }
        }

        private int _referenceCount = 0;

        public void Retain()
        {
            _referenceCount++;
        }

        public void Release()
        {
            _referenceCount--;
            if (_referenceCount != 0) return;
            Resources.UnloadAsset(Asset);
            ResLoder.SharedReses.Remove(this);
            Asset = null;
        }

    }
}