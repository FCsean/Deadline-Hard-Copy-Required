using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionButtonScript : MonoBehaviour {

	public GameObject instructions;
	public GameObject startButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown() {
		ShowInstructions();
	}

	void ShowInstructions() {
		instructions.GetComponent<SpriteRenderer> ().enabled = true;
		instructions.GetComponent<BoxCollider2D> ().enabled = true;
		instructions.GetComponent<SpriteRenderer> ().sortingOrder = 3;
		GetComponent<BoxCollider2D> ().enabled = false;
		startButton.GetComponent<BoxCollider2D> ().enabled = false;
	}

}
