using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script can be used to link a text element from the ui to an InfoGatherer
// The current version of this script is for test purposes
public class UIEnergyTracker : MonoBehaviour {

    public EnergyComponent energy;
    private Slider slider;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(energy != null)
        {
            slider.value = energy.CurrentEnergy / energy.Capacity;
        }
	}
}
