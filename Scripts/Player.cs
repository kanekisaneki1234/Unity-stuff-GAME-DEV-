using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveforce = 10f;
    public float jumpforce = 7f;

    // [SerializeField]
    float movementX;
    [SerializeField]
    private Rigidbody2D myBody;
    private Animator anim;
    private SpriteRenderer sr;
    private string SPRINT_ANIMATION = "Sprint";
    private string JUMP_ANIMATION = "Jump";
    private string DEATH_ANIMATION = "Death";
    private bool isGrounded = true;
    private string GROUND_TAG="Ground";
    private string MOB_TAG="Hostile Mob";
    public static int Health = 100;
    public static int playerDamage = 50;
    Enemy enemy_;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public float nextAttackTime =0f;
    public float attackRate = 2f;
    public static bool coolDown=false;

    // float raycastLength;

    // public float recoilSpeed = 100000f; // Speed of the recoil motion
    // public float recoilDuration = 0.001f; // Duration of the recoil effect
    // private Vector3 originalPosition; // Original position of the player GameObject
    // public static bool isRecoiling = false; // Flag to track if the player is currently recoiling
    // private float recoilStartTime; // Time when the recoil started
    // private Vector3 recoilDirection;
    // public float recoilDuration = 0.5f; // Time for recoil effect
    private bool isRecoiling = false;
    // private Vector3 recoilVelocity;
    public float recoilForce = 7f;
    // private SceneController scInstance;
    
    // public delegate void PlayerDeath();
    // public static event PlayerDeath PlayerDeathInfo;
    // public PlayerReference playerData;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemy_ = FindObjectOfType<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // MakeshiftSC.PlayAgainPressed += Respawn;
        // raycastLength = GetComponent<Collider2D>().bounds.extents.y;
        // InvokeRepeating("DebugFunc", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerMovementKeyboard();
        PlayerJump();
        PlayerAttack();
        AnimatePlayer();
        PlayerHeal();
        // RaycastHit2D hit = Physics2D.Raycast (transform.position,-Vector2. up, raycastLength, LayerMask.GetMask("Obstacle"));
        // Debug.DrawRay(transform.position, -Vector2.up * raycastLength, Color.red);

        // if (hit.collider != null && hit.collider.CompareTag(GROUND_TAG)) 
        // {    
        //     isGrounded = true;
        //     moveforce = 10f;
        //     anim.SetBool(JUMP_ANIMATION, false);
        // }
        // // else
        // // {
        // //     isGrounded = false;
        // // }
        // Debug.DrawRay(transform.position, -Vector3.up * raycastLength, Color.red);
        // if (isRecoiling)
        // {
        //     // Calculate the current recoil position based on the elapsed time
        //     float recoilProgress = (Time.time - recoilStartTime) / recoilDuration;
        //     Vector3 targetPosition = originalPosition + recoilProgress * recoilSpeed * recoilDirection;

        //     // Move the player towards the recoil direction
        //     transform.position = Vector3.MoveTowards(transform.position, targetPosition, recoilSpeed * Time.deltaTime);

        //     // Check if recoil is complete
        //     if (recoilProgress >= 1f)
        //     {
        //         isRecoiling = false;
        //     }
        // }
    }

    // private void FixedUpdate()
    // {
    //     PlayerJump();
    // }

    // void OnEnable()
    // {
    //     Listener.PlayerDeathInfo += Death;
    // }
    // void OnDisable()
    // {
    //     Listener.PlayerDeathInfo -= Death;
    // }
    // void DebugFunc()
    // {
    //     Debug.Log(isGrounded);
    // }
    void PlayerMovementKeyboard()
    {
        if (!isRecoiling)
        {
            if(Health>0)
            {
                movementX = Input.GetAxisRaw("Horizontal");
                // Debug.Log(movementX);
                transform.position += moveforce * Time.deltaTime * new Vector3(movementX, 0f, 0f);
            }
            else
            {
                movementX=0;
            }
        }
    }

    void AnimatePlayer()
    {
        if (Health>0)
        {
            if(movementX!=0)
            {
                anim.SetBool(SPRINT_ANIMATION, true);
                // sr.flipX = false;
                if(movementX<0)
                {
                    // anim.SetBool(SPRINT_ANIMATION, true);
                    sr.flipX = true;
                }
                else if(movementX>0)
                {
                    // anim.SetBool(SPRINT_ANIMATION, true);
                    sr.flipX = false;
                }
            }
            else
            {
                anim.SetBool(SPRINT_ANIMATION, false);
            }
        }
        else
        {    
            anim.SetBool(SPRINT_ANIMATION, false);
            anim.SetBool(JUMP_ANIMATION, false);
            anim.SetBool(DEATH_ANIMATION, true);
        }
    }
    void PlayerJump()
    {
        if(Input.GetButtonDown("Jump")&&isGrounded)
        {
            // Debug.Log("Jump");
            anim.SetBool(JUMP_ANIMATION, true);
            moveforce=8f;
            myBody.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
            isGrounded=false;
        }
    }
    void PlayerAttack()
    {
        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                anim.SetTrigger("Attack");
                
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
                
                foreach(Collider2D enemy in hitEnemies)
                {
                    // if(enemy_!=null)
                    {    
                        enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
                        // enemy_.TakeDamage(playerDamage);
                        if(enemy_.currentHealth>0)
                            StartCoroutine(DamageCooldown());
                    }
                }
                nextAttackTime = Time.time + 1f/attackRate;
            }
        }
    }

    void PlayerHeal()
    {
        if(Input.GetKeyDown(KeyCode.P))
            Health+=30;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(GROUND_TAG))
        {
            isGrounded=true;
            moveforce=10f;
            anim.SetBool(JUMP_ANIMATION, false);
        }
        if(collision.gameObject.CompareTag(MOB_TAG))
        {
            if(Health>0)
            {
                if(coolDown==false)
                    Health-=enemy_.damage;

                // Destroy(gameObject);
                // Destroy(gameObject, "death animation time");
                // OnDestroy
                // Vector3 damageDirection = (transform.position - Enemy.enemyLoc).normalized;
                // Pass the damage direction to the player's recoil handler
                // ******* ApplyRecoil(Enemy.enemyLoc);

                StartCoroutine(DamageCooldown());
                StartCoroutine(RecoilEffect());
            }
        }
        // if(Health<=0)
        // {
        //     // SceneController.fadeControl.SetTrigger("Fadeout");
        //     // SceneManager.LoadScene("GameOver");
        //     // Destroy(gameObject);
        //     // Destroyed=true;
        //     // PlayerDeathInfo?.Invoke();
        //     // SceneManager.LoadScene("GameOver");
        //     // SceneController scInstance = new(); 
        //     // movementX = 0;
        //     // Destroy(this);
        //     // Listener.Death
        //     anim.SetBool(DEATH_ANIMATION, true);
        // }
    }
    public IEnumerator DamageCooldown()
    {
        coolDown=true;
        yield return new WaitForSeconds(2f);
        coolDown=false;
    }
    // public void ApplyRecoil(Vector3 enemyTransform)
    // {
        // if (!isRecoiling)
        // {
        //     recoilDirection = (transform.position - enemyTransform).normalized;
        //     recoilStartTime = Time.time;
        //     isRecoiling = true;
        // }
    // }
    public void ApplyRecoil(Vector3 damageDirection)
    {
        // Calculate recoil velocity based on damage direction
        // recoilVelocity = -damageDirection.normalized * recoilForce;

        // // Start the recoil effect
        // StartCoroutine(RecoilEffect());
        if (Enemy.hasRecoil)
            myBody.AddForce(-damageDirection.normalized * recoilForce, ForceMode2D.Impulse);
    }

    IEnumerator RecoilEffect()
    {
        isRecoiling = true;
        // implement if player is ahead of the enemy then +7 else -7
        myBody.AddForce(new Vector2(-7f, 0), ForceMode2D.Impulse);

        yield return new WaitForSeconds(2);

        isRecoiling = false;
    }
    

    // public void Respawn()
    // {
    //     Health = 100;
    // }
    // void OnDestroy()
    // {
        // triggerSet.trigger = true;
    // }
    // void Death()
    // {
    //     anim.SetBool(DEATH_ANIMATION, true);
    // }
}
