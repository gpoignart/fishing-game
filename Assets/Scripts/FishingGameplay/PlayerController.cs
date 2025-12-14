using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Allow to call PlayerController.Instance anywhere (singleton)
    public static PlayerController Instance { get; private set; }

    [SerializeField]
    private Transform fishContainer;

    [SerializeField]
    private Transform fishingRodTip;

    private float detectionRadius = 0.4f;
    private float speed = 2f;
    private float minX, maxX;

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

    void Start()
    {
        CalculateScreenEdges();
    }

    public void UpdatePlayer()
    {
        HandleMovement();
        HandleFlip();

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
        Vector3 newPosition = transform.position + Vector3.right * horizontal * speed * Time.deltaTime;

        // Clamp so the whole player stays visible
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
    }

    // Flip player
    private void HandleFlip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
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
        return topFish;
    }
}
