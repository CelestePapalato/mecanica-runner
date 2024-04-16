using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    NonVisible balaNonVisible;
    CapsuleCollider balaCollider;
    [SerializeField]
    LayerMask capaObstaculo;
    void Start()
    {
        balaNonVisible = GetComponentInChildren<NonVisible>();
        if (balaNonVisible)
        {
            balaNonVisible.action += DestroyThis;
        }
        balaCollider = GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        bool isColliding = Physics.CheckCapsule(balaCollider.bounds.center, 
                             new Vector3(balaCollider.bounds.center.x, balaCollider.bounds.min.y, balaCollider.bounds.center.z), 
                             balaCollider.radius, capaObstaculo);
        if (isColliding)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
