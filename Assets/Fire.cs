using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private enum FireState
    {
        WADDLE_IN,
        FALL_DOWN,
        END_STATE
    }

    [SerializeField] private List<Transform> WaypointList = new List<Transform>();

    [SerializeField] private float GoalDistanceThreshold = 0.1f;
    [SerializeField] private float MovementSpeed = 1.0f;
    [SerializeField] private float RotationSpeed = 180.0f;

    [SerializeField] private PlayerController Paper = null;

    private float Health = 1.0f;

    private FireState CurrentState = FireState.WADDLE_IN;

    private Transform CurrentWaypoint = null;

    void Start()
    {
        CurrentWaypoint = WaypointList[0];
    }

    void Update()
    {
        switch(CurrentState)
        {
            case FireState.WADDLE_IN:
                MoveToWaypoint(CurrentWaypoint);
                if (IsGoalReached(CurrentWaypoint))
                {
                    StartCoroutine(FallDown());
                    NextState();
                }
                break;
           case FireState.END_STATE:
                break;
        }
        transform.localScale = new Vector3(Health, Health, Health);
    }

    private void MoveToWaypoint(Transform pWaypoint)
    {
        if (pWaypoint == null)
        {
            Debug.LogWarning("Waypoint is not assigned");
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, pWaypoint.position, MovementSpeed * Time.deltaTime);
    }

    private bool IsGoalReached(Transform pWaypoint)
    {
        if(Vector3.Distance(pWaypoint.position, transform.position) <= GoalDistanceThreshold)
        {
            return true;
        }
        return false;
    }

    private void NextState()
    {
        CurrentState++;
        if (WaypointList.Count > 1)
        {
            WaypointList.RemoveAt(0);
            CurrentWaypoint = WaypointList[0];
        }
    }

    private IEnumerator FallDown()
    {

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.z += 90;
        Quaternion rotationGoal = transform.rotation;
        rotationGoal.eulerAngles = eulerRotation;

        Quaternion originalRotation = transform.rotation;


        while(Vector3.Distance(transform.rotation.eulerAngles, rotationGoal.eulerAngles) >= 1.0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationGoal, RotationSpeed * Time.deltaTime);
            yield return null;
        }
        Health -= 0.5f;
        yield return new WaitForSeconds(1.0f);
        while (Vector3.Distance(transform.rotation.eulerAngles, originalRotation.eulerAngles) >= 1.0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, RotationSpeed * Time.deltaTime);
            yield return null;
        }

        NextState();
        Paper.enabled = true;
        yield return null;

    }
}
