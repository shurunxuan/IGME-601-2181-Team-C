using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReticuleController : MonoBehaviour {

    // The list of Reticles on the UI
    public List<Image> Reticles;

    // Active index
    private int index;

    // The most recently added UIReticuleController
    public static UIReticuleController ActiveInScene;

    // Use this for initialization
    void Start () {
        ActiveInScene = this;
        index = 0;
        for (int i = 0; i < Reticles.Count; i++)
        {
            Reticles[i].enabled = index == i;
        }
        enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show(int id)
    {
        if(index != -1)
        {
            Reticles[index].enabled = false;
        }
        if (id > Reticles.Count)
        {
            index = -1;
        }
        else if (id >= 0)
        {
            index = id;
            Reticles[index].enabled = true;
        }
    }

}
