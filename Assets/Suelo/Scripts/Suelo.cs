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
    NonVisible destroyNonVisible;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        destroyNonVisible = GetComponent<NonVisible>();
        destroyNonVisible.action += destroySuelo;
        spawnPosition = transform.position;
    }
    private void FixedUpdate()
    {
        if (meshRenderer.isVisible)
        {
            transform.Translate(deviation * deviationSpeed * GameManager.VelocidadDeJuego * Time.deltaTime);
        }
        spawnPosition += SueloManager.DireccionDeMovimiento * SueloManager.Velocidad * Time.deltaTime * GameManager.VelocidadDeJuego;
    }

    private void destroySuelo()
    {
        SueloManager.eliminarReferenciaASuelo(this);
        Destroy(gameObject);
    }

}
