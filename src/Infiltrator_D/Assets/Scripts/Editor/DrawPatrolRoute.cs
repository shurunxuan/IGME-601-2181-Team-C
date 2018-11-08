#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

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
        var property = serializedObject.GetIterator();
        while (property.Next(true))
        {
            if (property.name == "PatrolPoints")
            {
                for (int i = 0; i < property.arraySize; ++i)
                {
                    Handles.color = Color.white;
                    Handles.zTest = CompareFunction.Always;
                    Handles.Label(property.GetArrayElementAtIndex(i).vector3Value, "Waypoint " + (i + 1));
                    Vector3 handleValue = Handles.PositionHandle(property.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity);
                    Ray castDownRay = new Ray(handleValue, Vector3.down);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(castDownRay, out hitInfo, float.PositiveInfinity))
                    {
                        handleValue.y = hitInfo.point.y + 0.1f;
                        property.GetArrayElementAtIndex(i).vector3Value = handleValue;
                    }

                    serializedObject.ApplyModifiedProperties();
                    Handles.color = Color.red;
                    Handles.zTest = CompareFunction.Always;
                    Handles.DrawDottedLine(property.GetArrayElementAtIndex(i).vector3Value, property.GetArrayElementAtIndex((i + 1) % property.arraySize).vector3Value, 5);
                    Handles.zTest = CompareFunction.LessEqual;
                    if (i == 0)
                        Handles.color = Color.blue;
                    Handles.CubeHandleCap(i, property.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity, 0.1f, EventType.Repaint);
                    //Handles.DrawWireCube(property.GetArrayElementAtIndex(i).vector3Value, 0.1f * Vector3.one);
                }
            }
        }
    }
}
#endif