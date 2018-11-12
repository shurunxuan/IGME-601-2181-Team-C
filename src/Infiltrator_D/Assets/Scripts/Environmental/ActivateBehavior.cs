using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class ActivateBehavior : EnvironmentBehavior {

    // Object to Activate
    public GameObject Track;

    // Use this for initialization
    void Start ()
    {
        if(Track == null)
        {
            Track = gameObject;
        }
        Activate(false);
        // Since this script will not act until it is activated, we don't need to update it every frame
        enabled = false;
	}
	
    public override void Activate()
    {
        Activate(true);
    }

    public override void Activate(bool state)
    {
        Track.SetActive(state);
    }

    public override void Activate(int state)
    {
        Activate(state != 0);
    }
}
