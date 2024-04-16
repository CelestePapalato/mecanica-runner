using UnityEngine;
using TMPro;

public class PuntuaciónTexto : MonoBehaviour
{
    enum PUNTUACION
    {
        actual,
        maxima
    }
    [SerializeField]
    PUNTUACION tipoAMostrar;

    TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (tipoAMostrar)
        {
            case PUNTUACION.actual: text.text = GameManager.Puntaje + "";
                                    break;
            default: text.text = GameManager.PuntajeMaximo + "";
                break;
        }
    }
}
