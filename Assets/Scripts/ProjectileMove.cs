using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMove : MonoBehaviour
{
    private Rigidbody _rb;
    private MeteorController _meteorController;
    private bool _created;

    [SerializeField] private float _speed;
    [SerializeField] private GameObject _impactPrefab;
    [SerializeField] private List<ParticleSystem> _trails;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _meteorController = FindObjectOfType<MeteorController>();
    }

    private void FixedUpdate()
    {
        _rb.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_created)
        {
            return;
        }

        _speed = 0;

        var contact = collision.contacts[0];
        var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        var pos = contact.point;

        if (_impactPrefab != null)
        {
            var impactVFX = Instantiate(_impactPrefab, pos, rot, _meteorController.transform);
            impactVFX.transform.localPosition = _meteorController.EndPoint.position;
            Destroy(impactVFX, 5);
        }

        if (_trails.Count > 0)
        {
            for(int i = 0; i < _trails.Count; i++)
            {
                _trails[i].transform.parent = _meteorController.transform;

                var currentTrail = _trails[i];

                if (currentTrail != null)
                {
                    currentTrail.Stop();
                    Destroy(currentTrail.gameObject, 
                        currentTrail.main.duration + currentTrail.main.startLifetime.constantMax);
                }
            }
        }

        Destroy(gameObject);
        _created = true;
    }
}
