using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    [SerializeField]
    Vector3 deviation;
    [SerializeField]
    float deviationSpeed;

    public Vector3 spawnPosition { get; private set; }

    MeshRenderer meshRenderer;

    bool wasOnCamera;
    bool shouldDestroy = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        spawnPosition = transform.position;
    }

    private void Start()
    {
        wasOnCamera = meshRenderer.isVisible;
    }

    private void FixedUpdate()
    {
        transform.Translate(SueloManager.DireccionDeMovimiento * SueloManager.Velocidad * Time.deltaTime * GameManager.VelocidadDeJuego);
        if (meshRenderer.isVisible)
        {
            transform.Translate(deviation * deviationSpeed * GameManager.VelocidadDeJuego * Time.deltaTime);
        }
        spawnPosition += SueloManager.DireccionDeMovimiento * SueloManager.Velocidad * Time.deltaTime * GameManager.VelocidadDeJuego;
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
