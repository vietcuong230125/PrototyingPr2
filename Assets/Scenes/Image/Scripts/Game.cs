using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ENUM thể hiện các trạng thái nhân vật
public enum CharacterState
{
    Idle,
    Running,
    Jumping,
    DoubleJump,
    Falling,
    Attacking,
    AirborneAttack,
    Dashing,
    Rolling
}
public class Game : MonoBehaviour
{
    public float t2;

    private Player player;
    private CharacterState currentState;
    public Collider2D collision;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    public Animator animator;

    private Vector2 dashStartPos;
    private Vector2 dashEndPos;
    private bool canDash = true;
    private float dashTime;

    private Vector2 rollStartPos;
    private Vector2 rollEndPos;
    private bool canRoll = true;
    private float rollTime;

    public GameObject ghost;
    public float ghostDelay;
    private float ghostDelayTime;

    public int comboStep = 0;
    public float comboTimer = 0f;
    public float comboDelay = 0.58f;
    public bool canAttack = true;
    public float attackTimer = 0f;
    public float attackDelay = 0.35f;

    public bool canDoubleJump;

    public bool canAirborneAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

        currentState = CharacterState.Idle;

        // Khởi tạo player
        player = new Player(100, 50, 10, 20,4,7,4,0.3f,1,3.5f,0.5f,0.5f);
    }

    void Update() 
    {
        // Input chuyển động
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Xác định hướng của nhân vật ( 1 hướng phải -1 hướng trái )
        if (horizontalInput > 0)
        {
            player.xDirection = 1;
        }
        else if (horizontalInput < 0)
        {
            player.xDirection = -1;
        }
        // Hàm xử lý trạng thái của player
        HandleState(horizontalInput, verticalInput);      

    }
    public void HandleState(float horizontalInput, float verticalInput)
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                // ANIMATION
                animator.Play("idle");
                // XDIRECTION
                if (player.xDirection > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
                // RUN
                if (Mathf.Abs(horizontalInput) > 0.1f)
                {
                    ChangeState(CharacterState.Running);
                }
                // JUMP
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Jump();
                }
                // ROLL
                if (verticalInput < -0.1f && canRoll)
                {
                    Roll();
                }
                // ATTACK
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                }
                break;
            case CharacterState.Running:
                // ANIMATION
                animator.Play("run");
                // XDIRECTION
                if (player.xDirection > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
                // MOVE
                transform.Translate(new Vector3(horizontalInput * Time.deltaTime * player.moveSpeed, 0, 0));
                // IDLE
                if (Mathf.Abs(horizontalInput) < 0.1f)
                {
                    ChangeState(CharacterState.Idle);
                }
                // JUMP
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Jump();
                }
                // ROLL
                if (verticalInput < -0.1f && canRoll)
                {
                    Roll();
                }
                // ATTACK
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                }
                break;
            case CharacterState.Jumping:
                // ANIMATION
                animator.Play("jump");
                // XDIRECTION
                if (player.xDirection > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
                // MOVE
                transform.Translate(new Vector3(horizontalInput * Time.deltaTime * player.moveSpeed, 0, 0));
                // FALL
                if (rb.velocity.y < 0)
                {
                    ChangeState(CharacterState.Falling);
                }
                // DASH
                if (Input.GetKeyDown(KeyCode.E) && canDash)
                {
                    Dash();
                }
                // DOUBLEJUMP
                /*
                if (Input.GetKeyDown(KeyCode.W) && canDoubleJump)
                {
                /   DoubleJump();
                }
                */
                // AIRBORNEATTACK
                if (Input.GetMouseButtonDown(0) && canAirborneAttack && rb.velocity.y > -5)
                {
                    ChangeState(CharacterState.AirborneAttack);
                }

                break;
            case CharacterState.Falling:
                // ANIMATION
                animator.Play("fall");
                // XDIRECTION
                if (player.xDirection > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
                // MOVE
                transform.Translate(new Vector3(horizontalInput * Time.deltaTime * player.moveSpeed, 0, 0));
                // DASHING
                if (Input.GetKeyDown(KeyCode.E) && canDash)
                {
                    Dash();
                }
                // DOUBLEJUMP
                if (Input.GetKeyDown(KeyCode.W) && canDoubleJump)
                {
                    DoubleJump();
                }
                // AIRBORNEATTACK
                if (Input.GetMouseButtonDown(0) && canAirborneAttack && rb.velocity.y > -5)
                {
                    ChangeState(CharacterState.AirborneAttack);
                }
                break;
            case CharacterState.DoubleJump:
                // ANIMATION
                animator.Play("jump");
                // XDIRECTION
                if (player.xDirection > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
                // MOVE
                transform.Translate(new Vector3(horizontalInput * Time.deltaTime * player.moveSpeed, 0, 0));
                // FALL
                if (rb.velocity.y < 0)
                {
                    ChangeState(CharacterState.Falling);
                }
                // DASH
                if (Input.GetKeyDown(KeyCode.E) && canDash)
                {
                    Dash();
                }
                // AIRBORNEATTACK
                if (Input.GetMouseButtonDown(0) && canAirborneAttack && rb.velocity.y > -5)
                {
                    ChangeState(CharacterState.AirborneAttack);
                }
                break;            
            case CharacterState.Dashing:
                // ANIMATION
                animator.Play("dash");
                // DASH
                dashTime += Time.deltaTime;
                float t = dashTime / player.dashDuration;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.position = Vector2.Lerp(dashStartPos, dashEndPos, t);
                if (t >= 1f)
                {
                    EndDash();
                }
                // GHOST EFFECT
                if (ghostDelayTime > 0)
                {
                    ghostDelayTime -= Time.deltaTime;
                }
                else
                {
                    GameObject ghostInstance = Instantiate(ghost, transform.position, Quaternion.identity);
                    if (sp.flipX)
                    {
                        ghostInstance.transform.localScale = new Vector2(ghostInstance.transform.localScale.x * (-1), ghostInstance.transform.localScale.y);
                    }
                    Destroy(ghostInstance, 0.3f);
                    ghostDelayTime = ghostDelay;
                }
                break;
            case CharacterState.Rolling:
                // ANIMATION
                animator.Play("roll");
                rollTime += Time.deltaTime;
                t2 = rollTime / player.rollDuration;
                rb.position = Vector2.Lerp(rollStartPos, rollEndPos, t2);
                if (t2 >= 1f)
                {
                    EndRoll();
                }
                // XDIRECTION
                if (player.xDirection > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
                break;
            case CharacterState.Attacking:
                // IDLE
                if (comboTimer > 0)
                {
                    comboTimer -= Time.deltaTime;
                }
                else
                {
                    comboStep = 0;
                    ChangeState(CharacterState.Idle);
                }
                // ROLL
                if (verticalInput < -0.1f && canRoll)
                {
                    Roll();
                }
                // ATTACK
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                }
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    canAttack = true;
                }
                break;
            case CharacterState.AirborneAttack:
                // ANIMATION
                animator.Play("airborneattack");
                // VELOCITY
                rb.velocity = new Vector2(0, -0.3f);
                canAirborneAttack = false;
                break;
        }
    }



    // Phương thức thay đổi trạng thái
    public void ChangeState(CharacterState newState)
    {
        currentState = newState;
        Debug.Log("Trang Thai: " + currentState);
    }



    // JUMP
    public void Jump()
    {
        ChangeState(CharacterState.Jumping);
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);          
    }


    // DOUBLEJUMP
    public void DoubleJump()
    {
        ChangeState(CharacterState.DoubleJump);
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        canDoubleJump = false;
    }



    // ATTACK
    public void Attack()
    {
        ChangeState(CharacterState.Attacking);

        if (comboStep == 0 && canAttack)
        {
            animator.Play("attack1");
            comboStep = 1;
            comboTimer = comboDelay;
            attackTimer = attackDelay;
            canAttack = false;
        }
        else if (comboStep == 1 && canAttack)
        {
            animator.Play("attack2");
            comboStep = 2;
            comboTimer = comboDelay;
            attackTimer = attackDelay;
            canAttack = false;
        }
        else if (comboStep == 2 && canAttack)
        {
            animator.Play("attack3");
            comboStep = 0;
            comboTimer = comboDelay;
            attackTimer = attackDelay;
            canAttack = false;
        }
    }



    // AIRBORNEATTACK
    public void EndAirborneAttack()
    {
        ChangeState(CharacterState.Falling);
    }

    // DASH
    public void Dash()
    {
        ghostDelayTime = ghostDelay;
        ChangeState(CharacterState.Dashing);
        canDash = false;
        dashTime = 0f;
        dashStartPos = rb.position;
        dashEndPos = dashStartPos + new Vector2(player.xDirection * player.dashDistance, 0);
    }
    void EndDash()
    {
        ChangeState(CharacterState.Falling);
        Invoke("ResetDash", player.dashCooldown);
    }

    void ResetDash()
    {
        canDash = true;
    }



    //ROLL
    public void Roll()
    {
        ChangeState(CharacterState.Rolling); ;
        canRoll = false;
        rollTime = 0f;
        rollStartPos = rb.position;
        rollEndPos = rollStartPos + new Vector2(player.xDirection * player.rollDistance, 0);
    }
    void EndRoll()
    {
        ChangeState(CharacterState.Idle);
        Invoke("ResetRoll", player.rollCooldown);
    }
    void ResetRoll()
    {
        canRoll = true;
    }



    // ADD FORCE
    public void AddForceAttack()
    {
        rb.velocity = new Vector2(2 * player.xDirection, rb.velocity.y);
        //transform.Translate(new Vector2(1,0));
    }



    // Phương thức nhận sát thương
    /*
    public void TakeDamage(int damage)
    {
        if (player.isInvincible)
        {
            Debug.Log("Player is invincible and took no damage.");
        }
        else
        {
            int damageTaken = Mathf.Max(0, damage - player.armor);
            player.health -= damageTaken;
            Debug.Log("Player took " + damageTaken + " damage. Health left: " + player.health);
            if (player.health <= 0)
            {
                Debug.Log("Player died.");
                ChangeState(CharacterState.Idle);
            }
        }
    }
    */


    // Phương thức phát hiện va chạm với mặt đất
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ChangeState(CharacterState.Idle);
            canDoubleJump = true;
            canAirborneAttack = true;
        }
    }
}