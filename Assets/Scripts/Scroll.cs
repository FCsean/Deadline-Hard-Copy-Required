using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

    public float speed;
    public float lastSpeed;
    public int time;

    private int over = 15;

    // Use this for initialization
    void Start()
    {
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
    }

    public void ResumeSpeed()
    {
        speed = lastSpeed;
    }
}
