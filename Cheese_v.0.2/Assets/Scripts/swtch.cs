using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class swtch : MonoBehaviour {
	
	public GameObject ImageOnPanel;  ///set this in the inspector
	public Texture NewTexture;
	private RawImage img;
	
	void Start () {
		img = (RawImage)ImageOnPanel.GetComponent<RawImage>();
		

	}
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.U)) {
			img.texture = (Texture)NewTexture;
		}



	}
}