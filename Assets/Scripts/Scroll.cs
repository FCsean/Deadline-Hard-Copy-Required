using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

    public float speed;
    public float lastSpeed;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
