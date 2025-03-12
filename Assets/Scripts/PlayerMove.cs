using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float jumpStrength = 20f;
    float horizontalValue;

    private bool isGrounded = true;

    Rigidbody2D rb;
    // Start is called before the first frame update
    SpriteRenderer sr;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        Move(horizontalValue);
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
           // isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
