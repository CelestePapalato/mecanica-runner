using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float velocidadInicial = 1f;
    [SerializeField]
    private float razonDeCambio;
    [SerializeField]
    private float tiempoParaAumentarPuntaje = .5f;
    [SerializeField]
    private int puntosParaAumentarVelocidad = 100;
    [SerializeField]
    Canvas canvasComenzarJuego;
    [SerializeField]
    Canvas canvasFinDelJuego;

    public static float VelocidadDeJuego { get; private set; }
    static int variableVelocidad = 1;
    public static int puntaje { get; private set; }
    public static int puntajeMaximo { get; private set; }

    void Awake()
    {
        Time.timeScale = 0;
        puntaje = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerEvent playerEvent = player.GetComponent<PlayerEvent>();
        playerEvent.GameOver += _GameOver;
    }

    private IEnumerator aumentarPuntaje()
    {
        while(Time.timeScale > 0)
        {
            yield return new WaitForSeconds(tiempoParaAumentarPuntaje);
            puntaje++;
            int nuevaVariableVelocidad = puntaje / puntosParaAumentarVelocidad;
            if(nuevaVariableVelocidad > variableVelocidad)
            {
                variableVelocidad = nuevaVariableVelocidad;
                updateVelocity();
            }
            if(puntajeMaximo < puntaje)
            {
                puntajeMaximo = puntaje;
            }
        }
    }

    private void updateVelocity()
    {
        int x = puntaje / puntosParaAumentarVelocidad;
        VelocidadDeJuego = razonDeCambio * x + velocidadInicial;
    }

    public static UnityAction GameOver;
    private static void _GameOver()
    {
        Time.timeScale = 0;
        if(GameOver != null)
        {
            GameOver();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static UnityAction GameStart;
    public void iniciarJuego()
    {
        if (canvasComenzarJuego)
        {
            canvasComenzarJuego.enabled = false;
        }
        Time.timeScale = 1;
        updateVelocity();
        StartCoroutine(aumentarPuntaje());
        if(GameStart != null)
        {
            GameStart();
        }
    }

    public static void modificarVelocidad(int cantidad)
    {
        variableVelocidad = Mathf.Max(1, variableVelocidad - cantidad);
    }
}
