using UnityEngine;
using DefinedEnums;

public abstract class StatBase : MonoBehaviour
{
    string _name;
    bool _isDead;

    // base status
    protected int _str;
    protected int _vit;
    protected int _dex;
    // 2nd status
    protected int _maxHp, _nowHp;
    protected int _avo, _acc;
    protected int _perRecoveryValue;




    public bool _isDeath { get { return _isDead; } set { _isDead = value; } }

    public float _HPRate
    {
        get { return (float)_nowHp / _maxHp; }
    }

    public string _myName
    {
        get { return _name; }
    }

    public abstract eCharIconKind _charKind
    {
        get;
    }

    protected void InitBase(string name, int str, int vit, int dex)
    {
        _name = name;
        _isDeath = false;

        _str = str;
        _vit = vit;
        _dex = dex;

        Setting2ndStatus();

        _nowHp = _maxHp;
    }

    protected void Setting2ndStatus()
    {
        _maxHp = (_vit + (_str / 4)) * 10;
        _acc = 85 + (_dex / 2) + (_str / 10);
        _avo = 15 + (_dex / 3) + (_vit / 10);
        _perRecoveryValue = _vit / 15;
        _perRecoveryValue = (_perRecoveryValue < 1) ? 1 : _perRecoveryValue;
    }

    public abstract void ChangeAnimationToAction(eCharacterAnimState state);
}
