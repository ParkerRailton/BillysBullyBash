using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float jumpStrength = 2f;
    float horizontalValue;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
    }
    void FixedUpdate()
    {
        Move(horizontalValue);
    }

    void Move(float dir)
    {
        Vector2 targetVelocity = new Vector2(dir * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;
    }
}
