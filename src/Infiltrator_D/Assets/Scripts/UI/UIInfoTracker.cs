using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
