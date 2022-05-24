//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyMove : MonoBehaviour
//{
//    Rigidbody2D rigid;
//    Animator anim;
//    SpriteRenderer spriteRenderer;
//    public int nextMove; // �ൿ��ǥ�� ������ ����

//    void Awake()
//    {
//        rigid = GetComponent<Rigidbody2D>();
//        anim = GetComponent<Animator>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        Invoke("Think", 5); // 1. 5�ʰ� ������ Think �Լ��� �̵��Ѵ�.
//    }

//    void FixedUpdate()
//    {

//        // Move
//        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

//        // Platform Check
//        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
//        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
//        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
//        if (rayHit.collider == null) // �� �κп� �ݶ��̴��� ���� ���¶�� Turn �Լ��� �̵��Ѵ�.
//        {
//            Turn();
//        }
//    }

//    // ����Լ� : �ڽ��� ������ ȣ���ϴ� �Լ�, ������ ���� ����Լ��� ����ϴ� ���� ���� ����!
//    void Think() 
//    {
//        // Set Next Active
//        nextMove = Random.Range(-1, 2); // -1���� 2���� �������� ���ڰ� ���´�. 

//        // Sprite Animation
//        anim.SetInteger("WalkSpeed", nextMove); // �޸��� �ӵ��� X���� ����

//        // Flip Sprite
//        if(nextMove != 0) // ��� �����̴µ�
//        spriteRenderer.flipX = nextMove == 1; // ���������� �����̸� �������� ȸ���Ѵ�.

//        //Recursive : ���
//        float nextThinkTime = Random.Range(2f, 5f); 
//        Invoke("Think", nextThinkTime); // 2. 2�� �Ǵ� 5�ʸ��� Think �Լ��� �̵��ϴ� ������ �缳��
//    }

//    void Turn()
//    {
//        nextMove *= -1; //-1�� ���Ͽ� �̵� ������ �ٲٴµ�
//        spriteRenderer.flipX = nextMove == 1;//�̵� ������ -1�� ���ϰ� ���� ���� �ִϸ��̼��� ���⵵ �Բ� �ٲ۴�

//        CancelInvoke(); // Think �Լ��� �����Ͽ� 
//        Invoke("Think", 2); // 2�� ���� Think �Լ��� ���ư��� �Ѵ�.
//    }
//}
