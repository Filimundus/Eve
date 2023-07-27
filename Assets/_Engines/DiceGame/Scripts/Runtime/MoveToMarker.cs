using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class MoveToMarker : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            StopFlash();
        }

        public void Flash()
        {
            _animator.SetBool("isFlashing", true);
        }

        public void StopFlash()
        {
            _animator.SetBool("isFlashing", false);
        }
    }
}
