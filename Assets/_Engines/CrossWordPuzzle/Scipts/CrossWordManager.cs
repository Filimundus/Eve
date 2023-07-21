using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossWordPuzzle;
using DG.Tweening;
using UnityEngine.Events;

namespace CrossWordPuzzle
{

    public struct LetterPosition
    {
        public string letterID;
        public Vector3 position;
        public bool occupied;
    }

    public class CrossWordManager : MonoBehaviour
    {
        [HideInInspector]
        public static CrossWordManager instance;

        [Header("Settings")]
        public ImageAlphabet customAlphaBetFile;
        public float snapDistance = 6;
        public float snapDuration = 8;
        public Ease letterEaseType;

        [Header("Events")]
        public UnityEvent onLetterCorrect;
        public UnityEvent onLetterIncorrect;
        public UnityEvent onCrosswordCompleted;

        private Letter current;
        private Letter[] letters;
        private LetterPool letterPool;
        private int currentLetterIndex;
        private List<Letter> poolOfLetters;
        private LetterPosition[] letterPositions;

        private void Awake()
        {
            letters = GameObject.FindObjectsOfType<Letter>();
            letterPool = GameObject.FindObjectOfType<LetterPool>();
            poolOfLetters = new List<Letter>();
            RecordStartPositions();
            HideAllLetters();
            ShowLetter();
        }

        void RecordStartPositions()
        {
            letterPositions = new LetterPosition[letters.Length];

            for(int i = 0; i < letters.Length; i++)
            {
                LetterPosition lp = new LetterPosition();
                lp.position = letters[i].transform.position;
                lp.letterID = letters[i].letterID;

                letterPositions[i] = lp;

                poolOfLetters.Add(letters[i]);
            }
        }

        void ShowLetter()
        {
            if(poolOfLetters.Count > 0)
            {
                currentLetterIndex = Random.Range(0, poolOfLetters.Count);
                poolOfLetters[currentLetterIndex].gameObject.SetActive(true);

                poolOfLetters.RemoveAt(currentLetterIndex);
                poolOfLetters.TrimExcess();
            }
            else
            {
                Debug.Log("CROSSWORDPUZZLE DONE");
            }
         
        }

        void HideAllLetters()
        {
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i].gameObject.SetActive(false);
                letters[i].transform.position = letterPool.transform.position;
            }
        }

        private void Update()
        {
            MouseInput();
        }

        void MouseInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                GrabLetter();
            }

            if(Input.GetMouseButtonUp(0))
            {
                DropLetter();
            }

            MoveLetter();
        }

        void TouchInput()
        {
            if(Input.touchCount > 0)
            {
                for(int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if(touch.phase == TouchPhase.Began)
                    {
                        GrabLetter();
                    }

                    if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        DropLetter();
                    }
                }
            }
        }

        void GrabLetter()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.collider.gameObject.GetComponent<Letter>())
                {
                    Letter letterHit = hit.collider.gameObject.GetComponent<Letter>();

                    if (letterHit.isLocked == false)
                    {
                        current = hit.collider.gameObject.GetComponent<Letter>();
                        current.Grab();
                    }
                }
            }
        }

        void MoveLetter()
        {
            if (current)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                current.transform.position = new Vector3(mousePos.x, mousePos.y, current.transform.position.z);
            }
        }

        void DropLetter()
        {
            if (current)
            {
                current.Drop();
                GetClosestLetterPosition(current);
                current = null;
            }
        }

        void GetClosestLetterPosition(Letter letter)
        {
            int index = -1;
            float maxDist = Mathf.Infinity;

            for(int i = 0; i < letterPositions.Length; i++)
            {
                if(letterPositions[i].occupied == false)
                {
                    float dist = Vector3.Distance(letter.transform.position, letterPositions[i].position);

                    if (dist < maxDist)
                    {
                        maxDist = dist;

                        if (letterPositions[i].letterID == letter.letterID && dist < snapDistance)
                        {
                            index = i;
                        }
                    }
                }
            }

            if(index != -1)
            {
                letter.transform.DOMove(letterPositions[index].position,snapDuration).SetEase(letterEaseType);
                letterPositions[index].occupied = true;
                letter.Lock();

                Debug.Log("LETTERPOSITION FOUND");
                ShowLetter();
            }
            else
            {
                Debug.Log("LETTERPOSITION NOT FOUND");
                letter.transform.DOMove(letterPool.transform.position, 1);
            }
        }
    }


}

