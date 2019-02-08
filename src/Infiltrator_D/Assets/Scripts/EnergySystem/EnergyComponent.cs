using UnityEngine;

public class EnergyComponent : MonoBehaviour
{

    // The maximum allotment of energy
    public float Capacity = 100;

    // Unlimited Power
    public bool Infinite;

    // The amount of energy the drone currently has
    private float energy;
    public float CurrentEnergy { get { return energy; } }

    private const string CheatCommand = "infinite";
    private int cheatCommandPtr = 0;

    // Use this for initialization
    void Start()
    {
        energy = Capacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Infinite)
        {
            energy = Capacity;
        }

        // Cheating Command
        if (Input.anyKeyDown)
        {
            string inputString = Input.inputString;
            if (inputString.Length == 0) return;
            char key = inputString[0];
            if (key == CheatCommand.ToLower()[cheatCommandPtr])
            {
                cheatCommandPtr++;
                if (cheatCommandPtr == CheatCommand.Length)
                {
                    Infinite = !Infinite;
                    cheatCommandPtr = 0;
                }
            }
            else
            {
                cheatCommandPtr = 0;
            }
        }
    }

    /// <summary>
    /// Charges the device by a given amount of energy.
    /// Returns true when fully charged.
    /// </summary>
    public bool Charge(float amount)
    {
        energy += amount;
        if (energy > Capacity)
        {
            energy = Capacity;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes a given amount of energy regardless of whether or not enough energy is present
    /// </summary>
    public void Expend(float amount)
    {
        energy -= amount;
        if (energy < 0)
        {
            energy = 0;
        }
    }

    /// <summary>
    /// Only removes energy if "CurrentEnergy" exceeds "amount".
    /// </summary>
    public bool TryExpend(float amount)
    {
        if (energy >= amount)
        {
            energy -= amount;
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
