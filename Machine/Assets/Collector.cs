using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {
	
	public GameObject box;
	public Vector3 spawnZone;
	public uint cheeseBox = 10;
	private uint cheeseCount;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Cheese") {
			Destroy (other.gameObject);
			cheeseCount++;
			if (cheeseCount == cheeseBox) {
				cheeseCount = 0;
				Instantiate (box, spawnZone + Random.insideUnitSphere, Quaternion.identity);
			}
		}
	}

	public static void Break(){
		Creator.Halt ();
		ConveyorBelt.Halt ();
	}

	public static void Repair(){
		ConveyorBelt.Continue ();
		Creator.Continue ();
	}
}
