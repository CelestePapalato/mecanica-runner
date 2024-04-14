using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float addedSpeed;
    void FixedUpdate()
    {
        transform.Translate(SueloManager.DireccionDeMovimiento * (SueloManager.Velocidad + addedSpeed) * Time.deltaTime * GameManager.VelocidadDeJuego);
    }
}
