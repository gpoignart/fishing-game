using UnityEngine;
using System.Collections;

public class TheEyes : MonoBehaviour
{
    public Sprite eyesNormal;   // default open eyes
    public Sprite eyesSquint;   // squint eyes sprite

    private SpriteRenderer sr;
    private bool isHit = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(int side)
    {
        // Eyes appear immediately but very faint
        sr.sprite = eyesNormal;
        sr.color = new Color(1f, 1f, 1f, 0.2f);
    }

    public void HitByFlashlight()
    {
        if (!isHit)
        {
            isHit = true;
            StartCoroutine(EyesReaction());
        }
    }

    IEnumerator EyesReaction()
    {
        // Brighten eyes quickly
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            float alpha = Mathf.Lerp(0.2f, 1f, t);
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // Switch sprite → squint eyes
        sr.sprite = eyesSquint;
        yield return new WaitForSeconds(0.15f);

        // Fade out
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * 3f;
            float alpha = Mathf.Clamp01(t);
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // 4) End encounter
        MonsterGameManager.Instance.PlayerWin();
    }
}
