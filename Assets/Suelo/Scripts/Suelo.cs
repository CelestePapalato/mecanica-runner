using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    [SerializeField]
    Vector3 movementVector = Vector3.forward;

    Rigidbody rb;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        rb.AddForce(movementVector * SueloManager.getVelocidad(), ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        /*
        transform.Translate(movementVector * SueloManager.getVelocidad() * Time.deltaTime);
        */
    }

    private void Update()
    {
        if(Time.time == 0)
        {
            return;
        }
        bool isVisible = meshRenderer.isVisible;
        if (!isVisible)
        {
            SueloManager.eliminarReferenciaASuelo(this);
            Destroy(gameObject);
        }
    }

    public Vector3 getMovementVector()
    {
        return movementVector;
    }


}
