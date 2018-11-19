using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoGatherer : MonoBehaviour {

    // Colors for info messages
    public Color NewInfoColor;
    public Color RepeatInfoColor;

    public string Prefix;

    private List<string> info;

	// Use this for initialization
	void Start () {
        info = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Adds info only if it is new
    public bool AddInfo(string newInfo)
    {
        if (!info.Contains(newInfo))
        {
            UITextManager.ActiveInScene.Show(Prefix + newInfo, NewInfoColor);
            info.Add(newInfo);
            return true;
        }
        UITextManager.ActiveInScene.Show(Prefix + newInfo, RepeatInfoColor);
        return false;
    }

    public int GetInfoCount()
    {
        return info.Count;
    }

    // Percentage of info gathered
    public float GetInfoPercentage()
    {
        if(TopSecretInfo.InfoCount() <= 0)
        {
            return 1;
        }
        return ((float)info.Count) / TopSecretInfo.InfoCount();
    }

    // Clears the info record
    public void Clear()
    {
        info.Clear();
    }
}
