using UnityEngine;
using System.Collections;

public class LeverInteraction : MonoBehaviour {

	public float breakingRatio;
	public float minRunTime;
	static private bool[] pattern;
	static private bool[] state;
	private float lastTime;

	void Start () {
		pattern = new bool[3];
		state = new bool[3];
		for (int i = 0; i < 3; i++) {
			pattern [i] = (Random.Range(0,1) == 1);
			state [i] = pattern [i]; 
		}
		lastTime = Time.time;
	}

	void Update () {
		if (lastTime == -1 && !Collector.broken && !Creator.broken)
			lastTime = Time.time;
		if(Time.time - lastTime >= minRunTime){
			if (Random.value < breakingRatio) {
				if (!Collector.broken) {
					uint untouched = (uint)Random.Range (0,2);
					lastTime = -1;
					for (int i = 0; i < 3; i++) {
						if (i != untouched)
							pattern [i] = !pattern [i];
					}
				}

			}
		}
		for (int i = 0; i < 3; i++) {
			if (pattern [i] != state [i]) {
				Collector.Break ();
				lastTime = -1;
			}
		}
	}

	static public void Use(GameObject lever){
		switch (lever.name) {
		case("Lever1"):
			state [0] = !state [0];
			break;
		case("Lever2"):
			state [1] = !state [1];
			break;
		case("Lever3"):
			state [2] = !state [2];
			break;
		}
	}
}
