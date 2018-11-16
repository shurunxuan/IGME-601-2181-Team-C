using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonFollowing : MonoBehaviour {

    public Transform DroneTransform;

	// Use this for initialization
	void Start ()
	{
	    transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        transform.position = DroneTransform.position;
    }
}
