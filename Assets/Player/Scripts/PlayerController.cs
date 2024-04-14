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
    float jumpImpulse;
    [SerializeField]
    float distanciaMaximaLateral = 7;
    [SerializeField]
    LayerMask capaSuelo;
    [SerializeField]
    [Range(0f, 0.01f)] float raycastLength;
    [SerializeField]
    float jumpPowerUpLength;
    [SerializeField]
    float jumpPowerUpMultiplier;

    float currentJumpImpulse;

    Rigidbody rb;
    CapsuleCollider col;
    Animator animator;
    PlayerEvent playerEvent;

    Vector2 movement_input;
    Vector3 puntoMaximoIzq;
    Vector3 puntoMaximoDer;

    bool jumpPowerUpOn = false;

    private void Awake()
    {
        currentJumpImpulse = jumpImpulse;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        playerEvent = GetComponent<PlayerEvent>();
        playerEvent.jumpPowerUp += jumpPowerUp;
    }

    private void Start()
    {
        Vector3 forward = -SueloManager.DireccionDeMovimiento;
        float rotacionObjetivo = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        transform.Rotate(transform.up, rotacionObjetivo);
        //rb.MoveRotation(Quaternion.Euler(0, rotacionObjetivo, 0));

        puntoMaximoIzq = Vector3.Scale(transform.position, transform.right) + transform.right * -distanciaMaximaLateral;
        puntoMaximoDer = Vector3.Scale(transform.position, transform.right) + transform.right * distanciaMaximaLateral;
    }

    private void FixedUpdate()
    {
        move();
        clampPosition();
    }
    private void Update()
    {
        getInput();
        checkIfLanded();
    }

    // Update is called once per frame
    void getInput()
    {
        movement_input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    void checkIfLanded()
    {
        bool isJumping = animator.GetBool("Jumping");
        if (!isJumping)
        {
            return;
        }
        if (estaEnPiso())
        {
            animator.SetTrigger("Land");
        }
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
        bool isJumping = animator.GetBool("Jumping");

        if ((Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)) && estaEnPiso() && !isJumping)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(jumpImpulse * Vector3.up, ForceMode.Impulse);
        }
    }
    private bool estaEnPiso()
    {
        float altura = col.height * transform.lossyScale.y;
        return Physics.Raycast(transform.position, -transform.up, altura / 2 + raycastLength, capaSuelo);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        int sueloLayer = (int)Mathf.Log(capaSuelo.value, 2);
        if(collision.gameObject.layer == sueloLayer)
        {
            estaEnPiso = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        int sueloLayer = (int)Mathf.Log(capaSuelo.value, 2);
        if (collision.gameObject.layer == sueloLayer)
        {
            estaEnPiso = false;
        }
    }
    */

    void jumpPowerUp()
    {
        if (jumpPowerUpOn)
        {
            StopCoroutine(jumpRestart());
        }
        currentJumpImpulse = jumpImpulse * jumpPowerUpMultiplier;
        StartCoroutine(jumpRestart());
    }

    IEnumerator jumpRestart()
    {
        jumpPowerUpOn = true;
        yield return new WaitForSeconds(jumpPowerUpLength);
        currentJumpImpulse = jumpImpulse;
        jumpPowerUpOn = false;
    }
}
