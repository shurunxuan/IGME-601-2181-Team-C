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

    private RectTransform pos;

    public static UITextManager ActiveInScene;
    
    // Info message properties
    public Color NewInfoColor;
    public Color RepeatInfoColor;
    public string InfoPrefix;

    // Use this for initialization
    void Awake () {

        InfoGatherer.InfoNotifier += OnInfoGather;

        ActiveInScene = this;
        text = new List<Text>(GetComponentsInChildren<Text>());
        foreach(Text obj in text)
        {
            obj.enabled = false;
            obj.CrossFadeAlpha(0, 0, true);
        }

        pos = GetComponent<RectTransform>();
        Transition();   
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show(string data, Color col)
    {
        Transition();
        text[index].enabled = true;
        text[index].text = data;
        text[index].color = col;
        text[index].CrossFadeAlpha(1, FadePeriod, false);

        // Reset timer if it is inactive or we're overflowing
        if (inUse == 0)
        {
            StartCoroutine(Fade());
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

    // Logic for fading out the icon
    private IEnumerator Fade()
    {
        do
        {
            yield return new WaitForSeconds(SolidPeriod);
            text[(index + inUse - 1) % text.Count].CrossFadeAlpha(0, FadePeriod, false);
            yield return new WaitForSeconds(FadePeriod);
            text[(index + inUse - 1) % text.Count].enabled = false;
            inUse--;
        } while (inUse > 0);
    }

    private void OnInfoGather(InfoGatherer gatherer, string info, bool isNew)
    {
        Show(InfoPrefix + info, isNew ? NewInfoColor : RepeatInfoColor);
    }

}
