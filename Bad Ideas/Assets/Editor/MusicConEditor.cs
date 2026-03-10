using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicController))]
public class MusicConEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MusicController controller = (MusicController)target;

        // Check if exactly one MusicController is selected
        if (Selection.objects.Length == 1)
        {
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Stop Music"))
                {
                    controller.StopSpeakers();
                }

                if (GUILayout.Button("Start Music"))
                {
                    controller.StartSpeakers();
                }
            }
        }
        else
        {
            // Optionally, display a message if multiple objects are selected
            EditorGUILayout.HelpBox("Multi-object editing not supported. Please select a single object.", MessageType.Warning);
        }

        DrawDefaultInspector();
    }
}

