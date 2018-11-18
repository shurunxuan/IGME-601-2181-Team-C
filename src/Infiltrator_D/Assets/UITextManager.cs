using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour {

    // Distance between elements
    public float Spread;

    // Seconds for which the text is solid
    public float SolidPeriod;

    // Seconds over which the text fades
    public float FadePeriod;

    // The lists of texts
    private List<Text> text;

    // The index of the bottom text
    private int index;

    // The number of text components in use
    private int inUse;

    // This is the timer used to fade out text
    private float timer;

    private RectTransform pos;

    private bool fading;

    public static UITextManager ActiveInScene;

	// Use this for initialization
	void Awake () {
        ActiveInScene = this;
        text = new List<Text>(GetComponentsInChildren<Text>());
        foreach(Text obj in text)
        {
            obj.enabled = false;
        }

        pos = GetComponent<RectTransform>();
        Transition();   
    }
	
	// Update is called once per frame
	void Update () {
		if(inUse > 0)
        {
            timer -= Time.deltaTime;
            if(timer < FadePeriod)
            {
                if(timer < 0)
                {
                    timer = SolidPeriod + FadePeriod;
                    text[(index + inUse - 1) % text.Count].enabled = false;
                    inUse--;
                    fading = false;
                }
                else if (!fading)
                {
                    fading = true;
                    text[(index + inUse - 1) % text.Count].CrossFadeAlpha(0, FadePeriod, false);
                }
            }
        }
	}

    public void Show(string data, Color col)
    {
        Transition();
        text[index].enabled = true;
        text[index].text = data;
        text[index].color = col;
        text[index].CrossFadeAlpha(1, FadePeriod, false);

        // Reset timer if it is inactive or we're overflowing
        if (inUse == 0 || inUse == text.Count)
        {
            timer = SolidPeriod + FadePeriod;
            fading = false;
        }

        // Increment count unless overflowing
        if (inUse < text.Count)
        {
            inUse++;
        }
    }

    // Rotates the text objects to put the new one at the top.
    private void Transition()
    {
        index = (index + text.Count - 1) % text.Count;
        for (int i = 0; i < text.Count; i++)
        {
            text[(index + i) % text.Count].rectTransform.anchoredPosition = Vector2.down * Spread * i;
        }
    }

}
