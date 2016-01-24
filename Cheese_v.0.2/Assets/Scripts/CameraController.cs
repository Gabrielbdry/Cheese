using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public PlayerController player;
    public float crouchingHeight = 0.75f;
    public float distance = 10.0f;
    public float sensitivity = 3.0f;
    public float boxCheckDistance = 6.0f;

    private Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = (transform.position - player.transform.position).normalized * distance;
		offset.y += 3;
        transform.position = player.transform.position + offset;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        player.UpdateDirection(transform.forward);
        Quaternion qX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, Vector3.up);
        Quaternion qY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * sensitivity, Vector3.Cross(new Vector3(transform.forward.x, 0.0f, transform.forward.z), Vector3.up));
        offset = qX * qY * offset;
        transform.rotation = qX * qY * transform.rotation;

        if (!player.isCrouching())
            transform.position = player.transform.position + offset;
        else
            transform.position = player.transform.position + offset + new Vector3(0, -crouchingHeight, 0);
    }
}
