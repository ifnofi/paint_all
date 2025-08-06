

using System;
using UnityEngine;

namespace LFramework
{
    public class ViewControllerInspectorStyle
    {
        public readonly Lazy<GUIStyle> BigTitleStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 15
        });
    }
}