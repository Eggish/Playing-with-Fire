using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSequenceTrigger : MonoBehaviour
{
    [SerializeField] private FirePlatformBurner FirePlatformBurner = null;

    // Start is called before the first frame update
    void Start()
    {
        if (FirePlatformBurner == null)
        {
            FirePlatformBurner = FindObjectOfType<FirePlatformBurner>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D pCollider)
    {
        FirePlatformBurner.enabled = true;
        enabled = false;
        Destroy(gameObject);
    }
}
