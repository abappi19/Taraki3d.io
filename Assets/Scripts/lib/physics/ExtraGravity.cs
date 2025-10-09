using UnityEngine;
public class ExtraGravity : MonoBehaviour
{

    public bool preventGoingUnderGround = true;
    public LayerMask groundLayer;
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

    private float? GetGroundHeight()
    {

        Vector3 origin = new(objectTransform.position.x, 100f, objectTransform.position.z);
        Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 200f, groundLayer);
        if (hit.collider != null)
        {
            return hit.point.y;
        }
        return null;
    }

    private void PreventGoingUnderGround()
    {
        float? terrainHeight = GetGroundHeight();
        if (terrainHeight == null) return;

        if (terrainHeight >= objectTransform.position.y)
        {
            objectTransform.position = new Vector3(objectTransform.position.x, terrainHeight.Value + jumpHeight, objectTransform.position.z);
        }
    }

}