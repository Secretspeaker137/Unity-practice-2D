using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid; // 물리이동을 위한 변수 선언
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // 변수 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
            //Jump
            if(Input.GetButtonDown("Jump")) { 
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }

            //Stop Speed
            if(Input.GetButtonDown("Horizontal")) // normalized : 벡터 크기를 1로 만든 상태 (단위벡터)
            { rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); }

            //Direction Sprite
            if(Input.GetButtonDown("Horizontal"))
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1; // spriteRenderer.flipX : 조건문

            //Animation
            if(Mathf.Abs(rigid.velocity.x) < 0.3) // Mathf.Abs() : 절대값
                anim.SetBool("isWalking", false);
            else
                anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if(rigid.velocity.x > maxSpeed) // Right Max Speed // velocity : 리지드바디의 현재 속도
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed 
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if(rayHit.collider != null)
        {
            if(rayHit.distance < 0.5f)
                //Debug.Log(rayHit.collider.name);
                anim.SetBool("isJumping", false);
        }
    }
}
