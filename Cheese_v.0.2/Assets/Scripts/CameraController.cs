using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public PlayerController player;
	public float crouchingHeight = 0.75f;
	public float distance = 5.0f;
	public float minDistance = 0f;
	public float maxDistance = 3.0f;
	public float sensitivity = 3.0f;
	public float boxCheckDistance = 6.0f;

	private Vector3 offset;
	private Vector3 rangeOffset;
	private Vector3 scopeOffset;
	private Vector3 rotTangent;
	private bool collided = false;

	// Use this for initialization
	void Start() {
		rangeOffset = (transform.position - (transform.forward * 2)).normalized;
		offset = rangeOffset;
		transform.position = player.transform.position + rangeOffset * distance;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown (0))
			Debug.DrawRay (this.transform.position, this.transform.forward * 20, Color.blue, 5);
		player.UpdateDirection(transform.forward);
		Quaternion qX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, new Vector3(0, 0.9f, 0));
		Quaternion qY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * sensitivity, Vector3.Cross(new Vector3(transform.forward.x, 0.0f, transform.forward.z), Vector3.up));
		offset = qX * qY * offset;
		offset.Normalize();
		transform.rotation = qX * qY * transform.rotation;
		if (transform.rotation.y < -0.6f)
			transform.rotation.Set(transform.rotation.x, -0.6f, transform.rotation.z, transform.rotation.w);
		else if (transform.rotation.y > 0.6f)
			transform.rotation.Set(transform.rotation.x, 0.6f, transform.rotation.z, transform.rotation.w);

		Vector3 expectedCamPos = player.transform.position + offset * maxDistance;

		RaycastHit hit = new RaycastHit();
		if (Physics.Linecast(player.transform.position, expectedCamPos, out hit)) {
			if (!hit.collider.gameObject.CompareTag("MainCamera") && !hit.collider.gameObject.CompareTag("Player")) {
				expectedCamPos = hit.point;
				if (collided) {
					expectedCamPos.y = transform.position.y;
				}
				collided = true;
			}
		} 
		else
			collided = false;

		transform.position = expectedCamPos;
	}
}
