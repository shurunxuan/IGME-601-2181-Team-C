using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is purely for storing data created in the editor
public class MissionInfo : MonoBehaviour {

    public string Scene;

    public string Title;

    [TextArea]
    public string Description;
}
