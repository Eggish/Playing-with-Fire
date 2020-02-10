using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform FollowTarget = null;
    [SerializeField] private Vector3 FollowOffset = new Vector3(5, 0, -10);
    private Vector3 LastVelocity = Vector3.zero;

    [SerializeField] private float LerpVelocity = 5.0f;

    [SerializeField] private bool FollowY = false;

    void Start()
    {
        MoveToTarget();
        if (FollowTarget == null)
        {
            FollowTarget = FindObjectOfType<PlayerController>().transform;
        }
    }

    void Update()
    {

    }

    private void MoveToTarget()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = FollowTarget.position.x + FollowOffset.x;
        if (FollowY)
        {
            targetPosition.y = FollowTarget.position.y + FollowOffset.y;
        }

        transform.position = targetPosition;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = FollowTarget.position.x + FollowOffset.x;
        if (FollowY)
        {
            targetPosition.y = FollowTarget.position.y + FollowOffset.y;
        }

        Vector3 lastPos = transform.position;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref LastVelocity, LerpVelocity * Time.deltaTime);
        
        LastVelocity = lastPos - transform.position;
    }
}
