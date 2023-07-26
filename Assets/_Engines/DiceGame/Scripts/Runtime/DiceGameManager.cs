using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace DiceGame
{

    public class DiceGameManager : MonoBehaviour
    {
        [Header("Settings")]
        public MoveSpot[] moveSpots;
        public List<MoveSpot> avaibleSpots;

        public int maxDiceNumber;
        public int minDiceNumber;
        public int currentNumber;

        public UnityEvent onDiceRolled;
        public UnityEvent onPawnMoved;

        public PlayerPawn[] playerPawns;
        public PlayerPawn currentPawnSelected;
        public TMPro.TMP_Text diceDebugText;
        public static DiceGameManager instance;
        private Camera mainCam;

        private void Awake()
        {
            instance = this;
            mainCam = Camera.main;
        }

        void SetupMoveSpots()
        {
            for(int i = 0; i < moveSpots.Length; i++)
            {
                moveSpots[i].moveSpotIndex = i;
            }
        }

        private void Start()
        {
            SetupMoveSpots();
            GetAvaiblePawns();
        }

        void GetAvaiblePawns()
        {
            playerPawns = GameObject.FindObjectsOfType<PlayerPawn>();
        }

        public void RollDice()
        {
            currentNumber = Random.Range(minDiceNumber, maxDiceNumber);
            diceDebugText.text = ""+currentNumber;
            onDiceRolled.Invoke();
        }

        private void Update()
        {
            if(Input.GetKeyDown("r"))
            {
                RollDice();
                MovePawnAlongPath(currentPawnSelected);
            }

            if(Application.isMobilePlatform)
            {

            }
            else
            {
                MouseControl();
            }
       
        }

        void MouseControl()
        {

            if(Input.GetMouseButtonDown(0))
            {
                GrabPawn();
            }

            if(Input.GetMouseButtonUp(0))
            {
                DropPawn();
            }

            if(currentPawnSelected)
            {
                if(currentPawnSelected.isGrabbed)
                {
                    Vector3 point = new Vector3();

                    point = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane));
                    point.z = currentPawnSelected.transform.position.z;
                    currentPawnSelected.transform.position = point;
                }
             
            }
        }

        void MovePawnAlongPath(PlayerPawn pawnToMove)
        {
            if (pawnToMove.isComputer)
            {
                List<MoveSpot> path = new List<MoveSpot>();

                int startIndex = pawnToMove.currentMoveSpot.moveSpotIndex + 1;
                int endIndex = startIndex + currentNumber;

                for (int i = startIndex; i < endIndex; i++)
                {
                    path.Add(moveSpots[i]);
                }

                pawnToMove.MoveToSlot(path.ToArray());
            }
        }

        void GrabPawn()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                if(hit.collider.GetComponent<PlayerPawn>())
                {
                    PlayerPawn playerPawnHit = hit.collider.GetComponent<PlayerPawn>();

                    if(playerPawnHit.isSelectable)
                    {
                        currentPawnSelected = playerPawnHit;
                        currentPawnSelected.Grab();
                    }
                }
            }
        }

        public MoveSpot GetClosestMoveSpotFromPosition(Vector3 position)
        {
            MoveSpot moveSpotFound = null;
            float dist = Mathf.Infinity;

            for(int i = 0; i < moveSpots.Length; i++)
            {
                float currentDistance = Vector3.Distance(position,moveSpots[i].transform.position);

                if(currentDistance < dist)
                {
                    moveSpotFound = moveSpots[i];
                    dist = currentDistance;
                }
            }

            return moveSpotFound;
        }

        void DropPawn()
        {
            if(currentPawnSelected)
            {
                currentPawnSelected.Drop();
            }
        }
        
        private void OnDrawGizmos()
        {
            if(moveSpots !=null && moveSpots.Length > 1)
            {
                for (int i = 0; i < moveSpots.Length; i++)
                {
                    if(i+1< moveSpots.Length)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(moveSpots[i].transform.position, moveSpots[i + 1].transform.position);
                    }
                }
            }

        }
    }

}

