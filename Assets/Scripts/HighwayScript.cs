using UnityEngine;
using System.Collections;

public class HighwayScript : MonoBehaviour {

    public HighwayRandomizer randomizer;

    // Use this for initialization
    void Start()
    {
        if (!name.Contains("Clone"))
        {
            Destroy(gameObject);
            return;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        var posi = transform.position;
        transform.position = new Vector2(posi.x - randomizer.speed, posi.y);
    }
}
