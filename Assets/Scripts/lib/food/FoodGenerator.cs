using UnityEngine;
public class FoodGenerator : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject player;
    public float radius = 10f;
    public Terrain terrain;
    public float surfaceYOffset = 0.05f;
    public float spawnIntervalSeconds = 1f;
    public int maxFoodPerBatch = 10;
    public bool autoStart = true;

    private void OnEnable()
    {
        if (autoStart) StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    private void OnDestroy()
    {
        StopSpawning();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            StopSpawning();
        }
        else if (autoStart && isActiveAndEnabled)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (foodPrefab == null || terrain == null)
        {
            Debug.LogWarning("FoodGenerator requires foodPrefab and terrain assigned.");
            return;
        }
        if (!IsInvoking(nameof(GenerateFoodBatch)))
        {
            // Optional immediate spawn
            InvokeRepeating(nameof(GenerateFoodBatch), spawnIntervalSeconds, spawnIntervalSeconds);
        }
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(GenerateFoodBatch));
    }

    public void GenerateFoodBatch()
    {
        for (int i = 0; i < maxFoodPerBatch; i++)
        {
            GenerateSingleFood();
        }
    }

    public void GenerateSingleFood()
    {
        if (foodPrefab == null || terrain == null) return;

        TerrainData td = terrain.terrainData;
        if (td == null) return;

        Vector3 terrainOrigin = terrain.transform.position;
        Vector3 terrainSize = td.size;

        // float x = Random.Range(terrainOrigin.x, terrainOrigin.x + terrainSize.x);
        // float z = Random.Range(terrainOrigin.z, terrainOrigin.z + terrainSize.z);
        float x = Random.Range(player.transform.position.x - radius, player.transform.position.x + radius);
        float z = Random.Range(player.transform.position.z - radius, player.transform.position.z + radius);
        float y = terrain.SampleHeight(new Vector3(x, 0f, z)) + terrainOrigin.y + surfaceYOffset;

        Vector3 spawnPos = new Vector3(x, y, z);
        GameObject food = Instantiate(foodPrefab, spawnPos, Quaternion.identity, transform);
        food.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }
}