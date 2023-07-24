using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class MoveSpot : MonoBehaviour
    {
        public int moveSpotIndex;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }
}


