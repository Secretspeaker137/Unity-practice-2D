using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;

    Rigidbody2D rigid; // �����̵��� ���� ���� ����
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // ���� �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
            //Stop Speed
            if(Input.GetButtonDown("Horizontal")) // normalized : ���� ũ�⸦ 1�� ���� ���� (��������)
            { rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); }

            //Direction Sprite
            if(Input.GetButtonDown("Horizontal"))
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1; // spriteRenderer.flipX : ���ǹ�
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if(rigid.velocity.x > maxSpeed) // Right Max Speed // velocity : ������ٵ��� ���� �ӵ�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed 
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
