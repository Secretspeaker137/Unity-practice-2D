//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyMove : MonoBehaviour
//{
//    Rigidbody2D rigid;
//    Animator anim;
//    SpriteRenderer spriteRenderer;
//    public int nextMove; // 행동지표를 결정할 변수

//    void Awake()
//    {
//        rigid = GetComponent<Rigidbody2D>();
//        anim = GetComponent<Animator>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        Invoke("Think", 5); // 1. 5초가 지나면 Think 함수로 이동한다.
//    }

//    void FixedUpdate()
//    {

//        // Move
//        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

//        // Platform Check
//        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
//        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
//        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
//        if (rayHit.collider == null) // 앞 부분에 콜라이더가 없는 상태라면 Turn 함수로 이동한다.
//        {
//            Turn();
//        }
//    }

//    // 재귀함수 : 자신을 스스로 호출하는 함수, 딜레이 없이 재귀함수를 사용하는 것은 아주 위험!
//    void Think() 
//    {
//        // Set Next Active
//        nextMove = Random.Range(-1, 2); // -1에서 2까지 무작위의 숫자가 나온다. 

//        // Sprite Animation
//        anim.SetInteger("WalkSpeed", nextMove); // 달리기 속도와 X값이 동일

//        // Flip Sprite
//        if(nextMove != 0) // 계속 움직이는데
//        spriteRenderer.flipX = nextMove == 1; // 오른쪽으로 움직이면 왼쪽으로 회전한다.

//        //Recursive : 재귀
//        float nextThinkTime = Random.Range(2f, 5f); 
//        Invoke("Think", nextThinkTime); // 2. 2초 또는 5초마다 Think 함수로 이동하는 것으로 재설정
//    }

//    void Turn()
//    {
//        nextMove *= -1; //-1을 곱하여 이동 방향을 바꾸는데
//        spriteRenderer.flipX = nextMove == 1;//이동 방향이 -1을 향하고 있을 때만 애니메이션의 방향도 함께 바꾼다

//        CancelInvoke(); // Think 함수를 해제하여 
//        Invoke("Think", 2); // 2초 마다 Think 함수로 돌아가게 한다.
//    }
//}
