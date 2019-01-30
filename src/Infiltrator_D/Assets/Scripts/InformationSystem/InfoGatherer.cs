using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InfoGatherNotify(InfoGatherer gatherer, string info, bool isNew);

public class InfoGatherer : MonoBehaviour {

    public static event InfoGatherNotify InfoNotifier;

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
            if (InfoNotifier != null)
            {
                InfoNotifier(this, newInfo, true);
            }
            info.Add(newInfo);
            return true;
        }
        if (InfoNotifier != null)
        {
            InfoNotifier(this, newInfo, false);
        }
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
