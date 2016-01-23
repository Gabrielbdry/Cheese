using UnityEngine;
using System.Collections;

public class ShippingDeck : MonoBehaviour {

	public float scoreMultiplier;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Box") {
			//setScore
			Destroy(other);
		}
	}

}
