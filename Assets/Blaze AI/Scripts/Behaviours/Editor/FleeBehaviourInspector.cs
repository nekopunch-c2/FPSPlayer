using UnityEngine;
using UnityEditor;
using BlazeAISpace;

namespace BlazeAISpace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FleeBehaviour))]
    public class FleeBehaviourInspector : Editor
    {
        SerializedProperty distanceRun,

        moveSpeed,
        turnSpeed,

        moveAnim,
        moveAnimT,

        goToPosition,
        setPosition,
        showPosition,
        reachEvent,

        playAudio,
        alwaysPlayAudio,

        onStateEnter,
        onStateExit;


        FleeBehaviour script;


        void OnEnable()
        {
            distanceRun = serializedObject.FindProperty("distanceRun");

            moveSpeed = serializedObject.FindProperty("moveSpeed");
            turnSpeed = serializedObject.FindProperty("turnSpeed");

            moveAnim = serializedObject.FindProperty("moveAnim");
            moveAnimT = serializedObject.FindProperty("moveAnimT");

            goToPosition = serializedObject.FindProperty("goToPosition");
            setPosition = serializedObject.FindProperty("setPosition");
            showPosition = serializedObject.FindProperty("showPosition");
            reachEvent = serializedObject.FindProperty("reachEvent");

            playAudio = serializedObject.FindProperty("playAudio");
            alwaysPlayAudio = serializedObject.FindProperty("alwaysPlayAudio");

            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");
        }

        protected virtual void OnSceneGUI()
        {
            if (script == null) return;

            if (script.showPosition) {
                DrawFleePosition();
            }
        }

        public override void OnInspectorGUI () 
        {
            script = (FleeBehaviour) target;
            int spaceBetween = 20;
            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("DISTANCE RUN", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(distanceRun);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("SPEEDS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(moveSpeed);
            EditorGUILayout.PropertyField(turnSpeed);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("ANIMATION", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(moveAnim);
            EditorGUILayout.PropertyField(moveAnimT);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("SPECIFIC LOCATION", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(goToPosition);
            if (script.goToPosition) {
                EditorGUILayout.PropertyField(setPosition);
                EditorGUILayout.PropertyField(showPosition);
                EditorGUILayout.PropertyField(reachEvent);
            }
            
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("AUDIO", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudio);
            if (script.playAudio) {
                EditorGUILayout.PropertyField(alwaysPlayAudio);
            }
            EditorGUILayout.Space(spaceBetween);
            
            
            EditorGUILayout.LabelField("STATE EVENTS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);
            

            serializedObject.ApplyModifiedProperties();
        }

        void DrawFleePosition()
        {
            EditorGUI.BeginChangeCheck();

            Vector3 currentPoint = script.setPosition;
            Vector3 newTargetPosition = Handles.PositionHandle(currentPoint, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Change Flee Position");
                script.setPosition = newTargetPosition;
            }
        }
    } 
}
