using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackableDetector : EnvironmentDetector {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Calls the base Trigger function based on its current state.
    public void Hack()
    {
        Trigger();
    }
}
