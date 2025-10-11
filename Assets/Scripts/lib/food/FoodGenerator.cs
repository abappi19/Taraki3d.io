using UnityEngine;
public class FoodGenerator : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject player;
    public float radius = 10f;
    public LayerMask groundLayer;
    public float surfaceYOffset = 0.05f;
    public float spawnIntervalSeconds = 5;
    public int maxFoodPerBatch = 10;
    public bool autoStart = true;
    public int maxTotalFood = 100;
    public int minTotalFood = 90;

    private bool maxLimitReached = false;

    private void Start()
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
        if (foodPrefab == null) return;
        if (!IsInvoking(nameof(InvokableGenerateFoodBatch)))
        {
            // Optional immediate spawn
            InvokeRepeating(nameof(InvokableGenerateFoodBatch), spawnIntervalSeconds, spawnIntervalSeconds);
        }
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(InvokableGenerateFoodBatch));
    }

    public void InvokableGenerateFoodBatch()
    {
        int totalFood = GameObject.FindGameObjectsWithTag("Food").Length;

        if (totalFood >= maxTotalFood)
        {
            maxLimitReached = true;
            return;
        }

        if (maxLimitReached)
        {
            if (totalFood >= minTotalFood) return;
            maxLimitReached = false;
        }


        int randomMaxFoodPerBatch = Random.Range(1, maxFoodPerBatch + 1);
        for (int i = 0; i < randomMaxFoodPerBatch; i++)
        {
            GenerateSingleFood();
        }
    }


    public void GenerateFoodBatch(Vector3? origin = null, bool ignoreLimit = false, int? randomFoodCount = null, float maxScale = 0.25f, float minScale = 0.05f)
    {
        int totalFood = GameObject.FindGameObjectsWithTag("Food").Length;

        if (!ignoreLimit)
        {
            if (totalFood >= maxTotalFood)
            {
                maxLimitReached = true;
                return;
            }

            if (maxLimitReached)
            {
                if (totalFood >= minTotalFood) return;
                maxLimitReached = false;
            }
        }


        int randomMaxFoodPerBatch = randomFoodCount == null ? Random.Range(1, maxFoodPerBatch + 1) : randomFoodCount.Value;
        for (int i = 0; i < randomMaxFoodPerBatch; i++)
        {
            Vector3? randomPosition = origin;
            if (randomPosition != null)
            {
                float smallerRadius = 0.15f;
                float randomX = Random.Range(origin.Value.x - smallerRadius, origin.Value.x + smallerRadius);
                float randomZ = Random.Range(origin.Value.z - smallerRadius, origin.Value.z + smallerRadius);
                randomPosition = new Vector3(randomX, origin.Value.y - surfaceYOffset, randomZ);
            }

            GenerateSingleFood(randomPosition, maxScale, minScale);
        }
    }


    private Vector3? GetRandomPosition()
    {

        // float x = Random.Range(terrainOrigin.x, terrainOrigin.x + terrainSize.x);
        // float z = Random.Range(terrainOrigin.z, terrainOrigin.z + terrainSize.z);
        float x = Random.Range(player.transform.position.x - radius, player.transform.position.x + radius);
        float z = Random.Range(player.transform.position.z - radius, player.transform.position.z + radius);
        Vector3 origin = new Vector3(x, 100f, z);
        Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 100f, groundLayer);
        if (hit.collider != null)
        {
            return hit.point;
        }

        return null;
    }

    public void GenerateSingleFood(Vector3? position = null, float maxScale = 0.25f, float minScale = 0.05f)
    {
        if (foodPrefab == null) return;

        Vector3? spawnPos = position ?? GetRandomPosition();
        if (spawnPos == null) return;
        GameObject food = Instantiate(foodPrefab, spawnPos.Value, Quaternion.identity, transform);
        float randomScale = Random.Range(minScale, maxScale);
        //random color 
        Color randomColor = ColorUtil.GetRandomColor();
        food.GetComponent<Renderer>().material.color = randomColor;
        food.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}