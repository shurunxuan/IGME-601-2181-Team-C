using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script can be used to link a text element from the ui to an InfoGatherer
// The current version of this script is for test purposes
public class UIInfoTracker : MonoBehaviour {

    public InfoGatherer info;
    private Slider slider;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        if (info == null)
        {
            info = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponentInChildren<InfoGatherer>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(info != null)
        {
            slider.value = info.GetInfoPercentage();
        }
	}
}
