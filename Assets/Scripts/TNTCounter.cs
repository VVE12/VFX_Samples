using System.Collections;
using UnityEngine;

public class TNTCounter : MonoBehaviour
{
    [SerializeField] private float _timeBefore = 1f;
    [SerializeField] private float _timeBetween = 1f;

    [SerializeField] private Texture2D[] _numbers;
    private Material _material;

    private Color _color = new Color(1, 1, 1, 1);
    private Color _alpha = new Color(1, 1, 1, 0);

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().sharedMaterial;
        StartCoroutine(Counter());
    }

    private IEnumerator Counter()
    {
        _material.SetColor("_BaseColor", _alpha);
        yield return new WaitForSeconds(_timeBefore);
        _material.SetColor("_BaseColor", _color);
        _material.SetTexture("_BaseMap", _numbers[0]);
        yield return new WaitForSeconds(_timeBetween);
        _material.SetTexture("_BaseMap", _numbers[1]);
        yield return new WaitForSeconds(_timeBetween);
        _material.SetTexture("_BaseMap", _numbers[2]);
        yield return new WaitForSeconds(_timeBetween);

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        _material.SetColor("_BaseColor", _alpha);
    }
}
