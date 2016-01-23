using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {
	
	public static Vector3 speed = new Vector3(0,0,-2);

	void OnCollisionStay(Collision other) {
		other.rigidbody.velocity = speed;
	}

	public static void Break(){
		if(speed == new Vector3(0,0,-2))
			speed = Vector3.zero;
	}

	public static void Repair(){
		if(speed == Vector3.zero)
			speed = new Vector3(0,0,-2);
	}
}
