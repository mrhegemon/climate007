using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveControlManager : MonoBehaviour {

	public static InteractiveControlManager instance;
	//public List<UIWand> controllerList;

	public List<WobblyButtonOutput> wobblyButtons;
	public List<LeverOutput> levers;
	public List<SliderOutput> sliders;

	public bool IgnitionOne;
	public bool IgnitionTwo;
	public GameObject weights;
	public GameObject rocket;

	//garage door management
	public Animator garageDoorAnimator;
	public bool doorUp;
	public bool doorMoving;

	/* Button ID Index
	 * 0-11 - SimonSays Keyboard
	 * 12 - Ignition 1
	 * 13 - Ignition 2
	 *
	 */

	/* Lever ID Index
	* 0 - Main Lights
	* 1 - Thrusters
	*
	*/

	void Awake() {
		instance = this;
	}

	void Update() {
//		if (Input.GetKeyDown(KeyCode.Y)){
//			GameObject go = (GameObject)Instantiate(weights);
//			go.transform.SetParent(rocket.transform, false);
//		}
	}

	public void AddWobblyButton(WobblyButtonOutput button) {
		wobblyButtons.Add (button);
	}

	public void AddLever(LeverOutput lever) {
		levers.Add (lever);
	}

	public void AddSlider(SliderOutput slider) {
		sliders.Add (slider);
	}

	public void WobblyButtonPressed(int ID) {
		switch (ID) {
		//case 12:
			//if (SequenceManager.instance.chapter == 4) {
			//	SequenceManager.instance.NextChapter ();
			//}
		//break;
		case 13:
			//if (SequenceManager.instance.chapter == 5) {
			//	SequenceManager.instance.NextChapter ();
			//}
		break;
		case 15:
			if(!doorMoving) {
				doorMoving = true;

				if (doorUp) {
					garageDoorAnimator.SetTrigger ("DoorDown");
					Invoke ("DoorIsDown", 4.0f);

					LightAndSoundManager.instance.CustomizeLight (22, Color.green);
					LightAndSoundManager.instance.StartFlashing (22,0.5f);
				} else {
					garageDoorAnimator.SetTrigger ("DoorUp");
					Invoke ("DoorIsUp", 4.0f);

					LightAndSoundManager.instance.CustomizeLight (22, Color.red);
					LightAndSoundManager.instance.StartFlashing (22,0.5f);
				}
			}
		break;
		default: // Any case that isn't specifically listed above (i.e. SimonSays) will do the following...
                 //SimonSays.instance.ButtonPressed (wobblyButtons [ID].SimonSaysLetter.ToString ());
                 //SimonSays2.instance.TryLetter (wobblyButtons [ID].SimonSaysLetter);
                if (LightAndSoundManager.instance && ID >= 0)
                {
                    LightAndSoundManager.instance.TurnLightOn(ID);
                    LightAndSoundManager.instance.ChangeBulbColor(ID, LightAndSoundManager.instance.lights[ID].myLED.color);
                }
			break;
		}
	}

	public void WobblyButtonReleased(int ID) {
		switch (ID) {
		//case 12:
			//IgnitionOne = false;
		//	break;
		case 13:
			//IgnitionTwo = false;
			break;
		case 15:
			// This prevents the GarageDoorButton from turning off the SimonSays Light. I know it sounds weird, but having it this way allowed us to reduce all SimonSays buttons to the below line of code.
			break;
		default:
			LightAndSoundManager.instance.TurnLightOff (ID);
			LightAndSoundManager.instance.ChangeBulbColor (ID, new Color(0.25f,0.25f,0.25f));
			break;
		}
	}

	public void LeverOn(int ID) {
		switch (ID) {
		case 0: //Main Lights
			//if (SequenceManager.instance.chapter == 0) {
			//	SequenceManager.instance.NextChapter ();
			//}

			//UIManager.instance.EditElement (0, "ON");
			//UIManager.instance.ColorElement (0, 0);

			//LightAndSoundManager.instance.CustomizeLight (14, UIManager.instance.colors[9]);

			LightAndSoundManager.instance.TurnLightOn (14);

			LightAndSoundManager.instance.CustomizeLight (23, Color.green);
			LightAndSoundManager.instance.TurnLightOn (23);
			break;
		}
	}

	public void LeverOff(int ID) {
		switch (ID) {
		case 0: //Main lights
			//UIManager.instance.EditElement (0, "OFF");
			//UIManager.instance.ColorElement (0, 1);

			LightAndSoundManager.instance.CustomizeLight (14, new Color(0.25f,0.25f,0.25f));
			LightAndSoundManager.instance.TurnLightOff (14);

			LightAndSoundManager.instance.CustomizeLight (23, Color.red);
			break;
		case 1: //Thruster Lever
			//if (SequenceManager.instance.chapter >= 7) {
			//	RocketManager2.instance.TurnThrustersOff ();
			//}

//			if (SequenceManager.instance.initialLaunchButton1 && SequenceManager.instance.initialLaunchButton2) {
//				if (RocketManager.instance.rocketState != RocketManager.RocketState.Launch) {
//					RocketManager.instance.TurnThrustersOff ();
//				}
//			}
			break;
		}
	}

	/*public void SliderOn(int ID) {
		switch (ID) {
		case 0: //Thrusters
			if (SequenceManager.instance.chapter >= 7) {
				RocketManager2.instance.TurnThrustersOn ();
			} else {
				if (SequenceManager.instance.chapter == 6) {
					SequenceManager.instance.NextChapter ();
				}
			}
			break;
		}
	}*/

	/*public void SliderOff(int ID) {
		switch (ID) {
		case 0: //Thrusters
			if (SequenceManager.instance.chapter >= 7) {
				RocketManager2.instance.TurnThrustersOff ();
			}
			break;
		}
	}*/

	void DoorIsUp() {
		doorMoving = false;
		doorUp = true;
		LightAndSoundManager.instance.StopFlashing (22);
		LightAndSoundManager.instance.TurnLightOn (22);
	}

	void DoorIsDown() {
		doorMoving = false;
		doorUp = false;
		LightAndSoundManager.instance.StopFlashing (22);
		LightAndSoundManager.instance.TurnLightOn (22);
	}
}