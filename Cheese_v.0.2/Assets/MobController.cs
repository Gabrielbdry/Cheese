using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        //if(rand.Next(0, 2) == 0)
            this.actualState = new FollowingPathState(this);
       // else
           // this.actualState = new FollowingPlayerState(this);
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

    Vector3 m_GroundNormal;
    bool is_OnGround;
    float angle = 0;

    public FollowingPathState(MobController mob) {
        this.mob = mob;
        movementClock.Start();
        nextNode = mob.startingNode;
        GetRightDirection();
    }
    void CheckGroundStatus(float slopeAngle) {
        RaycastHit hitInfo;

        if (Physics.Raycast(mob.transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hitInfo, 0.5f)) {
            if (hitInfo.distance <= 0.5) {
                if (hitInfo.collider.gameObject.CompareTag("Ground")) {
                    m_GroundNormal = hitInfo.normal;
                    is_OnGround = true;
                }
            } else {
                slopeAngle = Vector3.Angle(m_GroundNormal, Vector3.up);
                if (slopeAngle > 0) {
                    if (hitInfo.collider.gameObject.CompareTag("Ground")) {
                        m_GroundNormal = hitInfo.normal;
                        is_OnGround = true;
                    }
                }
            }
        } else {
            is_OnGround = false;
            m_GroundNormal = Vector3.up;
        }
    }

    public override void Update() {
        GetRightDirection();
        CheckGroundStatus(angle);
        float magnitude = movement.magnitude;
        Vector3.ProjectOnPlane(movement, m_GroundNormal);

        movement = movement.normalized * magnitude;


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

public class FollowingPlayerState : MobState {
    private PlayerController player;
    MobController mob;
    System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);

    Stopwatch movementClock = new Stopwatch();

    Vector3 movement = new Vector3();

    Node currentNode = null;

    bool foundLastNode = false;
    bool died = false;

    bool disturbedDirection = false;

    public FollowingPlayerState(MobController mob) {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.mob = mob;
        movementClock.Start();
        currentNode = mob.startingNode;
        GetRightDirection();
    }

    public override void Update() {
        GetRightDirection();
        this.mob.GetComponent<Rigidbody>().velocity = movement * mob.speed;

        this.mob.transform.forward = this.mob.GetComponent<Rigidbody>().velocity;

        if (foundLastNode)
            this.mob.setState(new DeadState(this.mob));
    }

    public void GetRightDirection() {
        movement = currentNode.transform.position - mob.transform.position;
        movement.Normalize();
    }

    Node FindNearestNode(Vector3 position) {
        GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag("Node");
        float nearestDistanceSqr = (taggedGameObjects[0].transform.position - position).sqrMagnitude;
        GameObject nearestObj = taggedGameObjects[0];
        foreach (GameObject obj in taggedGameObjects) {
            var objectPos = obj.transform.position;
            var distanceSqr = (objectPos - position).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr) {
                nearestObj = obj;
                nearestDistanceSqr = distanceSqr;
            }
        }
        return nearestObj.GetComponent<Node>();
    }

    public Node findPath() {
        Node s = this.currentNode;
        Node e = this.FindNearestNode(player.transform.position);
        if (e == s)
            return s;

        Node[] path = new Node[100];
        for (int i = 0; i < 100; i++)
            path[i] = null;

        Queue<Node> q = new Queue<Node>();

        q.Enqueue(s);

        Node current;
        bool found = false;

        while (q.Count != 0) {

            current = q.Dequeue();

            foreach (Node next in current.linkedNodes) {
                if (path[next.id] == null) {
                    path[next.id] = current;
                    q.Enqueue(next);
                }
                if (next == e) {
                    found = true;
                    break;
                }
            }

            if (found)
                break;
        }

        while (path[e.id] != s) {
            e = path[e.id];
        }

        return e;
    }

    public override void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.transform.position == currentNode.transform.position) {
            Node next = findPath();
            if (next != null) {
                currentNode = next;
                GetRightDirection();
            } else {
                UnityEngine.Debug.Log("Not found node path");
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

    Node FindNearestNode(Vector3 position) {
        GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag("Node");
        float nearestDistanceSqr = (taggedGameObjects[0].transform.position - position).sqrMagnitude;
        GameObject nearestObj = taggedGameObjects[0];
        foreach (GameObject obj in taggedGameObjects) {
            var objectPos = obj.transform.position;
            var distanceSqr = (objectPos - position).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr) {
                nearestObj = obj;
                nearestDistanceSqr = distanceSqr;
            }
        }
        return nearestObj.GetComponent<Node>();
    }


    public override void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "PickUp") {
            this.mob.DestroyObj(collision.collider.gameObject);

            Node nextNode = FindNearestNode(mob.transform.position);

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