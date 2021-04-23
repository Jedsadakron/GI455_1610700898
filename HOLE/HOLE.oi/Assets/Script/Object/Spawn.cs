using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject objects;
    public float speed;

    // scripe for object in prefabs

    //Generate Repeat
    private void Start()
    {
        InvokeRepeating("Generate", 0, speed);
    }

    //Generate object on main camera
    void Generate()
    {
        int x = Random.Range(0, Camera.main.pixelWidth);
        int y = Random.Range(0, Camera.main.pixelHeight);

        Vector3 Target = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
        Target.z = 0;

        Instantiate(objects, Target, Quaternion.identity);
    }

}
