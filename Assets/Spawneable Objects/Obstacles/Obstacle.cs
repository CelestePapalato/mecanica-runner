using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] LayerMask SueloLayerMask;
    [SerializeField] float distanciaRaycast;
    NonVisible _nonVisible;
    void Awake()
    {
        _nonVisible = GetComponent<NonVisible>();
        _nonVisible.action += destroy;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        Vector3 scale = new Vector3(0, 0, transform.localScale.z) * 1/2;
        bool center = Physics.Raycast(position, Vector3.down, distanciaRaycast, SueloLayerMask);
        bool left = Physics.Raycast(position + scale, Vector3.down, distanciaRaycast, SueloLayerMask);
        bool right = Physics.Raycast(position - scale, Vector3.down, distanciaRaycast, SueloLayerMask);
        if(!(center || left || right))
        {
            Destroy(gameObject);
        }
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _nonVisible.action -= destroy;
    }
}
