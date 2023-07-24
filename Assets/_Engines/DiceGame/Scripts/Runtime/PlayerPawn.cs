using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DiceGame
{
    public class PlayerPawn : MonoBehaviour
    {
        public bool isGrabbed;
        public bool isSelectable;
        public MoveSpot currentMoveSpot;


        private void Start()
        {
            currentMoveSpot = DiceGameManager.instance.GetClosestMoveSpotFromPosition(transform.position);
            SnapToMoveSpot();


        }

        void SnapToMoveSpot()
        {
            if(currentMoveSpot)
            {
                transform.position = currentMoveSpot.transform.position;
            }
        }

        public void Grab()
        {
            isGrabbed = true;
        }

        public void Drop()
        {
            isGrabbed = false;

            currentMoveSpot = DiceGameManager.instance.GetClosestMoveSpotFromPosition(transform.position);
            SnapToMoveSpot();
        }
    }

}

