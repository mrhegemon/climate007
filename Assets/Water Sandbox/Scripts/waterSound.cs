using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource onEnter;
    public AudioSource onExit;
    public AudioSource onStay;
    public int i;
    void Start()
    {
        i = 0;
        onStay = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        i = 1;
        Debug.Log("DsaDA" + other.tag);
        if (other.tag == "water")

        {
            i = 1;

            onEnter.Play(0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "water")
        {
            i = 2;
            onExit.Play(0);
        }
    }
}
