using UnityEngine;
using UnityEngine.Events;

public class NonVisible : MonoBehaviour
{
    MeshRenderer meshRenderer;

    bool wasOnCamera;
    bool shouldDestroy = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Time.time == 0)
        {
            return;
        }
        bool isVisible = meshRenderer.isVisible;
        wasOnCamera = isVisible || wasOnCamera;
        shouldDestroy = wasOnCamera && !isVisible;
        if (shouldDestroy)
        {
            _nonVisible();
        }
    }

    public UnityAction action;
    void _nonVisible()
    {
        if(action != null)
        {
            action();
        }
    }
}
