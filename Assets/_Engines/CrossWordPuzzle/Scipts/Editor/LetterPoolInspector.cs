using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrossWordPuzzle
{
    [CustomEditor(typeof(LetterPool))]
    [CanEditMultipleObjects]
    public class LetterPoolInspector : Editor
    {
        LetterPool targetObject;

        void OnEnable()
        {
            targetObject = (LetterPool)target;

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate pool position"))
            {
                targetObject.ClearPoolPositions();
                targetObject.GeneratePoolPositions();
                EditorUtility.SetDirty(targetObject);
            }

            if (GUILayout.Button("Clear pool positions"))
            {
                targetObject.ClearPoolPositions();
                EditorUtility.SetDirty(targetObject);
            }
        }
    }
}
