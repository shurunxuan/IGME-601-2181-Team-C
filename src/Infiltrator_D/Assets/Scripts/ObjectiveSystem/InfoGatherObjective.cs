using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoGatherObjective : Objective {
    private float threshold;

    public bool State { get; private set; }

    public InfoGatherObjective(float threshold)
    {
        this.threshold = threshold;
        InfoGatherer.InfoNotifier += InfoGatherListener;
    }

    ~InfoGatherObjective()
    {
        InfoGatherer.InfoNotifier -= InfoGatherListener;
    }

    private void InfoGatherListener(InfoGatherer a, string info, bool isNew)
    {
        State = a.GetInfoPercentage() >= threshold;
        ObjectiveManager.ObjectiveStateChanged();
    }
}
