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

    // Property of the tool: cooldown
    public float CoolDownTime;
    private float coolDownTimer;

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
        if (coolDownTimer < 0)
        {
            if (CanActivate() && (ToggledOn || Energy.TryExpend(InitialEnergyCost)))
            {
                Activate();
                coolDownTimer = CoolDownTime;
                return true;
            }
        }
        return false;
    }

    // This is called by PlayerController.Update()
    public void Cooldown()
    {
        // Tool cooldown
        coolDownTimer -= Time.deltaTime;
        // i use this strange number because I want to make sure it is negative.
        if (coolDownTimer < -0.001f) coolDownTimer = -0.001f;
    }

    public float CooldownPercentage()
    {
        if (coolDownTimer < 0) return 0.0f;
        return coolDownTimer / CoolDownTime;
    }

    // Abstract implementation
    protected abstract void Activate();
    public virtual void SetCurrent(bool state) { enabled = state; }
    public abstract void Cancel();
    protected bool CanActivate() { return true; }
    public virtual string GetName() { return "Tool Component"; }
}
