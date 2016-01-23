using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour {

	private static float lastTime;
	private static bool status = true;
	public static bool broken = false;
	public float frequency;
	public GameObject cheese;

	void Start () {
		lastTime = Time.time;
	}

	void Update () {
		if (status && !broken && Time.time - lastTime >= frequency) {
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
			if(status)
				lastTime = Time.time;
		}
	}

	public static void Halt(){
		if(status)
			status = false;
	}

	public static void Continue(){
		if (!status) {
			status = true;
			if(!broken)
				lastTime = Time.time;
		}
	}
}
