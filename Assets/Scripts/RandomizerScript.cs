using UnityEngine;
using System.Collections;

public class RandomizerScript : MonoBehaviour {

    private Queue obstacles = new Queue();
    private GameObject lastAdded;

    private ArrayList prefabs = new ArrayList();

    public Scroll road;
    public GameObject sliding_construction_prefab;
    public GameObject sliding_highwaysign_prefab;
    public GameObject jumping_manhole_prefab;
    public GameObject jumping_puddle_prefab;
    public GameObject umbrella_rain_prefab;
    public GameObject sliding_jeep_prefab;
    public GameObject umbrella_hydrant_prefab;

    public GameObject highway_front_prefab;
    public GameObject construction_front_prefab;

    // Use this for initialization
    void Start () {
        prefabs.Add(sliding_construction_prefab);
        prefabs.Add(sliding_highwaysign_prefab);
        prefabs.Add(jumping_manhole_prefab);
        prefabs.Add(jumping_puddle_prefab);
        prefabs.Add(umbrella_rain_prefab);
        prefabs.Add(sliding_jeep_prefab);
        prefabs.Add(umbrella_hydrant_prefab);
    }

    // Update is called once per frame
    void Update () {
        if (obstacles.Count < 5)
        {
            var randomNum = (int) Random.Range(0, prefabs.Count);
            GameObject obstacle = Instantiate((GameObject) prefabs[randomNum]);
            var script = obstacle.GetComponent<SlidingObstacleScript>();
            script.road = road;
            script.lastAdded = lastAdded;

            obstacles.Enqueue(obstacle);
            lastAdded = obstacle;
            if (obstacle.name.Contains(sliding_construction_prefab.name))
            {
                var front = Instantiate(construction_front_prefab);
                front.GetComponent<FrontObstacleScript>().obstacle = obstacle;
                front.GetComponent<FrontObstacleScript>().road = road;
            } else if (obstacle.name.Contains(sliding_highwaysign_prefab.name))
            {
                var front = Instantiate(highway_front_prefab);
                front.GetComponent<FrontObstacleScript>().obstacle = obstacle;
                front.GetComponent<FrontObstacleScript>().road = road;
            }
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
