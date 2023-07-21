using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossWordPuzzle;

namespace CrossWordPuzzle
{

    public class LetterPool : MonoBehaviour
    {
        [Header("Settings")]

        public List<LayoutSpot> layoutSpots;
        public int amount;
        public float spacing;
        public Vector2 layoutArea;
        public GameObject layoutSpotPrefab;
        private SpriteRenderer spriteRenderer;


        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            GenerateLayout();
        }

        void GenerateLayout()
        {
            layoutSpots = new List<LayoutSpot>();

            for (int i = 0; i < amount; i++)
            {
                Vector3 position = transform.TransformPoint(new Vector3(((-layoutArea.x + 0.4f) * 0.5f) + (spacing * i), 0, 0));

                LayoutSpot layoutSpot =  GameObject.Instantiate(layoutSpotPrefab).GetComponent<LayoutSpot>();
                layoutSpot.transform.position = position;
                layoutSpot.isOccupied = false;

                layoutSpots.Add(layoutSpot);
            }
        }

        public LayoutSpot GetAvaiblePosition()
        {
            LayoutSpot avaibleSpot = null;

            for(int i = 0; i < layoutSpots.Count; i++)
            {
               if(layoutSpots[i].isOccupied == false)
                {
                    avaibleSpot = layoutSpots[i];
                    break;
                }
              
            }

            return avaibleSpot;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(layoutArea.x, layoutArea.y, 0));
                
            spacing = layoutArea.x / amount;

            for (int i = 0; i < amount; i++)
            {
                Gizmos.DrawWireSphere(new Vector3(((-layoutArea.x + 0.4f) * 0.5f) + (spacing * i), 0, 0), 0.2f);
            }
        }
    }
}
