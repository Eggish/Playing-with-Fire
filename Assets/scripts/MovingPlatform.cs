using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 DestinationDelta = Vector3.zero;
    [SerializeField] private float DistanceThreshold = 0.1f;
    [SerializeField] private float TimeToDestination = 1.0f;

    private Vector3 LerpStartPoint;
    private float CurrentLerpTimer;
    private Vector3 LerpDestination;


    void Start()
    {
        LerpDestination = transform.position + DestinationDelta;
        LerpStartPoint = transform.position;
    }
    private void FixedUpdate()
    {
        CurrentLerpTimer += Time.deltaTime;
        if (Vector3.Distance(transform.position, LerpDestination) > DistanceThreshold)
        {
            float percentageLerpDone = CurrentLerpTimer / TimeToDestination;
            transform.position = Vector3.Lerp(LerpStartPoint, LerpDestination, percentageLerpDone);
        }
        else
        {
            Vector3 temp = LerpDestination;
            LerpDestination = LerpStartPoint;
            LerpStartPoint = temp;
            CurrentLerpTimer = 0;
        }
    }
}
