using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 90 * Time.deltaTime);
    }
}
