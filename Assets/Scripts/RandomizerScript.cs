using UnityEngine;
using System.Collections;

public class RandomizerScript : MonoBehaviour {

    private Queue obstacles = new Queue();
    private GameObject lastAdded;

    private ArrayList prefabs = new ArrayList();

    public Scroll road;
    public GameObject sliding_construction_prefab;
    public GameObject sliding_highwaysign;

    // Use this for initialization
    void Start () {
        prefabs.Add(sliding_construction_prefab);
        prefabs.Add(sliding_highwaysign);
	}
	
	// Update is called once per frame
	void Update () {
        if (obstacles.Count < 5)
        {
            var randomNum = (int) Random.Range(0, prefabs.Count);
            GameObject sliding_construction = Instantiate((GameObject) prefabs[randomNum]);
            var script = sliding_construction.GetComponent<SlidingObstacleScript>();
            script.road = road;
            script.lastAdded = lastAdded;

            obstacles.Enqueue(sliding_construction);
            lastAdded = sliding_construction;
        }
        if(obstacles.Count != 0)
        {
            var obs = (GameObject) obstacles.Peek();
            if (obs.transform.position.x < -12.8 - obs.GetComponent<SpriteRenderer>().bounds.size.x)
            {
                DestroyImmediate(obs);
                obstacles.Dequeue();
            }
        }
	}

}
