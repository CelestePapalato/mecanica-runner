using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SueloManager : MonoBehaviour
{
    static List<Suelo> suelosInstanciados = new List<Suelo>();
    static Suelo _sueloPrefab;
    static List<Suelo> suelos = new List<Suelo>();
    static List<int> probabilidades = new List<int>();
    public static float Velocidad { get; private set; }
    public static Vector3 DireccionDeMovimiento { get; private set; }

    [SerializeField]
    float velocidad;
    [SerializeField]
    Vector3 direccionDeMovimiento = Vector3.back;
    [SerializeField]
    Suelo primerSuelo;
    [SerializeField]
    [Tooltip("Cuántos suelos debe haber al comienzo de la partida")]
    int cantidadDeSuelosAInstanciar = 4;
    [SerializeField]
    List<Suelo> sueloPrefabs;
    [SerializeField]
    List<int> probabilidadesSuelo;

    private void Awake()
    {
        suelos = sueloPrefabs;
        for(int i = 0; i < probabilidadesSuelo.Count; i++)
        {
            for(int n = 0; n < probabilidadesSuelo[i]; n++)
            {
                probabilidades.Add(i);
            }
        }
        randomizeList(probabilidades);

        Velocidad = velocidad;
        if(direccionDeMovimiento != Vector3.zero)
        {
            DireccionDeMovimiento = direccionDeMovimiento;
        }
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
        if(Time.time == 0)
        {
            _sueloPrefab = suelos[0];
        }
        else
        {
            _sueloPrefab = pickSuelo();
        }
        if (suelosInstanciados.Count == 0)
        {
            Instantiate(_sueloPrefab);
            return;
        }
        Suelo ultimoSuelo = suelosInstanciados.Last();
        Vector3 posicionUltimo = ultimoSuelo.spawnPosition;
        Vector3 tamaño = ultimoSuelo.transform.localScale/2 + _sueloPrefab.transform.localScale/2;
        Vector3 directionOfMovement = DireccionDeMovimiento;
        directionOfMovement *= -1;
        Vector3 offset = Vector3.Scale(tamaño, directionOfMovement);
        Vector3 positionOffset = posicionUltimo + offset;
        Suelo nuevo = Instantiate(_sueloPrefab, positionOffset, Quaternion.identity);
        suelosInstanciados.Add(nuevo);
    }

    List<int> randomizeList(List<int> _list)
    {
        List<int> list = _list;
        for (int i = 0; i < list.Count; i++)
        {
            int aux = list[i];
            int r = Random.Range(0, list.Count);
            int randomSelected = list[r];
            list[i] = randomSelected;
            list[r] = aux;
        }
        return list;
    }

    static Suelo pickSuelo()
    {
        int r = Random.Range(0, probabilidades.Count);
        int index = probabilidades[r];
        Suelo suelo = suelos[index];
        return suelo;
    }
}
