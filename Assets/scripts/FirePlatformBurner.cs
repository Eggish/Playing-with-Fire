using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class FirePlatformBurner : MonoBehaviour
{
    [SerializeField] private BurnablePlatform BurnablePlatform = null;
    [SerializeField] private float VelocityToPlatform = 3.0f;
    [SerializeField] private float PlatformDistanceThreshold = 1.0f;

    [SerializeField] private float RealizationDelay = 0.1f;

    [SerializeField] private float BackupDistance = 2.0f;
    [SerializeField] private float BackupVelocity = 5.0f;

    [SerializeField] private float WiggleDistance = 0.1f;
    [SerializeField] private int WiggleTimes = 5;

    [SerializeField] private float StopDelay = 0.3f;

    [SerializeField] private Transform StringStart = null;
    [SerializeField] private float FleeToStringVelocity = 7.0f;

    [SerializeField] private GameObject ExitString = null;

    [SerializeField] private Fire Fire = null;

    private bool ReachedPlatform = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D pCollider)
    {
        if (pCollider.CompareTag("Platform"))
        {
            ReachedPlatform = true;
        }
    }

    private IEnumerator PlatformBurnCutScene()
    {
        while (!ReachedPlatform)
        {
            Vector3 goalPosition = BurnablePlatform.transform.position;
            goalPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, VelocityToPlatform * Time.deltaTime);
            if (Vector3.Distance(goalPosition, transform.position) < PlatformDistanceThreshold)
            {
                ReachedPlatform = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(RealizationDelay);

        Vector3 backupPosition = transform.position - new Vector3(BackupDistance, 0, 0);

        while (Vector3.Distance(transform.position, backupPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, backupPosition, VelocityToPlatform * Time.deltaTime);
            yield return null;
        }

        Vector3 wigglePosition = transform.position - new Vector3(WiggleDistance, 0, 0);
        Vector3 originalPosition = transform.position;

        for (int i = 0; i < WiggleTimes; i++)
        {
            while (Vector3.Distance(transform.position, wigglePosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, wigglePosition, VelocityToPlatform * Time.deltaTime);
                yield return null;
            }

            Vector3 temp = wigglePosition;
            wigglePosition = originalPosition;
            originalPosition = temp;
        }
        
        yield return new WaitForSeconds(StopDelay);

        Vector3 levelStringStart = StringStart.position;
        levelStringStart.y = transform.position.y;
        while (Vector3.Distance(transform.position, levelStringStart) > 0.1f)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, levelStringStart, FleeToStringVelocity * Time.deltaTime);
            yield return null;
        }
        Fire.ExitLevel();
        Fire.ReEnablePaper();

        enabled = false;
        yield return null;
    }

    private void OnEnable()
    {
        StartCoroutine(PlatformBurnCutScene());
        ExitString.SetActive(true);
    }
}
