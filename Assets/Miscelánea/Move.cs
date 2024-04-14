using UnityEngine;

public class Move : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(SueloManager.DireccionDeMovimiento * SueloManager.Velocidad * Time.deltaTime * GameManager.VelocidadDeJuego);
    }
}
