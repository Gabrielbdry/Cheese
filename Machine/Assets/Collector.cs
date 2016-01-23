using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {
	
	public GameObject box;
	public Vector3 spawnZone;
	public uint cheeseBox = 10;
	public static bool broken = false;
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
		if (!broken) {
			Creator.Halt ();
			ConveyorBelt.Halt ();
		}
	}

	public static void Repair(){
		if (broken) {
			ConveyorBelt.Continue ();
			Creator.Continue ();
		}
	}
}
