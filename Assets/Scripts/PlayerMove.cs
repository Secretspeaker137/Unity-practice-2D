using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rigid; // 물리이동을 위한 변수 선언
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>(); // 변수 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")) // 한 번 점프했을 때 점프 금지
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal")) // normalized : 벡터 크기를 1로 만든 상태 (단위벡터)
        { rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1; // spriteRenderer.flipX : 조건문

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.1) // Mathf.Abs() : 절대값
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h * 2, ForceMode2D.Impulse); // 힘이 부족하여 경사로를 올라가지 못해 2를 곱함

        //Max Speed
        if (rigid.velocity.x > maxSpeed) // Right Max Speed // velocity : 리지드바디의 현재 속도
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed 
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0) // 오브젝트가 점프했다가 다시 내려올 때
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1)
                    //Debug.Log(rayHit.collider.name); // 오브젝트의 위치를 실시간으로 알려줌
                    anim.SetBool("isJumping", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y) // 아래로 낙하 중 + 몬스터보다 위에 있음 = 밟음
            {
                OnAttack(collision.transform);
            }
            else // Damaged
            OnDamaged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;

            // Deactive Item
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
        }
    }

    void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;

        // Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }
    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown();

        // Change Layer (Immortal Active)
        gameObject.layer = 11;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; // 0보다 크다 -> 1, 0보다 작다 -> -1
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        // Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        spriteRenderer.flipY = true;
        // Collider Disable
        capsuleCollider.enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
}