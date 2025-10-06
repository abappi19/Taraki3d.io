using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private NormalizedInputDirection NID = new NormalizedInputDirection(Vector3.forward, true);

    //states
    public GameObject playerHead;
    public float moveSpeed = 5f;
    public float rotateSpeed = 750f;
    public GameObject playerBodyPrefab;
    public float bodyGap = 0.5f;
    public float bodyScale = 1f;
    private Vector3 cachedMoveDirection;



    private List<GameObject> playerBodies = new List<GameObject>();
    private Rigidbody headRb;


    void Start()
    {
        headRb = playerHead != null ? playerHead.GetComponent<Rigidbody>() : null;
        for (int i = 0; i < 1000; i++)
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
              headRb.rotation,
              targetRotation,
              rotateSpeed * Time.fixedDeltaTime
          );
            headRb.MoveRotation(newRotation);
        }

        Vector3 forwardMovement = headRb.transform.forward * moveSpeed * Time.fixedDeltaTime;
        headRb.MovePosition(headRb.position + forwardMovement);
    }

    private void UpdateBodyPositions()
    {

        Vector3 prevPosition = headRb.transform.position;
        Quaternion prevRotation = headRb.transform.rotation;
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
        Vector3 lastPlayerBodyPosition = playerBodies.Count == 0 ? headRb.transform.position : playerBodies.Last().transform.position;



        body.transform.position = lastPlayerBodyPosition;
        body.transform.localScale = new Vector3(bodyScale, bodyScale, bodyScale);
        playerBodies.Add(body);
    }
    private void DecreaseHealth()
    {

    }
}
