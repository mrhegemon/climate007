using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velocityWater : MonoBehaviour
{
    // Start is called before the first frame update
    MaestroInteractable m;
   public GameObject hand;
  //  Rigidbody rb;
    public float speed;
    private Vector3 prev;
    public GameObject water;
    public int vibrationEffect;
    void Start()
    {
       m = this.GetComponent<MaestroInteractable>();
        // rb = hand.GetComponent<Rigidbody>();
        // speed = rb.velocity.magnitude;
        prev = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
        speed = (hand.transform.position - prev).magnitude / Time.deltaTime;
        if (hand.transform.position.y > water.transform.position.y)
        {
            m.VibrationEffect = 0;
            vibrationEffect = 0;
        }
        else
        {
            if (speed > .5f)
            {
                m.VibrationEffect = 6;
                vibrationEffect = 6;
            }
            else
            {
                m.VibrationEffect = 9;
                vibrationEffect = 9;
            }
        }
           
        prev = hand.transform.position;
     
    }
}
