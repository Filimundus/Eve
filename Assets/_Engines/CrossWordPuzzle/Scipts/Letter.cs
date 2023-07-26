using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossWordPuzzle
{
    public class Letter : MonoBehaviour
    {
        public bool useFont;
        public string letterID;
        public LayoutSpot currentLayoutSpot;
        public LetterPosition currentPosition;
        public SpriteRenderer spriteRenderer;
        public TMPro.TMP_Text letterText;
        public Vector3 startScale;
        public bool isGrabbed;
        public bool isLocked;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            letterText = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
            startScale = transform.localScale;
        }

        public void SetSprite(Sprite sprite)
        {
            if(useFont == false)
            {
                spriteRenderer.sprite = sprite;
            }
        }

        public void Grab()
        {
            if (isLocked)
                return;

            isGrabbed = true;
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void UnLock()
        {
            isLocked = false;
        }

        public void Drop()
        {
            isGrabbed = false;
        }

        public void SetPosition(LetterPosition letterPosition)
        {
            currentPosition = letterPosition;
        }

        public void ClearPosition()
        {
            if(currentPosition != null)
                currentPosition.occupied = false;
        }
    }
}

