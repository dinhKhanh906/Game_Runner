using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public float spawnRate = 30f; // percent

    public bool Spawn(Transform pointHolder)
    {
        float randomResult = Random.Range(0, 100f);

        if(randomResult <= spawnRate)
        {
            int index = Random.Range(0, powerUpPrefabs.Length);
            Instantiate(powerUpPrefabs[index], pointHolder.position, Quaternion.identity, pointHolder);

            return true;
        }

        return false;
    }
}
