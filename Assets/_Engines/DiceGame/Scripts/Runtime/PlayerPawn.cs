using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DiceGame
{
    public class PlayerPawn : MonoBehaviour
    {
        public bool isGrabbed;
        public bool isSelectable;
        public bool isComputer;
        public MoveSpot currentMoveSpot;
        private bool isMoving;

        private void Start()
        {
            currentMoveSpot = DiceGameManager.instance.GetClosestMoveSpotFromPosition(transform.position);
            SnapToMoveSpot();
        }

        void SnapToMoveSpot()
        { 
            currentMoveSpot = DiceGameManager.instance.GetClosestMoveSpotFromPosition(transform.position);
            transform.position = currentMoveSpot.transform.position;
        }

        void SnapToMoveSpotOnPath()
        {
            currentMoveSpot = DiceGameManager.instance.GetClosestMoveSpotFromCurrentPath(transform.position);
            transform.position = currentMoveSpot.transform.position;
        }

        public void MoveToSlot(MoveSpot[] path)
        {
            if (isMoving)
                return;

            StartCoroutine("MoveAlongPath",path);
        }

        IEnumerator MoveAlongPath(MoveSpot[] path)
        {
            isMoving = true;

            for (int i = 0; i < path.Length; i++)
            {
                transform.DOJump(path[i].transform.position,1,1,0.5f).OnComplete(SnapToMoveSpot);

                yield return new WaitForSeconds(0.8f);

            }

            if(path[path.Length-1].alternativePath != null)
            {
                transform.DOJump(path[path.Length - 1].alternativePath.transform.position, 1, 1, 0.5f).OnComplete(SnapToMoveSpot);

                yield return new WaitForSeconds(0.8f);
            }

            isMoving = false;
        }

        public void Grab()
        {
            isGrabbed = true;
        }

        public void Drop()
        {
            isGrabbed = false;

            if(DiceGameManager.instance.currentPath != null && DiceGameManager.instance.currentPath.Length > 0)
                SnapToMoveSpotOnPath();
        }
    }

}

