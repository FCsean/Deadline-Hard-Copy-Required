﻿using UnityEngine;
using System.Collections;

public class SlidingObstacleScript : MonoBehaviour {

    public Scroll road;
    public GameObject lastAdded;

    public readonly float MAX_DISTANCE = 15f;
    public readonly float MIN_DISTANCE = 4f;

    // Use this for initialization
    void Start () {
        if (!name.Contains("Clone")) {
            Destroy(gameObject);
            return;
        }
        if (name.Contains("highway"))
        {
            GetComponent<Animator>().speed = 1 / 6f;
        }
        var y = transform.position.y;
        var x = lastAdded == null ? 12.80f : lastAdded.transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x + Random.value * (MAX_DISTANCE - MIN_DISTANCE) + MIN_DISTANCE;
        transform.position = new Vector3(x, y);
		int mute = PlayerPrefs.GetInt ("mute", 0);
		var audio = GetComponent<AudioSource> ();
		if (audio != null) {
			audio.mute = mute == 0 ? false : true;
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {
        var posi = transform.position;
        transform.position = new Vector2(posi.x - road.speed, posi.y);
	}

	void OnBecameVisible() {
		var audio = GetComponent<AudioSource> ();
		if (audio != null) {
			audio.Play ();
		}
	}

	void OnBecameInvisible() {
		var audio = GetComponent<AudioSource> ();
		if (audio != null) {
			audio.Pause ();
		}
	}
}
