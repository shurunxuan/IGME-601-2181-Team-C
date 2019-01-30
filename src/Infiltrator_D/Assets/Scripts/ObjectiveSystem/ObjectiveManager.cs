using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour {

    public static void ObjectiveStateChanged() { OnObjectiveStateChange(); }
    public static event SimpleNotify OnObjectiveStateChange;

    private List<Objective> objectiveList;

    public void Awake()
    {
        OnObjectiveStateChange += CheckCompletion;
        objectiveList = new List<Objective>();
    }

    public void OnDestroy()
    {
        OnObjectiveStateChange -= CheckCompletion;
    }

    public void SetUp(MissionInfo info)
    {
        objectiveList.Add(new InfoGatherObjective(1));
    }


    public void CheckCompletion()
    {
        for (int i = 0; i < objectiveList.Count; i++)
        {
            if(!objectiveList[i].State)
            {
                return;
            }
        }

        // On completion
        MenuManager.ActiveManager.LoadMenu((int)MenuManager.MenuState.MissionEndMenu);
    }

}
