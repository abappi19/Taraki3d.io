using UnityEngine;


public enum InputType
{
    Keyboard,
    Mouse,
    Touch
}
public class NormalizedInputDirection
{

    private Vector3 NormDirection;
    private bool PreventBackwardMovement;
    private InputType inputType;
    private GameObject playerHead;
    private Camera mainCamera;
    public NormalizedInputDirection(InputType inputType, Vector3 initialDirection, bool preventBackwardMovement, GameObject playerHead)
    {
        NormDirection = initialDirection.normalized;
        PreventBackwardMovement = preventBackwardMovement;
        this.inputType = inputType;
        this.playerHead = playerHead;
        this.mainCamera = Camera.main;
    }

    public Vector3 GetDirection()
    {
        return inputType switch
        {
            InputType.Mouse => GetDirectionFromMouse(),
            InputType.Touch => GetDirectionFromTouch(),
            InputType.Keyboard => GetDirectionFromKeyboard(),
            _ => Vector3.zero,
        };
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