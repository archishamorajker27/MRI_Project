using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class UniversalInspector : Editor
{
    public override void OnInspectorGUI()
    {
        MonoBehaviour mono =
            (MonoBehaviour)target;

        if (mono != null)
        {
            string namespaceName =
                mono.GetType().Namespace;

            if (!string.IsNullOrEmpty(namespaceName) &&
                namespaceName.StartsWith("LGA_SYS"))
            {
                GUILayout.Space(10);

                GUIStyle style =
                    new GUIStyle(EditorStyles.boldLabel);

                style.fontSize = 14;

                style.normal.textColor =
                    Color.cyan;

                GUILayout.Label(
                    "BY: Abhijeet Patil",
                    style);

                GUILayout.Space(5);
            }
        }

        DrawDefaultInspector();
    }
}