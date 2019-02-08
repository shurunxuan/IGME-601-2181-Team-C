using UnityEngine;
using UnityEngine.UI;

// This script can be used to link a text element from the ui to an InfoGatherer
// The current version of this script is for test purposes
public class UIEnergyTracker : MonoBehaviour
{

    public EnergyComponent Energy;
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
        if (Energy != null)
        {
            slider.value = Energy.CurrentEnergy / Energy.Capacity;
        }
    }

    // OnRefresh is called when a scene is loaded
    void OnRefresh()
    {
        slider = GetComponent<Slider>();
        if (Energy == null)
        {
            Energy = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponentInChildren<EnergyComponent>();
        }
    }

    private void OnDestroy()
    {
        MenuManager.Refresh -= OnRefresh;
    }
}
