using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossWordPuzzle;

namespace CrossWordPuzzle
{

    public class LetterPool : MonoBehaviour
    {
        [Header("Settings")]

        public bool useAllLetters;
        public int amount;
        public float spacing;
        public GameObject layoutSpotPrefab;
        public List<Vector2> points;

        [HideInInspector]
        public List<LayoutSpot> layoutSpots;
        [HideInInspector]
        public bool poolDone;

        public int limit = 200;
        [HideInInspector]
        public int numberOfTries;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            GenerateLayoutSpots();
        }

        void GenerateLayoutSpots()
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

        public void GeneratePoolPositions()
        {

            if (useAllLetters)
                amount = GameObject.FindObjectsOfType<Letter>().Length;

            for (int i = 0; i < amount; i++)
            {
                Vector2 newPoint = GetRandomPoint();
                while (PointTooClose(newPoint))
                {
                    newPoint = GetRandomPoint();
                    numberOfTries++;


                    if (numberOfTries > limit)
                    {
                        spacing -= 0.1f;

                        if (spacing < 0)
                        {
                            spacing = 0;
                        }

                        numberOfTries = 0;
                    }
                }
                numberOfTries = 0;
                points.Add(newPoint);
            }

            poolDone = true;
        }

        public void ClearPoolPositions()
        {
            points.Clear();
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

        private void OnDrawGizmos()
        {
            if(points != null && points.Count > 0)
            {
                Gizmos.color = Color.blue;

                for(int i = 0; i < points.Count; i++)
                {
                    Gizmos.DrawSphere(points[i], 0.25f);
                }
            }
        }
    }

    
}
