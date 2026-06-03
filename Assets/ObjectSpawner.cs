using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private PlacementIndicator placementIndicator;

    void Start()
    {
        placementIndicator = FindAnyObjectByType<PlacementIndicator>();
    }

    void Update()
    {
        if (placementIndicator == null || objectToSpawn == null)
        {
            return;
        }

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            GameObject obj = Instantiate(objectToSpawn);
            obj.transform.SetPositionAndRotation(
                placementIndicator.transform.position,
                placementIndicator.transform.rotation
            );
        }
    }
}
