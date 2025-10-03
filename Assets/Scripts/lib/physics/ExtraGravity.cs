using UnityEngine;
public class ExtraGravity : MonoBehaviour
{
    public bool preventGoingUnderGround = true;
    public float allowedUnderGroundHeight = -0.6f;
    public float defaultUnderGroundHeight = -0.1f;
    public bool useRigidBodyTransform = false;

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
        if (objectTransform.position.y < allowedUnderGroundHeight)
        {
            objectTransform.position = new Vector3(objectTransform.position.x, defaultUnderGroundHeight, objectTransform.position.z);
        }
    }

}