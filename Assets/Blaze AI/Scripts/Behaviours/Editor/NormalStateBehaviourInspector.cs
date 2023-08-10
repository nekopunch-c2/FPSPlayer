using UnityEditor;
using BlazeAISpace;

namespace BlazeAISpace 
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NormalStateBehaviour))]
    public class NormalStateBehaviourInspector : Editor
    {
        SerializedProperty moveSpeed,
        turnSpeed,
        idleAnim,
        moveAnim,
        animT,
        idleTime,
        playAudios,
        audioTime,
        avoidFacingObstacles,
        obstacleLayers,
        obstacleRayDistance,
        obstacleRayOffset,
        showObstacleRay,
        onStateEnter,
        onStateExit;


        void OnEnable()
        {
            moveSpeed = serializedObject.FindProperty("moveSpeed");
            turnSpeed = serializedObject.FindProperty("turnSpeed");

            idleAnim = serializedObject.FindProperty("idleAnim");
            moveAnim = serializedObject.FindProperty("moveAnim");
            animT = serializedObject.FindProperty("animT");

            idleTime = serializedObject.FindProperty("idleTime");

            playAudios = serializedObject.FindProperty("playAudios");
            audioTime = serializedObject.FindProperty("audioTime");

            avoidFacingObstacles = serializedObject.FindProperty("avoidFacingObstacles");
            obstacleLayers = serializedObject.FindProperty("obstacleLayers");
            obstacleRayDistance = serializedObject.FindProperty("obstacleRayDistance");
            obstacleRayOffset = serializedObject.FindProperty("obstacleRayOffset");
            showObstacleRay = serializedObject.FindProperty("showObstacleRay");

            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");
        }

        public override void OnInspectorGUI () 
        {
            NormalStateBehaviour script = (NormalStateBehaviour) target;
            int spaceBetween = 20;

            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);
            

            EditorGUILayout.LabelField("SPEEDS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(moveSpeed);
            EditorGUILayout.PropertyField(turnSpeed);


            EditorGUILayout.Space(spaceBetween);
            EditorGUILayout.LabelField("ANIMATIONS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(idleAnim);
            EditorGUILayout.PropertyField(moveAnim);
            EditorGUILayout.PropertyField(animT);


            EditorGUILayout.Space(spaceBetween);
            EditorGUILayout.LabelField("IDLE", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(idleTime);
           

            EditorGUILayout.Space(spaceBetween);
            EditorGUILayout.LabelField("AUDIOS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudios);
            if (script.playAudios) {
                EditorGUILayout.PropertyField(audioTime);
            }


            EditorGUILayout.Space(spaceBetween);
            EditorGUILayout.LabelField("OBSTACLES", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(avoidFacingObstacles);

            if (script.avoidFacingObstacles) {
                EditorGUILayout.PropertyField(obstacleLayers);
                EditorGUILayout.PropertyField(obstacleRayDistance);
                EditorGUILayout.PropertyField(obstacleRayOffset);
                EditorGUILayout.PropertyField(showObstacleRay);
            }
            EditorGUILayout.Space(spaceBetween);
            
            EditorGUILayout.LabelField("STATE EVENTS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);


            serializedObject.ApplyModifiedProperties();
        }
    }
}
