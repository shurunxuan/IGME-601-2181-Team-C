using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TransformBehavior : EnvironmentBehavior {

    // Transforma to interpolate between
    public Transform StartLocation;
    public Transform EndLocation;

    // The period of the transform measured in seconds
    public float Period;

    // Keeps track of where it is going
    private bool state;

    // Lerps properly based on Time.deltaTime
    private float lerp;

    // Use this for initialization
    void Start ()
    {
        // Since this script will not act until it is activated, we don't need to update it every frame
        enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Update lerp based on delta time with respect to period
        lerp += (state ? 1 : -1) * Time.deltaTime / Period;
        
        // Prevent overflow
        if (lerp > 1)
        {
            lerp = 1;
            enabled = false;
        }
        if (lerp < 0)
        {
            lerp = 0;
            enabled = false;
        }

        // Lerp/Slerp all tracked properties
        transform.position = Vector3.Lerp(StartLocation.position, EndLocation.position, lerp);
        transform.rotation = Quaternion.Slerp(StartLocation.rotation, EndLocation.rotation, lerp);
    }

    public override void Activate()
    {
        Activate(!state);
    }

    public override void Activate(bool state)
    {
        // Check for quick out
        if(this.state == state)
        {
            return;
        }

        this.state = state;

        // Enable it so it can update.
        enabled = true;
    }

    public override void Activate(int state)
    {
        Activate(state != 0);
    }
}
