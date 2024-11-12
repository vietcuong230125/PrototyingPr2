using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private SpriteRenderer sp;
    private Rigidbody2D rb;
    public bool DkJump = true;
    public int xDirection;
    public Animator animator;
    public string animationName;
    private float time1;
    private bool Dk;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveRL = Input.GetAxis("Horizontal");
        float moveUD = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(moveRL * Time.deltaTime * moveSpeed, 0, 0));
        if (moveRL < 0)
        {
            animator.SetBool("walk", true);
            sp.flipX = true;
            xDirection = -1;
        }
        else if (moveRL > 0)
        {
            animator.SetBool("walk", true);
            sp.flipX = false;
            xDirection = 1;
        }
        else
        {
            animator.SetBool("walk", false);
        }
        if (moveUD > 0 && DkJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            DkJump = false;
        }
        else if (moveUD < 0)
        {
            rb.AddForce(new Vector3(3 * xDirection, 0, 0));
            animator.SetBool("abc", true);
            Dk = true;
        }
        if (Dk)
        {
            time1 += Time.deltaTime;
            if (time1 > 0.2f)
            {
                animator.SetBool("abc", false);
                time1 = 0;
                Dk = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            DkJump = true;
        }
    }
}
