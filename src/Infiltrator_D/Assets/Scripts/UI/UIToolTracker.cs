using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToolTracker : MonoBehaviour {

    // Seconds for which the icon is solid
    public float SolidPeriod;

    // Seconds over which the icon fades
    public float FadePeriod;

    // The list of Tool Icons on the UI
    public List<Image> ToolIcons;

    // Active index
    private int index;

    // The most recently added UIToolTracker
    public static UIToolTracker ActiveInScene;

    // Use this for initialization
    void Awake () {
        ActiveInScene = this;

        for (int i = 0; i < ToolIcons.Count; i++)
        {
            ToolIcons[i].enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    // Logic for displaying an icon
    public void Show(int index)
    {
        if (ToolIcons[this.index].enabled)
        {
            ToolIcons[this.index].enabled = false;
            ToolIcons[index].CrossFadeAlpha(1, 0, false);
        }
        ToolIcons[index].enabled = true;
        StopCoroutine(Fade());
        StartCoroutine(Fade());
    }

    // Logic for fading out the icon
    private IEnumerator Fade()
    {
        ToolIcons[index].CrossFadeAlpha(1, FadePeriod, false);
        yield return new WaitForSeconds(SolidPeriod);
        ToolIcons[index].CrossFadeAlpha(0, FadePeriod, false);
        yield return new WaitForSeconds(FadePeriod);
        ToolIcons[index].enabled = false;
    }

}
