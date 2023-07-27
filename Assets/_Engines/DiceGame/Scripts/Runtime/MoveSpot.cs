using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DiceGame
{
    public class MoveSpot : MonoBehaviour
    {
        public int moveSpotIndex;
        public MoveSpot nextMoveSpot;
        public MoveSpot alternativePath;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;

            moveSpotIndex = int.Parse(gameObject.name.Replace("MoveSpot_",""));
        }

        private void OnDrawGizmos()
        {
            if(alternativePath != null)
            {

                if(alternativePath.transform.position.y > transform.position.y)
                {
                    Handles.color = Color.blue;
                    Handles.DrawLine(transform.position, alternativePath.transform.position, 10);
                }
                else if (alternativePath.transform.position.y < transform.position.y)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(transform.position, alternativePath.transform.position, 10);
                }

            }

            if(nextMoveSpot)
            {

                Handles.color = Color.green;
                Handles.DrawLine(transform.position, nextMoveSpot.transform.position, 5);
            }
        }
    }
}


