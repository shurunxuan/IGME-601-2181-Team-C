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
        if(energy == null)
        {
            energy = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponentInChildren<EnergyComponent>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(energy != null)
        {
            slider.value = energy.CurrentEnergy / energy.Capacity;
        }
	}

    // OnRefresh is called when a scene is loaded
    void OnRefresh()
    {
        slider = GetComponent<Slider>();
        if (energy == null)
        {
            energy = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponentInChildren<EnergyComponent>();
        }
    }

    private void OnDestroy()
    {
        MenuManager.Refresh -= OnRefresh;
    }
}
