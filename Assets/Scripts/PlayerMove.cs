using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState {
    IDLE = 0,
WALKING = 1,
JUMPING =2
}

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float jumpStrength = 20f;
    float horizontalValue;
    private Animator anim;
    private bool isGrounded = true;
    public PlayerState state = PlayerState.IDLE;

    Rigidbody2D rb;
    // Start is called before the first frame update
    SpriteRenderer sr;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();//make reference to the animator which is on this gameObject with the script. 


    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if (SceneManager.sceneCount == 1)
        {
            Move(horizontalValue);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        DetermineState();

    }


    void Move(float dir)
    {
        if (dir > 0) sr.flipX = false;
        else if (dir < 0) sr.flipX = true;
        Vector2 targetVelocity = new Vector2(dir * moveSpeed, rb.velocity.y);
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

    

    void DetermineState()
    {
        state = PlayerState.IDLE;
        if (horizontalValue != 0)
        {
            state = PlayerState.WALKING;
        }
        if (!isGrounded)
        {
             state = PlayerState.JUMPING;
        }
        anim.SetInteger("State", (int)state);
    }
  
}

