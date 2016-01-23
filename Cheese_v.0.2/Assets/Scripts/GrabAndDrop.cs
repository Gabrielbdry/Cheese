using UnityEngine;
using System.Collections;

public class GrabAndDrop : MonoBehaviour {

    private GameObject grabbedObject;

    void Start()
    {
        grabbedObject = null;
    }

    GameObject getMouseHoverObject(float range)
    {
        Vector3 position = gameObject.transform.position;
        RaycastHit hit;
        Vector3 target = position + Camera.main.transform.forward * range;

        if (Physics.Linecast(position, target, out hit))
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

        if (grabObject == null || !CanGrab(grabObject))
            return;
        
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

        if (grabbedObject.GetComponent<Rigidbody>() != null)
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        grabbedObject = null;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (grabbedObject == null)
            {
                tryGrabObject(getMouseHoverObject(20));
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            DropObject();
        }
        else if (grabbedObject != null)
        {
            Vector3 newPosition = gameObject.GetComponent<CameraController>().player.transform.position + Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * 1.5f;
            grabbedObject.transform.position = newPosition;
        }
    }
}
