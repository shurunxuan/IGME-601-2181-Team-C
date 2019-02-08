using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A template for the trigger system.
/// </summary>
public class EnvironmentDetector : MonoBehaviour
{

    public List<EnvironmentBehavior> Events;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger()
    {
        foreach (var e in Events)
        {
            if (e != null)
                e.Activate();
        }
    }

    public void Trigger(bool state)
    {
        foreach (var e in Events)
        {
            if (e != null)
                e.Activate(state);
        }
    }

    public void Trigger(int state)
    {
        foreach (var e in Events)
        {
            if (e != null)
                e.Activate(state);
        }
    }
}
