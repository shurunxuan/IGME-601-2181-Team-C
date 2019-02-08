using UnityEngine;

/// <summary>
/// The template for a triggered event within the trigger system.
/// </summary>
public abstract class EnvironmentBehavior : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Three protocols need to be implemented
    public abstract void Activate();
    public abstract void Activate(bool state);
    public abstract void Activate(int state);

}
