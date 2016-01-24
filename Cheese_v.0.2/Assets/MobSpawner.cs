using UnityEngine;
using System.Collections;

public class MobSpawner : MonoBehaviour {
    
	private System.Diagnostics.Stopwatch spawnClock = new System.Diagnostics.Stopwatch();
	public float spawnPerSec;
	public Rigidbody _mob;

	void Start () {
		spawnClock.Start ();
	}
    
	void Update () {
		if (spawnClock.Elapsed.Seconds >= 1 / spawnPerSec) {
			string start = "";
			switch(Random.Range(0,3)) {
			case(0):
				start = "Node";
				break;
			case(1):
				start = "Node (4)";				
				break;
			case(2):
				start = "Node (9)";			
				break;
			}
			GameObject mob = Instantiate (_mob.transform.gameObject);
			mob.transform.gameObject.GetComponent<MobController> ().startingNode = GameObject.Find (start).GetComponent<Node>();
			mob.transform.gameObject.GetComponent<MobController> ().endNode = GameObject.Find ("Node (6)").GetComponent<Node> ();
			mob.transform.position = GameObject.Find (start).transform.position + (Vector3.up * 0.5f);
			spawnClock.Reset ();
			spawnClock.Start ();
		}
	}
}
