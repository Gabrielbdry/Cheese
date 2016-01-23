using UnityEngine;
using System.Collections;

public class LeverInteraction : MonoBehaviour {

	delegate void Use();

	void Update () {
		if(Input.GetKeyDown (KeyCode.Q))
			Creator.Break ();
		if (Input.GetKeyDown (KeyCode.W))
			Creator.Repair ();
		if (Input.GetKeyDown (KeyCode.E))
			Collector.Break ();
		if (Input.GetKeyDown (KeyCode.R))
			Collector.Repair ();
	}
}
