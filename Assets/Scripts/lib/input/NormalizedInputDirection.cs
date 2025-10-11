using UnityEngine;
using UnityEngine.AI;


public enum InputType
{
    Keyboard,
    Mouse,
    Touch,
    AI
}
public class NormalizedInputDirection
{

    private Vector3 NormDirection;
    private bool PreventBackwardMovement;
    private InputType inputType;
    private GameObject playerHead;
    private Camera mainCamera;

    private NavMeshAgent navMeshAgent;
    private int ignoreFrameForAIDirection = 15;
    private int frameIgnored = 0;
    public NormalizedInputDirection(InputType inputType, Vector3 initialDirection, bool preventBackwardMovement, GameObject playerHead)
    {
        NormDirection = initialDirection.normalized;
        PreventBackwardMovement = preventBackwardMovement;
        this.inputType = inputType;
        this.playerHead = playerHead;
        this.mainCamera = Camera.main;
        this.navMeshAgent = playerHead.GetComponent<NavMeshAgent>();
    }

    public Vector3 GetDirection()
    {

        return inputType switch
        {
            InputType.Mouse => GetDirectionFromMouse(),
            InputType.Touch => GetDirectionFromTouch(),
            InputType.Keyboard => GetDirectionFromKeyboard(),
            InputType.AI => GetAIDirection(),
            _ => Vector3.zero,
        };
    }

    private Vector3 getNearestFoodPosition()
    {
        // Pick a random food position first
        Vector3 nearestPos = playerHead.transform.position;
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        float nearestDist = float.MaxValue;
        float searchRadius = 100f; // You can adjust search radius if you want a limit

        foreach (var food in foods)
        {
            float dist = Vector3.Distance(playerHead.transform.position, food.transform.position);
            if (dist < nearestDist && dist <= searchRadius)
            {
                nearestDist = dist;
                nearestPos = food.transform.position;
            }
        }
        return nearestPos;
    }

    public Vector3 GetAIDirection()
    {

        Vector3 nearestFoodPosition = getNearestFoodPosition();
        if (navMeshAgent == null) return NormDirection;
        navMeshAgent.destination = nearestFoodPosition;



        if (frameIgnored < ignoreFrameForAIDirection)
        {
            frameIgnored++;
            return NormDirection;
        }
        frameIgnored = 0;


        Vector3 inputDirection = navMeshAgent.steeringTarget - playerHead.transform.position;
        inputDirection.y = 0f;

        if (inputDirection != Vector3.zero)
        {
            Vector3 normalizedDir = inputDirection.normalized;

            if (PreventBackwardMovement && Vector3.Dot(normalizedDir, NormDirection) < -0.9f)
            {
                return NormDirection;
            }
            NormDirection = normalizedDir;
        }

        return NormDirection;
    }

    public Vector3 GetDirectionFromKeyboard()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new(x, 0f, z);

        if (inputDirection != Vector3.zero)
        {
            Vector3 normalizedDir = inputDirection.normalized;

            if (PreventBackwardMovement && Vector3.Dot(normalizedDir, NormDirection) < -0.9f)
            {
                return NormDirection;
            }
            NormDirection = normalizedDir;
        }

        return NormDirection;
    }

    public Vector3 GetDirectionFromMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(mainCamera.pixelWidth * 0.5f, mainCamera.pixelHeight * 0.5f, 0f);

        // Calculate offset from screen center
        float x = (mousePos.x - screenCenter.x) / Screen.width;
        float z = (mousePos.y - screenCenter.y) / Screen.height;

        Vector3 inputDirection = new Vector3(x, 0f, z);

        if (inputDirection != Vector3.zero)
        {
            Vector3 normalizedDir = inputDirection.normalized;

            NormDirection = normalizedDir;
        }

        return NormDirection;
    }

    public Vector3 GetDirectionFromTouch()
    {
        return Vector3.zero;
    }
}