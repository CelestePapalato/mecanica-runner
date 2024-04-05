using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float acceleration;

    Rigidbody rb;
    Vector2 movement_input;
    Vector3 suelo_velocity = Vector3.zero;

    List<Suelo> sueloOverlaps = new List<Suelo>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        getInput();
        move();
    }

    // Update is called once per frame
    void getInput()
    {
        movement_input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement_input = Vector2.ClampMagnitude(movement_input, 1);
    }

    void move()
    {
        Vector3 movement_vector = new Vector3(movement_input.x, 0, movement_input.y);
        rb.AddForce(movement_vector * acceleration, ForceMode.Acceleration);
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
