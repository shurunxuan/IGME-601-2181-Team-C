using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script allows us to declare death and rewrite the death message
public class UIDeathTracker : MonoBehaviour {

    // The text attributed to the death tracker
    public Text DeathMessage;

    // The most recently awoken DeathTracker
    public static UIDeathTracker ActiveInScene { get; private set; }

    // Use this for initialization
    void Awake() {
        ActiveInScene = this;
        Hide();
    }

    // Update is called once per frame
    void Update() {

    }

    // Shows the death message 
    public void Show(string deathMessage)
    {
        gameObject.SetActive(true);
        DeathMessage.text = deathMessage;
    }

    // Hides the death indicator
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
