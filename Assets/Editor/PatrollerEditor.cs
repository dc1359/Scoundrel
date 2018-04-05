using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Patroller))]
public class PatrollerEditor : Editor {
    SerializedProperty commandProperty;
    SerializedProperty commandParametersProperty;
    private static Dictionary<int, string> commandOptions = new Dictionary<int, string>();
    private int commandLength;
    private int[] commandArray;
    private int[] commandParameter;
    private bool showCommandProperties = true;

    void OnEnable()
    {
        commandOptions.Clear();
        commandOptions.Add(Patroller.AI_WAIT, "Wait # ms");
        commandOptions.Add(Patroller.AI_TURN_TO_POINT, "Face Angle");
        commandOptions.Add(Patroller.AI_FORWARD, "Advance to Point");

        commandProperty = serializedObject.FindProperty("commands");
        commandParametersProperty = serializedObject.FindProperty("commandParameters");
        commandLength = commandProperty.arraySize;
        commandArray = new int[commandLength];
        commandParameter = new int[commandLength];

        for (int i = 0; i < commandLength; i++)
        {
            commandArray[i] = commandProperty.GetArrayElementAtIndex(i).intValue;
            commandParameter[i] = commandParametersProperty.GetArrayElementAtIndex(i).intValue;
            //Debug.Log(commandProperty.GetArrayElementAtIndex(i).intValue);
        }
    }

    public override void OnInspectorGUI()
    {
        
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        serializedObject.Update();

        GUICommands();

        serializedObject.ApplyModifiedProperties();

    }

    public void GUICommands()
    {
        showCommandProperties = EditorGUILayout.Foldout(showCommandProperties, "Commands");

        if (showCommandProperties) {
            commandLength = EditorGUILayout.IntField("Number of Commands", commandLength);
            
            if (commandLength <= 0) {
                commandArray = new int[1];
                commandParameter = new int[1];
                commandLength = 0;

            } else if (commandLength != commandArray.Length) {
                int[] newCommandArray = new int[commandLength];
                int[] newCommandParameter = new int[commandLength];

                for (int i = 0; i < commandLength; i++) {
                    if (i < commandArray.Length) {
                        newCommandArray[i] = commandArray[i];
                        newCommandParameter[i] = commandParameter[i];
                    } else {
                        newCommandArray[i] = 0;
                        newCommandParameter[i] = 0;
                    }
                }

                commandArray = newCommandArray;
                commandParameter = newCommandParameter;
            }

            commandProperty.arraySize = commandLength;
            commandParametersProperty.arraySize = commandLength;

            for (int i = 0; i < commandLength; i++)
            {
                EditorGUILayout.BeginHorizontal();
                commandArray[i] = EditorGUILayout.Popup("   Command " + (i + 1), commandArray[i], new List<string>(commandOptions.Values).ToArray(), GUILayout.Width(250));

                if (commandArray[i] == Patroller.AI_FORWARD) {
                    int limit = serializedObject.FindProperty("patrolPoints").arraySize;
                    int[] patrolPointRange = new int[limit];
                    string[] patrolPointNames = new string[limit];

                    for (int j = 0; j < limit; j++) {
                        patrolPointRange[j] = j;
                        patrolPointNames[j] = "Patrol Point " + (j + 1);
                    }
                    commandParameter[i] = EditorGUILayout.IntPopup(commandParameter[i], patrolPointNames, patrolPointRange);
                } else {
                    commandParameter[i] = EditorGUILayout.IntField(commandParameter[i]);
                }
                EditorGUILayout.EndHorizontal();

                if (commandArray[i] < 0)
                {
                    commandArray[i] = 0;
                }


                commandProperty.GetArrayElementAtIndex(i).intValue = commandArray[i];
                commandParametersProperty.GetArrayElementAtIndex(i).intValue = commandParameter[i];
            }
        }

        
    }
}
