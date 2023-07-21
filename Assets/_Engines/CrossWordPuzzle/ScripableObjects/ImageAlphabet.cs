using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossWordPuzzle
{
    [System.Serializable]
    public struct ImageLetter
    {
        public string letterString;
        public Sprite letterSprite;
    }

    [CreateAssetMenu(fileName = "Data", menuName = "CrossWordPuzzle/ImageAlphabetObject", order = 1)]
    public class ImageAlphabet : ScriptableObject
    {
        public ImageLetter[] alpabetLetters;
    }

}

