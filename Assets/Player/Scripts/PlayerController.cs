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
    float distanciaMaximaLateral = 7;
    [SerializeField]
    LayerMask capaSuelo;
    [SerializeField]
    [Range(0f, 0.01f)] float raycastLength;

    Rigidbody rb;
    CapsuleCollider col;
    Animator animator;

    Vector2 movement_input;
    Vector3 suelo_velocity = Vector3.zero;
    Vector3 puntoMaximoIzq;
    Vector3 puntoMaximoDer;

    List<Suelo> sueloOverlaps = new List<Suelo>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Vector3 forward = -SueloManager.DireccionDeMovimiento;
        float rotacionObjetivo = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        transform.Rotate(transform.up, rotacionObjetivo);
        //rb.MoveRotation(Quaternion.Euler(0, rotacionObjetivo, 0));

        puntoMaximoIzq = Vector3.Scale(transform.position, transform.right) + transform.right * -distanciaMaximaLateral;
        puntoMaximoDer = Vector3.Scale(transform.position, transform.right) + transform.right * distanciaMaximaLateral;
        Debug.Log(Mathf.Approximately(0, puntoMaximoIzq.x));
        Debug.Log(Mathf.Approximately(0, puntoMaximoDer.x));
        Debug.Log("Izq " + puntoMaximoIzq);
        Debug.Log("Der " + puntoMaximoDer);
    }

    private void FixedUpdate()
    {
        move();
        clampPosition();
    }
    private void Update()
    {
        getInput();
    }

    // Update is called once per frame
    void getInput()
    {
        movement_input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    bool approximately(float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance; 
    }

    void clampPosition()
    {
        float x = transform.position.x;
        if (!approximately(puntoMaximoDer.x, puntoMaximoIzq.x, 0.001f))
        {
            // MÁGICAMENTE ESTOS NÚMEROS DEJAN DE SER 0 DE LA NADA SI EN EL VECTOR VALEN ESO??
            float x_max = puntoMaximoDer.x;
            float x_min = puntoMaximoIzq.x;
            if (x_max < x_min)
            {
                x = Mathf.Clamp(x, x_max, x_min);
            }
            if (x_max > x_min)
            {
                x = Mathf.Clamp(x, x_min, x_max);
            }
        }

        float z = transform.position.z;
        if (!approximately(puntoMaximoDer.z, puntoMaximoIzq.z, 0.001f))
        {
            // MÁGICAMENTE ESTOS NÚMEROS DEJAN DE SER 0 DE LA NADA SI EN EL VECTOR VALEN ESO??
            float z_max = puntoMaximoDer.z;
            float z_min = puntoMaximoIzq.z;
            if (z_max > z_min)
            {
                z = Mathf.Clamp(z, z_min, z_max);
            }
            if (z_max < z_min)
            {
                z = Mathf.Clamp(z, z_max, z_min);
            }
        }

        Vector3 newPositon = transform.position;
        newPositon.x = x;
        newPositon.z = z;
        transform.position = newPositon;
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
