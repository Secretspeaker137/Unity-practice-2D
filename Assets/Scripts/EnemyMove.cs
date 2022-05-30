using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

    public int nextMove; // �ൿ��ǥ�� ������ ����

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think", 5); // 1. 5�ʰ� ������ Think �Լ��� �̵��Ѵ�, 5�� �Ŀ� Invoke�� ������ ����
    }

    void FixedUpdate()
    {

        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) // �� �κп� �ݶ��̴��� ���� ����, �� ����������� Turn �Լ��� �̵��Ѵ�.
        {
            Turn();
        }
    }

    // ����Լ� : �ڽ��� ������ ȣ���ϴ� �Լ�, ������ ���� ����Լ��� ����ϴ� ���� ���� ����!
    void Think()
    {
        // Set Next Active
        nextMove = Random.Range(-1, 2); // ����,����,������ 3���� ������ �� �ϳ��� ���õȴ�. 

        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove); // nextMove�� ���� 0�� �� WalkSpeed �۵�X -> Idle�� ����

        // Flip Sprite 
        if (nextMove != 0) // ��� �����̴� ����
            spriteRenderer.flipX = nextMove == 1; // ���������� �����̸� ���������� ȸ���ϴ� �ִϸ��̼����� ��ȯ

        //Recursive : ���
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // 2. ��� 2�� �Ǵ� 5�ʸ��� ����,����,������ 3���� ������ �� �ϳ��� ����
    }

    void Turn()
    {
        nextMove *= -1; // -1�� ���Ͽ� �̵� ������ �ٲٴµ�
        spriteRenderer.flipX = nextMove == 1;//�̵� ������ -1�� ���ϰ� ���� ���� �ִϸ��̼��� ���⵵ �Բ� �ٲ۴�

        CancelInvoke(); // Think �Լ��� �����Ͽ� 
        Invoke("Think", 2); // 2�� ���� Think �Լ��� ���ư��� �Ѵ�.
    }

    public void OnDamaged()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        spriteRenderer.flipY = true;
        // Collider Disable
        capsuleCollider.enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // Destroy
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
