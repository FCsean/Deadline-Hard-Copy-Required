using UnityEngine;
using System.Collections;

public class SlidingObstacleScript : MonoBehaviour {

    public Scroll road;
    public GameObject lastAdded;

    public readonly float MAX_DISTANCE = 15f;
    public readonly float MIN_DISTANCE = 4f;

    // Use this for initialization
    void Start () {
        var y = transform.position.y;
        var x = lastAdded == null ? 12.80f : lastAdded.transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x + Random.value * (MAX_DISTANCE - MIN_DISTANCE) + MIN_DISTANCE;
        transform.position = new Vector3(x, y);
	}
	
	// Update is called once per frame
	void Update () {
        var posi = transform.position;
        transform.position = new Vector2(posi.x - road.speed, posi.y);
	}
}
