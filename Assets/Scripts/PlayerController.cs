using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // private Rigidbody headRb;
    public bool isPlayer = false;

    //states
    public GameObject playerHead;
    public float moveSpeed = 5f;
    public float rotateSpeed = 750f;
    public GameObject playerBodyPrefab;
    public float bodyGap = 0.01f;
    public float bodyScale = 1f;
    public float initialEnergy = 10f;

    [Header("Input Settings")]
    public InputType inputType = InputType.Keyboard;
    private Vector3 cachedMoveDirection;



    private NormalizedInputDirection NID;
    private MovementTracker movementTracker;
    private List<GameObject> playerBodies;

    private float energy;

    private void Awake()
    {
        NID = new NormalizedInputDirection(inputType, Vector3.forward, true, playerHead);
        movementTracker = new MovementTracker(2, 15);
        playerBodies = new List<GameObject>();


        // headRb = playerHead != null ? playerHead.GetComponent<Rigidbody>() : null;


    }


    void Start()
    {
        energy = initialEnergy;
        adjustBodyPart();
    }

    void Update()
    {
        if (!isPlayer) return;
        cachedMoveDirection = NID.GetDirection();
    }

    void FixedUpdate()
    {
        FixedTransform(cachedMoveDirection);
    }

    private void FixedTransform(Vector3 moveDirection)
    {

        // if (isPlayer) UpdateHeadPosition(moveDirection);

        movementTracker.InsertMovementPoint(playerHead.transform.position, playerHead.transform.rotation);

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
    }

    private void UpdateBodyPositions()
    {
        movementTracker.FollowMovement(playerBodies);

        return;

        // Vector3 prevPosition = playerHead.transform.position;
        // Quaternion prevRotation = playerHead.transform.rotation;
        // foreach (var bodyPart in playerBodies)
        // {
        //     Vector3 currentPos = bodyPart.transform.position;
        //     Quaternion currentRotation = bodyPart.transform.rotation;

        //     float distance = Vector3.Distance(prevPosition, currentPos);
        //     if (distance > bodyGap)
        //     {
        //         Vector3 direction = (prevPosition - currentPos).normalized;
        //         currentPos = prevPosition - (direction * bodyGap);
        //         // currentRotation = prevRotation.normalized;
        //     }
        //     bodyPart.transform.position = currentPos;
        //     prevPosition = currentPos;

        //     bodyPart.transform.rotation = currentRotation;
        //     // prevRotation = currentRotation;
        // }
    }


    public void ImproveHealth(float newEnergy = 0.1f)
    {
        energy += newEnergy;
        adjustBodyPart();
    }
    private void adjustBodyPart()
    {
        int totalEnergy = (int)Mathf.Round(energy);

        if (totalEnergy < playerBodies.Count)
        {
            // remove 1 body part ahead from last one
            playerBodies.RemoveRange(totalEnergy - 1, playerBodies.Count - totalEnergy);
            return;
        }
        //return if no change
        if (totalEnergy == playerBodies.Count) return;

        for (int i = playerBodies.Count; i < totalEnergy; i++)
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
    }
    public void DecreaseHealth(float energy = 0.1f)
    {
        this.energy -= energy;
        adjustBodyPart();
    }
}
