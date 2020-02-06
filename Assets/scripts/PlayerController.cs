﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    RIGHT,
    DOWN,
    LEFT
};


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rigidbody = null;

    [SerializeField] private bool CanMoveInAir = true;

    [SerializeField] private float MaxRunspeed = 5.0f;
    [SerializeField] private float SecondsToMaxSpeed = 0.5f;
    [SerializeField] private float SecondsToNoSpeed = 0.5f;

    [SerializeField] private KeyCode RightKey = KeyCode.D;
    [SerializeField] private KeyCode LeftKey = KeyCode.A;
    [SerializeField] private KeyCode JumpKey = KeyCode.W;

    [SerializeField] private float FireMaxBurnDistance = 4.0f;
    [SerializeField] private float FireMaxBurnVelocity = 2.0f;

    [SerializeField] private float JumpForce = 500.0f;

    private bool OnGround = false;

    [SerializeField] private Fire Fire = null;

    void Start()
    {
        if (Fire == null)
            Fire = FindObjectOfType<Fire>();
    }

    void Update()
    {
        Vector2 velocity = Rigidbody.velocity;

        if(Input.GetKey(RightKey))
        {
            velocity = RunAcceleration(velocity, Direction.RIGHT);
        }

        if (Input.GetKey(LeftKey))
        {
            velocity = RunAcceleration(velocity, Direction.LEFT);
        }

        if(OnGround && !Input.GetKey(LeftKey) && !Input.GetKey(RightKey))
        {
            velocity = RunDeceleration(velocity);
        }

        if(!OnGround && CanMoveInAir && !Input.GetKey(LeftKey) && !Input.GetKey(RightKey))
        {
            velocity = AirDeceleration(velocity);
        }

        if(Input.GetKeyDown(JumpKey))
        {
            Jump();
        }
        float distanceToFire = Vector2.Distance(transform.position, Fire.transform.position);
        if (distanceToFire < FireMaxBurnDistance)
        {
            Burn(distanceToFire);
        }

        Rigidbody.velocity = velocity;
    }

    private Vector2 RunAcceleration(Vector2 pVelocity, Direction pDirection)
    {
        if (!CanMoveInAir
           && !OnGround)
            return pVelocity;
        if (pVelocity.magnitude >= MaxRunspeed)
            return pVelocity;
        if (pDirection == Direction.RIGHT)
        {
            pVelocity += (Vector2.right * (MaxRunspeed / SecondsToMaxSpeed) * Time.deltaTime);
        }
        else if (pDirection == Direction.LEFT)
        {
            pVelocity += (Vector2.left * (MaxRunspeed / SecondsToMaxSpeed) * Time.deltaTime);
        }
        return pVelocity;
    }

    private Vector2 RunDeceleration(Vector2 pVelocity)
    {
        float velocityMagnitude = pVelocity.magnitude;
        velocityMagnitude -= (MaxRunspeed / SecondsToNoSpeed) * Time.deltaTime;
        if (velocityMagnitude < 0)
        {
            velocityMagnitude = 0;
        }
        pVelocity = pVelocity.normalized * velocityMagnitude;
        return pVelocity;
    }

    private Vector2 AirDeceleration(Vector2 pVelocity)
    {
        float horizontalMagnitude = pVelocity.magnitude;

        horizontalMagnitude -= (MaxRunspeed / SecondsToNoSpeed) * Time.deltaTime;
        if (horizontalMagnitude < 0)
        {
            horizontalMagnitude = 0;
        }

        pVelocity.x = pVelocity.normalized.x * horizontalMagnitude;
        return pVelocity;
    }

    private void Jump()
    {
        if(OnGround)
        {
            Rigidbody.AddForce(JumpForce * Vector2.up);
        }
    }

    private void OnCollisionEnter2D(Collision2D pCollision)
    {
        OnGround = true;
    }

    private void OnCollisionExit2D(Collision2D pCollision)
    {
        OnGround = false;
    }

    private void Burn(float pDistanceToFire)
    {
        float burnPercentage = FireMaxBurnDistance - pDistanceToFire / FireMaxBurnDistance;
        float scaleMagnitude = transform.localScale.magnitude;
        scaleMagnitude -= FireMaxBurnVelocity * burnPercentage * Time.deltaTime;
        transform.localScale = transform.localScale.normalized * scaleMagnitude;
    }
}