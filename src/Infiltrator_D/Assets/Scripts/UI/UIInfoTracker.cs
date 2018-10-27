using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script can be used to link a text element from the ui to an InfoGatherer
// The current version of this script is for test purposes
public class UIInfoTracker : MonoBehaviour {

    public InfoGatherer info;
    private Text _text;

	// Use this for initialization
	void Start () {
        _text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(info != null)
        {
            _text.text = (int)(info.GetInfoPercentage() * 100) + "%";
        }
	}
}
