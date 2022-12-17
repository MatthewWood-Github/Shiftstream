using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private BaseMove car;
    private Rigidbody rb;

    protected float jumpForce = 10000;

    protected bool jumped = false;

    protected float jumpCooldown;

    protected float maxJumpCooldown = 0.5f;

    protected void JumpManager()
    {
        if (car.jumping && jumped == false && jumpCooldown == maxJumpCooldown)
        {
            rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumped = true;
            jumpCooldown = 0;
        }
        else if (car.jumping == false && car.grounded)
            jumped = false;
    }

    protected void JumpTimer()
    {
        jumpCooldown = Mathf.Clamp(jumpCooldown, 0, maxJumpCooldown - Time.deltaTime);

        if (jumped == false)
            jumpCooldown += Time.deltaTime;
    }

    private void Start()
    {
        car = this.GetComponent<BaseMove>();
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        JumpTimer();
    }

    private void FixedUpdate()
    {
        JumpManager();
    }
}
