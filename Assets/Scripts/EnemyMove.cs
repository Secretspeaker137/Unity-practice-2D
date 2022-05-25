using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove; // 행동지표를 결정할 변수

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5); // 1. 5초가 지나면 Think 함수로 이동한다, 5초 후에 Invoke에 무한히 갇힘
    }

    void FixedUpdate()
    {

        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) // 앞 부분에 콜라이더가 없는 상태, 즉 땅떠러지라면 Turn 함수로 이동한다.
        {
            Turn();
        }
    }

    // 재귀함수 : 자신을 스스로 호출하는 함수, 딜레이 없이 재귀함수를 사용하는 것은 아주 위험!
    void Think()
    {
        // Set Next Active
        nextMove = Random.Range(-1, 2); // 왼쪽,멈춤,오른쪽 3개의 선택지 중 하나가 선택된다. 

        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove); // nextMove의 값이 0일 때 WalkSpeed 작동X -> Idle로 직행

        // Flip Sprite 
        if (nextMove != 0) // 계속 움직이는 동안
            spriteRenderer.flipX = nextMove == 1; // 오른쪽으로 움직이면 오른쪽으로 회전하는 애니메이션으로 전환

        //Recursive : 재귀
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // 2. 계속 2초 또는 5초마다 왼쪽,멈춤,오른쪽 3개의 선택지 중 하나가 선택
    }

    void Turn() 
    {
        nextMove *= -1; // -1을 곱하여 이동 방향을 바꾸는데
        spriteRenderer.flipX = nextMove == 1;//이동 방향이 -1을 향하고 있을 때만 애니메이션의 방향도 함께 바꾼다

        CancelInvoke(); // Think 함수를 해제하여 
        Invoke("Think", 2); // 2초 마다 Think 함수로 돌아가게 한다.
    }
}
