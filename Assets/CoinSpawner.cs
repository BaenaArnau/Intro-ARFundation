using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CoinSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject coinPrefab;
    public ARPlaneManager planeManager;

    [Header("Spawn")]
    [Min(1)] public int maxActiveCoins = 8;
    [Min(0.01f)] public float spawnInterval = 0.4f;
    public float coinHeightOffset = 0.03f;

    [Header("UI")]
    public TMP_Text counterText;
    public Text legacyCounterText;

    private readonly List<ARPlane> trackedPlanes = new List<ARPlane>();
    private float nextSpawnTime;
    private int activeCoins;
    private int collectedCoins;

    private void Awake()
    {
        if (planeManager == null)
        {
            planeManager = FindAnyObjectByType<ARPlaneManager>();
        }

        UpdateCounterUI();
    }

    private void Update()
    {
        if (coinPrefab == null || planeManager == null)
        {
            return;
        }

        if (Time.time < nextSpawnTime || activeCoins >= maxActiveCoins)
        {
            return;
        }

        nextSpawnTime = Time.time + spawnInterval;

        if (TryGetRandomPointOnTrackedPlane(out Vector3 spawnPoint, out Quaternion rotation))
        {
            SpawnCoin(spawnPoint + (Vector3.up * coinHeightOffset), rotation);
        }
    }

    public void NotifyCoinCollected()
    {
        activeCoins = Mathf.Max(0, activeCoins - 1);
        collectedCoins++;
        UpdateCounterUI();
    }

    private void SpawnCoin(Vector3 position, Quaternion rotation)
    {
        GameObject coinObject = Instantiate(coinPrefab, position, rotation);

        coin coinComponent = coinObject.GetComponent<coin>();
        if (coinComponent == null)
        {
            coinComponent = coinObject.AddComponent<coin>();
        }

        coinComponent.Initialize(this);
        activeCoins++;
    }

    private bool TryGetRandomPointOnTrackedPlane(out Vector3 worldPosition, out Quaternion worldRotation)
    {
        trackedPlanes.Clear();

        foreach (ARPlane plane in planeManager.trackables)
        {
            if (plane.trackingState == TrackingState.Tracking)
            {
                trackedPlanes.Add(plane);
            }
        }

        if (trackedPlanes.Count == 0)
        {
            worldPosition = default;
            worldRotation = Quaternion.identity;
            return false;
        }

        ARPlane selectedPlane = trackedPlanes[Random.Range(0, trackedPlanes.Count)];

        if (TryGetPointInPlaneBoundary(selectedPlane, out Vector3 point))
        {
            worldPosition = point;
            worldRotation = Quaternion.LookRotation(selectedPlane.transform.forward, selectedPlane.transform.up);
            return true;
        }

        worldPosition = selectedPlane.center;
        worldRotation = Quaternion.LookRotation(selectedPlane.transform.forward, selectedPlane.transform.up);
        return true;
    }

    private bool TryGetPointInPlaneBoundary(ARPlane plane, out Vector3 worldPoint)
    {
        NativeArray<Vector2> boundary = plane.boundary;

        if (!boundary.IsCreated || boundary.Length < 3)
        {
            worldPoint = plane.center;
            return false;
        }

        Vector2 min = boundary[0];
        Vector2 max = boundary[0];

        for (int i = 1; i < boundary.Length; i++)
        {
            Vector2 point = boundary[i];
            min = Vector2.Min(min, point);
            max = Vector2.Max(max, point);
        }

        for (int attempt = 0; attempt < 20; attempt++)
        {
            Vector2 localPoint = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            if (!IsPointInsidePolygon(localPoint, boundary))
            {
                continue;
            }

            worldPoint = plane.transform.TransformPoint(new Vector3(localPoint.x, 0f, localPoint.y));
            return true;
        }

        worldPoint = plane.center;
        return false;
    }

    private static bool IsPointInsidePolygon(Vector2 point, NativeArray<Vector2> polygon)
    {
        bool inside = false;

        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[j];

            bool crossesEdge = (a.y > point.y) != (b.y > point.y);
            if (!crossesEdge)
            {
                continue;
            }

            float xIntersect = ((b.x - a.x) * (point.y - a.y) / (b.y - a.y)) + a.x;
            if (point.x < xIntersect)
            {
                inside = !inside;
            }
        }

        return inside;
    }

    private void UpdateCounterUI()
    {
        string counterValue = $"Monedas: {collectedCoins}";

        if (counterText != null)
        {
            counterText.text = counterValue;
        }

        if (legacyCounterText != null)
        {
            legacyCounterText.text = counterValue;
        }
    }
}
