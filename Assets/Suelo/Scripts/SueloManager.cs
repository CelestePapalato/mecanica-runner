using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SueloManager : MonoBehaviour
{
    static List<Suelo> suelosInstanciados = new List<Suelo>();
    static Suelo _sueloPrefab;
    static float _velocidad;

    [SerializeField]
    Suelo sueloPrefab;
    [SerializeField]
    float velocidad;
    [SerializeField]
    Suelo primerSuelo;
    [SerializeField]
    [Tooltip("Cuántos suelos debe haber al comienzo de la partida")]
    int cantidadDeSuelosAInstanciar = 4;

    private void Awake()
    {
        _sueloPrefab = sueloPrefab;
        _velocidad = velocidad;
        if (primerSuelo)
        {
            suelosInstanciados.Add(primerSuelo);
        }
        else
        {
            instanciarNuevoSuelo();
        }
        for(int i = 1; i < cantidadDeSuelosAInstanciar; i++)
        {
            instanciarNuevoSuelo();
        }
    }

    public static void eliminarReferenciaASuelo(Suelo suelo)
    {
        if (suelosInstanciados.Contains(suelo))
        {
            suelosInstanciados.Remove(suelo);
        }
        instanciarNuevoSuelo();
    }

    public static void agregarSuelo(Suelo suelo)
    {
        suelosInstanciados.Add(suelo);
    }

    public static void instanciarNuevoSuelo()
    {
        if(_sueloPrefab == null)
        {
            return;
        }
        if (suelosInstanciados.Count == 0)
        {
            Instantiate(_sueloPrefab);
            return;
        }
        Suelo ultimoSuelo = suelosInstanciados.Last();
        Vector3 posicionUltimo = ultimoSuelo.transform.position;
        Vector3 tamaño = ultimoSuelo.transform.localScale;
        Vector3 directionOfMovement = ultimoSuelo.getMovementVector();
        directionOfMovement *= -1;
        Vector3 offset = new Vector3(tamaño.x * directionOfMovement.x, tamaño.y * directionOfMovement.y, tamaño.z * directionOfMovement.z);
        Vector3 positionOffset = posicionUltimo + offset;
        Suelo nuevo = Instantiate(_sueloPrefab, positionOffset, Quaternion.identity);
        suelosInstanciados.Add(nuevo);
    }

    public static float getVelocidad()
    {
        return _velocidad;
    }
}
