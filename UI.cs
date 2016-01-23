using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class UI : MonoBehaviour {
	//public Text timetext = Gameobject.Find("Text").getComponent<Text>();
	public Text timetext;
	public RawImage enveloppeuse;
	public RawImage encaisseuse;
	private int min;
	private int seconds;
	private int fraction;
	private float starttime;
	private float time;
	public Text objectivetruck;
	public int cheesestatus;
	public Texture2D envelop;
	// Use this for initialization
	void Start () {
		starttime = Time.time;
		min = 2;
		seconds = 0;
		timetext.rectTransform.position.Set (32, 133, 0);
		timetext.text = "Next Truck :" + min.ToString () + ":" + seconds.ToString ();
		objectivetruck.text = cheesestatus.ToString () + "/10";
	


	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= 1) {
			if(seconds != 0)
				seconds -= 1;
			else {
				seconds = 59;
				min -= 1;

			}

			time -= 1;
		}
		timetext.rectTransform.position.Set (32, 133, 0);
		timetext.text = "Next Truck :" + min.ToString () + ":" + seconds.ToString ();

	}
}
