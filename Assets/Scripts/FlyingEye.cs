using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;
    


    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

        public bool _hasTarget = false;
    

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }
    // Update is called once per frame
    void Update()
    {
        HasTarget= biteDetectionZone.detectedColliders.Count > 0;
    }

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public void FixedUpdate()
    {
        if(damageable.IsAlive)
        {
            if(canMove)
            {
                flight();
            } else
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
      
    }

    private void flight()
    {
        // fly to next way point
      Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.linearVelocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        // see if we need to switch
        if(distance <= waypointReachedDistance)
        {
            // switch the next waypoint 
            waypointNum++;

            if(waypointNum>= waypoints.Count)
            {
                //loop back to original waypoint
                waypointNum=0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale=transform.localScale;

        if (transform.localScale.x > 0)
        {
            if (rb.linearVelocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb.linearVelocity.x >  0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        deathCollider.enabled = true;
    }
}
