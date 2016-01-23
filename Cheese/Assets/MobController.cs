using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public abstract class MobState{
    public abstract void Update();
    public abstract void OnCollisionEnter(Collision collision);
}


public class MobController : MonoBehaviour {

    public Vector3 startPosition;
    public float speed;
    public Vector3 startDirection;

    private MobState actualState;

	void Start () {
        this.actualState = new WalkingState(this, speed);
        this.transform.position = startPosition;
	}

    void OnCollisionEnter(Collision collision){
        this.actualState.OnCollisionEnter(collision);
    }

	void Update () {
        this.actualState.Update();
	}

    public void setState(MobState state){
        this.actualState = state;
    }
}

public class WalkingState : MobState
{
    MobController mob;
    float speed;
    System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);

    Stopwatch movementClock = new Stopwatch();

    Vector3 movement = new Vector3();

    public WalkingState(MobController mob, float speed){
        this.mob = mob;
        this.speed = speed;
        movementClock.Start();

        movement = mob.startDirection * this.speed;
    }

    public void SetRandomMovement(){
        float translateX = rand.Next(-10, 10) / 10f * this.speed;
        float translateY = 0;
        float translateZ = this.speed - translateX;

        movement.Set(translateX, translateY, translateZ);
    }

    public override void Update(){
        Console.WriteLine("Walking.");

        if (movementClock.ElapsedMilliseconds >= 2000){
            SetRandomMovement();
            movementClock.Reset();
            movementClock.Start();
        }

        this.mob.transform.Translate(movement * Time.deltaTime);
    }

    public override void OnCollisionEnter(Collision collision) {
        if(collision.rigidbody.tag == "Border") {
            ContactPoint contact = collision.contacts[0];
            movement = contact.normal * this.speed;
            movementClock.Reset();
            movementClock.Start();
        }
    }
}

public class DeadState : MobState
{
    public override void OnCollisionEnter(Collision collision) {
        throw new NotImplementedException();
    }

    public override void Update() {
        Console.WriteLine("Dead.");
    }
}
