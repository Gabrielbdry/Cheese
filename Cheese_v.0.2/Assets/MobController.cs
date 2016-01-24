 using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public abstract class MobState{
    public abstract void Update();
    public abstract void OnTriggerEnter(Collider collision);
    public abstract void OnCollisionEnter(Collision collision);
}


public class MobController : MonoBehaviour {
    public float speed;
    public Node startingNode;
    public Node endNode;

    public Transform explosion;

    private MobState actualState = null;
	private MobState changedState = null;

	void Start () {
        this.actualState = new FollowingPathState(this);
		this.changedState = this.actualState;
        UnityEngine.Debug.Log("Start");
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void Instantiate(UnityEngine.Object obj, Vector3 position) {
        Instantiate(obj, position, Quaternion.identity);
    }


	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == this.tag || collision.gameObject.tag == "Player")
			Physics.IgnoreCollision (collision.collider, this.GetComponent<Collider>());
        this.actualState.OnCollisionEnter(collision);
	}

	void OnCollisionStay(Collision collision){
		if(collision.gameObject.tag == this.tag)
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

    bool disturbedDirection = false;

    public FollowingPathState(MobController mob){
        this.mob = mob;
        movementClock.Start();
        nextNode = mob.startingNode;
        GetRightDirection();
    }

	public override void Update(){
        //if (movementClock.Elapsed.Seconds >= 1.0f) {
        //    disturbedDirection = !disturbedDirection;
        //    movementClock.Reset();
        //    movementClock.Start();
        //    if (disturbedDirection)
        //        DisturbDirection();
        //    else
        //        GetRightDirection();
        //}
       
       
        this.mob.GetComponent<Rigidbody>().velocity = movement * mob.speed;

        this.mob.transform.up = this.mob.GetComponent<Rigidbody>().velocity;

        if (foundLastNode)
			this.mob.setState (new DeadState (this.mob));
    }

    public void DisturbDirection() {
        float angle = (Mathf.Atan(movement.x / movement.z) / Mathf.PI) * 180;
        angle += rand.Next(-2, 2);
        movement.x = Mathf.Sin((angle * Mathf.PI) / 180f);
        movement.z = Mathf.Cos((angle * Mathf.PI) / 180f);

        movement.Normalize();
    }

    public void GetRightDirection() {
        movement = nextNode.transform.position - mob.transform.position;
        movement.Normalize();
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
                        GetRightDirection();
					}

                }

            }
         
        }
    }

    public override void OnCollisionEnter(Collision collision) {
        throw new NotImplementedException();
    }
}

public class DeadState : MobState
{
    public DeadState(MobController mob) {
        mob.Instantiate(mob.explosion.gameObject, mob.transform.position);
        mob.Destroy();
    }

    public override void OnCollisionEnter(Collision collision) {
    }

    public override void OnTriggerEnter(Collider collision) {
    }

    public override void Update() {
    }
}
