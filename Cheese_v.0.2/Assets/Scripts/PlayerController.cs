using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed = 20;
    public float gravity = 9.8f;
    public float runFactor = 1.6f;
	public static bool weaponEquiped = false;

    private Rigidbody rb;
    private Vector3 moveFoward;
    private Vector3 currentMovement;
    private Vector3 GravityPull;
    private float m_GroundCheckDistance = 0.5f;
    private float slope;
    private Vector3 m_GroundNormal;

    // States...
    bool is_OnGround;
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

	// Update is called once per frame
	void Update ()
    {
        CheckGroundStatus();

        bool moveFront = Input.GetKey(KeyCode.W);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveBack = Input.GetKey(KeyCode.S);

        is_Walking = ((moveRight || moveLeft || moveFront || moveBack) && !is_Jumping && is_OnGround && !Input.GetKey(KeyCode.LeftShift)) ? true : false;
        is_Running = ((moveRight || moveLeft || moveFront || moveBack) && is_OnGround && !is_Jumping && !is_Holding && Input.GetKey(KeyCode.LeftShift)) ? true : false;
        is_Jumping = (is_OnGround) ? Input.GetKey(KeyCode.Space) : false;
        is_Reloading = (!is_Holding) ? Input.GetKey(KeyCode.R) : false;
        is_Standing = (!is_Walking && !is_Running && !is_Jumping) ? true : false;

        float slopeAngle = Vector3.Angle(m_GroundNormal, Vector3.up);
        if (!moveBack && !moveFront && !moveLeft && !moveRight)
            currentMovement = Vector3.zero;
        else if (slopeAngle < 45)
        {
            if (moveFront && !moveBack)
            {
                if (!moveLeft && !moveRight)
                {
                    currentMovement = Vector3.Cross(Vector3.Cross(m_GroundNormal, moveFoward), m_GroundNormal).normalized * speed * ((is_Running) ? runFactor : 1);
                }
                else if (moveLeft && !moveRight)
                {
                    Vector3 left = Vector3.Cross(m_GroundNormal, moveFoward);
                    Vector3 front = Vector3.Cross(left, m_GroundNormal);
                    currentMovement = (front - left).normalized * speed * ((is_Running) ? runFactor : 1);
                }
                else if (moveRight && !moveLeft)
                {
                    Vector3 right = Vector3.Cross(moveFoward, m_GroundNormal);
                    Vector3 front = Vector3.Cross(m_GroundNormal, right);
                    currentMovement = (front - right).normalized * speed * ((is_Running) ? runFactor : 1);
                }
            }
            else if (moveBack && !moveFront)
            {
                if (!moveLeft && !moveRight)
                {
                    currentMovement = -Vector3.Cross(Vector3.Cross(m_GroundNormal, moveFoward), m_GroundNormal).normalized * speed * ((is_Running) ? runFactor : 1);
                }
                else if (moveLeft && !moveRight)
                {
                    Vector3 left = Vector3.Cross(m_GroundNormal, moveFoward);
                    Vector3 back = -Vector3.Cross(left, m_GroundNormal);
                    currentMovement = (-left + back).normalized * speed * ((is_Running) ? runFactor : 1);
                }
                else if (moveRight && !moveLeft)
                {
                    Vector3 right = Vector3.Cross(moveFoward, m_GroundNormal);
                    Vector3 back = -Vector3.Cross(m_GroundNormal, right);
                    currentMovement = (-right + back).normalized * speed * ((is_Running) ? runFactor : 1);
                }
            }
            else if (moveLeft && !moveRight)
            {
                currentMovement = Vector3.Cross(moveFoward, m_GroundNormal).normalized * speed * ((is_Running) ? runFactor : 1);
            }
            else if (moveRight && !moveLeft)
            {
                currentMovement = Vector3.Cross(m_GroundNormal, moveFoward).normalized * speed * ((is_Running) ? runFactor : 1);
            }
        }

        Debug.Log(is_OnGround.ToString());
        Vector3 newForward = new Vector3(currentMovement.x, 0, currentMovement.z);
        this.transform.forward = Vector3.Slerp(this.transform.forward, newForward, Time.deltaTime * 20);

        if (is_Jumping)
        {
            GravityPull = Vector3.zero;
            GravityPull.x = currentMovement.x;
			GravityPull.y = jumpSpeed;
            GravityPull.z = currentMovement.z;
            is_OnGround = false;
            is_Jumping = false;
			GetComponent<Animator> ().SetBool ("Jump", true);
        }
        
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (weaponEquiped) {
				GameObject.Find ("weapon").GetComponent<MeshRenderer> ().enabled = false;
				GameObject.Find ("holster").GetComponent<MeshRenderer> ().enabled = true;;
				weaponEquiped = false;
			} else {

				GameObject.Find ("weapon").GetComponent<MeshRenderer> ().enabled = true;
				GameObject.Find ("holster").GetComponent<MeshRenderer> ().enabled = false;
				weaponEquiped = true;
			}
		}

		if (!is_OnGround) {
            if (GravityPull.y > 0)
				GravityPull.y -= gravity * Time.deltaTime * 0.8f;
			else
				GravityPull.y -= gravity * Time.deltaTime;
		} else {
			GetComponent<Animator> ().SetBool ("Jump", false);
		}
        
		if (is_Walking) {
			GetComponent<Animator> ().SetBool ("Walk", true);
		} else {
			GetComponent<Animator> ().SetBool ("Walk", false);
		}
        
		if (is_Running) {
            GetComponent<Animator> ().SetBool ("Run", true);
		} else {
			GetComponent<Animator> ().SetBool ("Run", false);
		}

        if (is_OnGround)
            rb.velocity = currentMovement;
        else
        {
            Vector3 f = new Vector3(currentMovement.x, GravityPull.y, currentMovement.z);
            rb.velocity = f;
        }
    }

    
    /*void OnCollisionEnter(Collision other)
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
    */

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
			if (hitInfo.collider.gameObject.CompareTag("Ground")) {
				m_GroundNormal = hitInfo.normal;
                is_Jumping = false;
				is_OnGround = true;
                GravityPull.y = 0;
			}
        }
        else
        {
            is_OnGround = false;
            m_GroundNormal = Vector3.up;
        }
    }
}
