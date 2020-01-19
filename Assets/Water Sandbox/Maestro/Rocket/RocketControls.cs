using UnityEngine;
using System.Collections;

public class RocketControls : MonoBehaviour {

	public enum Axis {X,Z}
	public Axis axis;

	public float joystickOutput;
	public float xOutput;
	public float zOutput;

	public float tiltRate;
	public float rocketSpeed;

	public Transform xJoystick;
	public Transform zJoystick;
	public Transform rocketTrans;

	void Update () {
		AssessJoystickAngles();

		GradualTilt();

		//MoveRocket ();
	}

	void AssessJoystickAngles() {
		xJoystick.localPosition = Vector3.zero;
		zJoystick.localPosition = Vector3.zero;

		xJoystick.localEulerAngles = new Vector3 (Mathf.Clamp (xJoystick.localEulerAngles.x, 45f, 135f), xJoystick.localEulerAngles.y, xJoystick.localEulerAngles.z);
		xOutput = 90-xJoystick.localEulerAngles.x;

		zJoystick.localEulerAngles = new Vector3 (0, 0, Mathf.Clamp (zJoystick.localEulerAngles.z, 45f, 135f));
		zOutput = 90-zJoystick.localEulerAngles.z;
	}

	void GradualTilt() {

	}

	void MoveRocket() {
		rocketTrans.position += rocketTrans.up * rocketSpeed;

		rocketTrans.eulerAngles += new Vector3 (xOutput / 10, 0, zOutput / 10);
	}
}

