using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject target;
    public List<Vector3> offsets = new List<Vector3> {
        new Vector3(0, 1, -2.5f),
        new Vector3(0, 2, -3.5f),
    };
    public int currentOffsetIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsets.Add(transform.position);
        transform.position = target.transform.position + offsets[currentOffsetIndex];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offsets[currentOffsetIndex];
    }
}
