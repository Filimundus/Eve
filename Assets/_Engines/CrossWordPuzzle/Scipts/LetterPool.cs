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
        public List<Vector2> points;
        public float radius;
        public bool poolDone;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;

           StartCoroutine("SlotPositioning");
        }

        void GenerateLayout()
        {
            layoutSpots = new List<LayoutSpot>();

            for (int i = 0; i < points.Count; i++)
            {
                Vector2 position = points[i];

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
            layoutArea = new Vector2(transform.localScale.x, transform.localScale.y);
            Gizmos.DrawWireCube(transform.position, new Vector3(layoutArea.x, layoutArea.y, 0));
        }

        IEnumerator SlotPositioning()
        {
            points.Clear();
            for (int i = 0; i < amount; i++)
            {
                Vector2 newPoint = GetRandomPoint();
                while (PointTooClose(newPoint))
                {
                    newPoint = GetRandomPoint();
                    yield return null;
                }
                points.Add(newPoint);
            }

            GenerateLayout();

            poolDone = true;
        }

        private Vector2 GetRandomPoint()
        {
            float x = Random.Range(transform.position.x - transform.localScale.x * 0.5f,transform.position.x + transform.localScale.x * 0.5f);
            float y = Random.Range(transform.position.y - transform.localScale.y * 0.5f, transform.position.y + transform.localScale.y * 0.5f);

            Vector2 pos = new Vector2(x,y);

            return new Vector2(x, y);
        }

        private bool PointTooClose(Vector2 point)
        {
            foreach (var existingPoint in points)
            {
                float distance = Vector2.Distance(point, existingPoint);
                if (distance < spacing)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
