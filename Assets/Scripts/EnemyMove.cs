using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove; // �ൿ��ǥ�� ������ ����

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Think();
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); 
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2); // -1���� 2���� �������� ���ڰ� ���´�. 
    }
}
