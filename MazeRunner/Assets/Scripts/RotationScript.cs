using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] float xSpeed;
    [SerializeField] float ySpeed;

    [SerializeField] float rotSpeed;
    [SerializeField] float drag;


    bool negativeXForce;
    bool negativeYForce;

    float extraScale = 2.5f; // this has been dialed in. (don't change unless wierdness happens)

    float xForce;
    float yForce;

    private void Update()
    {
        RotateUsingMouse();

        if (xForce != 0) return;
        if (yForce != 0) return;

        ApplyStandardRotation();
    }

    private void ApplyStandardRotation()
    {
        transform.Rotate(Vector3.up, xSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, ySpeed * Time.deltaTime, Space.World);
    }

    private void RotateUsingMouse()
    {
        transform.Rotate(Vector3.up, -xForce, Space.World);
        transform.Rotate(Vector3.right, -yForce, Space.World);

        if (negativeXForce)
        {
            xForce = Mathf.Clamp(xForce += drag * Time.deltaTime, int.MinValue, 0);
        }
        else
        {
            xForce = Mathf.Clamp(xForce -= drag * Time.deltaTime, 0, int.MaxValue);
        }

        if (negativeYForce)
        {
            yForce = Mathf.Clamp(yForce += drag * Time.deltaTime, int.MinValue, 0);
        }
        else
        {
            yForce = Mathf.Clamp(yForce -= drag * Time.deltaTime, 0, int.MaxValue);
        }
    }

    private void OnMouseDrag()
    {
        xForce = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        yForce = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        if (xForce > 0)
            negativeXForce = false;
        else
            negativeXForce = true;

        if (yForce > 0)
            negativeYForce = false;
        else
            negativeYForce = true;

    }

    public void SetScale(Vector2Int size)
    {
        GetComponent<BoxCollider>().size = new Vector3(size.x + extraScale, size.y + extraScale, size.x + extraScale);
    }

}
