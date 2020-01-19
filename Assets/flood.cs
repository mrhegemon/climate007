using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flood : MonoBehaviour
{

    public Rigidbody rb;

    public GameObject water;

    public float t;

    public bool s;
    // Start is called before the first frame update
    void Start()
    {
        rb = water.GetComponent<Rigidbody>();
        t = 50f;
        s = false;
        //rb.velocity = new Vector3(0,1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (s == true)
        {
            t -= Time.deltaTime;
            if(t < 0)
            {
                rb.velocity = new Vector3(0,0f, 0);
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (s == false)
        {
            rb.velocity = new Vector3(0,.1f, 0);
            //t = Time.time;
            s = true;
        }
       
        //throw new NotImplementedException();
    }
}
