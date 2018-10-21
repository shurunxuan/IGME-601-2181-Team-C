using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private DroneMovement movement;
	// Use this for initialization
	void Start () {
		movement = GetComponent<DroneMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		var right = Input.GetAxis("Right");
		var forward = Input.GetAxis("Forward");
		var up = Input.GetAxis("Up");

		movement.TargetForce = (forward * 2 * movement.Forward + up * Vector3.up + right * 2 * Vector3.Cross(Vector3.up, movement.Forward) - movement.Gravity) * movement.SpeedFactor;
	}
}
