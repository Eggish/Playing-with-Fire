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

    [SerializeField] private PlayerController Paper = null;

    [SerializeField] private List<Transform> WaypointList = new List<Transform>();

    [SerializeField] private float GoalDistanceThreshold = 0.1f;
    [SerializeField] private float MovementSpeed = 1.0f;

    [SerializeField] private float SmokeDeactivationDelay = 1.0f;

    [SerializeField] private GameObject Smoke = null;


    private Transform CurrentWaypoint = null;

    void Start()
    {
        CurrentWaypoint = WaypointList[0];
        transform.localScale = new Vector3(GameManager.FireHealth, GameManager.FireHealth, GameManager.FireHealth);
    }

    void Update()
    {
        
    }

    private void MoveToWaypoint(Transform pWaypoint, float pMovementSpeed)
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

    public void ChangeHealth(float pHealthChange)
    {
        GameManager.FireHealth += pHealthChange;
        Smoke.SetActive(true);

        Invoke("DeactivateSmoke", SmokeDeactivationDelay);
        transform.localScale = new Vector3(GameManager.FireHealth, GameManager.FireHealth, GameManager.FireHealth);
    }

    private void DeactivateSmoke()
    {
        Smoke.SetActive(false);
    }
}
