using System;
using UnityEngine;

namespace LFramework
{
    public enum EnvironmentMode
    {
        Developing,
        Test,
        Production
    }

    public abstract class MainManager : MonoBehaviour
    {
        public EnvironmentMode mode;

        private static EnvironmentMode _sharedMode;
        private static bool _mModeChanged;

        static MainManager()
        {
            _mModeChanged = false;
        }

        private void Start()
        {
            if (!_mModeChanged)
            {
                _sharedMode = mode;
                _mModeChanged = true;
            }
            
            
            switch (_sharedMode)
            {
                case EnvironmentMode.Developing:
                    LaunchInDevelopingMode();
                    break;
                case EnvironmentMode.Test:
                    LaunchInTestMode();
                    break;
                case EnvironmentMode.Production:
                    LaunchInProductionMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract void LaunchInDevelopingMode();
        protected abstract void LaunchInTestMode();
        protected abstract void LaunchInProductionMode();
    }
}