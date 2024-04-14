using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] RectTransform shadowObject;
    [SerializeField] float raycastDistance;
    [SerializeField] float heightAddition;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask sueloLayer;

    void Start()
    {
        placeOnSurface();
    }

    // Update is called once per frame
    void Update()
    {
        placeOnSurface();
    }

    void placeOnSurface()
    {
        Debug.Log("aaaa");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, obstacleLayer))
        {
            Vector3 position = hit.point;
            position.y += heightAddition;
            shadowObject.position = position;
            return;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, sueloLayer))
        {
            Vector3 position = hit.point;
            position.y += heightAddition;
            shadowObject.position = position;
            return;
        }
    }
}
