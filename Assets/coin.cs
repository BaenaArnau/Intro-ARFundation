using UnityEngine;

public class coin : MonoBehaviour
{
    private MonoBehaviour owner;
    private bool wasCollected;

    public void Initialize(MonoBehaviour spawner)
    {
        owner = spawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryCollect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryCollect(collision.gameObject);
    }

    private void TryCollect(GameObject other)
    {
        if (wasCollected)
        {
            return;
        }

        // Accept collider on the car itself or on child objects.
        Coche car = other.GetComponentInParent<Coche>();
        if (car == null)
        {
            return;
        }

        wasCollected = true;
        owner?.Invoke("NotifyCoinCollected", 0f);
        Destroy(gameObject);
    }
}
