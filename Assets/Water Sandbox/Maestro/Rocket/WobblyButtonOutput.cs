using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class ButtonEvent : UnityEvent<bool> { }

public class WobblyButtonOutput : MonoBehaviour {

	public Vector3 origin;
	Rigidbody rb;

	public Transform buttonTrans;

	public bool buttonDown;
	public bool On;
	public float buttonSensitivity;

	//public int ID;
	//public char SimonSaysLetter;
	//public string EventCaller;

	public float slideForce;

    public static AudioClip down, up;

    public float pitchOffset = 0.0f;

    private AudioSource source;

    public ButtonEvent onStateChanged;
    public UnityEvent onDown, onUp;

	void Awake() {
        if (!down) down = Resources.Load<AudioClip>("Sounds/button_down");
        if (!up) up = Resources.Load<AudioClip>("Sounds/button_up");

        source = this.gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = 0.2f;

		origin = buttonTrans.localPosition;
		rb = buttonTrans.GetComponent<Rigidbody> ();
		rb.velocity = Vector3.zero;
		//Invoke ("AddButtonToManager", ID*0.01f); //We add the buttons to the manager with a timer based on its ID number. By doing this, we ensure that the buttons are reliably added in the same order as their ID.

		/*if (SimonSaysLetter != 'x') { // A 'char' can't be null or empty or anything like that. There you go, something new everyday. I guess if you re-use this system, use a string instead?
			//gameObject.name = "SimonSaysButton (" + SimonSaysLetter + ")";

			if (GetComponentInChildren<LightObject> ()) {
				LightObject l = GetComponentInChildren<LightObject> ();
				l.ID = ID;

				if (SimonSaysLetter != 'x') {
					l.name = "LED " + SimonSaysLetter;
				}
			}
		}*/
	}

	/*void AddButtonToManager() {
        if (InteractiveControlManager.instance != null)
		    InteractiveControlManager.instance.AddWobblyButton (this);
	}*/

	void Update () {
		float y = buttonTrans.localPosition.y;

		if (y < 0) {
			//rb.velocity += new Vector3 (0, slideForce, 0);
			rb.velocity += buttonTrans.up * slideForce;

			if (y < -buttonSensitivity) {
				if (!buttonDown) {
					ButtonPushed ();
				}
			}
		} else {
			if (buttonDown) {
				ButtonReleased ();
			}
			buttonTrans.localPosition = Vector3.zero;
			rb.velocity = Vector3.zero;
		}

		buttonTrans.localPosition = new Vector3 (origin.x, Mathf.Clamp (buttonTrans.localPosition.y, -buttonSensitivity, 0), origin.z);
		//buttonTrans.localPosition = new Vector3(0,Mathf.Clamp(buttonTrans.localPosition.y,-buttonSensitivity,0),0);
	}

	void ButtonPushed() {
		Debug.Log ("Button pushed: " + this.name);

		/*if(LightAndSoundManager.instance.buttonTones[ID] != null){

			LightAndSoundManager.instance.StartClip(8, LightAndSoundManager.instance.buttonTones[ID]);
			Debug.Log(LightAndSoundManager.instance.buttonTones[ID]);
		}*/

		buttonDown = true;

        onDown.Invoke();
        if (onStateChanged != null)
            onStateChanged.Invoke(true);

		On = !On;

        /*if (EventCaller != null && InteractiveControlManager.instance != null && ID >= 0)
        {
            InteractiveControlManager.instance.SendMessage(EventCaller, SendMessageOptions.DontRequireReceiver);


            InteractiveControlManager.instance.WobblyButtonPressed(ID);
        }*/
        //AudioSource.PlayClipAtPoint(down, this.transform.position, volume);

        source.clip = down;
        source.pitch = pitchOffset + Random.Range(0.95f, 1.05f);
        source.Play();
	}

	void ButtonReleased() {
		Debug.Log ("Button released: " + this.name);

		buttonDown = false;
		buttonTrans.localPosition = Vector3.zero;
		rb.velocity = Vector3.zero;

        onUp.Invoke();
        if (onStateChanged != null)
            onStateChanged.Invoke(false);

        //if (InteractiveControlManager.instance != null && ID >= 0)
		//    InteractiveControlManager.instance.WobblyButtonReleased (ID);

        source.clip = up;
        source.pitch = pitchOffset + Random.Range(0.95f, 1.05f);
        source.Play();
        //AudioSource.PlayClipAtPoint(up, this.transform.position, volume);
    }
}