using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour
{
    public static FlashlightController Instance { get; private set; }

    [SerializeField] private RectTransform beam; // UI flashlight beam (Image)
    [SerializeField] private Transform cam; // Main Camera

    // Parameters
    private float hitRadius = 150f; // Distance threshold for hit
    private float maxPan = 6f; // How far camera pans left/right
    private float followSpeed = 5f; // Camera movement smoothness

    // Internal attributes
    private RectTransform beamParent;
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

    public void StartFlashlight()
    {
        StartCoroutine(CenterMouseCoroutine());

        beam.sizeDelta = GameManager.Instance.PlayerEquipmentRegistry.flashlightSO.beamSize;
        ShowFlashlightBeam();
        beamParent = beam.parent as RectTransform;
        camStartX = cam.localPosition.x;
    }

    public void UpdateFlashlight()
    {
        UpdateBeamPosition();
        UpdateCameraPan();
        CheckHitMonster();
    }

    // Show and hide flashlight beam
    public void ShowFlashlightBeam()
    {
        beam.gameObject.SetActive(true);
    }

    public void HideFlashlightBeam()
    {
        beam.gameObject.SetActive(false);
    }

    private IEnumerator CenterMouseCoroutine()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }

    // Beam follows mouse inside the UI canvas
    private void UpdateBeamPosition()
    {
        Vector2 targetPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            beamParent,
            Input.mousePosition,
            Camera.main,
            out targetPos
        );

        beam.localPosition = Vector2.Lerp(
            beam.localPosition,
            targetPos,
            Time.deltaTime * GameManager.Instance.PlayerEquipmentRegistry.flashlightSO.beamFollowSpeed
        );
    }

    // Camera pans horizontally based on mouse X
    private void UpdateCameraPan()
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

    // Check if flashlight beam hits the monster
    private void CheckHitMonster()
    {
        if (MonsterGameManager.Instance.CurrentMonsterObj == null) { return; }
        
        // Convert monster world position → UI space
        Vector2 screenPos = Camera.main.WorldToScreenPoint(MonsterGameManager.Instance.CurrentMonsterObj.transform.position);

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
            MonsterGameManager.Instance.CurrentMonsterObj.GetComponent<Monster>().HitByFlashlight();
        }
    }
}
