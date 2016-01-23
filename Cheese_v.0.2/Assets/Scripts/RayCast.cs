using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {

    public GameObject testObject;

    private bool canHover;

	// Use this for initialization
	void Update() {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Linecast(transform.position, fwd, out hit))
        {
            if (hit.distance <= 500 && hit.collider.gameObject.tag == "Usable")
            {
                canHover = true;

                if (Input.GetKey(KeyCode.E))
                {
                    // do stuff...
                }
            }
            else
            {
                canHover = false;
            }
        }
	}
	
	void OnGUI()
    {
        if (canHover)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 150, 60), "Press E to pick up");
        }
    }
}
