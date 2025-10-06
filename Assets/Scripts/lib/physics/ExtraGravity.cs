using UnityEngine;
public class ExtraGravity : MonoBehaviour
{

    public Terrain groundTerrain;
    public bool preventGoingUnderGround = true;
    public bool useRigidBodyTransform = true;
    public float jumpHeight = 0.05f;

    private Transform objectTransform;

    void Start()
    {
        objectTransform = useRigidBodyTransform ? GetComponent<Rigidbody>().transform : transform;
    }

    void FixedUpdate()
    {
        if (preventGoingUnderGround) PreventGoingUnderGround();

    }

    private void PreventGoingUnderGround()
    {
        Vector3 origin = new(objectTransform.position.x, 0f, objectTransform.position.z);
        float terrainHeight = groundTerrain.SampleHeight(origin);

        if (terrainHeight >= objectTransform.position.y)
        {
            objectTransform.position = new Vector3(objectTransform.position.x, terrainHeight + jumpHeight, objectTransform.position.z);
        }
    }

}