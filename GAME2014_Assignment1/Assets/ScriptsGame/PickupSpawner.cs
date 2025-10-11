using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] GameObject pickupPrefab; 
    [SerializeField] Boundary horizontalBoundary; 
    [SerializeField] Boundary verticalBoundary;   

    [SerializeField] float spawnInterval = 3f;  
    [SerializeField] float pickupSpeed = 2f;    
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnPickup();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnPickup()
    {
        float randomX = Random.Range(horizontalBoundary.min, horizontalBoundary.max);
        Vector3 spawnPos = new Vector3(randomX, verticalBoundary.max, 0);
        GameObject newPickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        newPickup.AddComponent<PickupBehaviour>().Initialize(verticalBoundary, pickupSpeed);
    }
}
