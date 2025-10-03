using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collision entered: " + collision.gameObject.name + " with " + collision.gameObject.tag);
    }

}
