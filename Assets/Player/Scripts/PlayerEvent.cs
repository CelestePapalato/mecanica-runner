using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    [SerializeField]
    LayerMask deathZoneLayerMask;
    [SerializeField]
    string jumpPowerUpTag;
    [SerializeField]
    string gameSpeedDebuffTag;
    [SerializeField]
    string obstacleTag;
    int deathZoneLayerMaskValue;


    public UnityAction GameOver;
    public UnityAction jumpPowerUp;

    private void Awake()
    {
        deathZoneLayerMaskValue = (int) Mathf.Log(deathZoneLayerMask.value, 2);
    }
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.CompareTag(jumpPowerUpTag))
        {
            Debug.Log("gaaaa " + collider.name);
            jumpPowerUp();
            return;
        }
        if (collider.gameObject.CompareTag(gameSpeedDebuffTag))
        {
            GameManager.modificarVelocidad(-2);
        }
        if (collider.gameObject.layer == deathZoneLayerMaskValue || collider.gameObject.CompareTag(obstacleTag))
        {
            if (GameOver != null)
            {
                GameOver();
            }
        }
    }
}
