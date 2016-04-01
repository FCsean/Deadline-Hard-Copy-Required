using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionButtonScript : MonoBehaviour {

	public GameObject instructions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1)
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			if (GetComponent<BoxCollider2D>().OverlapPoint(touchPos))
			{
				ShowInstructions();
			}
		}
	}

	void OnMouseUp() {
		ShowInstructions();
	}

	void ShowInstructions() {
		SceneManager.LoadScene ("InstructionScene");
	}

}
