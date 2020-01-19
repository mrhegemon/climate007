using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floater : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject water;
    private float tempVal;
    private float amp;
    private Vector3 tempPos;
    void Start()
    {
        tempPos = transform.position;
        tempVal = water.transform.position.y;
        amp = GetComponent<Renderer>().bounds.size.y * .1f;

    }

    // Update is called once per frame
    void Update()
    {
        tempPos.y = water.transform.position.y + amp * Mathf.Sin(2f * Time.time);
        transform.position = tempPos;
    }
}
