using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolComponent : MonoBehaviour {

    protected EnergyComponent _energy;
    public float EnergyCost;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Used by player to ensure that the correct energy component is assigned.
    public virtual void Assign(EnergyComponent energy)
    {
        _energy = energy;
    }

    // Player Controller will use this to request activation
    public bool TryActivate()
    {
        if(CanActivate() && _energy.TryExpend(EnergyCost))
        {
            Activate();
            return true;
        }
        return false;
    }

    // Abstract implementation
    protected abstract void Activate();
    protected abstract void Cancel();
    protected bool CanActivate() { return true; }
}
