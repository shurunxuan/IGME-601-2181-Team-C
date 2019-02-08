using UnityEngine;
using UnityEngine.UI;

public class MissionConfirmMenu : MonoBehaviour
{

    // Text forms to fill in
    public Text MissionTitle;
    public Text MissionDescription;

    private string missionScene;

    public void Confirm()
    {
        MenuManager.ActiveManager.LoadStage(missionScene);
    }

    public void SetUp(MissionInfo mission)
    {
        MissionTitle.text = mission.Title;
        MissionDescription.text = mission.Description;
        missionScene = mission.Scene;
    }

}
