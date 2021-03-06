﻿using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {
	
	public static Vector3 speed = new Vector3(0,0,5);

	void OnCollisionStay(Collision other) {
		other.rigidbody.velocity = speed;
	}

	public static void Halt(){
		if(speed == new Vector3(0,0,5))
			speed = Vector3.zero;
	}

	public static void Continue(){
		if(speed == Vector3.zero)
			speed = new Vector3(0,0,5);
	}
}
