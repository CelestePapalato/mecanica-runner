using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    EnemigoEspecial enemigoEspecial;
    [SerializeField]
    int puntosParaActivarEnemigoEspecial;
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

    private static GameManager current;
    public static float VelocidadDeJuego { get; private set; }
    private static int VariableVelocidad = 1;
    public static int Puntaje { get; private set; }
    public static int PuntajeMaximo { get; private set; }
    public static UnityAction<int> PuntajeModificado;

    void Awake()
    {
        current = this;
        Time.timeScale = 0;
        Puntaje = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerEvent playerEvent = player.GetComponent<PlayerEvent>();
        playerEvent.GameOver += _GameOver;
    }

    private IEnumerator aumentarPuntaje()
    {
        while(Time.timeScale > 0)
        {
            yield return new WaitForSeconds(tiempoParaAumentarPuntaje);
            Puntaje++;
            if(PuntajeModificado != null)
            {
                PuntajeModificado(Puntaje);
            }
            PuntajeMaximo = (PuntajeMaximo < Puntaje)? Puntaje : PuntajeMaximo;

            if (Puntaje % puntosParaAumentarVelocidad == 0)
            {
                int nuevaVariableVelocidad = Puntaje / puntosParaAumentarVelocidad;
                if (nuevaVariableVelocidad > VariableVelocidad)
                {
                    modificarVelocidad(1);
                }
            }

            if(Puntaje % puntosParaActivarEnemigoEspecial == 0)
            {
                current.enemigoEspecial.gameObject.SetActive(true);
            }
        }
    }

    private void updateVelocity()
    {
        VelocidadDeJuego = razonDeCambio * VariableVelocidad + velocidadInicial;
        Debug.Log(VelocidadDeJuego);
    }

    public static UnityAction GameOver;
    private static void _GameOver()
    {
        Time.timeScale = 0;
        if(GameOver != null)
        {
            GameOver();
        }
        current.StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static UnityAction GameStart;
    public void iniciarJuego()
    {
        if (canvasComenzarJuego)
        {
            canvasComenzarJuego.gameObject.SetActive(false);
        }
        Time.timeScale = 1;
        VariableVelocidad = 0;
        updateVelocity();
        current.StartCoroutine(aumentarPuntaje());
        if(GameStart != null)
        {
            GameStart();
        }
    }

    public static void modificarVelocidad(int cantidad)
    {
        VariableVelocidad = Mathf.Max(0, VariableVelocidad + cantidad);
        current.updateVelocity();
    }
}
