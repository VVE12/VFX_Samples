using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalRotator : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;

    private Vector3 _direction;

    private void Update()
    {
        if (!_target)
        {
            return;
        }

        _direction = transform.position - _target.position;

        transform.Rotate(Vector3.up, 100 * Time.deltaTime);
        transform.RotateAround(_target.position, Vector3.up, 100 * Time.deltaTime);

        float currentDistance = Vector3.Distance(_target.position, transform.position);
        transform.position += (_distance - currentDistance) * _direction.normalized;
    }
}
