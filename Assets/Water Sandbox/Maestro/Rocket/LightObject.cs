using UnityEngine;
using System.Collections;

public class LightObject : MonoBehaviour {

	public int ID;
	public Light myLED;
	public Material myMat;
	IEnumerator flashing;

	void Awake() {
		flashing = Flashing (0.5f);
	}

	void Start() {
		myLED = GetComponentInChildren<Light> ();
		myMat = GetComponent<MeshRenderer> ().material;
		//Invoke ("AddLightToManager", ID * 0.01f);
		AddLightToManager ();
	}

	void AddLightToManager() {
		//LightAndSoundManager.instance.AddLightObject (this, ID);
		LightAndSoundManager.instance.lights [ID] = this;
	}

	public void StartFlashing(float d) {
		flashing = Flashing (d);
		StartCoroutine (flashing);
	}

	public void StopFlashing() {
		StopCoroutine (flashing);
	}

	IEnumerator Flashing(float delay) {
		yield return new WaitForSeconds(0);

		if (myLED.enabled) {
			myMat.color = new Color (0.25f, 0.25f, 0.25f, 0.75f);
			myLED.enabled = false;
		} else {
			myMat.color = new Color (myLED.color.r, myLED.color.g, myLED.color.b, 0.75f);
			myLED.enabled = true;
		}

		yield return new WaitForSeconds (delay);
		StartFlashing (delay);
		yield break;
	}
}