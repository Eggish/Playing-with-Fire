using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHealthCheck : MonoBehaviour
{
    [SerializeField] private GameObject FireToEnable = null;

    [SerializeField] private float HealthThreshold = 0.5f;

    private void OnCollisionEnter2D(Collision2D pCollision)
    {
        if (GameManager.FireHealth >= HealthThreshold)
        {
            FireToEnable.SetActive(true);
        }
        else
        {
            FireToEnable.SetActive(false);
        }

        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D pCollider)
    {
        if (GameManager.FireHealth >= HealthThreshold)
        {
            FireToEnable.SetActive(true);
        }
        else
        {
            FireToEnable.SetActive(false);
        }

        enabled = false;
    }
}
