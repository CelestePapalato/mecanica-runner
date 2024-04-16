using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoEspecial : MonoBehaviour
{
    [Tooltip("Durante cuántos puntos el enemigo actuará")]
    [SerializeField] int PuntosDeVida;
    [SerializeField] float TiempoDisparo;
    [SerializeField] Move PrefabBala;
    [SerializeField] Transform Waypoints;
    [SerializeField] float distanciaParaCambiarDePunto;
    [SerializeField] float velocidadDeMovimiento;

    int _puntosVivo = 0;
    int _currentWaypoint = 0;
    List<Transform> _waypointsList = new List<Transform>();

    private void Awake()
    {
        foreach(Transform point in Waypoints)
        {
            if(point != Waypoints)
            {
                _waypointsList.Add(point);
            }
        }
    }

    private void OnEnable()
    {
        _puntosVivo = 0;
        _currentWaypoint = 0;
        transform.position = _waypointsList[_currentWaypoint].position;
        GameManager.PuntajeModificado += actualizarPuntosVivo;
        StartCoroutine(shoot());
    }

    private void OnDisable()
    {
        GameManager.PuntajeModificado -= actualizarPuntosVivo;
    }

    private void OnDestroy()
    {
        GameManager.PuntajeModificado -= actualizarPuntosVivo;
    }

    public void actualizarPuntosVivo(int puntajeActual)
    {
        _puntosVivo++;
        if(_puntosVivo >= PuntosDeVida)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Vector3 posicionWaypointObjetivo = _waypointsList[_currentWaypoint].position;
        Vector3 posicionActual = transform.position;
        if (Vector3.Magnitude(posicionWaypointObjetivo - posicionActual) <= distanciaParaCambiarDePunto) {
            _currentWaypoint++;
            if(_currentWaypoint >= _waypointsList.Count)
            {
                _currentWaypoint = 0;
            }
            posicionWaypointObjetivo = _waypointsList[_currentWaypoint].position;
        }

        Vector3 direccionMovimiento = posicionWaypointObjetivo - posicionActual;
        direccionMovimiento.Normalize();
        transform.Translate(direccionMovimiento * velocidadDeMovimiento * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        while(Time.timeScale > 0)
        {
            yield return new WaitForSeconds(TiempoDisparo);
            Instantiate(PrefabBala, transform.position, Quaternion.identity);
        }
    }
}
