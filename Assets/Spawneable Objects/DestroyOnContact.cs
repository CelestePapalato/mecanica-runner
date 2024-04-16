using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOnContact : MonoBehaviour
{
    [SerializeField] UnityEvent eventWhenTriggered;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + " | " + other.name);
        if (eventWhenTriggered != null)
        {
            eventWhenTriggered.Invoke();
        }
        Destroy(gameObject);
    }
}
