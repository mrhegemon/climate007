//=============================================================================
//
// Purpose: Keeps track of what this fingertip is touching. This script is added automatically; do not place it in your scene.
//
//=============================================================================

using UnityEngine;
using System.Collections;
using Maestro.Haptics.ForceFeedback;
using Maestro.Haptics.Vibration;

public class FingerTipCollider : MonoBehaviour
{

    public MaestroHand hpi; // the parent hand's interaction script.

    public Rigidbody rb; // my rigidbody

    public MaestroInteractable touchtemp;

    public MaestroInteractable touching; // the thing I'm touching currently

    public MaestroInteractable lastTouching;

    public Renderer rend; // the debug renderer for the collider. Is disabled by default.

    public Vector3 colnrm; // the normal of the current collision

    public int index; // which finger am I?

    float timer; //time since I touched something

    [HideInInspector]
    public Collider col; //keep track of my collider so I can turn it off in certain situations

    public Collider releaseCol;

    public bool TriggerTouching = true;

    public PullOnCollideBehaviour pocb;
    public VibrateOnCollideBehaviour vocb;

    public PullOnCollideBehaviour[] pocbs;
    public VibrateOnCollideBehaviour[] vocbs;

    public AudioSource source;

    public Vector3 lastLocation;

    private Color _paintColor = Color.clear;
    public Color PaintColor {
        get { return _paintColor; }
        set
        {
            _paintColor = value;
            rend.material.color = _paintColor == Color.clear ? Color.clear : _paintColor;
        }
    }

    public bool isTip = false;

    private Vector3 netImpulse;
    public Vector3 NetImpulse {
        get {
            Vector3 temp = netImpulse;
            netImpulse = Vector3.zero;
            return temp;
        }
    }

    void Awake()
    {
        netImpulse = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        vocb = GetComponent<VibrateOnCollideBehaviour>();
        pocb = GetComponent<PullOnCollideBehaviour>();

        lastLocation = this.transform.position;

        TriggerTouching = false;
    }

    void OnCollisionExit(Collision c)
    {
        /*if (isvalid(c.collider) && c.collider.GetComponent<MaestroInteractable>().Equals(touching))
        {
            touching = null;
        }*/

        if (c.collider.attachedRigidbody)
        {
            MaestroInteractable mi = c.collider.attachedRigidbody.GetComponent<MaestroInteractable>();

            if (mi != null)
            {
                if (mi.Equals(touching))
                {
                    touching = null;
                    TriggerTouching = false;
                }

                if (c.collider.GetComponentInParent<FingerTipCollider>() == null)
                    mi.Untouch(this);
            }

            /*if (mi != null && c.collider.GetComponentInParent<FingerTipCollider>() == null)
            {
                mi.Untouch(this);
            }*/
        }

        netImpulse += c.impulse;
    }

