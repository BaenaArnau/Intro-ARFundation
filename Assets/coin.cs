using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class coin : MonoBehaviour
{
    private ARRaycastManager rayManager;
    
    void Start()
    {
        rayManager = FindAnyObjectByType<ARRaycastManager>();
    }

    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width /2, Screen.height /2), hits, TrackableType.Planes);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            Destroy(gameObject);
        }
    }

    void RandomSpawn()
    {
        float x = Random.Range(-5.0f, 5.0f);
        float y = Random.Range(-5.0f, 5.0f);
        float z = Random.Range(-5.0f, 5.0f);

        transform.position = new Vector3(x, y, z);
    }
}
