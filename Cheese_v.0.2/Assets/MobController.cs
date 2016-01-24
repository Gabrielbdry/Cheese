using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public abstract class MobState {
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

    System.Random rand = new System.Random();

    void Start() {
        if(rand.Next(0, 2) == 0)
            this.actualState = new FollowingPathState(this);
        else
            this.actualState = new ToTargetState(this);
        this.changedState = this.actualState;
        // UnityEngine.Debug.Log("Start");
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void DestroyObj(GameObject gameObject) {
        Destroy(gameObject);
    }

    public void Instantiate(UnityEngine.Object obj, Vector3 position) {
        Instantiate(obj, position, Quaternion.identity);
    }


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == this.tag || collision.gameObject.tag == "Player")
            Physics.IgnoreCollision(collision.collider, this.GetComponent<Collider>());
        this.actualState.OnCollisionEnter(collision);
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == this.tag || collision.gameObject.tag == "Player")
            Physics.IgnoreCollision(collision.collider, this.GetComponent<Collider>());
    }

    void OnTriggerEnter(Collider collision) {
        if (this.actualState != changedState)
            this.actualState = changedState;
        this.actualState.OnTriggerEnter(collision);
    }

    void Update() {
        if (!this.actualState.Equals(changedState))
            this.actualState = changedState;
        this.actualState.Update();
    }

    public void setState(MobState state) {
        changedState = state;
    }
}

public class FollowingPathState : MobState {
    MobController mob;
    System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);

    Stopwatch movementClock = new Stopwatch();

    Vector3 movement = new Vector3();

    Node lastNode = null;
    Node nextNode = null;

    bool foundLastNode = false;
    bool died = false;

    bool disturbedDirection = false;

    public FollowingPathState(MobController mob) {
        this.mob = mob;
        movementClock.Start();
        nextNode = mob.startingNode;
        GetRightDirection();
    }

    public override void Update() {
        this.mob.GetComponent<Rigidbody>().velocity = movement * mob.speed;

        this.mob.transform.forward = this.mob.GetComponent<Rigidbody>().velocity;

        if (foundLastNode)
            this.mob.setState(new DeadState(this.mob));
    }

    public void GetRightDirection() {
        movement = nextNode.transform.position - mob.transform.position;
        movement.Normalize();
    }

    public override void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.transform.position == nextNode.transform.position) {
            bool found = false;
            while (!found) {
                if (nextNode.linkedNodes.Length == 0) {
                    //UnityEngine.Debug.Log("Found last Node.");
                    foundLastNode = true;
                    return;
                } else {
                    Node tmp = null;
                    tmp = nextNode.linkedNodes[rand.Next(0, nextNode.linkedNodes.Length)];
                    if (!tmp.Equals(lastNode)) {
                        lastNode = nextNode;
                        nextNode = tmp;
                        found = true;
                        //UnityEngine.Debug.Log("Next Node: " + nextNode.name);
                        GetRightDirection();
                    }

                }

            }

        }
    }

    public override void OnCollisionEnter(Collision collision) {
        //throw new NotImplementedException();
    }
}

public class DeadState : MobState {
    MobController mob;
    public DeadState(MobController mob) {
        this.mob = mob;
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

public class ToTargetState : MobState {
    MobController mob;
    Vector3 movement;
    PlayerController player;
    public ToTargetState(MobController mob) {
        this.mob = mob;
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        movement = player.transform.position - mob.transform.position;
        movement.Normalize();
    }

    GameObject FindNearestNode() {
        GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag("Node");
        float nearestDistanceSqr = (taggedGameObjects[0].transform.position - mob.transform.position).sqrMagnitude;
        GameObject nearestObj = taggedGameObjects[0];
        foreach (GameObject obj in taggedGameObjects) {
            var objectPos = obj.transform.position;
            var distanceSqr = (objectPos - mob.transform.position).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr) {
                nearestObj = obj;
                nearestDistanceSqr = distanceSqr;
            }
        }
        return nearestObj;
    }


    public override void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "PickUp") {
            this.mob.DestroyObj(collision.collider.gameObject);

            GameObject nextNode = FindNearestNode();

            this.mob.startingNode = nextNode.GetComponent<Node>();
            this.mob.setState(new FollowingPathState(mob));
        }
    }

    public override void OnTriggerEnter(Collider collision) {
    }

    public override void Update() {
        movement = player.transform.position - mob.transform.position;
        movement.Normalize();

        this.mob.GetComponent<Rigidbody>().velocity = movement * mob.speed;

        this.mob.transform.forward = this.mob.GetComponent<Rigidbody>().velocity;
    }
}