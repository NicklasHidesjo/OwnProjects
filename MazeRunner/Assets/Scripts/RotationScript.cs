using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] float xSpeed = 5f;
    [SerializeField] float ySpeed = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up, ySpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, xSpeed * Time.deltaTime);
    }
}
