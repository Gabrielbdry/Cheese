using UnityEngine;
using System.Collections;

public class RayCasting : MonoBehaviour {

	public float range;

	void OnMouseDown(Transform character) {
		RaycastHit hit;
		if(Physics.Linecast (character.position, (transform.forward.normalized - character.position.normalized - transform.position.normalized).normalized * range	,out hit)){
			if (hit.transform.tag == "Mouse") {
				Destroy (hit.transform.gameObject);
			}
		}
	}
}
