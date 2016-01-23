using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {
	
	public static Vector3 speed = new Vector3(0,0,5);

	void OnCollisionStay(Collision other) {
		other.rigidbody.velocity = speed;
	}

	public static void Halt(){
		speed = Vector3.zero;
	}

	public static void Continue(){
		speed = new Vector3(0,0,5);
	}
}
