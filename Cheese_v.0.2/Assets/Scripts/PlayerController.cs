using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed = 20;
    public float gravity = 9.8f;

    private Rigidbody rb;
    private Vector3 moveFoward;
    private Vector3 currentMovement;
    private Vector3 GravityPull;
    private float m_GroundCheckDistance = 1.0f;
    private float slope;
    private Vector3 m_GroundNormal;

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
        is_OnGround = false;
        is_Crouching = false;
        is_Standing = true;
        is_Walking = false;
        is_Running = false;
        is_Holding = false;
        is_Jumping = false;
        is_Reloading = false;
        rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotationX  | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        currentMovement = Vector3.zero;
        GravityPull = Vector3.zero;
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
        CheckGroundStatus();

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
        currentMovement.y = 0;
        currentMovement.z = 0;

        float slopeAngle = Vector3.Angle(m_GroundNormal, Vector3.up);

        if (slopeAngle < 45)
        {
            if (moveFront && !moveBack)
            {
                if (!moveLeft && !moveRight)
                {
                    currentMovement += Vector3.Cross(Vector3.Cross(m_GroundNormal, moveFoward), m_GroundNormal).normalized * speed * ((is_Running) ? 2 : 1);
                }
                else if (moveLeft && !moveRight)
                {
                    Vector3 left = Vector3.Cross(m_GroundNormal, moveFoward);
                    Vector3 front = Vector3.Cross(left, m_GroundNormal);
                    currentMovement += (front - left).normalized * speed * ((is_Running) ? 2 : 1);
                }
                else if (moveRight && !moveLeft)
                {
                    Vector3 right = Vector3.Cross(moveFoward, m_GroundNormal);
                    Vector3 front = Vector3.Cross(m_GroundNormal, right);
                    currentMovement += (front - right).normalized * speed * ((is_Running) ? 2 : 1);
                }
            }
            else if (moveBack && !moveFront)
            {
                if (!moveLeft && !moveRight)
                {
                    currentMovement -= Vector3.Cross(Vector3.Cross(m_GroundNormal, moveFoward), m_GroundNormal).normalized * speed * ((is_Running) ? 2 : 1);
                }
                else if (moveLeft && !moveRight)
                {
                    Vector3 left = Vector3.Cross(m_GroundNormal, moveFoward);
                    Vector3 back = -Vector3.Cross(left, m_GroundNormal);
                    currentMovement += (-left + back).normalized * speed * ((is_Running) ? 2 : 1);
                }
                else if (moveRight && !moveLeft)
                {
                    Vector3 right = Vector3.Cross(moveFoward, m_GroundNormal);
                    Vector3 back = -Vector3.Cross(m_GroundNormal, right);
                    currentMovement += (-right + back).normalized * speed * ((is_Running) ? 2 : 1);
                }
            }
            else if (moveLeft && !moveRight)
            {
                currentMovement += Vector3.Cross(moveFoward, m_GroundNormal).normalized * speed * ((is_Running) ? 2 : 1);
            }
            else if (moveRight && !moveLeft)
            {
                currentMovement += Vector3.Cross(m_GroundNormal, moveFoward).normalized * speed * ((is_Running) ? 2 : 1);
            }
        }

        if (is_Jumping)
        {
            GravityPull = Vector3.zero;
			GravityPull.y = jumpSpeed;
            is_OnGround = false;
			GetComponent<Animator> ().SetBool ("Jump", true);
        }
        
		if (!is_OnGround) {
			if (GravityPull.y > 0)
				GravityPull.y -= gravity * Time.deltaTime * 0.8f;
			else
				GravityPull.y -= gravity * Time.deltaTime;
		} else {
			GetComponent<Animator> ().SetBool ("Jump", false);
		}
        
		if (moveLeft || moveRight || moveFront || moveBack) {
			Vector3 newForward = new Vector3 (currentMovement.x, 0, currentMovement.z);
			this.transform.forward = Vector3.Slerp (this.transform.forward, newForward, Time.deltaTime * 10);
			GetComponent<Animator> ().SetBool ("Walk", true);
		} else {
			GetComponent<Animator> ().SetBool ("Walk", false);
		}

		if (is_Running) {
			GetComponent<Animator> ().SetBool ("Run", true);
		} else {
			GetComponent<Animator> ().SetBool ("Run", false);
		}

        rb.velocity = currentMovement + GravityPull;
    }

    
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Ground"))
        {
            is_Jumping = false;
            is_OnGround = true;
            GravityPull.y = 0;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Ground"))
        {
            is_Jumping = false;
            is_OnGround = true;
            GravityPull.y = 0;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Ground"))
        {
            is_OnGround = false;
        }
    }
    

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
			if (hitInfo.collider.gameObject.CompareTag("Ground")) {
				m_GroundNormal = hitInfo.normal;
				is_OnGround = true;
			}
        }
        else
        {
            is_OnGround = false;
            m_GroundNormal = Vector3.up;
        }
    }
}
