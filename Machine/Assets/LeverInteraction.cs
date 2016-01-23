using UnityEngine;
using System.Collections;

public class LeverInteraction : MonoBehaviour {

	public float breakingRatio;
	static private bool[] pattern;
	static private bool[] state;

	void Start () {
		pattern = new bool[6];
		state = new bool[6];
		for (int i = 0; i < 6; i++) {
			pattern [i] = (Random.Range(0,1) == 1);
			state [i] = pattern [i]; 
		}
	}

	void Update () {
		if (Random.value > breakingRatio) {
			uint untouched = (uint)Random.Range (0,2);
			if (Random.value >= 0.5f) {
				for (int i = 0; i < 3; i++) {
					if (i != untouched)
						pattern [i] = !pattern [i];
				}
			} 
			else {
				untouched += 3;
				for (int i = 3; i < 6; i++) {
					if (i != untouched)
						pattern [i] = !pattern [i];
				}
			}

		}
		for (int i = 0; i < 6; i++) {
			if (pattern [i] != state [i]) {
				if (i < 3)
					Creator.Break ();
				else
					Collector.Break ();
			}
		}
	}

	static public void Use(GameObject lever){
		switch (lever.tag) {
		case("L1"):
			state [0] = !state [0];
			break;
		case("L2"):
			state [1] = !state [1];
			break;
		case("L3"):
			state [2] = !state [2];
			break;
		case("L4"):
			state [3] = !state [3];
			break;
		case("L5"):
			state [4] = !state [4];
			break;
		case("L6"):
			state [5] = !state [5];
			break;
		}
	}
}
