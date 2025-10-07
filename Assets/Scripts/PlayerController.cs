using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // private Rigidbody headRb;

    //states
    public GameObject playerHead;
    public float moveSpeed = 5f;
    public float rotateSpeed = 750f;
    public GameObject playerBodyPrefab;
    public float bodyGap = 0.01f;
    public float bodyScale = 1f;
    private Vector3 cachedMoveDirection;



    private NormalizedInputDirection NID;
    private MovementTracker movementTracker;
    private List<GameObject> playerBodies;

    private void Awake()
    {
        NID = new NormalizedInputDirection(Vector3.forward, true);
        movementTracker = new MovementTracker(2, 15);
        playerBodies = new List<GameObject>();

        // headRb = playerHead != null ? playerHead.GetComponent<Rigidbody>() : null;


    }


    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            ImproveHealth();
        }

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
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            targetRotation.Normalize();
            Quaternion newRotation = Quaternion.RotateTowards(
              playerHead.transform.rotation,
              targetRotation,
              rotateSpeed * Time.fixedDeltaTime
          );
            playerHead.transform.rotation = newRotation;
        }

        Vector3 forwardMovement = playerHead.transform.forward * moveSpeed * Time.fixedDeltaTime;
        playerHead.transform.position = playerHead.transform.position + forwardMovement;

        movementTracker.InsertMovementPoint(playerHead.transform.position, playerHead.transform.rotation);
    }

    private void UpdateBodyPositions()
    {
        movementTracker.FollowMovement(playerBodies);

        return;

        Vector3 prevPosition = playerHead.transform.position;
        Quaternion prevRotation = playerHead.transform.rotation;
        foreach (var bodyPart in playerBodies)
        {
            Vector3 currentPos = bodyPart.transform.position;
            Quaternion currentRotation = bodyPart.transform.rotation;

            float distance = Vector3.Distance(prevPosition, currentPos);
            if (distance > bodyGap)
            {
                Vector3 direction = (prevPosition - currentPos).normalized;
                currentPos = prevPosition - (direction * bodyGap);
                // currentRotation = prevRotation.normalized;
            }
            bodyPart.transform.position = currentPos;
            prevPosition = currentPos;

            bodyPart.transform.rotation = currentRotation;
            // prevRotation = currentRotation;
        }
    }


    private void ImproveHealth()
    {
        GameObject body = Instantiate(playerBodyPrefab, transform);
        Vector3 lastPlayerBodyPosition = playerBodies.Count == 0 ? playerHead.transform.position : playerBodies.Last().transform.position;

        if (playerBodies.Count < 3)
        {
            Collider collider = body.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }


        body.transform.position = lastPlayerBodyPosition;
        body.transform.localScale = new Vector3(bodyScale, bodyScale, bodyScale);
        playerBodies.Add(body);
    }
    private void DecreaseHealth()
    {

    }
}
