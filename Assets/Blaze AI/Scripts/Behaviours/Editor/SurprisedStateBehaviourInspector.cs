using UnityEditor;

namespace BlazeAISpace 
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SurprisedStateBehaviour))]
    public class SurprisedStateBehaviourInspector : Editor
    {
        SerializedProperty anim,
        animT,
        duration,
        turnSpeed,
        playAudio,
        onStateEnter,
        onStateExit;


        void OnEnable()
        {
            anim = serializedObject.FindProperty("anim");
            animT = serializedObject.FindProperty("animT");
            duration = serializedObject.FindProperty("duration");
            turnSpeed = serializedObject.FindProperty("turnSpeed");
            playAudio = serializedObject.FindProperty("playAudio");
            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");
        }


        public override void OnInspectorGUI () 
        {
            SurprisedStateBehaviour script = (SurprisedStateBehaviour) target;
            int spaceBetween = 10;

            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("ANIMATION", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(anim);
            EditorGUILayout.PropertyField(animT);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("DURATION", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(duration);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("TURN SPEED", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(turnSpeed);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("PLAY AUDIO", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudio);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("STATE EVENTS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);


            serializedObject.ApplyModifiedProperties();
        }
    }
}
