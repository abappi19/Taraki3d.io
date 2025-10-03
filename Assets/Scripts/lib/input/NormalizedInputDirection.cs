using UnityEngine;

public class NormalizedInputDirection
{

    private Vector3 NormDirection;
    private bool PreventBackwardMovement;

    public NormalizedInputDirection(Vector3 initialDirection, bool preventBackwardMovement)
    {
        NormDirection = initialDirection == Vector3.zero ? Vector3.forward : initialDirection.normalized;
        this.PreventBackwardMovement = preventBackwardMovement;
    }

    public Vector3 GetDirection()
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
}