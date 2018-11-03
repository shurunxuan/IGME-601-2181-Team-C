using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackableDetector : EnvironmentDetector {

    // Denotes whether or not this HackableDetector can be hacked again to reverse the effect.
    public bool Reversible;

    // Used to track state
    private bool on = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Calls the base Trigger function based on its current state.
    public void Hack()
    {
        on = Reversible ? !on : true;
        Trigger(on);
    }
}
