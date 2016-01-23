using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	private bool ready = true;

	void OnTriggerEnter (Collider other) {
		if (ready && other.tag == "Mouse") {
			Destroy (other);
			ready = false;
		}
	}

	void Arm(){
		ready = true;
	}

}
