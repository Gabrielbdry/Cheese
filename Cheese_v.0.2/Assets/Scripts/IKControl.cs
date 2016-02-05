using UnityEngine;
using System.Collections;
 
public class IKControl : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetKey (KeyCode.LeftShift))
			animator.SetBool ("Carry", true);
		else
			animator.SetBool ("Carry", false);
		if(Input.GetKeyDown(KeyCode.Space))
			animator.SetBool("Jump", false);
*/
	}
}
