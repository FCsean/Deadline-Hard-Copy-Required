using UnityEngine;
using System.Collections;

public class HighwayRandomizer : MonoBehaviour {

    public GameObject highwayAdPrefab;
    public GameObject highwayAdPrefab1;
    public GameObject highwayAdPrefab2;

    public float speed = 0.05f;
    public float lastSpeed;

    private ArrayList prefabs = new ArrayList();

    private GameObject current;

    // Use this for initialization
    void Start () {
        prefabs.Add(highwayAdPrefab);
        prefabs.Add(highwayAdPrefab1);
        prefabs.Add(highwayAdPrefab2);
    }

    // Update is called once per frame
    void Update () {
	
        if (current == null)
        {
            var randomNum = (int)Random.Range(0, prefabs.Count);
            current = Instantiate((GameObject)prefabs[randomNum]);
            current.GetComponent<HighwayScript>().randomizer = this;
        }
        else
        {
            if (current.transform.position.x < -12 - current.GetComponent<SpriteRenderer>().bounds.size.x)
            {
                Destroy(current);
                current = null;
            }
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
