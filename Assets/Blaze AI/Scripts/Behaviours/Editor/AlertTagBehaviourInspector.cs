using UnityEditor;

namespace BlazeAISpace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AlertTagBehaviour))]
    public class AlertTagBehaviourInspector : Editor
    {
        SerializedProperty checkLocation,
        onSightAnim,
        onSightDuration,
        reachedLocationAnim,
        reachedLocationDuration,
        animT,
        playAudio,
        audioIndex,
        callOtherAgents,
        callRange,
        showCallRange,
        otherAgentsLayers,
        callPassesColliders,
        randomizeCallPosition,
        onStateEnter,
        onStateExit;


        void OnEnable()
        {
            checkLocation = serializedObject.FindProperty("checkLocation");
            onSightAnim = serializedObject.FindProperty("onSightAnim");
            onSightDuration = serializedObject.FindProperty("onSightDuration");
            reachedLocationAnim = serializedObject.FindProperty("reachedLocationAnim");
            reachedLocationDuration = serializedObject.FindProperty("reachedLocationDuration");
            animT = serializedObject.FindProperty("animT");
            playAudio = serializedObject.FindProperty("playAudio");
            audioIndex = serializedObject.FindProperty("audioIndex");
            callOtherAgents = serializedObject.FindProperty("callOtherAgents");
            callRange = serializedObject.FindProperty("callRange");
            showCallRange = serializedObject.FindProperty("showCallRange");
            otherAgentsLayers = serializedObject.FindProperty("otherAgentsLayers");
            callPassesColliders = serializedObject.FindProperty("callPassesColliders");
            randomizeCallPosition = serializedObject.FindProperty("randomizeCallPosition");
            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");
        }

        public override void OnInspectorGUI()
        {
            AlertTagBehaviour script = (AlertTagBehaviour) target;
            int spaceBetween = 20;
            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("CHECK LOCATION", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(checkLocation);

            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("ANIMATIONS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onSightAnim);
            EditorGUILayout.PropertyField(onSightDuration);
            EditorGUILayout.Space(5);
            if (script.checkLocation) {
                EditorGUILayout.PropertyField(reachedLocationAnim);
                EditorGUILayout.PropertyField(reachedLocationDuration);
            }
            EditorGUILayout.PropertyField(animT);

            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("AUDIO", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudio);
            if (script.playAudio) {
                EditorGUILayout.PropertyField(audioIndex);
            }

            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("CALL OTHERS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(callOtherAgents);
            if (script.callOtherAgents) {
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(callRange);
                EditorGUILayout.PropertyField(showCallRange);
                EditorGUILayout.Space();
                
                EditorGUILayout.PropertyField(otherAgentsLayers);
                EditorGUILayout.PropertyField(callPassesColliders);
                EditorGUILayout.Space();
                
                EditorGUILayout.PropertyField(randomizeCallPosition);
            }
            EditorGUILayout.Space(spaceBetween);
            

            EditorGUILayout.LabelField("STATE EVENTS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);
            

            serializedObject.ApplyModifiedProperties();
        }
    }
}
