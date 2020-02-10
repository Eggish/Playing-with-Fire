using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    [SerializeField] private PlayerController Paper = null;

    [SerializeField] private Transform LeftMaximum = null;
    [SerializeField] private Transform RightMaximum = null;

    [SerializeField] private float FollowSpeed = 4.0f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 goalPosition = transform.position;
        goalPosition.x = Mathf.Clamp(Paper.transform.position.x, LeftMaximum.transform.position.x, RightMaximum.transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, goalPosition, FollowSpeed * Time.deltaTime);
    }
}
