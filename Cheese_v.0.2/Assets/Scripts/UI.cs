﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class UI : MonoBehaviour {
	public Text timetext;
	private int min;
	private int seconds;
	private float starttime;
	private float time;
	public Text objectivetruck;
	public int cheesestatus;
	public Texture texturemachine;
	// Use this for initialization
	void Start () {
        //money = 0;
		min = 2;
		seconds = 0;
		timetext.transform.position.Set (32, 133, 0);
		timetext.text = "Next Truck :" + min.ToString () + ":" + seconds.ToString ();
		objectivetruck.text = cheesestatus.ToString () + "/10";
        objectivetruck.text = GrabAndDrop.cheesestatus.ToString() + "/" + GrabAndDrop.cheeseObj.ToString();
        //moneyText.text = money.ToString();


		
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

        if (min == 0 && seconds == 0)
        {
            min = 2;
            GrabAndDrop.cheesestatus = 0;

        }
		timetext.transform.position.Set (32, 133, 0);
        timetext.text = string.Format("{0:00}:{1:00}", min, seconds); 
		//timetext.text = "Next Truck :" + min.ToString () + ":" + seconds.ToString ();
        objectivetruck.text = GrabAndDrop.cheesestatus.ToString() + "/" + GrabAndDrop.cheeseObj.ToString();
        //moneyText.text = money.ToString();
		
	}
}
