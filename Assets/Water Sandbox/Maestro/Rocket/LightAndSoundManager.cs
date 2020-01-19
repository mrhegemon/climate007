using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class LightAndSoundManager : MonoBehaviour {

	public static LightAndSoundManager instance;

	public List<LightObject> lights;
	public List<AudioSpeaker> audioSpeaker;
	//public List<MonitorGlow> monitorScreens;

	public List<Animator> animator;

	public AudioClip[] buttonTones;

	public AudioClip[] klaxons;


	public Color[] colors;
	//public Color[] colorToEmit; //array for use setting lighting colors
	//public Color[] bulbColor;

	/* LED ID Index
	 * 0-11 - SimonSays Keyboard
	 * 12 - Ignition 1
	 * 13 - Ignition 2
	 * 14 - Main Roof Lights
	 * 15 - SimonSays All Clear
	 * 16 - Flight Controls All Clear
	 * 17 - Fuel Barrel All Clear
	 * 18 - Jettison All Clear
	 * 19 - Thrusters Off
	 * 20 - Thrusters On
	 * 21 - Emergency!
	 */

	void Awake() {
		instance = this;
	}

	void Start() {
		foreach (LightObject l in lights) {
			if (l != null) {
                if (l.ID == 14)
                {
                    l.myLED.enabled = true;
                }
                else
                {
                    l.myLED.enabled = false;
                }
			}
		}


	}

	//list of lighting objects (allows for targeting specific light objects individually)
	public void AddLightObject(LightObject lightToAdd, int ID) {
		lights.Insert(ID, lightToAdd);
		lightToAdd.myLED.enabled = false;
	}

	//list of audio objects (allows for targeting specific audio objects individually)
	public void AddAudioSpeaker(AudioSpeaker audioToAdd) {
		audioSpeaker.Add(audioToAdd);
	}

/*	public void AddMonitor(MonitorGlow monitor) {
		monitorScreens.Add (monitor);
	}*/


	//Controls for lighting, Sequencer/tasks can pass function and command to access specific lights
	public void TurnLightOn(int ID) {
        if (lights[ID])
		    lights [ID].myLED.enabled = true;
	}

	public void TurnLightOff(int ID) {
		lights [ID].myLED.enabled = false;
	}

	public void ChangeIntensity(int ID, float i) {
		lights [ID].myLED.intensity = i;
	}

	public void ChangeColor(int ID, Color c) {
		lights [ID].myLED.color = c;
	}

	public void ChangeBulbColor(int ID, Color c) {
		c.a = 0.75f;
		lights[ID].myMat.color = c;
	}

	public void CustomizeLight(int ID, Color c){
		ChangeBulbColor (ID, c);
		ChangeColor (ID, c);
	}

	public void CustomizeLight(int ID, Color c, float i){
		TurnLightOn (ID);
		ChangeBulbColor (ID, c);
		ChangeColor (ID, c);
		ChangeIntensity(ID,i);
	}

	public void ChangeBulbColor(int ID, int cID){
		ChangeBulbColor (ID, colors [cID]);
	}

	public void StartFlashing(int ID, float delay){
		lights[ID].StartFlashing(delay);
	}
	public void StopFlashing(int ID){
		lights[ID].StopFlashing();
	}

	//controls for Audio, call function with ID to target specific sound source
	public void StartClip(int ID, AudioClip clipToPlay){

		audioSpeaker[ID].PlayClip(clipToPlay);
	}

	public void StartClipRepeating(int ID, AudioClip clipToPlay){

		audioSpeaker[ID].PlayClipRepeating(clipToPlay);
	}

	public void StopClip(int ID){
		audioSpeaker[ID].StopClip();
	}


	public void StartClipLoop(int ID, AudioClip clipToPlay, int delay){
		audioSpeaker[ID].StartClipLoop(clipToPlay, delay);
	}
	public void StopClipLoop(int ID){
		audioSpeaker[ID].StopClipLoop();
	}

	public void SendTrigger(int ID, string trigger){

		animator[ID].SetTrigger(trigger);

	}
/*

	public IEnumerator ChangeMonitorGlow(int ID, Color a, Color b, float speed, string type ){

		float c = 0;
		//Debug.Log(c);

		while (c < 1){

			switch(type){

			case "material":

				Material mat = monitorScreens[ID].GetComponent<Renderer>().material;
				mat.SetColor("_EmissionColor",Color.Lerp(a, b, c));

			break;

			case "image":

				Image screen = monitorScreens[ID].GetComponent<Image>();
				screen.color = Color.Lerp(a,b,c);

			break;

			case "sprite":
				SpriteRenderer spriteImage = monitorScreens[ID].GetComponent<SpriteRenderer>();
				spriteImage.color = Color.Lerp(a,b,c);
			break;
			}

			yield return new WaitForSeconds(speed);
			c = c + 0.01f;

		}

		yield return null;

	}*/
}
