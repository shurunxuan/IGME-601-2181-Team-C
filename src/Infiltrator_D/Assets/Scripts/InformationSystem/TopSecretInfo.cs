using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSecretInfo : MonoBehaviour {

    public enum InfoType
    {
        Visual, Digital
    }

    // Static info handler
    private static List<string> allInfo;

    // Personal info
    public string info;
    public InfoType type;

    static TopSecretInfo()
    {
        allInfo = new List<string>();
    }

    private void Awake()
    {
       
    }

    // Use this for initialization
    void Start ()
    {
        if (!allInfo.Contains(info))
        {
            allInfo.Add(info);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static void ClearInfo()
    {
        allInfo.Clear();
    }

    public static int InfoCount()
    {
        return allInfo.Count;
    }
}
