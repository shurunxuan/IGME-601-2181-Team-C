using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionConfirmMenu : MonoBehaviour {

    // Text forms to fill in
    public Text MissionTitle;
    public Text MissionDescription;

    private string missionScene;
    private MissionInfo info;

    public void Confirm()
    {
        MenuManager.ActiveManager.LoadStage(missionScene);
        transform.root.GetComponentInChildren<ObjectiveManager>().SetUp(info);
    }

    public void SetUp(MissionInfo mission)
    {
        MissionTitle.text = mission.Title;
        MissionDescription.text = mission.Description;
        missionScene = mission.Scene;
        info = mission;
    }

}
