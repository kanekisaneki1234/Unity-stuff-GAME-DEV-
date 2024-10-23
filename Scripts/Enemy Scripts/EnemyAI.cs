using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // public Transform target;
    // public float speed = 200f;
    // public float nextWaypointDistance = 3f;
    // SpriteRenderer sr;
    // Start is called before the first frame update

    [Header("Pathfinding")] 
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    // public float jumpNodeHeightRequirement = 0.8f;
    // public float JumpModitler = 0.3f;
    // public float jumpCheckOffset = 0.1f;

    Path path;
    int currentWaypoint = 0;
    // bool isGrounded = false;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // GameObject obj = GameObject.Find("Bee");

        // Access the SpriteRenderer component
        // sr = obj.GetComponent<SpriteRenderer>();
        // sr = GetComponent<SpriteRenderer>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (TargetInDistance())
        PathFollow();
    }
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }
    void PathFollow()
    {
        if(path==null)
            return;
        
        if(currentWaypoint>=path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance<nextWayPointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >=0.01f)
        {
            transform. localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(rb.velocity.x <= -0.01f)
        {
            transform. localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
