using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject target;
    public List<Vector3> offsets = new List<Vector3> {
        new Vector3(0,8.5f,-13f),
        new Vector3(0,13f,-20.5f),
        new Vector3(0,15,-25f),
    };
    public int currentOffsetIndex = 0;
    private Vector3 lastTargetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offsets.Add(transform.position);
        transform.position = target.transform.position + offsets[currentOffsetIndex];
        lastTargetPosition = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float animationSpeed = 5f;
        if (target != null)
        {
            lastTargetPosition = target.transform.position;
            animationSpeed = 50f;
        }

        Vector3 targetPosition = lastTargetPosition + offsets[currentOffsetIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationSpeed * Time.deltaTime);
    }

    public void SetCurrentOffsetIndex(int index)
    {
        currentOffsetIndex = index;

    }

}
