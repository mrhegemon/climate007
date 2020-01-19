using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class headUnderwater : MonoBehaviour
{
    public GameObject water;
    public GameObject pp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < water.transform.position.y )
        {
            pp.GetComponent<PostProcessVolume>().enabled = true;
        }
        else
        {
            pp.GetComponent<PostProcessVolume>().enabled = false;
        }
    }
}
