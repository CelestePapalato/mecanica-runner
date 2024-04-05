using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    Rigidbody rb;
    MeshRenderer meshRenderer;

    bool wasOnCamera;
    bool shouldDestroy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        wasOnCamera = meshRenderer.isVisible;
        //rb.AddForce(SueloManager.DireccionDeMovimiento * SueloManager.Velocidad, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        transform.Translate(SueloManager.DireccionDeMovimiento * SueloManager.Velocidad * Time.deltaTime);
    }

    private void Update()
    {
        if(Time.time == 0)
        {
            return;
        }
        bool isVisible = meshRenderer.isVisible;
        wasOnCamera = isVisible || wasOnCamera;
        shouldDestroy = wasOnCamera && !isVisible;
        if (shouldDestroy)
        {
            SueloManager.eliminarReferenciaASuelo(this);
            Destroy(gameObject);
        }
    }
}
