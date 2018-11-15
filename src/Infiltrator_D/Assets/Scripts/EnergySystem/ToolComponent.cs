using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolComponent : MonoBehaviour {

    // Cost of initially using the tool
    public float InitialEnergyCost;
    protected EnergyComponent _energy;

    // Toggled on allows child classes to negate the energy cost assosiated with the tool
    protected bool _toggledOn;

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
        if(CanActivate() && (_toggledOn || _energy.TryExpend(InitialEnergyCost)))
        {
            Activate();
            return true;
        }
        return false;
    }

    // Abstract implementation
    protected abstract void Activate();
    public virtual void SetCurrent(bool state) { enabled = state; }
    public abstract void Cancel();
    protected bool CanActivate() { return true; }
}
