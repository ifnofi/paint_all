﻿using UnityEditor;
using UnityEditor.UI;

namespace SRF.UI.Editor
{
    [CustomEditor(typeof(SRNumberButton))]
    [CanEditMultipleObjects]
    public class SRNumberButtonEditor : ButtonEditor
    {
        private SerializedProperty _amountProperty;
        private SerializedProperty _targetFieldProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            this._targetFieldProperty = this.serializedObject.FindProperty("TargetField");
            this._amountProperty = this.serializedObject.FindProperty("Amount");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this._targetFieldProperty);
            EditorGUILayout.PropertyField(this._amountProperty);
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
