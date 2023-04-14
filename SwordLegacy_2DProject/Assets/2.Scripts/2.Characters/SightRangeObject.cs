using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightRangeObject : MonoBehaviour
{
    StatBase _owner;
    public void InitSetData(StatBase owner, float range)
    {
        _owner = owner;
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(range, col.size.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //���ʿ��� �÷��̾ �����ȿ� ���Դٰ� �˷���.
            //Debug.Log("Player In");

            switch(_owner._charKind)
            {
                case DefinedEnums.eCharIconKind.Bandit1:
                case DefinedEnums.eCharIconKind.bandit2:
                    ((EnemyHumanoidObject)_owner).SetTargetChar(collision.transform);
                    break;
            }

        }
    }
}
