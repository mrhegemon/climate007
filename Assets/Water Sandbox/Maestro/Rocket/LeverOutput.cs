using UnityEngine;
using System.Collections;

public class LeverOutput : MonoBehaviour {

	Transform joystick;
	Rigidbody rb;
	Vector3 originAnchor;

	public int ID;
	public float angle;
	public float range;
	public bool BoolMode;
	public bool On;

	void Awake () {
		rb = GetComponentInChildren<Rigidbody> ();
		joystick = GetComponentInChildren<HingeJoint> ().transform;
		originAnchor = joystick.localPosition;
		HandleBoolMode ();
		Invoke ("AddLeverToManager", ID*0.01f);
	}

	void AddLeverToManager() {
		InteractiveControlManager.instance.AddLever (this);
	}

	void Update () {
		rb.angularVelocity = Vector3.zero;
		AssessAngle ();

		if (BoolMode) {
			HandleBoolMode ();
		}
	}

	void AssessAngle() {
		joystick.localPosition = originAnchor;
		joystick.localEulerAngles = new Vector3 (0, 0, Mathf.Clamp (joystick.localEulerAngles.z, 90-range, 90+range));

		angle = (90-joystick.localEulerAngles.z) / range;

		if (angle < 0.025f && angle > -0.025f) {
			angle = 0;
		}
	}

	void HandleBoolMode() {
		if (angle < -0.25f) {
			if (On) {
				SwitchedOff ();
			}
		} else if (angle > 0.25f) {
			if (!On) {
				SwitchedOn ();
			}
		}
	}

	void SwitchedOn() {
		On = true;
		Debug.Log ("Lever "+ID+" switched on!");
		InteractiveControlManager.instance.LeverOn (ID);
	}

	void SwitchedOff() {
		On = false;
		Debug.Log ("Lever "+ID+" switched off!");
		InteractiveControlManager.instance.LeverOff (ID);		
	}
}
