using UnityEngine;
using System.Collections;

public class SpinningLight : MonoBehaviour {

	public static SpinningLight i;

	public bool Spinning;
	public bool mainLightWasAlreadyOn;

	public void Awake() {
		i = this;
	}

	public void StartSpinning() {
		if (LightAndSoundManager.instance.lights[14].myLED.enabled) {
			mainLightWasAlreadyOn = true;
		} else {
			mainLightWasAlreadyOn = false;
		}

		LightAndSoundManager.instance.CustomizeLight (14, Color.red);
		LightAndSoundManager.instance.TurnLightOn (21);
		LightAndSoundManager.instance.StartFlashing (21,0.25f);
		Spinning = true;
	}

	public void StopSpinning() {
		if (mainLightWasAlreadyOn) {
			LightAndSoundManager.instance.CustomizeLight (14, Color.white);
			LightAndSoundManager.instance.TurnLightOn (14);
		} else {
			LightAndSoundManager.instance.TurnLightOff (14);
			LightAndSoundManager.instance.CustomizeLight (14, new Color(0.25f,0.25f,0.25f,0.75f));
		}

		LightAndSoundManager.instance.StopFlashing (21);
		LightAndSoundManager.instance.TurnLightOff (21);
		Spinning = false;
	}

	void Update() {
		if (Spinning) {
			transform.Rotate (new Vector3 (0, -5.0f, 0));
		}
	}
}