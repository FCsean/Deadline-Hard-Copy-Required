using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitGame : MonoBehaviour {

	public GameObject loading;
	public GameObject running;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		loading.GetComponent<SpriteRenderer> ().enabled = false;
		running.GetComponent<SpriteRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		loading.GetComponent<SpriteRenderer> ().enabled = true;	
		running.GetComponent<SpriteRenderer> ().enabled = true;	
		SceneManager.LoadSceneAsync("MenuScene");
	}
}
