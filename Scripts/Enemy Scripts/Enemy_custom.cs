using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// using System.Numerics;
using UnityEngine;

public class Enemy_custom : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chase
    }

    private string IDLE_ANIMATION_ENEMY = "Idle";
    private string CHASE_ANIMATION_ENEMY = "Chase";
    private string DEATH_ANIMATION_ENEMY = "IsDead";
    // private string ATTACK_ANIMATION_ENEMY = "Attack";
    
    // Roaming
    private Vector3 spawn;
    private Vector3 point1;
    private Vector3 point2;
    Vector3 destination;
    Animator anim;
    // Seeker seeker;
    Rigidbody2D rb;
    State state;
    Transform target;
    private Vector3 face_direction;
    private float angle;

    [Header("Behaviour")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int damage=10;
    public float activateDistance = 6f;
    public float roamDelay = 3f; // Adjust as needed
    private bool isRoaming = false;
    public static Vector3 enemyLoc;
    public static bool hasRecoil = false;

    [Header("Jump Mechanism")]
    public float jumpNodeHeightRequirement = 0.8f;
    public float JumpModifier = 0.3f;
    public float jumpCheckOffset = 3f;
    public float raycastLength = 0.7f;
    public float raycastHeight = 4f;
    Vector2 facingDirection;
    bool isGrounded = false;

    [Header("Physics")]
    public float speed = 200f;

    void Awake()
    {
        // seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        enemyLoc = transform.position;

        // spawn = new Vector3(3.68f, transform.position.y, 0);
        // point1 = spawn + Vector3.right * 10f;
        // point2 = spawn + Vector3.left * 10f;

        // destination = point1;
    }
    // void FacePlayersDirection()
    // {
    //     face_direction = target.position - transform.position;
    //     angle = Mathf.Atan2(face_direction.y, face_direction.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    // }

    void Update()
    {
        EnemyHeal();
        // FacePlayersDirection();
        Vector2 raycastOrigin = (Vector2)transform.position + Vector2.up * raycastHeight;

        spawn = new Vector3(3.68f, transform.position.y, 0);
        point1 = spawn + Vector3.right * 10f;
        point2 = spawn + Vector3.left * 10f;
        
        facingDirection = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, facingDirection, raycastLength, LayerMask.GetMask("Player"));
        Debug.DrawRay(raycastOrigin,facingDirection*raycastLength, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            // Debug.Log("Yuh");
        }

        switch (state)
        {
            default:
            case State.Roaming:
                // // anim.SetBool(ATTACK_ANIMATION_ENEMY, false);
                // // spawn = new Vector3(3.68f, transform.position.y, 0);
                // // point1 = spawn + Vector3.right * 10f;
                // // point2 = spawn + Vector3.left * 10f;
                // // destination = point1;
                // // Vector2 face_direction = destination - transform.position;
                // // rb.velocity = direction * speed * Time.deltaTime;

                // float angle = Mathf.Atan2(face_direction.y, face_direction.x)*Mathf.Rad2Deg; //this is the angle that the weapon must rotate around to face the cursor
                // Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.up); //Vector 3. forward is z axis
                // transform.rotation = rotation;
                RoamingBehavior();
                break;

            case State.Chase:
                ChaseBehavior();
                break;
        }

    }
    void FixedUpdate()
    {
        
    }

    void RoamingBehavior()
    {
        FindTarget();
        anim.SetBool(CHASE_ANIMATION_ENEMY, false);
        if(isRoaming)
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 3);
        
        if (Vector2.Distance(transform.position, destination) < 0.1f)
            StartCoroutine(RoamingCoroutine());

        if(!TargetInDistance())
        {
            if (destination == point1 && transform.position.x > point1.x)
                transform.localScale = new Vector3(5f, 5f, 1f);
            else if(destination == point2 && transform.position.x < point2.x)
                transform.localScale  = new Vector3(-5f, 5f, 1f);
        }
    }

    IEnumerator RoamingCoroutine()
    {
        if(!TargetInDistance())
        {
            isRoaming = false;
            anim.SetBool(IDLE_ANIMATION_ENEMY, true);
            yield return new WaitForSeconds(5);
            anim.SetBool(IDLE_ANIMATION_ENEMY, false);
            if (destination==point1)
            {
                destination = point2;
                transform.localScale = new Vector3(5f, 5f, 1f);
            }
            else if (destination==point2)
            {
                destination = point1;
                transform.localScale = new Vector3(-5f, 5f, 1f);
            }
        }
        else
        {
            anim.SetBool(CHASE_ANIMATION_ENEMY, true);
        }
        isRoaming = true;           
    }

    void ChaseBehavior()
    {
        FindTarget();
        anim.SetBool(CHASE_ANIMATION_ENEMY, true);
        if (transform.position.x < target.position.x)
            transform.localScale = new Vector3(-5, 5, 1);
        else
            transform.localScale = new Vector3(5, 5, 1);
        Vector2 direction = new(target.transform.position.x - rb.position.x, rb.position.y);
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        
        if (Vector3.Distance(transform.position, point1) < Vector3.Distance(transform.position, point2))   
            destination = point1;
        else
            destination = point2;
    }

    private bool TargetInDistance()
    {
        return Vector3.Distance(transform.position, target.transform.position) < activateDistance;
    }

    void FindTarget()
    {
        if (TargetInDistance())
            state = State.Chase;
        else
        {
            isRoaming = true;
            state = State.Roaming;
        }
    }

    void EnemyHeal()
    {
        if(Input.GetKeyDown(KeyCode.E))
            currentHealth += 30;
    }

}
