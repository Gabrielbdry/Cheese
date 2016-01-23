using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed = 20;
    public float gravity = 9.8f;

    private Rigidbody rb;
    private CharacterController controller;
    private Vector3 moveFoward;
    private Vector3 currentMovement;
    private float slopeAngle;

    // States...
    bool is_OnGround;
    bool is_Crouching;
    bool is_Standing;
    bool is_Walking;
    bool is_Running;
    bool is_Holding;
    bool is_Jumping;
    bool is_Reloading;

    // Use this for initialization
    void Start ()
    {
        slopeAngle = 0;
        is_OnGround = false;
        is_Crouching = false;
        is_Standing = true;
        is_Walking = false;
        is_Running = false;
        is_Holding = false;
        is_Jumping = false;
        is_Reloading = false;
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        currentMovement = Vector3.zero;
	}
	
    public void UpdateDirection(Vector3 Foward)
    {
        moveFoward = Foward;
    }
    
    public bool isCrouching()
    {
        return is_Crouching;
    }

	// Update is called once per frame
	void Update ()
    {
        bool moveFront = Input.GetKey(KeyCode.W);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveBack = Input.GetKey(KeyCode.S);
        
        is_Crouching = (!is_Running && !is_Jumping) ? Input.GetKey(KeyCode.LeftControl) : false;
        is_Walking = !Input.GetKey(KeyCode.LeftShift);
        is_Running = (!is_Jumping && !is_Holding && !is_Crouching) ? Input.GetKey(KeyCode.LeftShift) : false;
        is_Jumping = (!is_Crouching && is_OnGround) ? Input.GetKey(KeyCode.Space) : false;
        is_Reloading = (!is_Holding) ? Input.GetKey(KeyCode.R) : false;
        is_Standing = (!is_Crouching && !is_Walking && !is_Running && !is_Jumping) ? true : false;

        currentMovement.x = 0;
        currentMovement.z = 0;

        if (slopeAngle < 45 && is_OnGround)
        {
            if (moveFront)
            {
                currentMovement += Vector3.ProjectOnPlane(moveFoward, Vector3.up).normalized * speed * ((is_Running) ? 2 : 1);
            }
            if (moveBack)
            {
                currentMovement -= Vector3.ProjectOnPlane(moveFoward, Vector3.up).normalized * speed * ((is_Running) ? 2 : 1);
            }
            if (moveLeft)
            {
                currentMovement += Vector3.Cross(moveFoward, Vector3.up).normalized * speed * ((is_Running) ? 2 : 1);
            }
            if (moveRight)
            {
                currentMovement -= Vector3.Cross(moveFoward, Vector3.up).normalized * speed * ((is_Running) ? 2 : 1);
            }
        }
        
        
        if (is_Jumping)
        {
            currentMovement.y = jumpSpeed;
            is_OnGround = false;
        }

        
        if (!is_OnGround)
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
        

        rb.velocity = currentMovement;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Ground"))
        {
            is_Jumping = false;
            is_OnGround = true;
            currentMovement.y = 0;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Ground"))
        {
            is_Jumping = false;
            is_OnGround = true;
            currentMovement.y = 0;
            slopeAngle = Vector3.Angle(other.contacts[0].normal, Vector3.up);
            transform.Rotate(Vector3.right, slopeAngle);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Ground"))
        {
            is_OnGround = false;
        }
    }
}
