using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetHighScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {	
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowScore(float score) {
		float highscore = PlayerPrefs.GetFloat ("highscore", 0f);
		if (score > highscore)
			highscore = score;
		GetComponent<Text> ().text = "High Score: " + (int)highscore + "\nScore: " + (int)score;
	}

}
