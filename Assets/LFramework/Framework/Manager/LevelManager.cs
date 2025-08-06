using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LFramework
{
    public class LevelManager : MonoBehaviour
    {
        
        
        
        private static List<string> _levelNames;
        public static int Index { get; set; }

        public static void Init(List<string> levelNames)
        {
            
            _levelNames = levelNames;

            Index = 0;
        }

        public static void LoadCurrent()
        {
            SceneManager.LoadScene(_levelNames[Index]);
        }
        
        public static void LoadNext()
        {

            Index++;

            if (Index>=_levelNames.Count)
            {
                Index = 0;
            }
            
            SceneManager.LoadScene(_levelNames[Index]);
        }
        
        
        public static void Load()
        {
            SceneManager.LoadScene(_levelNames[Index]);
        }
    }
}