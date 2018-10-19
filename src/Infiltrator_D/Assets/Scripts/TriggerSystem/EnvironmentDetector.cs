using System.Collections;
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
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i] != null)
                Events[i].Activate();
        }
    }

    public void Trigger(bool state)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i] != null)
                Events[i].Activate(state);
        }
    }

    public void Trigger(int state)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i] != null)
                Events[i].Activate(state);
        }
    }
}