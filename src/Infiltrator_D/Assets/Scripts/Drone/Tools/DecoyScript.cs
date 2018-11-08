using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyScript : MonoBehaviour {

    // The time period for which the decoy will remain active
    public float TimeActive;

    // The timer currently in use for cleaning up the decoy
    // timer is -1 if inactive
    private float timer;

	// Use this for initialization
	void Start () {
        timer = -1;
        enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            // Destroy self after timer runs out
            Destroy(gameObject);
        }
	}

    // Becomes active on hit
    private void OnCollisionEnter(Collision collision)
    {
        if (timer == -1)
        {
            timer = TimeActive;
        }
        enabled = true;
    }
}
