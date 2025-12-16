using UnityEngine;

public class MonsterGameManager : MonoBehaviour
{
    // Singleton
    public static MonsterGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Monster")]
    public GameObject TheEyes;

    [Header("Spawn Zones")]
    public BoxCollider2D leftSpawn;
    public BoxCollider2D rightSpawn;

    private GameObject currentMonster;

    private float loseTime = 5f;
    private float loseTimer = 0f;
    private bool encounterActive = false;

    void Start()
    {
        StartEncounter();
    }

    public void StartEncounter()
    {
        int side = Random.Range(0, 2);
        Vector3 spawnPos = (side == 0)
            ? GetRandomPoint(leftSpawn)
            : GetRandomPoint(rightSpawn);

        currentMonster = Instantiate(TheEyes, spawnPos, Quaternion.identity);
        FlashlightController.Instance.SetMonster(currentMonster.transform);

        MonsterUIManager.Instance.ShowNoiseWarning(side);

        currentMonster.GetComponent<TheEyes>().Init(side);

        loseTimer = 0f;
        encounterActive = true;
    }

    private void Update()
    {
        if (!encounterActive) return;

        loseTimer += Time.deltaTime;

        if (loseTimer >= loseTime)
        {
            PlayerLose();
        }
    }

    private Vector3 GetRandomPoint(BoxCollider2D zone)
    {
        Bounds b = zone.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);

        return new Vector3(x, y, 0f);
    }

    public void PlayerWin()
    {
        encounterActive = false;
        GameManager.Instance.WinAgainstMonster();
    }

    public void PlayerLose()
    {
        encounterActive = false;
        GameManager.Instance.DeathAgainstMonster();
    }
}
