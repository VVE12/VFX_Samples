using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    private void Update()
    {
        var cameraDirection = Camera.main.transform.forward;
        transform.rotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
    }
}
