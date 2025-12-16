using UnityEngine;

using System.Linq;

public class FishSpawner : MonoBehaviour
{
    // Allow to call FishSpawner.Instance anywhere (singleton)
    public static FishSpawner Instance { get; private set; }

    [SerializeField]
    private GameObject fishPrefab;

    [SerializeField]
    private BoxCollider2D spawnZone;

    [SerializeField]
    private Transform fishContainer;

    // Parameters
    private int minFish = 3;
    private int maxFish = 7;
    private int targetFishCount = 5;
    private float updateInterval = 5f;
    private float minDistBetweenFish = 2f;

    // Internal references
    private Vector2 zoneSize;
    private Vector2 zoneOffset;

    
    // Make this class a singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Make one fish spawn for the tutorial
    public void SpawnTutorialFish()
    {
        zoneSize = spawnZone.size;
        zoneOffset = spawnZone.offset;

        // Spawn position at the right of the screen
        Vector2 localPoint = new Vector2( zoneSize.x / 2f - 0.5f, zoneSize.y / 2f - 0.5f) + zoneOffset;
        Vector2 spawnPosition = (Vector2) spawnZone.transform.position + localPoint;

        // Tutorial fish
        FishSO tutorialFish = GameManager.Instance.FishRegistry.carpSO;

        GameObject newFish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity, fishContainer);
        
        newFish.GetComponent<Fish>().fishSO = tutorialFish;
    }

    // Regular fish spawner
    public void StartFishSpawner()
    {
        zoneSize = spawnZone.size;
        zoneOffset = spawnZone.offset;

        InvokeRepeating(nameof(ManageFishCount), 0f, updateInterval);
    }

    private void ManageFishCount()
    {
        int currentFishCount = fishContainer.childCount;

        // Add fish if we don't have enough to reach the targetFishCount
        if (currentFishCount < targetFishCount)
        {
            int fishToSpawn = targetFishCount - currentFishCount;
            for (int i = 0; i < fishToSpawn; i++)
                SpawnFish();
        }

        // Change randomly the targetFishCount
        targetFishCount = Random.Range(minFish, maxFish + 1);
    }

    private void SpawnFish()
    {
        Vector2 spawnPosition = selectSpawnPosition();

        FishSO fish = selectFish();

        GameObject newFish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity, fishContainer);
        
        newFish.GetComponent<Fish>().fishSO = fish;
    }

    // Select random and valid spawn position (in the spawn zone and fish not too close from each other)
    private Vector2 selectSpawnPosition()
    {
        Vector2 spawnPosition;
        bool validPosition = false;

        do
        {
            float randomX = Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f);
            float randomY = Random.Range(-zoneSize.y / 2f, zoneSize.y / 2f);

            Vector2 localPoint = new Vector2(randomX, randomY) + zoneOffset;
            spawnPosition = (Vector2) spawnZone.transform.position + localPoint;

            validPosition = true;

            // Check the distance with all fish already spawned
            foreach (Transform fish in fishContainer)
            {
                if (Vector2.Distance(fish.position, spawnPosition) < minDistBetweenFish)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        return spawnPosition;
    }

    // Select a valid fish type to be spawned + depending of the spawn chance
    private FishSO selectFish()
    {
        // Filter valid types fish depending of the map and time of the day
        FishSO[] validFishes = GameManager.Instance.FishRegistry.AllFish
            .Where(f => (f.spawnMaps.Contains(GameManager.Instance.CurrentMap)
                     && f.spawnTimes.Contains(GameManager.Instance.CurrentTimeOfDay)))
            .ToArray();

        // Tirage pond�r� selon spawnChance
        int totalWeight = validFishes.Sum(f => f.spawnChance);
        int rand = Random.Range(0, totalWeight);
        FishSO selectedType = null;

        foreach (var fish in validFishes)
        {
            if (rand < fish.spawnChance)
            {
                selectedType = fish;
                break;
            }
            rand -= fish.spawnChance;
        }

        return selectedType;
    }
}
