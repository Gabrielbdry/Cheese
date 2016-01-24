using UnityEngine;
using System.Collections;

public class BoxInteraction : MonoBehaviour {

	public int integrity = 10;

	void Update () {
		if(integrity <= 0)
			//Score--?
			Destroy(gameObject);
	}
    
}
