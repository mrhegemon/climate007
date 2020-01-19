using UnityEngine;
using System.Collections;

public class AudioSpeaker : MonoBehaviour {
	public int ID;
	public AudioSource Speaker;
	int timedDelay;
	bool looping;


	//this script listens for these commands from light and sound manager 
	//this gives us the ability to control audio from a bunch of different points

	void Awake() {
		Speaker = GetComponent<AudioSource>(); 
		Invoke ("AddAudioToManager", ID*0.01f); //We add the buttons to the manager with a timer based on its ID number. By doing this, we ensure that the buttons are reliably added in the same order as their ID.
	}

	void AddAudioToManager() {
		LightAndSoundManager.instance.AddAudioSpeaker (this);
	}

	public void PlayClip(AudioClip clipToPlay){
		Speaker.clip = clipToPlay;
		Speaker.Play();
	}

	public void PlayClipRepeating(AudioClip clipToPlay) {
		Speaker.clip = clipToPlay;
		Speaker.Play();
		Speaker.loop = true;
	}

	public void StopClip(){
		Speaker.Stop();
	}

	public void StartClipLoop(AudioClip clipToPlay, int delay){
		Speaker.clip = clipToPlay;
		timedDelay = delay;
		looping = true;
		ClipLoop();
	}

	public void ClipLoop(){
		if(looping){
			Speaker.Play();
			Invoke("ClipLoop",timedDelay);
		}
	}

	public void StopClipLoop(){
		looping=false;
	}
}
