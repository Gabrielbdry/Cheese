using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public PlayerController player;
    public float distance = 10.0f;
    public float sensitivity = 3.0f;

    private Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = (transform.position - player.transform.position).normalized * distance;
        transform.position = player.transform.position + offset;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        player.UpdateDirection(transform.forward);
        Quaternion qX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, Vector3.up);
        Quaternion qY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * sensitivity, Vector3.Cross(new Vector3(transform.forward.x, 0.0f, transform.forward.z), Vector3.up));
        offset = qX * qY * offset;
        transform.rotation = qX * qY * transform.rotation;
        transform.position = player.transform.position + offset;
    }
}
