using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePuddleCutScene : MonoBehaviour
{
    [SerializeField] private Fire Fire = null;

    [SerializeField] private float GoalDistanceThreshold = 0.1f;

    [SerializeField] private PlayerController Paper = null;

    [SerializeField] private Transform FirstStop = null;
    [SerializeField] private float VelocityToFirstStop = 3.0f;
    [SerializeField] private float StopDelay = 1.0f;
    [SerializeField] private float PuddleJumpVelocity = 5.0f;

    [SerializeField] private GameObject Puddle = null;

    [SerializeField] private float WaterDamage = 0.25f;

    void Start()
    {
        if (Paper == null)
            Paper = FindObjectOfType<PlayerController>();

        if (Fire == null)
            Fire = FindObjectOfType<Fire>();
    }

    private IEnumerator PuddleCutScene()
    {
        Vector3 levelFirstStopPos = FirstStop.position;
        levelFirstStopPos.y = transform.position.y;
        while (Vector3.Distance(transform.position, levelFirstStopPos) > GoalDistanceThreshold)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, levelFirstStopPos, VelocityToFirstStop * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(StopDelay);

        Vector3 levelPuddlePos = Puddle.transform.position;
        levelPuddlePos.y = transform.position.y;
        while (Vector3.Distance(transform.position, levelPuddlePos) > GoalDistanceThreshold)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, levelPuddlePos, PuddleJumpVelocity * Time.deltaTime);
            yield return null;
        }

        Fire.ChangeHealth(-WaterDamage);
        Puddle.SetActive(false);

        Paper.enabled = true;
        enabled = false;
        yield return null;
    }

    private void OnEnable()
    {
        StartCoroutine(PuddleCutScene());
    }
}
