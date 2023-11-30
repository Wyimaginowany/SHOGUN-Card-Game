using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MapObject.MapInstance.GetComponent<MapObject>().ShowMap();
        MapObject.MapInstance.GetComponent<MapObject>().SetCamera(gameObject.GetComponent<Camera>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
