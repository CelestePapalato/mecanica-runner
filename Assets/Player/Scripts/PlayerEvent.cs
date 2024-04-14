using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    [SerializeField]
    LayerMask deathZoneLayerMask;
    [SerializeField]
    LayerMask jumpPowerUpLayerMask;
    int deathZoneLayerMaskValue;


    public UnityAction GameOver;
    public UnityAction jumpPowerUp;

    private void Awake()
    {
        deathZoneLayerMaskValue = (int) Mathf.Log(deathZoneLayerMask.value, 2);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == jumpPowerUpLayerMask && jumpPowerUp != null)
        {
            jumpPowerUp();
            return;
        }
        if (collider.gameObject.layer != deathZoneLayerMaskValue)
        {
            return;
        }
        if(GameOver != null)
        {
            GameOver();
        }
    }
}
