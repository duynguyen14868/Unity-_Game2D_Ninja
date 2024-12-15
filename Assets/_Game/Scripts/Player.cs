using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class NewBehaviourScript : MonoBehaviour
{
    // SerializeField: để nấy từ bên ngoài
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim; // animator: điền khiển anim, animation: clip
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;

    private bool isGrounded = true;    // check xem có đanh ở trên mặt đất không
    private bool isJumping = false;     // check xem có đanh nhảy hay không
    private bool isAttack = false;

    private float horizontal;

    private string currentAnimName;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        // Lấy điều kiển đầu vào từ bàn phím 
        // -1 -> 0 -> 1
        horizontal = Input.GetAxisRaw("Horizontal");    // Chiều ngang
        //verticle = Input.GetAxisRaw("Verticle");    // Chiều dọc

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            // jump
            // Nhảy
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                //isJumping = true;
                //ChangeAnim("jump");
                //rb.AddForce(jumpForce * Vector2.up);
                Jump();
            }

            // change anim run
            // Đang nhảy k thực hiện đc gì
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        // check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        // Moving
        // Khi bấm sẽ nấy hướng * deltaTime * speed còn không bấm gì thì sẽ dừng chực tiếp
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);

            // Quay mặt lại
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180));    // horizontal > 0 -> tra ve 0, horizontal <= 0 -> tra ve 180
            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        // idle
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    // Check xem có bắn chúng không
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        //if(hit.collider != null)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    // Di chuyển
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
