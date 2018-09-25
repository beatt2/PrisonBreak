using System.Collections;
using System.Diagnostics.CodeAnalysis;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Door
{
    public class Doors : MonoBehaviour, IDoorBehaviour
    {

        public int DoorID;
        public int DoorPrio;
        public int OpenAmount;
        public float CloseAfterSeconds;
        public int AmountOfUsers;

        public bool IsDoorOpen { get; private set; }



        public enum OpenRuleEnum
        {
            RuleAndID, Rule, ID, API
        }
        public OpenRuleEnum OpenRule;


        public enum AccessRuleEnum
        {
            Guard, Cow, Player, All
        }
        public AccessRuleEnum AccessRule;
        public AccessRuleEnum AccessRuleClose;


        public enum CloseRuleEnum
        {
            StayOpen, CloseAfter
        }
        public CloseRuleEnum CloseRule;


        public enum CloseAfterRuleEnum
        {
            Enter, Seconds
        }
        public CloseAfterRuleEnum CloseAfterRule;

        private DoorLight _doorLight;
        private LerpCoroutine _cylinder;

        private void Awake()
        {
            _doorLight = GetComponentInChildren<DoorLight>();
            _cylinder = GetComponentInChildren<LerpCoroutine>();
        }

        private void Start()
        {
            DoorManager.Instance.AddDoor(this);
        }

        public void Open()
        {
            _doorLight.Open();
            _cylinder.Open();
            IsDoorOpen = true;
        }

        public void Close()
        {
            if(CloseRule != CloseRuleEnum.StayOpen)
            StartCoroutine(IEClose());
        }

        public int ID()
        {
            return DoorID;
        }

        private IEnumerator IEClose()
        {
            yield return new WaitForSeconds(CloseAfterSeconds);
            _doorLight.Close();
            _cylinder.Close();
            IsDoorOpen = false;

        }



    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Doors))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public class DoorsEditor : Editor
    {
        private SerializedProperty DoorID;
        private SerializedProperty OpenRule;
        private SerializedProperty AccessRule;
        private SerializedProperty CloseRule;
        private SerializedProperty CloseAfterRule;
        private SerializedProperty CloseAfterSeconds;
        private SerializedProperty AccessRuleClose;
        private SerializedProperty AmountOfUsers;

        private void OnEnable()
        {
            DoorID = serializedObject.FindProperty("DoorID");
            OpenRule = serializedObject.FindProperty("OpenRule");
            AccessRule = serializedObject.FindProperty("AccessRule");
            CloseRule = serializedObject.FindProperty("CloseRule");
            CloseAfterRule = serializedObject.FindProperty("CloseAfterRule");
            CloseAfterSeconds = serializedObject.FindProperty("CloseAfterSeconds");
            AccessRuleClose = serializedObject.FindProperty("AccessRuleClose");
            AmountOfUsers = serializedObject.FindProperty("AmountOfUsers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("OPEN");
            EditorGUILayout.PropertyField(DoorID);
            EditorGUILayout.PropertyField(OpenRule);
            switch (OpenRule.enumValueIndex)
            {
                case 0:
                    EditorGUILayout.PropertyField(AccessRule);
                    break;
                case 1:
                    EditorGUILayout.PropertyField(AccessRule);
                    break;
                case 2:
                    //nothing
                    break;
                case 3:
                    EditorGUILayout.PropertyField(AmountOfUsers);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("CLOSE");
            EditorGUILayout.PropertyField(CloseRule);
            switch (CloseRule.enumValueIndex)
            {
                case 0:
                    //nothing yet
                    break;
                case 1:
                    EditorGUILayout.PropertyField(CloseAfterRule);
                    switch (CloseAfterRule.enumValueIndex)
                    {
                        case 0:
                            EditorGUILayout.PropertyField(AccessRuleClose);
                            break;
                        case 1:

                            EditorGUILayout.PropertyField(CloseAfterSeconds);
                            break;
                            
                            
                    }

                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
