using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class RayCasting : MonoBehaviour
{
    public float range;
    public float fireRate;
    private Stopwatch watch;
    Transform player;
    void start()
    {
        range = 1000;
        player = Camera.main.transform;
        watch = new Stopwatch();
        watch.Start();
    }

    void update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (watch.Elapsed.Seconds >= 1 / fireRate)
            {
                RaycastHit hit;
                if (Physics.Linecast(player.position, (transform.forward.normalized - player.position.normalized - transform.position.normalized).normalized * range, out hit))
                {
                    if (hit.transform.tag == "Mouse")
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