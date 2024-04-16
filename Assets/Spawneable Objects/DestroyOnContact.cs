using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOnContact : MonoBehaviour
{
    [SerializeField] UnityEvent eventWhenTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (eventWhenTriggered != null)
        {
            eventWhenTriggered.Invoke();
        }
        Destroy(gameObject);
    }
}
