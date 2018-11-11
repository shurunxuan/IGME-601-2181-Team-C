using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyScript : MonoBehaviour {
    
    // Collider for the sound emitted on collision
    public Collider SoundArea;
    // The period for which the sound is active after collision
    public float SoundPeriod;

    // The lifespan of the Decoy
    public float LifeSpan;

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
        // Check if still making sound
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SoundArea.enabled = false;
            }
        }
        // Check if lifespan is up
        LifeSpan -= Time.deltaTime;
        if(LifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Becomes active on hit
    private void OnCollisionEnter(Collision collision)
    {
        timer = SoundPeriod;
        SoundArea.enabled = true;
    }
}
