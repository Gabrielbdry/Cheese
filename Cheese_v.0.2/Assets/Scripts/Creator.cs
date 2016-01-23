using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour {

	private static float lastTime;
	public static bool broken = false;
	public float frequency;
	public GameObject cheese;

	void Start () {
		lastTime = Time.time;
	}

	void Update () {
		if (!broken && Time.time - lastTime >= frequency) {
			Instantiate (cheese);
			lastTime = Time.time;
		}
	}

	public static void Break(){
		if(!broken)
			broken = true;
	}

	public static void Repair(){
		if (broken) {
			broken = false;
			lastTime = Time.time;
		}
	}
}
