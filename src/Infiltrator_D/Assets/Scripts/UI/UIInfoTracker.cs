using UnityEngine;
using UnityEngine.UI;

// This script can be used to link a text element from the ui to an InfoGatherer
// The current version of this script is for test purposes
public class UIInfoTracker : MonoBehaviour
{

    public InfoGatherer Info;
    private Slider slider;

    // Use this for initialization
    void Start()
    {
        OnRefresh();
        MenuManager.Refresh += OnRefresh;
    }

    // Update is called once per frame
    void Update()
    {
        if (Info != null)
        {
            slider.value = Info.GetInfoPercentage();
        }
    }

    // OnRefresh is called when a scene is loaded
    void OnRefresh()
    {
        slider = GetComponent<Slider>();
        if (Info == null)
        {
            Info = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponentInChildren<InfoGatherer>();
        }
    }

    private void OnDestroy()
    {
        MenuManager.Refresh -= OnRefresh;
    }
}
