using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour
{
    public FishSO fishSO;

    private float lifeTime;
    private bool useLifeTime = true;

    // Called each time a new game object fish is instanciated
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = fishSO.sprite;

        // Make the fish disapear after a certain time
        lifeTime = Random.Range(10f, 25f);
        StartCoroutine(LifeRoutine());
    }

    public void DisableLifeTimeRoutine()
    {
        useLifeTime = false;
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        if ( useLifeTime ) { Destroy(gameObject); }
    }
}
