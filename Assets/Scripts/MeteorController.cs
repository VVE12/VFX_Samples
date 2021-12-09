using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    [SerializeField] private GameObject _meteor;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    public Transform EndPoint => _endPoint;

    private void OnEnable()
    {
        var prefab = Instantiate(_meteor, _startPoint.position, Quaternion.identity, transform);
        prefab.SetActive(false);

        var direction = _endPoint.position - prefab.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        prefab.transform.localRotation = Quaternion.Lerp(prefab.transform.rotation, rotation, 1);
        prefab.SetActive(true);
    }
}