    public void AddAudioSource()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.5f;
        source.clip = Resources.Load<AudioClip>("Sounds/tap");
    }

    public void SetPhysicMaterial(PhysicMaterial material)
    {
        this.col.material = material;
    }

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("e;alskjdf;la");
        if (c.collider.attachedRigidbody)
        {
            MaestroInteractable mi = c.collider.attachedRigidbody.GetComponent<MaestroInteractable>();

            if (mi != null && c.collider.GetComponentInParent<FingerTipCollider>() == null)
            {
                mi.Touch(this);
                //Debug.Log("TOUCH!");

                //hpi.hc.TriggerHaptics(600);
                timer = 0;
                //time to set the clamp value. The finger just touched the thing we're holding, so we don't want it to bend more.
                //index < 5 because index = 5 means this is the palm. Which doesn't bend.
                /*if (index < 5 && hpi.grabTarget == c.rigidbody)
                {
                    hpi.hc.SetClamp(index, hpi.hc.getposfromindex(index));
                }*/
                //c.rigidbody.GetComponent<MaestroInteractable>().Touch(this); // tell the Interactable it got touched by this finger

                if (!mi.IgnoreTaps)
                {
                   


                    float scale = 1.50f;
                    float helper = Mathf.Max(0.20f, Mathf.Min(1.0f, this.rb.velocity.magnitude * scale));

                    if (vocb)
                    {
                        vocb.PulseEffect = (byte)(128 * source.volume);
                        vocb.pulseHalfLife = 0.3f;
                    }

                    if (vocbs != null && vocbs.Length > 0)
                    {
                        foreach(VibrateOnCollideBehaviour v in vocbs)
                        {
                            if (v != null && source != null)
                            {
                                v.PulseEffect = (byte)(128 * source.volume);
                                v.pulseHalfLife = 0.3f;
                            }
                        }
                    }

                    if (source != null && mi.type == InteractionType.Static/*&& mi.type == InteractionType.Static*/)
                    {
                        source.volume = helper;
                        source.Play();
                    }
                }
            }
        }

        
        /*MeshDeformer md = c.gameObject.GetComponent<MeshDeformer>();
        if (md)
        {
            md.AddDeformingForce(c.contacts[0].point, 10f);
        }*/
    }

    void Update()
    {
        //update the public touching variable and reset touchtemp so it has to be set again by OnCollisionStay during the next frame
        
        if (touchtemp != null)
        {
            touching = touchtemp;
            
            touchtemp = null;
        }

        if (rb)
            lastLocation = this.rb.position;
        else
            lastLocation = this.transform.position;

        //this mess just resets the finger clamping after 0.2 seconds of not touching anything
        if (touching)
        {
            timer = 0;


            lastTouching = touching;
        }
        else
        {
            timer += Time.deltaTime;
            /*if (index < 5 && timer > 0.2f)
            {
                hpi.hc.SetClamp(index, 0);
            }*/
        }

    }

    //this makes our collider, plus a renderer in case we want to debug collisions.
    public void makeRend(float radius)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        col = g.GetComponent<Collider>();

        g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero;
        g.transform.localRotation = Quaternion.identity;
        g.transform.localScale = radius * Vector3.one * 2;
        g.GetComponent<Renderer>().material = (Material)Resources.Load("ContactAccent", typeof(Material));
        rend = g.GetComponent<Renderer>();



        GameObject k = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        releaseCol = k.GetComponent<Collider>();
        releaseCol.isTrigger = true;

        k.transform.parent = transform;
        k.transform.localPosition = Vector3.zero;
        k.transform.localRotation = Quaternion.identity;
        k.transform.localScale = radius * Vector3.one * 4f; //* 6; //4 //* 6;
        k.GetComponent<Renderer>().material = (Material)Resources.Load("ContactAccent", typeof(Material));
        Destroy(k.GetComponent<Renderer>());
        //k.gameObject.SetActive(false);


        //Ignore all collisions with the hand itself
        if (hpi != null)
        {
            foreach (Collider c in hpi.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(releaseCol, c);
                Physics.IgnoreCollision(col, c);
            }
        }
    }

    public void makeRend(Vector3 size)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        col = g.GetComponent<Collider>();
        g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero;
        g.transform.localRotation = Quaternion.identity;
        g.transform.localScale = size;
        g.GetComponent<Renderer>().material = (Material)Resources.Load("colliderDebug", typeof(Material));
        rend = g.GetComponent<Renderer>();


        GameObject k = GameObject.CreatePrimitive(PrimitiveType.Cube);
        releaseCol = k.GetComponent<Collider>();
        releaseCol.isTrigger = true;

        k.transform.parent = transform;
        k.transform.localPosition = Vector3.zero;
        k.transform.localRotation = Quaternion.identity;
        k.transform.localScale = size * 2f;
        k.GetComponent<Renderer>().material = (Material)Resources.Load("ContactAccent", typeof(Material));
        Destroy(k.GetComponent<Renderer>());


        if (hpi != null)
        {
            foreach (Collider c in hpi.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col, c);

                Physics.IgnoreCollision(releaseCol, c);
            }
        }
    }

    //check whether a collider is knuckles Interactable
    bool isvalid(Collider c)
    {
        if (c.attachedRigidbody == null)
        {
            return false;
        }
        else
        {
            return (c.attachedRigidbody.GetComponent<MaestroInteractable>() != null && c.GetComponentInParent<FingerTipCollider>() == null);
        }
    }

    void OnCollisionStay(Collision c)
    {
        if (isvalid(c.collider))
        {
            //colnrm = c.contacts[0].normal;
            MaestroInteractable temp = c.collider.attachedRigidbody.GetComponent<MaestroInteractable>();
            //if (temp.type != InteractionType.Static)
            //{ // make sure we're actually allowed to pick this up
                touchtemp = temp;// say what we're touching currently
                TriggerTouching = true;
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (lastTouching != null && lastTouching.gameObject.Equals(other.gameObject))
        {
            TriggerTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerTouching = false;
        lastTouching = null;
        // TODO does this help?
        if (other.attachedRigidbody)
        {
            MaestroInteractable mi = other.attachedRigidbody.GetComponent<MaestroInteractable>();
            if (mi && mi.Equals(touching))
                touching = null;
        }
        //Debug.Log("EXIT");
    }

    private void OnDisable()
    {
        rend.enabled = false;
    }
}
