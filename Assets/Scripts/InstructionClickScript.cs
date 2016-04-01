using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionClickScript : MonoBehaviour {

	public GameObject instructionButton;
	public GameObject startButton;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseUp() {
		HideInstructions();
	}

	void HideInstructions() {
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sortingOrder = -1;
		instructionButton.GetComponent<BoxCollider2D> ().enabled = true;
		startButton.GetComponent<BoxCollider2D> ().enabled = true;
	}
}
