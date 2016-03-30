using UnityEngine;
using System.Collections;

public class FrontObstacleScript : MonoBehaviour {

    public Scroll road;
    public GameObject obstacle;
	// Use this for initialization
	void Start () {
        if (!name.Contains("Clone"))
        {
            Destroy(gameObject);
            return;
        }
        transform.position = obstacle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var posi = transform.position;
        transform.position = new Vector2(posi.x - road.speed, posi.y);
        if (posi.x < -12.8 - GetComponent<SpriteRenderer>().bounds.size.x)
        {
            Destroy(gameObject);
        }
    }
}
