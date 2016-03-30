using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

	public float speed;
	public float lastSpeed;

	public GameObject otherStreet;

	// Use this for initialization
	void Start()
	{
		var posi = transform.position;
		posi.x = 0;
		transform.position = posi;

		otherStreet.transform.position = new Vector2(posi.x + GetComponent<SpriteRenderer>().bounds.size.x, posi.y);
	}

	// Update is called once per frame
	void Update()
	{
		var posi = transform.position;
		transform.position = new Vector2(posi.x - speed, posi.y);

		posi = otherStreet.transform.position;
		otherStreet.transform.position = new Vector2(posi.x - speed, posi.y);

		CheckOutOfBounds(gameObject, otherStreet);
		CheckOutOfBounds(otherStreet, gameObject);
	}

	void CheckOutOfBounds(GameObject first, GameObject second)
	{
		var posi = first.transform.position;

		if (posi.x < 0 - GetComponent<SpriteRenderer>().bounds.size.x)
		{
			posi.x = second.transform.position.x + second.GetComponent<SpriteRenderer>().bounds.size.x;
			first.transform.position = posi;
		}
	}

	public void StopSpeed()
	{
		lastSpeed = speed;
		speed = 0;
	}

	public void ResumeSpeed()
	{
		speed = lastSpeed;
	}
}
