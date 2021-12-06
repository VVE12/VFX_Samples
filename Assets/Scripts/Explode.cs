using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private List<Rigidbody> _rbChilds = new List<Rigidbody>();
    private List<BoxCollider> _colliders = new List<BoxCollider>();

    [SerializeField] private float _timeToExplode = 1;
    [SerializeField] private float _timeCleanParts = 1;
    [SerializeField] private GameObject _effectObject;
    [SerializeField] private bool _destroyParts;

   private void Start()
    {
        _rbChilds.AddRange(GetComponentsInChildren<Rigidbody>(true));
        _colliders.AddRange(GetComponentsInChildren<BoxCollider>(true));
    }

    private void OnEnable()
    {
        StartCoroutine(ExplodeFunction());
    }

    private IEnumerator ExplodeFunction() 
    {
        yield return new WaitForSeconds(_timeToExplode);

        foreach (var rb in _rbChilds)
        {
            rb.useGravity = true;
        }

        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }

        _effectObject.SetActive(true);

        if (_destroyParts)
        {
            yield return new WaitForSeconds(_timeCleanParts);

            foreach (var go in _rbChilds)
            {
                Destroy(go.gameObject);
            }
        }
    } 
}
