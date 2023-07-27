using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DiceGame
{
    public class MoveSpotEditor : EditorWindow
    {
        public static List<MoveSpot> createdMoveSpots;
        public static bool isCreating;
        public bool create;
        public static GameObject moveSpotPrefab;
       
        // Add menu named "My Window" to the Window menu
        [MenuItem("DiceGame/MoveSpotEditor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            MoveSpotEditor window = (MoveSpotEditor)EditorWindow.GetWindow(typeof(MoveSpotEditor));
            window.Show();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if(isCreating)
            {
                // Event.current houses information on scene view input this cycle
                Event current = Event.current;


                if (current.type == EventType.MouseDown && current.button == 0)
                {
                    Event e = Event.current;


                    if (e.type == EventType.MouseDown && e.modifiers == EventModifiers.Shift)
                    {
                        CreateNewMoveSpot();
                    }
                    else if (e.type == EventType.MouseDown)
                    {
                        SelectMoveSpot();
                    }

                }


                if (current.type == EventType.Layout)
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
            }
           
        }

        void SelectMoveSpot()
        {
            Vector3 worldPos = Vector3.zero;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Selection.activeGameObject = hit.collider.gameObject;
            }
        }

            void CreateNewMoveSpot()
        {
            Vector3 worldPos = Vector3.zero;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {


                if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<MoveSpot>())
                {
                    MoveSpot selectedObject = Selection.activeGameObject.GetComponent<MoveSpot>();

                    if (hit.collider.GetComponent<MoveSpot>())
                    {
                        selectedObject.alternativePath = hit.collider.GetComponent<MoveSpot>();
                    }
                }
                else
                {
                    GameObject moveSpotClone = GameObject.Instantiate(Resources.Load<GameObject>("MoveSpotPrefab"), hit.point, Quaternion.identity);
                    moveSpotClone.name = "MoveSpot_" + GameObject.FindObjectsOfType<MoveSpot>().Length;
                    createdMoveSpots.Add(moveSpotClone.GetComponent<MoveSpot>());

                    if (createdMoveSpots.Count > 1)
                    {
                        createdMoveSpots[createdMoveSpots.Count - 2].nextMoveSpot = moveSpotClone.GetComponent<MoveSpot>();
                        EditorUtility.SetDirty(createdMoveSpots[createdMoveSpots.Count - 1].gameObject);
                    }
                }

            }
        }

        void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnGUI()
        {
            GUILayout.Label("Move spots", EditorStyles.boldLabel);

            if(GUILayout.Button("Toggle Editing"))
            {
                if(isCreating)
                {
                    isCreating = false;
                }
                else
                {
                    isCreating = true;
                    createdMoveSpots = new List<MoveSpot>();
                }
            }

            

            if (isCreating)
            {
                GUILayout.Label("IS EDITING");

               
            }
            else
            {
                GUILayout.Label("NOT EDITING");
            }
        }
    }
}

