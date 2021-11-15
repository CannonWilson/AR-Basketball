using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject planePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(ballPrefab, new Vector3(0,5,0), Quaternion.identity);
        Instantiate(planePrefab, new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
