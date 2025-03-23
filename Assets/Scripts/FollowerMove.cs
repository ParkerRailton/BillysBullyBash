using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowerState
{
    IDLE = 0,
    WALKING = 1,
    JUMPING = 2
}
public class FollowerMove : MonoBehaviour
{
    [SerializeField]
    public GameObject master;

    private float moveSpeed;
    private float jumpStrength;
    public float jumpBuffer = 1f;
    public float moveBuffer = 0.5f;
    private bool isGrounded = true;
    public float slowdownRange = 1f;
    private float horizontalValue;
    public FollowerState state = FollowerState.IDLE;

    private PlayerMove masterMove;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform followPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        masterMove = master.GetComponent<PlayerMove>();
        moveSpeed = masterMove.moveSpeed;
        jumpStrength = masterMove.jumpStrength;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        followPoint = master.transform.Find("Follow Point");

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), master.GetComponent<Collider2D>(), true);
       
    }

    // Update is called once per frame
    void Update()
    {
        FollowLogic();
        Move(horizontalValue);
        DetermineState();
    }
    void Move(float dir)
    {
        float adjustedMovespeed = moveSpeed;
        float distToPoint = Mathf.Abs(followPoint.position.x - transform.position.x);
        if (distToPoint < slowdownRange)
        {
            adjustedMovespeed = moveSpeed * (distToPoint / slowdownRange);
        }
        if (dir > 0)
        {
            sr.flipX = false;


        }
        else if (dir < 0)
        {
            sr.flipX = true;
        }
        Vector2 targetVelocity = new Vector2(dir * adjustedMovespeed, rb.velocity.y);
        rb.velocity = targetVelocity;

    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            isGrounded = false;

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

    void FollowLogic()
    {
        if (followPoint.position.y - jumpBuffer > transform.position.y)
        {
            Jump();
        }
        horizontalValue = 0;
        if (followPoint.position.x - moveBuffer  > transform.position.x)
        {
            horizontalValue = 1;
        }
        else if (followPoint.position.x + moveBuffer  < transform.position.x)
        {
            horizontalValue = -1;
        }


    }

    void DetermineState()
    {
        state = FollowerState.IDLE;
        if (horizontalValue != 0)
        {
            state = FollowerState.WALKING;
        }
        if (!isGrounded)
        {
            state = FollowerState.JUMPING;
        }
        //anim.SetInteger("State", (int)state);
    }
}
