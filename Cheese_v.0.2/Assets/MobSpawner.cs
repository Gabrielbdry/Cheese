using UnityEngine;
using System.Collections;

public class MobSpawner : MonoBehaviour {
    
	private float lastTime;
	public float frequency;
	public Rigidbody _mob;

	void Start () {
		lastTime = Time.time;
	}
	
	void Update () {
		if (Time.time - lastTime >= frequency) {
			string start = "";
			switch(Random.Range(0,2)) {
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
			mob.transform.position = GameObject.Find (start).transform.position + (Vector3.up * 0.5f);
			lastTime = Time.time;
		}
	}
}
