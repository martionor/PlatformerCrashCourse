using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;
    public Collider2D deathCollider;

    Animator animator;

    Rigidbody2D rb;

    Damageable damageable;

    Transform nextWaypoint;
    int waypoint = 0;

    public bool _hasTarget = false;
    private int waypointNum;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }


    //private void OnEnable()
    //{
    //    damageable.damageableDeath += OnDeath();
    //}

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.DetectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        //Fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;

        UpdateDirection();

        //See if we need to switch waypoints
        if (distance <= waypointReachedDistance)
        {
            //Switch to the next waypoint
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                //Loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }

    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if(transform.localScale.x > 0)
        {
            //Facing the right
            if(rb.velocity.x < 0)
            {
                //Flip direction
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            //Facing the left
            if(rb.velocity.x > 0)
            {
                //Flip direction
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        //Dead flyier falls to the ground
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
