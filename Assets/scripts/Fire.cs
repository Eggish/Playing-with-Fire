using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private PlayerController Paper = null;
    [SerializeField] private FirePuddleCutScene FirePuddleCutScene = null;

    [SerializeField] private GameObject ConditionalFire = null;

    [SerializeField] private Transform ExitPoint = null;
    [SerializeField] private float ExitVelocity = 2.0f;

    [SerializeField] private float GoalDistanceThreshold = 0.1f;
    [SerializeField] private float SmokeDeactivationDelay = 1.0f;

    [SerializeField] private GameObject Smoke = null;

    void Start()
    {
        transform.localScale = new Vector3(GameManager.FireHealth, GameManager.FireHealth, GameManager.FireHealth);
        if(Paper == null)
            Paper = FindObjectOfType<PlayerController>();
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
        transform.position = Vector2.MoveTowards(transform.position, pWaypoint.position, pMovementSpeed * Time.deltaTime);
    }

    private bool IsGoalReached(Transform pWaypoint)
    {
        if(Vector3.Distance(pWaypoint.position, transform.position) <= GoalDistanceThreshold)
        {
            return true;
        }
        return false;
    }

    private IEnumerator LeaveOnString(Vector3 pExitPoint, float pVelocity)
    {
        while (Vector3.Distance(transform.position, pExitPoint) > GoalDistanceThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, pExitPoint, pVelocity * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    public void StartPuddleCutScene()
    {
        FirePuddleCutScene.enabled = true;
    }

    public void ExitLevel()
    {
        StartCoroutine(LeaveOnString(ExitPoint.position, ExitVelocity));
    }

    public void ChangeHealth(float pHealthChange)
    {
        GameManager.FireHealth += pHealthChange;
        
        Smoke.SetActive(true);

        Invoke("DeactivateSmoke", SmokeDeactivationDelay);
        transform.localScale = new Vector3(GameManager.FireHealth, GameManager.FireHealth, GameManager.FireHealth);
        if (ConditionalFire != null
            && pHealthChange > 0)
        {
            ConditionalFire.SetActive(true);
        }
    }

    private void DeactivateSmoke()
    {
        Smoke.SetActive(false);
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(GameManager.FireHealth, GameManager.FireHealth, GameManager.FireHealth);
    }
}
