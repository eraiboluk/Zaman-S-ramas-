using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInAnnularRegion : MonoBehaviour
{
    public GameObject[] prefabs;
    public int objectCount = 5;
    public float innerRadius = 3f;  // Inner radius of the annular region
    public float outerRadius = 7f;  // Outer radius of the annular region

    void Start()
    {
        SpawnObjectsInAnnularRegion();
    }

    void SpawnObjectsInAnnularRegion()
    {
        for (int i = 0; i < objectCount; i++)
        {
            GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];

            // Generate a random angle in radians
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            // Calculate random position within the annular region
            float randomRadius = Random.Range(innerRadius, outerRadius);
            float randomX = transform.position.x + randomRadius * Mathf.Cos(randomAngle);
            float randomZ = transform.position.z + randomRadius * Mathf.Sin(randomAngle);
            float randomY = transform.position.y; // You can adjust the Y position if needed

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            // Instantiate the selected object inside the annular region
            Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
        }
    }

}


