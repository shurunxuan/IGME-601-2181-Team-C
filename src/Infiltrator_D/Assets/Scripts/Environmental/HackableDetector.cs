using UnityEngine;

public class HackableDetector : EnvironmentDetector
{

    public string HackMessage = "Hack Successful.";

    public bool ShowMessage = true;

    public Color MessageColor = Color.green;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Calls the base Trigger function based on its current state.
    public void Hack()
    {
        if (ShowMessage)
        {
            UITextManager.ActiveInScene.Show(HackMessage, MessageColor);
        }
        Trigger();
    }
}
