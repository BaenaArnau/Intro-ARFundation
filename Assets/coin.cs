using UnityEngine;

public class coin : MonoBehaviour
{
    private MonoBehaviour owner;
    private bool wasCollected;
    private Rigidbody body;

    public void Initialize(MonoBehaviour spawner)
    {
        owner = spawner;
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody>();
            body.useGravity = false;
            body.isKinematic = true;
            Debug.Log("[coin] Added kinematic Rigidbody to the coin root so trigger events can be received.", this);
        }

        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            // Try children in case the collider is on a child object of the prefab
            col = GetComponentInChildren<Collider>();
            if (col == null)
            {
                Debug.LogWarning("[coin] No Collider found on coin (neither on root nor children). Add one to the prefab.", this);
                return;
            }
            else
            {
                Debug.Log("[coin] Found Collider on a child object — using that.", this);
            }
        }

        // Prefer trigger pickups to avoid physics response. If designer forgot to set it,
        // enable it at runtime to make the pickup work and log the change.
        if (!col.isTrigger)
        {
            Debug.LogWarning("[coin] Collider.isTrigger was false — enabling it so the coin can be collected (runtime change).", this);
            col.isTrigger = true;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[coin] OnTriggerEnter with {other.gameObject.name}", this);
        TryCollect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[coin] OnCollisionEnter with {collision.gameObject.name}", this);
        TryCollect(collision.gameObject);
    }

    private void TryCollect(GameObject other)
    {
        if (wasCollected)
        {
            Debug.Log("[coin] TryCollect: already collected, ignoring.", this);
            return;
        }

        // Accept collider on the car itself or on child objects.
        Coche car = other.GetComponentInParent<Coche>();
        if (car == null)
        {
            Debug.Log($"[coin] TryCollect: collider does not belong to a Coche (other={other.name}).", this);
            return;
        }

        Debug.Log($"[coin] Collected by car {car.name}", this);
        wasCollected = true;
        owner?.Invoke("NotifyCoinCollected", 0f);
        Destroy(gameObject);
    }
}
