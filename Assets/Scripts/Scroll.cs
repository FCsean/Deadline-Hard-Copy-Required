using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scroll : MonoBehaviour {

    public float speed;
    public float lastSpeed;
    public int time;

    private int over = 15;

	public BackgroundScript mountain;
	public BackgroundScript trees;

	private List<BackgroundScript> backgrounds = new List<BackgroundScript>();

    // Use this for initialization
    void Start()
    {
		backgrounds.Add (mountain);
		backgrounds.Add (trees);
    }

    // Update is called once per frame
    void Update()
    {
        if (time < (int)( Time.time / over) && speed != 0) {
            time = (int) (Time.time / over);
            speed += .01f;
        }
    }

    public void StopSpeed()
    {
        lastSpeed = speed;
        speed = 0;
		foreach(var background in backgrounds){
			background.StopSpeed ();
		}
    }

    public void ResumeSpeed()
    {
        speed = lastSpeed;
		foreach(var background in backgrounds){
			background.StopSpeed ();
		}
    }
}
