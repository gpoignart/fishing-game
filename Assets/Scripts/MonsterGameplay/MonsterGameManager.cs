using UnityEngine;
using System.Collections;

public class MonsterGameManager : MonoBehaviour
{
    // Singleton
    public static MonsterGameManager Instance { get; private set; }

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

    // Game elements
    [SerializeField] private BoxCollider2D leftSpawn;
    [SerializeField] private BoxCollider2D rightSpawn;
    [SerializeField] private Transform monsterContainer;
    
    // Parameters
    [SerializeField] private float loseTime = 3f;
    private float warningDuration = 0.7f;
    private float winMessageDuration = 2f;

    // Internal references
    private float loseTimer;
    private bool isTimerActive;
    private bool isFlashlightActive;

    // Attributes
    private GameObject currentMonsterObj;
    
    // READ-ONLY ATTRIBUTES, CAN BE READ ANYWHERE
    public GameObject CurrentMonsterObj => currentMonsterObj;

    
    // Tutorial states
    private enum MonsterTutorialState
    {
        Start,
        NoFlashlight1,
        NoFlashlight2,
        FlashlightMonster,
        MonsterRanAway,
        End
    }
    private MonsterTutorialState currentTutorialState = MonsterTutorialState.Start;


    // Start encounter
    private void Start()
    {
        // Tutorial initialization
        if (GameManager.Instance.IsFirstNight)
        {
            // Begin tutorial
            ChangeTutorialState(MonsterTutorialState.NoFlashlight1);
        }
        // Regular nights
        else
        {
            // Hide elements
            MonsterUIManager.Instance.HideTutorialPanel();
            MonsterUIManager.Instance.HideMonsterRanAwayText();

            // Position depending on side
            Vector3 spawnPos = (GameManager.Instance.MonsterApparitionSide == 0)
                ? GetRandomPoint(leftSpawn)
                : GetRandomPoint(rightSpawn);

            // Select randomly a monster to spawn
            MonsterSO[] allMonsters = GameManager.Instance.MonsterRegistry.AllMonsters;
            MonsterSO currentMonsterSO = allMonsters[Random.Range(0, allMonsters.Length)];

            // Spawn the monster
            currentMonsterObj = Instantiate(currentMonsterSO.monsterPrefab, spawnPos, Quaternion.identity, monsterContainer);

            // Show noise UI
            StartCoroutine(MonsterUIManager.Instance.ShowNoiseWarningForSeconds(GameManager.Instance.MonsterApparitionSide, warningDuration));

            // Start timer
            loseTimer = 0f;
            isTimerActive = true;

            // Start flashlight
            FlashlightController.Instance.StartFlashlight();
            isFlashlightActive = true;
        }
    }

    private void Update()
    {
        if (isFlashlightActive)
        {
            FlashlightController.Instance.UpdateFlashlight(loseTimer, loseTime);
        }

        if (isTimerActive)
        {
            loseTimer += Time.deltaTime;
            if (loseTimer >= loseTime)
            {
                PlayerLose();
            }
        }

        // Handle the game update logic for tutorial states
        switch (currentTutorialState)
        {
            case MonsterTutorialState.NoFlashlight1:
                if (Input.GetKeyDown(KeyCode.Return)) { OnTutorialNextButtonPressed(); } // Click on next button with return
                break;
            
            case MonsterTutorialState.NoFlashlight2:
                if (Input.GetKeyDown(KeyCode.Return)) { OnTutorialNextButtonPressed(); } // Click on next button with return
                break;

            case MonsterTutorialState.MonsterRanAway:
                if (Input.GetKeyDown(KeyCode.Return)) { OnTutorialNextButtonPressed(); } // Click on next button with return
                break;
        }
    }

    public void StopMonsterTimer()
    {
        isTimerActive = false;
    }

    public void OnTutorialNextButtonPressed()
    {   
        if (currentTutorialState == MonsterTutorialState.NoFlashlight1)
        {
            ChangeTutorialState(MonsterTutorialState.NoFlashlight2);
        }
        else if (currentTutorialState == MonsterTutorialState.NoFlashlight2)
        {
            ChangeTutorialState(MonsterTutorialState.FlashlightMonster);
        }
        else if (currentTutorialState == MonsterTutorialState.MonsterRanAway)
        {
            ChangeTutorialState(MonsterTutorialState.End);
        }
        AudioManager.Instance.PlayPressingButtonSFX();
    }

