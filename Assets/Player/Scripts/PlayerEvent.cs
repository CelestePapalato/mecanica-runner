using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour
{
    [SerializeField]
    LayerMask deathZoneLayerMask;


    public UnityAction GameOver;
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer != deathZoneLayerMask)
        {
            return;
        }
        if(GameOver != null)
        {
            GameOver();
        }
    }
}
