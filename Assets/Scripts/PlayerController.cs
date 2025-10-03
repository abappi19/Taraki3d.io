using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private NormalizedInputDirection NID = new NormalizedInputDirection(Vector3.zero, true);

    //states
    public float moveSpeed = 5f;
    public GameObject playerHead;
    public GameObject playerBodyPrefab;
    public float bodyGap = 0.5f;
    private Vector3 cachedMoveDirection;



    private List<GameObject> playerBodies = new List<GameObject>();
    private Rigidbody headRb;


    void Start()
    {
        headRb = playerHead != null ? playerHead.GetComponent<Rigidbody>() : null;
        // for (int i = 0; i < 1000; i++)
        // {
        //     ImproveHealth();
        // }

    }

    void Update()
    {
        cachedMoveDirection = NID.GetDirection();
    }

    void FixedUpdate()
    {
        FixedTransform(cachedMoveDirection);
    }
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collision entered: " + collision.gameObject.name + " with " + collision.gameObject.tag);
    }

    private void FixedTransform(Vector3 moveDirection)
    {

        UpdateHeadPosition(moveDirection);
        // AddForce();
        UpdateBodyPositions();

    }

    private void UpdateHeadPosition(Vector3 moveDirection)
    {
        headRb.transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
        if (moveDirection != Vector3.zero)
        {
            // headRb.transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void UpdateBodyPositions()
    {

        Vector3 prevPosition = headRb.transform.position;
        Quaternion prevRotation = headRb.transform.rotation;
        foreach (var bodyPart in playerBodies)
        {
            Vector3 currentPos = bodyPart.transform.position;
            float distance = Vector3.Distance(prevPosition, currentPos);

            if (distance > bodyGap)
            {
                Vector3 direction = (prevPosition - currentPos).normalized;
                currentPos = prevPosition - (direction * bodyGap);
            }
            bodyPart.transform.position = currentPos;
            prevPosition = currentPos;

            Quaternion currentRotation = bodyPart.transform.rotation;
            bodyPart.transform.rotation = prevRotation;
            prevRotation = currentRotation;
        }
    }


    private void ImproveHealth()
    {
        GameObject body = Instantiate(playerBodyPrefab, transform);
        Vector3 lastPlayerBodyPosition = playerBodies.Count == 0 ? headRb.transform.position : playerBodies.Last().transform.position;
        body.transform.position = lastPlayerBodyPosition;
        playerBodies.Add(body);
    }
    private void DecreaseHealth()
    {

    }

    private void AddForce()
    {
        Vector3 origin = headRb != null ? headRb.position : transform.position;
        bool grounded = Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f) && hit.collider is TerrainCollider;
        Debug.Log(
            "Grounded: "
        + grounded
        + " is Tarrain: "
        + hit.collider is TerrainCollider
        );

        // if (!grounded)
        // {
        //     headRb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        // }
    }

}
