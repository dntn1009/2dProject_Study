using DefinedEnums;
using UnityEngine;

public class HeroObject : StatBase
{
    //status
    string _designationName;

    // 정보 변수
    eCharacterAnimState _currentAnimState;
    int _attackCount = 0;
    [SerializeField] int _moveSpeed = 1;

    // 참조 변수
    Animator _animController;
    SpriteRenderer _model;
    BoxCollider2D _attackRange;
    Rigidbody2D _rigidbody2D;

    eCharIconKind _characterKind;
    public int _finalAttackPower
    {
        get { return _str + (_dex / 4) + (_vit / 8); }
    }

    public int _finalDefencePower
    { 
        get { return (_vit / 2) + (_str / 10); }
    }

    public string _designName
    {
        get { return _designationName; }
    }

    public override eCharIconKind _charKind
    {
        get { return _characterKind; } 
    }
    void Awake()
    {
        _animController = GetComponent<Animator>();
        _model = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _attackRange = transform.GetChild(1).GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IngameManager._instance._nowGameState != eGameState.Play)
            return;

        if (_isDeath) return;

        if (Input.GetButtonDown("Jump"))
        {
            _attackCount++;
            ChangeAnimationToAction(eCharacterAnimState.ATTACK);
        }
        if (_currentAnimState != eCharacterAnimState.ATTACK)
        {
            int dir = (int)Input.GetAxisRaw("Horizontal");

            if (dir == 0)
            {
                ChangeAnimationToAction(eCharacterAnimState.IDLE);
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
            else if (dir > 0)
            {
                ChangeAnimationToAction(eCharacterAnimState.RUN);
                _model.flipX = false;
                if (_attackRange.offset.x < 0) _attackRange.offset = new Vector2(-_attackRange.offset.x, _attackRange.offset.y);
            }
            else
            {
                ChangeAnimationToAction(eCharacterAnimState.RUN);
                _model.flipX = true;
                if (_attackRange.offset.x > 0) _attackRange.offset = new Vector2(-_attackRange.offset.x, _attackRange.offset.y);
            }

            _rigidbody2D.velocity = new Vector2(dir * _moveSpeed, _rigidbody2D.velocity.y);
            //_rigidbody2D.AddForce(Vector2.right * dir * _moveSpeed);
        }
    }


    public override void ChangeAnimationToAction(eCharacterAnimState state)
    {
        if (_isDeath) return;

        switch (state)
        {
            case eCharacterAnimState.IDLE:
            case eCharacterAnimState.RUN:
                _attackCount = 0;
                break;
            case eCharacterAnimState.ATTACK:
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
                break;
            case eCharacterAnimState.DEAD:
                _isDeath = true;
                _animController.SetTrigger("Dead");
                break;
        }
        _animController.SetInteger("AttackType", _attackCount);
        _animController.SetInteger("AnimState", (int)state);
        _currentAnimState = state;
    }
    bool HittingClac(int acc)
    {
        int finishRate = acc - _avo;
        if (finishRate > Random.Range(0, 100))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnAttackRange()
    {
        _attackRange.enabled = true;
    }
    public void OnAttackEnd(int type)
    {
        if (_attackCount <= type || type >= 3 && type <= _attackCount)
        {
            _attackCount = 0;
            ChangeAnimationToAction(eCharacterAnimState.IDLE);
        }
        _attackRange.enabled = false;
    }

    public void InitalizeData(string name, string dName, int str, int vit, int dex)
    {
        _designationName = dName;
        InitBase(name, str, vit, dex);
        _characterKind = eCharIconKind.Knight;
    }
}
