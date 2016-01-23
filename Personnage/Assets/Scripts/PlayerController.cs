using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;

    private Rigidbody rb;
    private Vector3 moveFoward;

    // States...
    bool isCrouching;
    bool is_Standing;
    bool is_Walking;
    bool is_Running;
    bool is_Holding;
    bool is_Jumping;
    bool is_Reloading;

    // Use this for initialization
    void Start ()
    {
        is_Standing = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
	}
	
    public void UpdateDirection(Vector3 Foward)
    {
        moveFoward = Foward;
    }

	// Update is called once per frame
	void Update ()
    {
        bool moveFront = Input.GetKey(KeyCode.W);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveBack = Input.GetKey(KeyCode.S);
        is_Running = Input.GetKey(KeyCode.LeftShift);

        Vector3 movement = Vector3.zero;

        if (moveFront)
        {
            movement += moveFoward.normalized;
        }
        if (moveBack)
        {
            movement -= moveFoward.normalized;
        }
        if (moveLeft)
        {
            movement += Vector3.Cross(moveFoward, Vector3.up).normalized;
        }
        if (moveRight)
        {
            movement -= Vector3.Cross(moveFoward, Vector3.up).normalized;
        }

        rb.velocity = movement * speed * ((is_Running) ? 2 : 1);
	}
}
