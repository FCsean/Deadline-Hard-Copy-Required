using UnityEngine;
using System.Collections;

public class MuteScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int mute = PlayerPrefs.GetInt ("mute", 0);
		GetComponent<AudioSource> ().mute = mute == 0 ? false : true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.M)) {
			int mute = PlayerPrefs.GetInt ("mute", 0);
			mute = mute == 0 ? 1 : 0;
			PlayerPrefs.SetInt ("mute", mute);
			GetComponent<AudioSource> ().mute = mute == 0 ? false : true;
		}
	}
}
