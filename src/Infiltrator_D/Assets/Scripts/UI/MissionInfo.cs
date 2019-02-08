using UnityEngine;

// This class is purely for storing data created in the editor
[CreateAssetMenu(fileName = "MissionInfo", menuName = "MissionInfo", order = 1)]
public class MissionInfo : ScriptableObject
{
    public string Scene;

    public string Title;

    [TextArea]
    public string Description;
}
