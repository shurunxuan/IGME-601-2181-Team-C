using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyComponent : MonoBehaviour {

    // The maximum allotment of energy
    public float Capacity = 100;

    // The amount of energy the drone currently has
    private float _energy;
    public float CurrentEnergy { get { return _energy; } }

	// Use this for initialization
	void Start () {
        _energy = Capacity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Charges the device by a given amount of energy.
    /// Returns true when fully charged.
    /// </summary>
    public bool Charge(float amount)
    {
        _energy += amount;
        if(_energy > Capacity)
        {
            _energy = Capacity;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes a given amount of energy regardless of whether or not enough energy is present
    /// </summary>
    public void Expend(float amount)
    {
        _energy -= amount;
        if(_energy < 0)
        {
            _energy = 0;
        }
    }

    /// <summary>
    /// Only removes energy if "CurrentEnergy" exceeds "amount".
    /// </summary>
    public bool TryExpend(float amount)
    {
        if(_energy >= amount)
        {
            _energy -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Simple method that allows you to check if we have run out of energy
    /// </summary>
    public bool IsDead()
    {
        return CurrentEnergy <= 0;
    }

    public bool IsFull()
    {
        return CurrentEnergy == Capacity;
    }

}
