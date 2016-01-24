using UnityEngine;
using System.Collections;

public class GrabAndDrop : MonoBehaviour {
    public static int cheesestatus;
    public static int cheeseObj;
    private GameObject grabbedObject;

    void Start()
    {
        grabbedObject = null;
        cheesestatus = 0;
        cheeseObj = 5;
    }

    GameObject getMouseHoverObject(float range)
    {
        Vector3 position = transform.position;
        RaycastHit hit;
		Vector3 target = transform.forward * range;
		target.y += 1;

		Debug.DrawRay (position, target, Color.green, 5);

		if (Physics.Raycast(position, target, out hit))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    void tryGrabObject(GameObject grabObject)
    {	
		if (grabObject.tag == "Lever") {
			LeverInteraction.Use (grabObject);
			return;
		}

		if (grabObject == null || !CanGrab (grabObject))
			return;
			
		GetComponent<Animator> ().SetBool ("Carry", true);
        
        grabbedObject = grabObject;
    }

    bool CanGrab(GameObject candidate)
    {
        return candidate.CompareTag("PickUp");
    }

    void DropObject()
    {
        if (grabbedObject == null)
            return;
		if (grabbedObject.GetComponent<Rigidbody> () != null) {
			grabbedObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			GetComponent<Animator> ().SetBool ("Carry", false);
			Physics.IgnoreCollision (grabbedObject.GetComponent<Collider>(), this.GetComponent<Collider>(), false);
		}
		Destroy (grabbedObject);
        grabbedObject = null;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (grabbedObject == null)
            {
                GameObject g = getMouseHoverObject(20);
                if (gameObject != null)
                {
                    Debug.Log("Detected");
                    tryGrabObject(g);
                }
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            DropObject();
        }
        else if (grabbedObject != null)
        {
			Physics.IgnoreCollision (grabbedObject.GetComponent<Collider>(), this.GetComponent<Collider>());
			grabbedObject.transform.forward = this.transform.forward;
            Vector3 newPosition = transform.position + transform.forward.normalized * 0.6f;
            grabbedObject.transform.position = new Vector3(newPosition.x, newPosition.y + 0.95f, newPosition.z);
        }
    }
}
