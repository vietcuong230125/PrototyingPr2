using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy1State
{
    Idle,
    Walk,
    Attacking1,
    Attacking2
}
public class ENEMY1 : MonoBehaviour
{
    private Enemy enemy;
    private Enemy1State currentState;
    private Game Character;
    private SpriteRenderer sp;
    public Animator animator;
    public float attackDelayTime1 = 1.5f;
    public float attackDelayTime2 = 1.5f;
    public bool canAttack;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        Character = FindObjectOfType<Game>();
        enemy = new Enemy(10, 5, 2);
        currentState = Enemy1State.Walk;
    }

    void Update()
    {
        if (enemy.xDirection == 1)
        {
            sp.flipX = true;
        }
        else if (enemy.xDirection == -1)
        {
            sp.flipX = false;
        }
        HandleState();
    }

    public void HandleState()
    {
        switch (currentState)
        {
            case Enemy1State.Idle:
                break;
            case Enemy1State.Walk:
                // ANIMATION
                animator.Play("walk");
                // MOVE
                transform.Translate(new Vector3(Time.deltaTime * enemy.moveSpeed * enemy.xDirection, 0, 0));
                // POSITION
                if (Character.transform.position.x < transform.position.x)
                {
                    enemy.xDirection = -1;
                }
                else if (Character.transform.position.x > transform.position.x)
                {
                    enemy.xDirection = 1;
                }
                // ATTACK
                if (canAttack)
                {
                    currentState = Enemy1State.Attacking1;
                }
                break;
            case Enemy1State.Attacking1:
                // ANIMATION
                animator.Play("attack1");
                // ATTACK2 || WALK
                attackDelayTime1 -= Time.deltaTime;
                if (canAttack && attackDelayTime1 < 0)
                {                   
                    attackDelayTime1 = 1.5f;
                    // POSITION
                    if (Character.transform.position.x < transform.position.x)
                    {
                        enemy.xDirection = -1;
                    }
                    else if (Character.transform.position.x > transform.position.x)
                    {
                        enemy.xDirection = 1;
                    }
                    currentState = Enemy1State.Attacking2;
                }
                else if (!canAttack && attackDelayTime1 < 0)
                {
                    attackDelayTime1 = 1.5f;
                    currentState = Enemy1State.Walk;
                }

                break;
            case Enemy1State.Attacking2:
                // ANIMATION
                animator.Play("attack2");              
                // ATTACK1 || WALK
                attackDelayTime2 -= Time.deltaTime;
                if (canAttack && attackDelayTime2 < 0)
                {
                    attackDelayTime2 = 1.5f;
                    // POSITION
                    if (Character.transform.position.x < transform.position.x)
                    {
                        enemy.xDirection = -1;
                    }
                    else if (Character.transform.position.x > transform.position.x)
                    {
                        enemy.xDirection = 1;
                    }
                    currentState = Enemy1State.Attacking1;                
                }
                else if (!canAttack && attackDelayTime2 < 0)
                {
                    attackDelayTime2 = 1.5f;
                    currentState = Enemy1State.Walk;
                }
                break;
        }
    }

    public void ChangeState(Enemy1State newState)
    {
        currentState = newState;
        Debug.Log("Trang Thai: " + currentState);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canAttack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canAttack = false;
        }
    }
}
