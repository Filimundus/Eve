using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossWordPuzzle;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CrossWordPuzzle
{
    public class LetterPosition
    {
        public string letterID;
        public Vector3 position;
        public bool occupied;
    }

    public enum CrosswordMode
    {
        DragOnlyCorrect,
        DragAllThenCorrect,
    }

    public enum PoolType
    {
        OneByOne,
        Several,
    }

    public class CrossWordManager : MonoBehaviour
    {
        [HideInInspector]
        public static CrossWordManager instance;

        [Header("Settings")]
        public ImageAlphabet customAlphabetFile;
        public CrosswordMode mode;

        [Header("Letter Setting")]
        public float snapDistance = 6;
        public float snapDuration = 8;
        public Ease letterEaseType;

        [Header("PoolType")]
        public PoolType poolType;

        [Header("Events")]
        public UnityEvent onLetterCorrect;
        public UnityEvent onLetterIncorrect;
        public UnityEvent onCrosswordFailed;
        public UnityEvent onCrosswordCompleted;

        private Letter current;
        private Letter[] letters;
        private LetterPool letterPool;
        private int currentLetterIndex;
        private List<Letter> poolOfLetters;
        private LetterPosition[] letterPositions;
        private List<Letter> avaibleLetters;

        private void Start()
        {
            letters = GameObject.FindObjectsOfType<Letter>();
            letterPool = GameObject.FindObjectOfType<LetterPool>();
            poolOfLetters = new List<Letter>();
            RecordStartPositions();
            HideAllLetters();

            StartCoroutine("WaitForLayout");
        }

        IEnumerator WaitForLayout()
        {
            while(letterPool.poolDone == false)
            {
                yield return null;
            }


            switch (mode)
            {
                case CrosswordMode.DragOnlyCorrect:
                    ShowNextLetter();
                    break;
                case CrosswordMode.DragAllThenCorrect:
                    LayoutLetters();
                    break;
            }
        }

        void LayoutLetters()
        {
            if(poolOfLetters.Count > 0)
            {
                avaibleLetters = new List<Letter>();

                for (int i = 0; i < letterPool.amount; i++)
                {
                    if (poolOfLetters.Count > 0)
                    {
                        LayoutSpot layoutSpot = letterPool.GetAvaiblePosition();

                        currentLetterIndex = Random.Range(0, poolOfLetters.Count);
                        poolOfLetters[currentLetterIndex].gameObject.SetActive(true);
                        poolOfLetters[currentLetterIndex].transform.position = layoutSpot.transform.position;
                        poolOfLetters[currentLetterIndex].currentLayoutSpot = layoutSpot;
                        avaibleLetters.Add(poolOfLetters[currentLetterIndex]);

                        layoutSpot.isOccupied = true;

                        poolOfLetters.RemoveAt(currentLetterIndex);
                        poolOfLetters.TrimExcess();
                    }
                }
            }
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

        void ShowNextLetter()
        {
            if (poolOfLetters.Count > 0)
            {
                currentLetterIndex = Random.Range(0, poolOfLetters.Count);
                poolOfLetters[currentLetterIndex].gameObject.SetActive(true);
                poolOfLetters[currentLetterIndex].gameObject.transform.position = letterPool.transform.position;
                poolOfLetters.RemoveAt(currentLetterIndex);
                poolOfLetters.TrimExcess();
            }
            else
            {
                onCrosswordCompleted.Invoke();
                Debug.Log("CROSSWORDPUZZLE DONE");
            }
        }

        void HideAllLetters()
        {
            for (int i = 0; i < poolOfLetters.Count; i++)
            {
                poolOfLetters[i].gameObject.SetActive(false);
                poolOfLetters[i].transform.position = letterPool.transform.position;
            }
        }
        private void Update()
        {
            if(Application.isMobilePlatform)
            {
                TouchInput();
            }
            else
            {
                MouseInput();
            }
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
                        current.ClearPosition();
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

                        switch(mode)
                        {
                            case CrosswordMode.DragOnlyCorrect:

                                if (letterPositions[i].letterID == letter.letterID && dist < snapDistance)
                                {
                                    index = i;
                                }

                                break;

                            case CrosswordMode.DragAllThenCorrect:

                                if (dist < snapDistance)
                                {
                                    index = i;
                                }

                                break;
                        }
                     
                    }
                }
            }


            switch(mode)
            {
                case CrosswordMode.DragOnlyCorrect:

                    if (index != -1)
                    {
                        letter.transform.DOMove(letterPositions[index].position, snapDuration).SetEase(letterEaseType);
                        letterPositions[index].occupied = true;
                        letter.currentPosition = letterPositions[index];
                        letter.Lock();

                        onLetterCorrect.Invoke();

                        Debug.Log("LETTERPOSITION FOUND");
                        ShowNextLetter();
                    }
                    else
                    {
                        Debug.Log("LETTERPOSITION NOT FOUND");
                        letter.transform.DOMove(letterPool.transform.position, snapDuration);
                        letter.ClearPosition();
                    }

                    break;

                case CrosswordMode.DragAllThenCorrect:

                    if (index != -1)
                    {
                        letter.transform.DOMove(letterPositions[index].position, snapDuration).SetEase(letterEaseType);
                        letter.currentPosition = letterPositions[index];
                        letterPositions[index].occupied = true;
                        letter.currentLayoutSpot.isOccupied = false;
                        

                        avaibleLetters.Remove(letter);
                        avaibleLetters.TrimExcess();

                        if(avaibleLetters.Count <= 0)
                        {
                            LayoutLetters();
                        }

                        Debug.Log("LETTERPOSITION FOUND");
                    }
                    else
                    {
                        Debug.Log("LETTERPOSITION NOT FOUND");

                        letter.transform.DOMove(letter.currentLayoutSpot.transform.position, snapDuration);
                        letter.ClearPosition();
                    }

                    break;
            }

        }

        public bool IsAllLettersPlaced()
        {
            bool allLettersPlaced = true;

            for (int i = 0; i < letterPositions.Length; i++)
            {
                if (letterPositions[i].occupied == false)
                {
                    allLettersPlaced = false;
                    break;
                }
            }

            return allLettersPlaced;
        }

        public void CheckIfCorrect()
        {
            bool allIsCorrect = true;
            
            List<Letter> letterInWrongPositions = new List<Letter>();

            if(IsAllLettersPlaced())
            {
                for(int i = 0; i < letters.Length; i++)
                {
                    if(letters[i].letterID != letters[i].currentPosition.letterID)
                    {
                        letterInWrongPositions.Add(letters[i]);
                        allIsCorrect = false;
                    }
                }
            }

            if(allIsCorrect)
            {
                Debug.Log("Crossword completed");
                onCrosswordCompleted.Invoke();
            }
            else
            {
                for(int i = 0; i < letterInWrongPositions.Count; i++)
                {
                    letterInWrongPositions[i].transform.DOMove(letterPool.transform.position, snapDuration);
                    letterInWrongPositions[i].ClearPosition();
                    poolOfLetters.Add(letterInWrongPositions[i]);
                }

                Invoke("HideAllLetters",snapDuration);
                Invoke("LayoutLetters",snapDuration+0.1f);

                onCrosswordFailed.Invoke();
                Debug.Log("Crossword not completed");
            }
        }
    }


}

