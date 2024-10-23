using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    // [HideInInspector]
    // public float speed;
    // public PlayerReference playerData;
    private enum State
    {
        Roaming,
        Chase
    }
    private string CHASE_ANIMATION_ENEMY = "Chase";
    private string DEATH_ANIMATION_ENEMY = "IsDead";
    private string ATTACK_ANIMATION_ENEMY = "Attack";
    Animator anim;
    Seeker seeker;
    Rigidbody2D rb;
    State state;
    private string GROUND_TAG="Ground";
    // public GameObject player;
    // public playerref playerLoc;
    
    [Header("Pathfinding")] 
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    // public float moveSpeed = 3f; // Movement speed of the mob
    // public GameObject player;
    // Transform playerLoc;
    public static Vector3 enemyLoc;
    public static bool hasRecoil = false;
    // public Transform spawn;
    private Vector3 spawn;
    private Vector3 point1;
    private Vector3 point2;
    Vector3 destination;
    private float bounds;
    // public bool trigger=false;
    // public BoarSpawner original;
    Path path;
    int currentWaypoint = 0;
    // bool reachedEndOfPath = false;

    [Header("Ground Mob")]
    public float jumpNodeHeightRequirement = 0.8f;
    public float JumpModifier = 0.3f;
    public float jumpCheckOffset = 3f;
    public float raycastLength = 0.7f;
    bool isGrounded = false;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    [Header("Behaviour")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int damage=10;
    public bool groundMob = false;
    
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        // enemyBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        // player = GameObject.FindWithTag("Player");
        // if(!player)
        // {
        //     // playerLoc=original.spawnLoc;
        //     return;
        // }
        // playerLoc = player.transform;

        currentHealth = maxHealth;

        enemyLoc = transform.position;

        spawn = AstarPath.active.data.gridGraph.center;
        point1 = spawn + Vector3.right * 10f + Vector3.up * 5; // 10 units to the right
        point2 = spawn + Vector3.left * 10f + Vector3.up * 5; // 10 units to the left

        destination = point1;
        // transform.localScale = new Vector3(-5f, 5f, 1f);
        // Debug.Log(bounds);
        float gridWidth = AstarPath.active.data.gridGraph.width;

            // Calculate the extreme right point based on the grid dimensions and cell size
        float cellSize = AstarPath.active.data.gridGraph.nodeSize;

        Vector3 extremeRightPoint = new(gridWidth * cellSize / 2f, 0f, 0f);

        bounds = Vector3.Distance(spawn, extremeRightPoint);
        // Debug.Log(bounds);
        // Debug.Log(spawn);

        if(TargetInDistance()&&(Vector3.Distance(spawn,target.position)<bounds))
            InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        InvokeRepeating("DebugFunc", 0f, 1f);
    }

    public void Update()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                if(!groundMob)
                    anim.SetBool(ATTACK_ANIMATION_ENEMY, false);
                anim.SetBool(CHASE_ANIMATION_ENEMY, false);
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed/100);
                if (rb.velocity.x >=0.01f)
                {
                    transform.localScale = new Vector3(5f, 5f, 1f);
                }
                else if(rb.velocity.x <= -0.01f)
                {
                    transform.localScale = new Vector3(-5f, 5f, 1f);
                }
                if (Vector3.Distance(transform.position, destination) < 0.1f)
                {
                    if (destination==point1)
                    {
                        // Debug.Log("yuh");
                        destination = point2;
                        transform.localScale = new Vector3(5f, 5f, 1f);
                    }
                    // else
                    else if (destination==point2)
                    {
                        destination = point1;
                        transform.localScale = new Vector3(-5f, 5f, 1f);
                        // Debug.Log("nuh");
                    }
                }
                FindTarget();
                // Debug.Log(TargetInDistance());
                break;

            case State.Chase:
                if(!groundMob)
                    anim.SetBool(ATTACK_ANIMATION_ENEMY, false);
                anim.SetBool(CHASE_ANIMATION_ENEMY, true);
                FindTarget();
                if (Vector3.Distance(transform.position, point1) < Vector3.Distance(transform.position, point2))
                    destination = point1;
                else
                    destination = point2;
                if (Vector3.Distance(transform.position,target.position) <= 7 && !groundMob)
                    anim.SetBool(ATTACK_ANIMATION_ENEMY, true);
                break;
        }  
    }
    void DebugFunc()
    {
        // Debug.Log(isGrounded);
        // // Debug.Log(GetComponent<Collider2D>().bounds.extents.y);
        // Debug.Log(target.position.y - transform.position.y);
        // // Debug.Log(Math.Abs(transform.position.y - target.position.y) > 1.5 && Math.Abs(transform.position.y - target.position.y) < jumpCheckOffset);

        // if ((target.position.y - transform.position.y) > 1.5)
        //     Debug.Log(true);
        // else
        //     Debug.Log(false);

        // if((target.position.y - transform.position.y) < jumpCheckOffset)
        //     Debug.Log(true);
        // else
        //     Debug.Log(false);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        // Debug.Log("Eyah");
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
            // Debug.Log("Eyah 2");
        }
    }
    void LateUpdate()
    {
        // PlayerFollow();
        // Debug.Log(isGrounded);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance())
        PathFollow();
    }
    private bool TargetInDistance()
    {
        return (Vector3.Distance(transform.position, target.transform.position) < activateDistance)&&(Vector3.Distance(spawn,target.position)<bounds);
    }

    private void FindTarget() 
    {
        // Vector3 bottomRight = graphBounds.max;

        if (TargetInDistance())
            state = State.Chase;
        else
            state = State.Roaming;
    }

    
    void PathFollow()
    {
        if(path==null)
            return;
        
        if(currentWaypoint>=path.vectorPath.Count)
        {
            // reachedEndOfPath = true;
            return;
        }
        else
        {
            // reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = speed * Time.deltaTime * direction;
        rb.AddForce(force);
        // Debug.Log(force);
        // Debug.Log(speed);
        // Debug.Log(direction);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance<nextWayPointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >=0.01f)
        {
            transform.localScale = new Vector3(-5f, 5f, 1f);
        }
        else if(rb.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(5f, 5f, 1f);
        }

        // float raycastLength = GetComponent<Collider2D>().bounds.extents.y;
        RaycastHit2D hit = Physics2D.Raycast (transform.position,-Vector2.up, raycastLength, LayerMask.GetMask("Obstacle"));
        Debug.DrawRay(transform.position, -Vector2.up * raycastLength, Color.red);

        if (hit.collider != null && hit.collider.CompareTag(GROUND_TAG)) 
            isGrounded = true;
        // else
        //     isGrounded = false;

        if(isGrounded)
        {
            // // if (Math.Abs(transform.position.y - target.position.y) > 1.5 && Math.Abs(transform.position.y - target.position.y) < jumpCheckOffset)
            // if ((target.position.y - transform.position.y) > 1.5 && (target.position.y - transform.position.y) < jumpCheckOffset)
            // {
            //     // rb.AddForce(Vector2.up * speed/10 * JumpModifier);
            //     rb.AddForce(new Vector2(0f, JumpModifier), ForceMode2D.Impulse);
            //     isGrounded=false;
            // }   
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(new Vector2(0f, JumpModifier), ForceMode2D.Impulse);
                isGrounded=false;
            }
        }
        // Debug.Log(Math.Abs(transform.position.y - target.position.y));
        // Debug.Log(Math.Abs(transform.position.y - target.position.y)> 1.5);
        // Debug.Log(Math.Abs(transform.position.y - target.position.y)<jumpCheckOffset);
        Debug.Log(direction.y);
        Debug.Log(direction.y>jumpNodeHeightRequirement);
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.gameObject.CompareTag(GROUND_TAG))
    //     {
    //         isGrounded=true;
    //     }
    // }
    // public void PlayerFollow()
    // {
    //     if(!player)
    //     {
    //         return;
    //     }
    //         // Calculate the distance between the mob and the player
    //     if(player)
    //     {
    //         // if(!Player.isRecoiling)
    //         {    
    //             float distanceToPlayer = Vector3.Distance(transform.position,playerLoc.position);

    //             // Check if the player is within the vicinity range
    //             if(Player.Health>0 && Player.coolDown == false)
    //             {
    //                 if (distanceToPlayer <= vicinityRange)
    //                 {
    //                     // Move towards the player's position
    //                     // Vector3 direction = (playerLoc.position - transform.position).normalized;
    //                     Vector3 direction = new Vector3(Mathf.Sign(playerLoc.position.x - transform.position.x), 0f, 0f);
    //                     transform.position += speed * Time.deltaTime * direction;
    //                     anim.SetBool(SPRINT_ANIMATION_ENEMY, true);
    //                 }
    //                 else
    //                 {
    //                     anim.SetBool(SPRINT_ANIMATION_ENEMY, false);
    //                 }
    //             }
    //             else
    //             {
    //                 anim.SetBool(SPRINT_ANIMATION_ENEMY, false);
    //             }
    //             if(playerLoc.position.x>transform.position.x)
    //             {
    //                 sr.flipX=true;
    //             }
    //             else
    //             {
    //                 sr.flipX=false;
    //             }
    //         }
    //     }
    // }
    public void TakeDamage(int Damage)
    {
        currentHealth-=Damage;
        anim.SetTrigger("Damage");
        if (currentHealth<=0)
        {
            Die();
        }
    }
    // IEnumerator Cooldown()
    // {
    //     anim.SetBool(SPRINT_ANIMATION_ENEMY, false);
    //     enemyBody.constraints= RigidbodyConstraints2D.FreezePositionX;
    //     yield return new WaitForSeconds(10f);
    // }
    void Die()
    {
        rb.gravityScale=1;
        anim.SetBool(DEATH_ANIMATION_ENEMY, true);
        // enemyBody.mass=0;
        // enemyBody.bodyType = RigidbodyType2D.Static;
        // GetComponent<PolygonCollider2D>().enabled=false;
        gameObject.layer = LayerMask.NameToLayer("Dead Mobs");
        this.enabled=false;
    }
}
    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     // myBody.velocity = new Vector2(speed, myBody.velocity.y);
    //     // anim.SetBool(SPRINT_ANIMATION_ENEMY, true);

    // }
    //Idle, Sprint, Damage, IsDead