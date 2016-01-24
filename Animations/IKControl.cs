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
		if(Input.GetKeyDown(KeyCode.C))
			animator.SetBool("Carry", true);
		if(Input.GetKeyDown(KeyCode.Space))
			animator.SetTrigger("Jump");
	}
}
