using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CrossWordPuzzle
{
    public class CompleteButton : MonoBehaviour
    {
        [Header("Settings")]
        public bool interactable;

        [Header("Button Sprite States")]
        public Sprite pressedImage;
        public Sprite defaultImage;
        public Sprite nonInteractableImage;
        private SpriteRenderer spriteRenderer;

        [Header("Events")]
        public UnityEvent OnPressed;
        private bool isPressed;
        private Collider collider;

        private void Awake()
        {
            collider = gameObject.GetComponent<Collider>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if(interactable == true)
            {
                if (collider.enabled == false)
                {
                    SetSpriteState(0);
                    collider.enabled = true;
                }

                if (Application.isMobilePlatform)
                {
                    TouchInput();
                }
                else
                {
                    MouseInput();
                }
            }
            else
            {
                if(collider.enabled)
                {
                    SetSpriteState(2);
                    collider.enabled = false;
                }
            }

        }

        void MouseInput()
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (PressedThisObject(Input.mousePosition))
                {
                    isPressed = true;
                    SetSpriteState(1);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (PressedThisObject(Input.mousePosition))
                {
                    OnPressed.Invoke();
                    SetSpriteState(0);
                    isPressed = false;
                }
                else if (isPressed)
                {
                    SetSpriteState(0);
                    isPressed = false;
                }
            }
        }

        void TouchInput()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if(touch.phase == TouchPhase.Began)
                    { 
                        if(PressedThisObject(touch.position))
                        {
                            isPressed = true;
                            SetSpriteState(1);
                        }
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if(PressedThisObject(touch.position))
                        {
                            OnPressed.Invoke();
                            SetSpriteState(0);
                            isPressed = false;
                        }
                        else if(isPressed)
                        {
                            SetSpriteState(0);
                        }
                    }
                }
            }
        }

        bool PressedThisObject(Vector2 inputPos)
        {
            bool pressedOnThis = false;

            Ray ray = Camera.main.ScreenPointToRay(inputPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    SetSpriteState(1);
                    pressedOnThis = true;
                }
            }

            return pressedOnThis;
        }
        void SetSpriteState(int state)
        {
            switch(state)
            {
                case 0:

                    spriteRenderer.sprite = defaultImage;
             
                    break;

                case 1:

                    spriteRenderer.sprite = pressedImage;

                    break;

                case 2:

                    spriteRenderer.sprite = nonInteractableImage;

                    break;
            }
        }
    }
}

