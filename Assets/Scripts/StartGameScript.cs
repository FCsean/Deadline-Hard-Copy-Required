﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameScript : MonoBehaviour {

	public GameObject loading;
	public GameObject running;

	// Use this for initialization
	void Start () {
		loading.GetComponent<SpriteRenderer> ().enabled = false;
		running.GetComponent<SpriteRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseUp() {
		LoadGame ();
	}


	void LoadGame() {
		loading.GetComponent<SpriteRenderer> ().enabled = true;	
		running.GetComponent<SpriteRenderer> ().enabled = true;	
		SceneManager.LoadSceneAsync("GameScene");
	}
}
