using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float velocidadMaxima;
    [SerializeField]
    float acceleration;
    [SerializeField]
    float deceleration;
    [SerializeField]
    float impulse;
    [SerializeField]
    LayerMask capaSuelo;
    [SerializeField]
    [Range(0f, 0.01f)] float raycastLength;

    Rigidbody rb;
    CapsuleCollider col;

    Vector2 movement_input;
    Vector3 suelo_velocity = Vector3.zero;

    List<Suelo> sueloOverlaps = new List<Suelo>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        float rotacionObjetivo = Camera.main.transform.eulerAngles.y;
        rb.MoveRotation(Quaternion.Euler(0, rotacionObjetivo, 0));
    }

    void Update()
    {
        getInput();
    }

    private void FixedUpdate()
    {
        move();
    }

    // Update is called once per frame
    void getInput()
    {
        movement_input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    void move()
    {
        Vector3 input_vector = movement_input.x * transform.right;

        Vector3 movement_vector = Vector3.zero;

        Vector3 velocidadActual = rb.velocity;
        velocidadActual -= Vector3.Scale(SueloManager.DireccionDeMovimiento, velocidadActual);
        velocidadActual.y = 0;
        Vector3 velocidadObjetivo = input_vector * velocidadMaxima;
        float diferenciaDeVelocidad = velocidadObjetivo.magnitude - velocidadActual.magnitude;


        if (!Mathf.Approximately(diferenciaDeVelocidad, 0))
        {
            float aceleracionAUsar;
            if (diferenciaDeVelocidad > 0)
            {
                aceleracionAUsar = Mathf.Min(acceleration * Time.deltaTime, diferenciaDeVelocidad);
            }
            else
            {
                aceleracionAUsar = Mathf.Max(-deceleration * Time.deltaTime, diferenciaDeVelocidad);
            }
            diferenciaDeVelocidad = 1f / diferenciaDeVelocidad;
            movement_vector = velocidadObjetivo - velocidadActual;
            movement_vector *= diferenciaDeVelocidad * aceleracionAUsar;
        }

        rb.velocity += movement_vector;

        //rb.AddForce(movement_vector, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.Space) && estaEnPiso())
        {
            rb.AddForce(impulse * Vector3.up, ForceMode.Impulse);
        }
    }
    private bool estaEnPiso()
    {
        return Physics.Raycast(transform.position, -transform.up, col.height / 2 + raycastLength, capaSuelo);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Suelo suelo = collision.gameObject.GetComponent<Suelo>();
        suelo_velocity = suelo.getMovementVector() * SueloManager.Velocidad;
        sueloOverlaps.Add(suelo);
    }

    private void OnCollisionExit(Collision collision)
    {
        Suelo suelo = collision.gameObject.GetComponent<Suelo>();
        if (!sueloOverlaps.Contains(suelo))
        {
            return;
        }
        sueloOverlaps.Remove(suelo);
        if(sueloOverlaps.Count == 0)
        {
            suelo_velocity = Vector3.zero;
        }
    }
    */
}
