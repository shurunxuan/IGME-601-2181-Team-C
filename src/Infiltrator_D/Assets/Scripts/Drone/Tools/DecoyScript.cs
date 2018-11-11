using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyScript : MonoBehaviour {
    
    // Collider for the sound emitted on collision
    public Collider SoundArea;
    // The period for which the sound is active after collision
    public float SoundPeriod;

    // The timer that turns off the sound
    private float timer;

    // Use this for initialization
    void Start ()
    {
        timer = 0;
        SoundArea.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SoundArea.enabled = false;
            }
        }
    }

    // Becomes active on hit
    private void OnCollisionEnter(Collision collision)
    {
        timer = SoundPeriod;
        SoundArea.enabled = true;
    }
}
