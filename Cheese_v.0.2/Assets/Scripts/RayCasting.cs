using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class RayCasting : MonoBehaviour
{
    public float range;
	public float fireRate;
	public Transform player;
    private Stopwatch watch;
    void Start()
    {
        range = 1000;
        watch = new Stopwatch();
        watch.Start();
    }

    void Update()
    {
		if (Input.GetMouseButtonDown (0) && PlayerController.currWeapon == 0);
        {
            if (watch.Elapsed.Seconds >= 1 / fireRate)
            {
                RaycastHit hit;
				if (Physics.Linecast(player.position, player.forward.normalized * range, out hit))
                {
                    if (hit.transform.tag == "Mob")
                    {	
						Destroy(hit.transform.gameObject);
                    }

                }
            }
            watch.Reset();
            watch.Start();
        }
    }



}