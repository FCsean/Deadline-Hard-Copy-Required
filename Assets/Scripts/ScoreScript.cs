using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	public Scroll road;

	public SetHighScoreScript show;

	public float score;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		score += road.speed * 10;
		GetComponent<Text> ().text = (int)score + "";
	}

	void OnDestroy() {
		float highscore = PlayerPrefs.GetFloat ("highscore", 0f);
		if (score > highscore)
			PlayerPrefs.SetFloat ("highscore", score);
	}

	public void ShowScore() {
		show.ShowScore (score);
	}

}
