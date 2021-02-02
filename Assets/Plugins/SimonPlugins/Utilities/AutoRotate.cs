using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationSpeed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
