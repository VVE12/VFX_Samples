using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _boxes = new List<GameObject>();
    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void OnEnable()
    {
        for (int i = 0; i < _boxes.Count; i++)
        {
            GameObject go = _boxes[i];
            var spawn = Instantiate(go);
            spawn.transform.SetParent(transform);
            _spawnedObjects.Add(spawn);
        } 
    }

    private void OnDisable()
    {
        foreach(var go in _spawnedObjects)
        {
            Destroy(go);
        }
    }
}
