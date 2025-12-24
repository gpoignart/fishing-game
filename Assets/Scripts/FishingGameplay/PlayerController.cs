using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Allow to call PlayerController.Instance anywhere (singleton)
    public static PlayerController Instance { get; private set; }

    // Game elements
    [SerializeField] private Transform fishContainer;
    [SerializeField] private Transform fishingRodTip;
    [SerializeField] private Transform fishCaughtAnchor;
    [SerializeField] private GameObject fishCaughtPrefab;

    // Sprites
    [SerializeField] private Sprite playerWaitingSprite;
    [SerializeField] private Sprite playerCatchingSprite;
    [SerializeField] private Sprite playerScaredSprite;
    [SerializeField] private Sprite playerLoseFishSprite;

    // Parameters
    private float detectionRadius = 0.4f;

    // Internal properties
    private float minX, maxX;
    private Vector3 firstPosition;
    private SpriteRenderer spriteRenderer;
    private GameObject fishCaught = null;

    // Make this class a singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Initialize player position
        transform.position = GameManager.Instance.FishingPlayerPosition;
        transform.localScale = GameManager.Instance.FishingPlayerOrientation;
        
        // Initialize internal references
        firstPosition = transform.position;
        spriteRenderer.sprite = playerWaitingSprite;

        CalculateScreenEdges();
    }

    public void UpdatePlayer()
    {
        HandleMovement();
        HandleFlip();

        // Update player position & orientation in game manager
        GameManager.Instance.UpdateFishingPlayerPosition(transform.position);
        GameManager.Instance.UpdateFishingPlayerOrientation(transform.localScale);

        // Check fish below
        Fish fishBelow = GetTopFishBelow();
        if (fishBelow)
        {
            FishingGameManager.Instance.PlayerAboveFish(fishBelow);
        }
        else
        {
            FishingGameManager.Instance.PlayerNotAboveFish();
        }
    }

    public IEnumerator OnCatchAnimation(FishSO fishCaughtSO, float duration)
    {
        spriteRenderer.sprite = playerCatchingSprite;

        // Instantiate fish
        fishCaught = Instantiate(fishCaughtPrefab, fishCaughtAnchor.position, fishCaughtPrefab.transform.rotation, fishCaughtAnchor);

        // Keep the fish toward left
        bool facingLeft = transform.localScale.x > 0;
        Vector2 scale = fishCaught.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingLeft ? 1f : -1f);
        fishCaught.transform.localScale = scale;

        fishCaught.GetComponent<SpriteRenderer>().sprite = fishCaughtSO.sprite;

        yield return new WaitForSeconds(duration);

        // Destroy the fish
        Destroy(fishCaught);
        fishCaught = null;
        spriteRenderer.sprite = playerWaitingSprite;
    }

    public IEnumerator OnLoseFishAnimation(float duration)
    {
        // Destroy the fish caught before changing sprite
        if (fishCaught != null) { Destroy(fishCaught); }

        spriteRenderer.sprite = playerLoseFishSprite;
        yield return new WaitForSeconds(duration);
        spriteRenderer.sprite = playerWaitingSprite;
    }

    public void OnMonsterApproach()
    {
        // Destroy the fish caught before changing sprite
        if (fishCaught != null) { Destroy(fishCaught); }

        spriteRenderer.sprite = playerScaredSprite;
    }

    // Calculate edges of screen to force the player inside when moving
    private void CalculateScreenEdges()
    {
        // Half width of the player
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float playerHalfWidth = sr.bounds.size.x / 2f;

        // Camera boundaries
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        // Edges left and right
        minX = cam.transform.position.x - camWidth / 2f + playerHalfWidth;
        maxX = cam.transform.position.x + camWidth / 2f - playerHalfWidth;
    }

    // Make player move left/right with horizontal inputs while staying within the screen
    private void HandleMovement()
    {
        // Take horizontal inputs (A/D or arrows)
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 newPosition = transform.position + Vector3.right * horizontal * GameManager.Instance.PlayerEquipmentRegistry.boatSO.speed * Time.deltaTime;

        // Clamp so the whole player stays visible
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        
        // Apply the new position
        transform.position = newPosition;

        // Inform the FishingGameManager the player has moved
        if (Vector3.Distance(transform.position, firstPosition) > 2f)
        {
            FishingGameManager.Instance.PlayerHasMoved();
        }
    }

    // Flip player
    private void HandleFlip()
    {
        if (!FishingGameManager.Instance.CanPlayerFlip()) { return; } // Verifies the player is allowed to flip

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            // Inform the FishingGameManager the player has flipped
            FishingGameManager.Instance.PlayerHasFlipped();
        }
    }

    // If the player is above a fish, return the fish (if there are several fish, get the top one)
    // Otherwise, return null
    private Fish GetTopFishBelow()
    {
        float fishingRodTipX = fishingRodTip.position.x;
        Fish topFish = null;
        float highestY = float.NegativeInfinity;

        foreach (Transform fishTransform in fishContainer)
        {
            float fishX = fishTransform.position.x;
            float fishY = fishTransform.position.y;

            if (Mathf.Abs(fishingRodTipX - fishX) <= detectionRadius)
            {
                if (fishY > highestY)
                {
                    highestY = fishY;
                    topFish = fishTransform.GetComponent<Fish>();
                }
            }
        }

        if (topFish != null && topFish.IsAvailable()) 
        { 
            return topFish;
        }
        else
        {
            return null;
        }
    }
}
