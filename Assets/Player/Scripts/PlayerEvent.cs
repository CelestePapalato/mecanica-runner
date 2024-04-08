using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    [SerializeField]
    LayerMask deathZoneLayerMask;
    int deathZoneLayerMaskValue;


    public UnityAction GameOver;

    private void Awake()
    {
        deathZoneLayerMaskValue = (int) Mathf.Log(deathZoneLayerMask.value, 2);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer != deathZoneLayerMaskValue)
        {
            return;
        }
        if(GameOver != null)
        {
            GameOver();
        }
    }
}