    public void PlayerWin()
    {        
        if (GameManager.Instance.IsFirstNight && currentTutorialState == MonsterTutorialState.FlashlightMonster)
        {
            ChangeTutorialState(MonsterTutorialState.MonsterRanAway);
        }
        else
        {
            isTimerActive = false;
            MonsterUIManager.Instance.HideSpotTheMonsterText();
            StartCoroutine(PlayPlayerWin());
        }
    }

    public IEnumerator PlayPlayerWin()
    {
        yield return new WaitForSeconds(0.5f);
        MonsterUIManager.Instance.ShowMonsterRanAwayText();
        yield return new WaitForSeconds(winMessageDuration);
        GameManager.Instance.WinAgainstMonster();
    }

    public void PlayerLose()
    {
        GameManager.Instance.DeathAgainstMonster();
    }


    // HELPING FUNCTIONS

    private Vector3 GetRandomPoint(BoxCollider2D zone)
    {
        Bounds b = zone.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);

        return new Vector3(x, y, 0f);
    }

    
    // HANDLE TUTORIAL STATES
    
    // Pass from one state to another
    private void ChangeTutorialState(MonsterTutorialState newTutorialState)
    {
        // Exit logic for the previous state
        OnTutorialStateExit(currentTutorialState);

        currentTutorialState = newTutorialState;

        // Enter logic for the new state
        OnTutorialStateEnter(currentTutorialState);
    }

    // Handle the game logic when entering states
    private void OnTutorialStateEnter(MonsterTutorialState state)
    {
        switch (state)
        {
            case MonsterTutorialState.Start:
                break;

            case MonsterTutorialState.NoFlashlight1:
                MonsterTutorialUIManager.Instance.ShowNoFlashlight1TutorialStepUI();
                break;
            
            case MonsterTutorialState.NoFlashlight2:
                MonsterTutorialUIManager.Instance.ShowNoFlashlight2TutorialStepUI();
                break;

            case MonsterTutorialState.FlashlightMonster:
                MonsterTutorialUIManager.Instance.ShowFlashlightMonsterTutorialStepUI();
                FlashlightController.Instance.StartFlashlight();
                isFlashlightActive = true;
                break;

            case MonsterTutorialState.MonsterRanAway:
                MonsterTutorialUIManager.Instance.ShowMonsterRanAwayTutorialStepUI();
                MonsterUIManager.Instance.HideNoiseWarning();
                break;
            
            case MonsterTutorialState.End:
                MonsterUIManager.Instance.HideTutorialPanel();
                GameManager.Instance.WinAgainstMonster();
                break;
        }
    }

    // Handle the game logic when exiting states
    private void OnTutorialStateExit(MonsterTutorialState state)
    {
        switch (state)
        {
            case MonsterTutorialState.Start:
                // Hide elements
                MonsterUIManager.Instance.HideSpotTheMonsterText();
                MonsterUIManager.Instance.HideMonsterRanAwayText();

                // Position depending on side
                Vector3 spawnPos = (GameManager.Instance.MonsterApparitionSide == 0)
                    ? GetRandomPoint(leftSpawn)
                    : GetRandomPoint(rightSpawn);

                // Tutorial monster
                MonsterSO currentMonsterSO = GameManager.Instance.MonsterRegistry.theEyesSO;

                // Spawn the monster
                currentMonsterObj = Instantiate(currentMonsterSO.monsterPrefab, spawnPos, Quaternion.identity, monsterContainer);

                // Show noise UI
                MonsterUIManager.Instance.ShowNoiseWarning(GameManager.Instance.MonsterApparitionSide);
                
                // No timer & no flashlight
                loseTimer = 0f;
                isTimerActive = false;
                isFlashlightActive = false;
                FlashlightController.Instance.HideFlashlightBeam();
                break;
            
            case MonsterTutorialState.NoFlashlight1:
                MonsterTutorialUIManager.Instance.HideNoFlashlight1TutorialStepUI();
                break;
            
            case MonsterTutorialState.NoFlashlight2:
                MonsterTutorialUIManager.Instance.HideNoFlashlight2TutorialStepUI();
                break;

            case MonsterTutorialState.FlashlightMonster:
                MonsterTutorialUIManager.Instance.HideFlashlightMonsterTutorialStepUI();
                break;

            case MonsterTutorialState.MonsterRanAway:
                MonsterTutorialUIManager.Instance.HideMonsterRanAwayTutorialStepUI();
                break;

            case MonsterTutorialState.End:
                break;
        }
    }
}
