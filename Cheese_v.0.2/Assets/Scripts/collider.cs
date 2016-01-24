using UnityEngine;
using System.Collections;

public class collider : MonoBehaviour {
	void OnTriggerEnter(Collider other) {

	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "PickUp" && Input.GetKey (KeyCode.Q)) {
			Debug.Log ("money money");
			Debug.Log ("money Q");
			other.gameObject.SetActive (false);
			Destroy (other);
			GrabAndDrop.cheesestatus += 1;
			if (GrabAndDrop.cheesestatus == GrabAndDrop.cheeseObj) {
				GrabAndDrop.cheesestatus = 0;
				UI.money += 1000;
				
			}
		}
	
	}
}