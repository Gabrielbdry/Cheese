 using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public abstract class MobState{
    public abstract void Update();
    public abstract void OnTriggerEnter(Collider collision);
}


public class MobController : MonoBehaviour {
    public float speed;
    public Node startingNode;
    public Node endNode;

    private MobState actualState = null;
	private MobState changedState = null;

	void Start () {
        this.actualState = new FollowingPathState(this);
		this.changedState = this.actualState;
        UnityEngine.Debug.Log("Start");
    }

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == this.tag || collision.gameObject.tag == "Player")
			Physics.IgnoreCollision (collision.collider, this.GetComponent<Collider>());
	}

	void OnCollisionStay(Collision collision){
		if(collision.gameObject.tag == this.tag || collision.gameObject.tag == "Player")
			Physics.IgnoreCollision (collision.collider, this.GetComponent<Collider>());
	}

    void OnTriggerEnter(Collider collision){
		if(this.actualState != changedState)
			this.actualState = changedState;
        this.actualState.OnTriggerEnter(collision);
    }

	void Update () {
		if(! this.actualState.Equals(changedState))
			this.actualState = changedState;
        this.actualState.Update();
	}

    public void setState(MobState state){
		changedState = state;
    }
}

public class FollowingPathState : MobState
{
    MobController mob;
    System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);

    Stopwatch movementClock = new Stopwatch();

    Vector3 movement = new Vector3();

    Node lastNode = null;
    Node nextNode = null;

	bool foundLastNode = false;
	bool died = false;

    public FollowingPathState(MobController mob){
        this.mob = mob;
        movementClock.Start();
        nextNode = mob.startingNode;
    }

	public override void Update(){
        movement = nextNode.transform.position - mob.transform.position;
        movement.Normalize();
        movement *= mob.speed;
        this.mob.transform.Translate(movement * Time.deltaTime);
		if (foundLastNode)
			this.mob.setState (new DeadState ());
    }

    public override void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.transform.position == nextNode.transform.position) {
            bool found = false;
            while(!found) {
				if (nextNode.linkedNodes.Length == 0) {
					UnityEngine.Debug.Log("Found last Node.");
					foundLastNode = true;
					return;
				} else {
					Node tmp = null;
					tmp = nextNode.linkedNodes[rand.Next(0, nextNode.linkedNodes.Length)];
					if(! tmp.Equals(lastNode)) {
						lastNode = nextNode;
						nextNode = tmp;
						found = true;
						UnityEngine.Debug.Log("Next Node: " + nextNode.name);
					}

                }

            }
         
        }
    }
}

public class DeadState : MobState
{
    public override void OnTriggerEnter(Collider collision) {
    }

    public override void Update() {
    }
}
