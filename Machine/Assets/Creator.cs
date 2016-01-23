using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour {

	private static float lastTime;
	private static bool status = true;
	public float frequency;
	public GameObject cheese;

	void Start () {
		lastTime = Time.time;
	}

	void Update () {
		if (status && Time.time - lastTime >= frequency) {
			Instantiate (cheese);
			lastTime = Time.time;
		}
	}

	public static void Break(){
		status = false;
	}

	public static void Repair(){
		status = true;
		lastTime = Time.time;
	}

	public static void Halt(){
		status = false;
	}

	public static void Continue(){
		status = true;
		lastTime = Time.time;
	}
}
