using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseUp() {
		LoadGame ();
	}


	void LoadGame() {
			SceneManager.LoadScene("GameScene");
	}
}
