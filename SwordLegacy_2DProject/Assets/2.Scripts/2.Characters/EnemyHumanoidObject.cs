using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinedEnums;

public class EnemyHumanoidObject : StatBase
{
    [SerializeField] BoxCollider2D _attackRng;
    [SerializeField] BoxCollider2D _sightRng;
    [SerializeField] float _attackRange = 0.7f;
    [SerializeField] float _SightRangeValue = 8;
    [SerializeField] float _Speed = 4f;

    eCharacterAnimState _currentAniState;
    eCharIconKind _enemyType;
    Transform _targetChar;
    Vector3 _targetPos;
    bool _isGround;
    SpriteRenderer _model;
    float _attakTicTime = 2f;
    float _delayTime = 0f;

    Animator _animController;

    SightRangeObject _sightRngObj;

    public override eCharIconKind _charKind
    {
        get { return _enemyType; }
    }

    void Awake()
    {
        _animController = GetComponent<Animator>();
        _model = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //임시
        InitEnemySetData("도적", 5, 1, 3, eCharIconKind.Bandit1);
        _currentAniState = eCharacterAnimState.IDLE;


    }

    void Update()
    {
        if(_isDeath)
            return;

        _delayTime += Time.deltaTime;

        int lMask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D rHit2D = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, 1, lMask);
        if (rHit2D.transform != null && Vector3.Distance(rHit2D.point, transform.position) <= 0.05f)
           _isGround = true;
        else
            _isGround = false;

        if (_isGround)
        {
            if(_targetChar != null)
            {
                float dir = _targetChar.position.x - transform.position.x;
                float _distance = (dir >= 0) ? dir : -dir; // Vecot2.Distance 보다 연산이 빠름.
                if (dir > 0)
                {
                    _model.flipX = false;
                    if (_attackRng.offset.x < 0) _attackRng.offset = new Vector2(-_attackRng.offset.x, _attackRng.offset.y);
                }
                else
                {
                    _model.flipX = true;
                    if (_attackRng.offset.x > 0) _attackRng.offset = new Vector2(-_attackRng.offset.x, _attackRng.offset.y);
                }

                if (_distance > _attackRange)
                {
                    if (_currentAniState != eCharacterAnimState.ATTACK)
                    {
                        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, rHit2D.point.y), new Vector2(_targetChar.position.x, rHit2D.point.y), _Speed * Time.deltaTime);
                        ChangeAnimationToAction(eCharacterAnimState.RUN);
                    }
                }
                else
                {
                    if(_attakTicTime <= _delayTime)
                    {
                        ChangeAnimationToAction(eCharacterAnimState.ATTACK);
                        _delayTime = 0;
                    }
                    else
                        ChangeAnimationToAction(eCharacterAnimState.BATTLE_IDLE);
                }

                transform.position = new Vector2(transform.position.x, rHit2D.point.y);
            }
            else
               transform.position = rHit2D.point;
        }
        else
        {
            Vector2 addGravity = Physics2D.gravity * Time.deltaTime;
            transform.position += new Vector3(addGravity.x, addGravity.y);

        }
    }

    bool CehckDirection(float distance)
    {
        return true;
    }

    public void InitEnemySetData(string name, int str, int vit, int dex, eCharIconKind Ikind, eDirectional dir = eDirectional.Left)
    {
        InitBase(name, str, vit, dex);
        _sightRngObj = _sightRng.GetComponent<SightRangeObject>();
        _sightRngObj.InitSetData(this, _SightRangeValue);
        _enemyType = Ikind;

        _attackRng.enabled = false;

        if (dir == eDirectional.Left)
        {
            _model.flipX = true;
            _attackRng.offset = new Vector2(-_attackRng.offset.x, _attackRng.offset.y);
        
        }
        else
        {
            _model.flipX = false;
            _attackRng.offset = new Vector2(-_attackRng.offset.x, _attackRng.offset.y);
        }
    }

    public void SetTargetChar(Transform target)
    {
        _targetChar = target;
    }

    public override void ChangeAnimationToAction(eCharacterAnimState state)
    {
        switch (state)
        {
            case eCharacterAnimState.IDLE:
            case eCharacterAnimState.RUN:
                break;
            case eCharacterAnimState.BATTLE_IDLE:
                _animController.ResetTrigger("Attack");
                break;

            case eCharacterAnimState.ATTACK:
                _animController.SetTrigger("Attack");
                break;
            case eCharacterAnimState.DEAD:
                _isDeath = true;
                _animController.SetTrigger("Dead");
                break;
        }
        _animController.SetInteger("AnimState", (int)state);
        _currentAniState = state;
    }

    void OnAttackRange()
    {
        _attackRng.enabled = true;
    }

    void OnAttackEnd()
    {
        ChangeAnimationToAction(eCharacterAnimState.BATTLE_IDLE);
        _attackRng.enabled = false;
    }

    void OnGUI()
    {
        if (_isDeath)
            return;

        if (GUI.Button(new Rect(0, 0, 220, 60), "RUN"))
        {
            ChangeAnimationToAction(eCharacterAnimState.RUN);
            _animController.SetInteger("AnimState", (int)_currentAniState);
        }

        if (GUI.Button(new Rect(0, 65, 220, 60), "BATTLEIDLE"))
        {
            if (_currentAniState != eCharacterAnimState.IDLE)
            {
                ChangeAnimationToAction(eCharacterAnimState.BATTLE_IDLE);
                _animController.SetInteger("AnimState", (int)_currentAniState);
            }
        }
        
        if (GUI.Button(new Rect(0, 130, 220, 60), "ATTACK"))
        {
            if (_currentAniState == eCharacterAnimState.BATTLE_IDLE || _currentAniState == eCharacterAnimState.RUN)
            {
                ChangeAnimationToAction(eCharacterAnimState.ATTACK);
                _animController.SetInteger("AnimState", (int)_currentAniState);
                _animController.SetTrigger("Attack");
            }
        }
        if (GUI.Button(new Rect(0, 195, 220, 60), "DEAD"))
        {
            ChangeAnimationToAction(eCharacterAnimState.DEAD);
            _animController.SetInteger("AnimState", (int)_currentAniState);
            _animController.SetTrigger("Dead");
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position + (Vector3.up * 0.5f);
        Debug.DrawLine(start, start + Vector3.down, Color.red);

        if(_targetChar != null)
        Debug.DrawLine(transform.position, _targetChar.position, Color.cyan);
    }

}
