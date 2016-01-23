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

    private MobState actualState;
    

	void Start () {
        this.actualState = new FollowingPathState(this);
        UnityEngine.Debug.Log("Start");
    }

    void OnTriggerEnter(Collider collision){
        this.actualState.OnTriggerEnter(collision);
    }

	void Update () {
        this.actualState.Update();
	}

    public void setState(MobState state){
        this.actualState = state;
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

    public FollowingPathState(MobController mob){
        this.mob = mob;
        movementClock.Start();
        nextNode = mob.startingNode;
    }

    public override void Update(){
        movement = nextNode.transform.position - mob.transform.position;
        movement.Normalize();
        movement *= mob.speed;
        UnityEngine.Debug.Log("UpdatingState");
        this.mob.transform.Translate(movement * Time.deltaTime);
    }

    public override void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.transform.position == nextNode.transform.position) {
            bool found = false;
            while(!found) {
                Node tmp = nextNode.linkedNodes[rand.Next(0, nextNode.linkedNodes.Length - 1)];
                if(tmp != lastNode) {
                    lastNode = nextNode;
                    nextNode = tmp;
                    found = true;
                    UnityEngine.Debug.Log("Next Node: " + nextNode.name);
                    if(nextNode.linkedNodes.Length == 0) {
                        UnityEngine.Debug.Log("Found last Node.");
                        mob.setState(new DeadState());
                    }
                }

            }
         
        }
    }
}

public class DeadState : MobState
{
    public override void OnTriggerEnter(Collider collision) {
        throw new NotImplementedException();
    }

    public override void Update() {
        Console.WriteLine("Dead.");
    }
}
