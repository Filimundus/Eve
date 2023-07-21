using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CrossWordPuzzle; 


namespace CrossWordPuzzle
{
    [CustomEditor(typeof(Letter))]
    [CanEditMultipleObjects]
    public class LetterInspector : Editor
    {
        SpriteRenderer spriteRenderer;
        private SerializedProperty m_spriterenderer;
        private string letterInput;
        private string text;
        private bool editingText;
        Letter targetObject;

        void OnEnable()
        {
            targetObject = (Letter)target;

        }

        public override void OnInspectorGUI()
        {
            if (EditorGUIUtility.editingTextField)
                editingText = true;

            targetObject.letterID = EditorGUILayout.TextField("LetterID", targetObject.letterID);



            if (GUILayout.Button("Clear"))
            {
                editingText = false;
                targetObject.letterID = "";
                ClearLetter();
            }

            if (GUILayout.Button("Refresh"))
            {
                editingText = false;
                UpdateLetters();
            }

            if (editingText && !EditorGUIUtility.editingTextField)
            {
                editingText = false;
                UpdateLetters();
            }
        }

        void UpdateLetters()
        {
            CrossWordManager crossWordManager = GameObject.FindObjectOfType<CrossWordManager>();

            if (crossWordManager.customAlphabetFile != null)
            {
                int index = -1;

                for (int i = 0; i < crossWordManager.customAlphabetFile.alpabetLetters.Length; i++)
                {
                    if (crossWordManager.customAlphabetFile.alpabetLetters[i].letterString.ToLower() == targetObject.letterID.ToLower())
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    targetObject.spriteRenderer.sprite = crossWordManager.customAlphabetFile.alpabetLetters[index].letterSprite;
                    targetObject.gameObject.name = "Letter_" + targetObject.letterID;
                    targetObject.letterText.text = "";
                }
            }
            else
            {
                targetObject.spriteRenderer.sprite = null;
                targetObject.letterText.text = targetObject.letterID;
            }

            EditorUtility.SetDirty(targetObject);
            Debug.Log(targetObject.letterID);
        }

        void ClearLetter()
        {
            targetObject.spriteRenderer.sprite = null;
            targetObject.letterText.text = "";

            EditorUtility.SetDirty(targetObject);
        }
    }
}

