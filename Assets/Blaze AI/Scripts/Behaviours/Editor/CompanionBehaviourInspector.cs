using UnityEngine;
using UnityEditor;

namespace BlazeAISpace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CompanionBehaviour))]
    public class CompanionBehaviourInspector : Editor
    {
        SerializedProperty follow,
        followIfDistance,
        walkIfDistance,
        movingWindow,

        idleAnim,

        runAnim,
        runSpeed,

        walkAnim,
        walkSpeed,

        turnSpeed,

        stayPutAnim,
        animsT,

        wanderAround,
        wanderPointTime,
        wanderMoveSpeed,
        wanderIdleAnims,
        wanderMoveAnim,

        moveAwayOnContact,
        layersToAvoid,
        walkOnMoveAway,

        playAudioForIdleAndWander,
        audioTimer,
        playAudioOnFollowState,

        onStateEnter,
        onStateExit,
        fireEventDistance,
        exceededDistanceEvent;


        string[] tabs = {"General", "Wandering & Contact", "Audios", "Events"};
        int tabSelected = 0;
        int tabIndex = -1;
        int spaceBetween = 20;


        void OnEnable()
        {
            GetSelectedTab();


            follow = serializedObject.FindProperty("follow");
            followIfDistance = serializedObject.FindProperty("followIfDistance");
            walkIfDistance = serializedObject.FindProperty("walkIfDistance");
            movingWindow = serializedObject.FindProperty("movingWindow");

            idleAnim = serializedObject.FindProperty("idleAnim");

            runAnim = serializedObject.FindProperty("runAnim");
            runSpeed = serializedObject.FindProperty("runSpeed");

            walkAnim = serializedObject.FindProperty("walkAnim");
            walkSpeed = serializedObject.FindProperty("walkSpeed");

            turnSpeed = serializedObject.FindProperty("turnSpeed");

            stayPutAnim = serializedObject.FindProperty("stayPutAnim");
            animsT = serializedObject.FindProperty("animsT");

            wanderAround = serializedObject.FindProperty("wanderAround");
            wanderPointTime = serializedObject.FindProperty("wanderPointTime");
            wanderMoveSpeed = serializedObject.FindProperty("wanderMoveSpeed");
            wanderIdleAnims = serializedObject.FindProperty("wanderIdleAnims");
            wanderMoveAnim = serializedObject.FindProperty("wanderMoveAnim");

            moveAwayOnContact = serializedObject.FindProperty("moveAwayOnContact");
            layersToAvoid = serializedObject.FindProperty("layersToAvoid");
            walkOnMoveAway = serializedObject.FindProperty("walkOnMoveAway");

            playAudioForIdleAndWander = serializedObject.FindProperty("playAudioForIdleAndWander");
            audioTimer = serializedObject.FindProperty("audioTimer");
            playAudioOnFollowState = serializedObject.FindProperty("playAudioOnFollowState");

            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");
            fireEventDistance = serializedObject.FindProperty("fireEventDistance");
            exceededDistanceEvent = serializedObject.FindProperty("exceededDistanceEvent");
        }

        public override void OnInspectorGUI () 
        {
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.55f, 0.55f, 0.55f, 1f);
            
            DrawToolbar();
            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);
            
            GUI.backgroundColor = oldColor;

            CompanionBehaviour script = (CompanionBehaviour) target;
            tabIndex = -1;


            switch (tabSelected)
            {
                case 0:
                    DrawGeneralTab();
                    break;
                case 1:
                    DrawWanderingTab(script);
                    break;
                case 2:
                    DrawAudiosTab(script);
                    break;
                case 3:
                    DrawEventsTab();
                    break;
            }

            EditorPrefs.SetInt("CompanionTabSelected", tabSelected);
            serializedObject.ApplyModifiedProperties();
        }

        #region DRAWING

        void GetSelectedTab()
        {
            if (EditorPrefs.HasKey("CompanionTabSelected")) {
                tabSelected = EditorPrefs.GetInt("CompanionTabSelected");
            }
            else {
                tabSelected = 0;
            }   
        }

        void DrawToolbar()
        {   
            // unselected btns style
            var unselectedStyle = new GUIStyle(GUI.skin.button);
            unselectedStyle.normal.textColor = Color.red;

            // selected btn style
            var selectedStyle = new GUIStyle();
            selectedStyle.normal.textColor = Color.white;
            selectedStyle.active.textColor = Color.white;
            selectedStyle.margin = new RectOffset(4,4,2,2);
            selectedStyle.alignment = TextAnchor.MiddleCenter;

            selectedStyle.normal.background = MakeTex(1, 1, new Color(1f, 0f, 0.1f, 0.8f));
            

            // draw the toolbar
            GUILayout.BeginHorizontal();
            
            foreach (var item in tabs) {
                tabIndex++;

                if (tabIndex == tabSelected) {
                    // selected button
                    GUILayout.Button(item, selectedStyle, GUILayout.MinWidth(105), GUILayout.Height(40));
                }
                else {
                    // unselected buttons
                    if (GUILayout.Button(item, unselectedStyle, GUILayout.MinWidth(105), GUILayout.Height(40))) {
                        // this will trigger when a button is pressed
                        tabSelected = tabIndex;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        void DrawGeneralTab()
        {
            EditorGUILayout.LabelField("FOLLOW", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(follow);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(followIfDistance);
            EditorGUILayout.PropertyField(walkIfDistance);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(movingWindow);

            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("SPEEDS & ANIMS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(idleAnim);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(runAnim);
            EditorGUILayout.PropertyField(runSpeed);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(walkAnim);
            EditorGUILayout.PropertyField(walkSpeed);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(turnSpeed);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(stayPutAnim);
            EditorGUILayout.PropertyField(animsT);
        }

        void DrawWanderingTab(CompanionBehaviour script)
        {
            EditorGUILayout.LabelField("WANDER AROUND", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(wanderAround);
            if (script.wanderAround) {
                EditorGUILayout.PropertyField(wanderPointTime);
                EditorGUILayout.PropertyField(wanderMoveSpeed);
                EditorGUILayout.PropertyField(wanderIdleAnims);
                EditorGUILayout.PropertyField(wanderMoveAnim);
            }

            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("SKIN & CONTACT", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(moveAwayOnContact);
            if (script.moveAwayOnContact) {
                EditorGUILayout.PropertyField(layersToAvoid);
                EditorGUILayout.PropertyField(walkOnMoveAway);
            }
        }

        void DrawAudiosTab(CompanionBehaviour script)
        {
            EditorGUILayout.LabelField("AUDIOS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudioForIdleAndWander);
            
            if (script.playAudioForIdleAndWander) {
                EditorGUILayout.PropertyField(audioTimer);
            }

            EditorGUILayout.PropertyField(playAudioOnFollowState);
        }

        void DrawEventsTab()
        {
            EditorGUILayout.LabelField("STATE EVENTS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("DISTANCE EVENT", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(fireEventDistance);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(exceededDistanceEvent);
        }

        Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            

            for (int i = 0; i < pix.Length; ++i) {
                pix[i] = col;
            }


            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();


            return result;
        }

        #endregion
    }
}
