using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    NonVisible nonVisible;
    void Awake()
    {
        nonVisible = GetComponent<NonVisible>();
        nonVisible.action += destroy;
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
}
