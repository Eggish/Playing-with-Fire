using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnablePlatform : MonoBehaviour
{
    [SerializeField] private float FireSpreadDelay = 0.2f;

    [SerializeField] private float SmokeScreenTime = 0.5f;

    [SerializeField] private List<GameObject> FireObjects = new List<GameObject>();
    [SerializeField] private GameObject DisappearingSmoke = null;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFire()
    {
        StartCoroutine(BurnPlatform());
    }

    private void OnTriggerEnter2D(Collider2D pCollider)
    {
        StartFire();
    }

    private IEnumerator BurnPlatform()
    {
        foreach (GameObject g in FireObjects)
        {
            g.SetActive(true);
            yield return new WaitForSeconds(FireSpreadDelay);
        }
        GetComponent<MovingPlatform>().enabled = false;
        DisappearingSmoke.SetActive(true);
        yield return new WaitForSeconds(SmokeScreenTime);
        gameObject.SetActive(false);
        yield return null;
    }
}
