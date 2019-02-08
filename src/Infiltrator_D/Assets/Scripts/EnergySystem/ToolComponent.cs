using UnityEngine;

public abstract class ToolComponent : MonoBehaviour
{

    // Cost of initially using the tool
    public float InitialEnergyCost;
    protected EnergyComponent Energy;

    // Toggled on allows child classes to negate the energy cost associated with the tool
    protected bool ToggledOn;

    // Tool Icon
    public Sprite Icon;

    // Property of the tool: need aiming
    public bool NeedAiming;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Used by player to ensure that the correct energy component is assigned.
    public virtual void Assign(EnergyComponent energy)
    {
        Energy = energy;
    }

    // Player Controller will use this to request activation
    public bool TryActivate()
    {
        if (CanActivate() && (ToggledOn || Energy.TryExpend(InitialEnergyCost)))
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
    public virtual string GetName() { return "Tool Component"; }
}
