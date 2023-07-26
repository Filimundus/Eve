using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DiceGame
{
    public class MoveSpot : MonoBehaviour
    {
        public int moveSpotIndex;
        public MoveSpot alternativePath;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        private void OnDrawGizmos()
        {
            if(alternativePath != null)
            {
                Handles.color = Color.blue;
                Handles.DrawLine(transform.position,alternativePath.transform.position,10);
            }
        }
    }
}


