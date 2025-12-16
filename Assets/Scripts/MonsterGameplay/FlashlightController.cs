using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public static FlashlightController Instance { get; private set; }

    [Header("Beam Settings")]
    public RectTransform beam;        // UI flashlight beam (Image)
    public float hitRadius = 150f;    // Distance threshold for hit

    [Header("Camera Pan Settings")]
    public Transform cam;             // Main Camera
    public float maxPan = 6f;         // How far camera pans left/right
    public float followSpeed = 5f;    // Camera movement smoothness

    private Transform monsterTransform;       // Assigned at runtime
    private RectTransform beamParent;         // Parent canvas rect
    private float camStartX;

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
        beamParent = beam.parent as RectTransform;
        camStartX = cam.localPosition.x;
    }

    void Update()
    {
        UpdateBeamPosition();
        UpdateCameraPan();
        CheckHitMonster();
    }


    //Beam follows mouse inside the UI canvas
 
    void UpdateBeamPosition()
    {
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            beamParent,
            Input.mousePosition,
            Camera.main,
            out uiPos
        );

        beam.localPosition = uiPos;
    }

  
    //Camera pans horizontally based on mouse X

    void UpdateCameraPan()
    {
        float mouse01 = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
        float normalized = (mouse01 - 0.5f) * 2f;   // -1 to +1

        float targetX = camStartX + normalized * maxPan;

        cam.localPosition = Vector3.Lerp(
            cam.localPosition,
            new Vector3(targetX, cam.localPosition.y, cam.localPosition.z),
            Time.deltaTime * followSpeed
        );
    }

   
    //Check if flashlight beam hits the monster
  
    void CheckHitMonster()
    {
        if (monsterTransform == null) return;

        // Convert monster world position → UI space
        Vector2 screenPos = Camera.main.WorldToScreenPoint(monsterTransform.position);

        Vector2 monsterUIpos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            beamParent,
            screenPos,
            Camera.main,
            out monsterUIpos
        );

        float distance = Vector2.Distance(beam.localPosition, monsterUIpos);

        if (distance < hitRadius)
        {
            monsterTransform.GetComponent<TheEyes>().HitByFlashlight();
        }
    }


    // Public function called by MonsterGameManager
    public void SetMonster(Transform t)
    {
        monsterTransform = t;
    }
}
