using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script allows us to declare death and rewrite the death message
public class UIDeathTracker : MonoBehaviour {

    // Ways to die
    public enum DeathTypes
    {
        EnergyLoss,
        Shooting
    }

    // The text attributed to the death tracker
    public Text DeathMessage;

    // Messages
    public List<string> OnDeathMessages;



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
    public void Show(DeathTypes dType)
    {
        gameObject.SetActive(true);
        DeathMessage.text = OnDeathMessages[(int) dType];
        UIReticuleController.ActiveInScene.Show(-1);
    }

    // Hides the death indicator
    public void Hide()
    {
        gameObject.SetActive(false);
        UIReticuleController.ActiveInScene.Show(0);
    }

}
