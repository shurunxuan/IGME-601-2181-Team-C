#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// The script will only have effects on GameObjects with EnemyMovement component
[CustomEditor(typeof(EnemyMovement), true)]
public class DrawPatrolRoute : Editor
{
    readonly GUIStyle style = new GUIStyle();
    void OnEnable()
    {
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
    }
    public void OnSceneGUI()
    {
        // Iterator
        var property = serializedObject.GetIterator();
        // Iterate the properties of the EnemyMovement component of the currently selected GameObject
        while (property.Next(true))
        {
            // Find the property named with PatrolPoints
            if (property.name == "PatrolPoints")
            {
                for (int i = 0; i < property.arraySize; ++i)
                {
                    // Reset style
                    Handles.color = Color.white;
                    Handles.zTest = CompareFunction.Always;

                    // Add a label to the scene window
                    Handles.Label(property.GetArrayElementAtIndex(i).vector3Value, "Waypoint " + (i + 1));

                    // Display a position handle and get its position
                    Vector3 handleValue = Handles.PositionHandle(property.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity);

                    // Cast a ray to make the position stick to the floor
                    Ray castDownRay = new Ray(handleValue, Vector3.down);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(castDownRay, out hitInfo, float.PositiveInfinity))
                    {
                        handleValue.y = hitInfo.point.y + 0.1f;
                        property.GetArrayElementAtIndex(i).vector3Value = handleValue;
                    }

                    // Apply the modification; this also enables the undo (Ctrl + Z) functionality
                    serializedObject.ApplyModifiedProperties();

                    // Set the color to red and always pass depth and stencil test
                    Handles.color = Color.red;
                    Handles.zTest = CompareFunction.Always;

                    // Draw a dotted line between two points
                    Handles.DrawDottedLine(property.GetArrayElementAtIndex(i).vector3Value, property.GetArrayElementAtIndex((i + 1) % property.arraySize).vector3Value, 5);

                    // Enable depth test
                    Handles.zTest = CompareFunction.LessEqual;

                    // Highlight the first point
                    if (i == 0)
                        Handles.color = Color.blue;

                    // Draw a cube which shows the position
                    Handles.CubeHandleCap(i, property.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity, 0.1f, EventType.Repaint);
                    //Handles.DrawWireCube(property.GetArrayElementAtIndex(i).vector3Value, 0.1f * Vector3.one);
                }
            }
        }
    }
}
#endif