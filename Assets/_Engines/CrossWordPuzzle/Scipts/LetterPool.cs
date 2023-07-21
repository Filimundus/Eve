using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossWordPuzzle;

namespace CrossWordPuzzle
{
    public class LetterPool : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;            
        }
    }
}
