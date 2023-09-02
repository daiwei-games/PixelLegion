using Assets.Scripts;
using Assets.Scripts.BaseClass;
using UnityEngine;

public class SoldierScript : SoldierBaseScript
{

    private void Awake()
    {
        SoldierDataInitializ();
    }
    public override void SoldierDataInitializ()
    {
        _transform = transform; // 取得物件transform
        _gameObject = gameObject; // 取得物件gameobject
        _body2D = GetComponent<Rigidbody2D>(); // 取得物件rigidbody2D
        _animator = GetComponent<Animator>(); // 取得動畫控制器

        _soldierChangeState = SoldierState.Move;
        GetAllAnimationClipName();

        soldierHp = 10;
    }
    public override void GetAllAnimationClipName()
    {
        if (_animator == null) return;
        string nameIndexOf = ""; // 動畫名稱
        string _name = ""; // 動畫名稱
        // 取得所有動畫名稱
        foreach (AnimationClip animationClip in _animator.runtimeAnimatorController.animationClips)
        {
            _animationClip.Add(animationClip);
            _name = animationClip.name;
            nameIndexOf = _name.ToLower();
            if (nameIndexOf.IndexOf("_idle") > 0) idleAnimationName = _name;
            if (nameIndexOf.IndexOf("_hit") > 0) hitAnimationName = _name;
            if (nameIndexOf.IndexOf("_atk") > 0) atkAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_die") > 0) dieAnimationName = _name;
            if (nameIndexOf.IndexOf("_run") > 0) runAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_dash") > 0) dashAnimationName.Add(_name);
        }
    }
    public override void SoldierStateAI()
    {
        if (animationTime < animationTimerMax) return;
        /*
         * 先判斷新的狀態指令後判斷當前狀態
         * 
         * 當狀態機「受傷、攻擊、死亡」與目前狀態相同時"並且"動畫為播完時將會retun
         * 當狀態機不是等待時且其他狀態動畫已經播放完畢，而參數也為等待時將會進入等待
         * 
         */
        int _soldierNowStateInt = (int)_soldierNowState; // 當前狀態
        switch (_soldierChangeState)
        {
            case SoldierState.Idle:
                _animator.Play(idleAnimationName);
                break;
            case SoldierState.Move:
                _animator.Play(runAnimationName[0]);
                break;
        }

        _animationInfo = _animator.GetCurrentAnimatorStateInfo(0); // 取得動畫資訊
        if (soldierHp <= 0) _soldierChangeState = SoldierState.Die;

        if (_soldierChangeState != SoldierState.Die)
        {
            if (_soldierNowStateInt == (int)SoldierState.Hit ||
                (_soldierNowStateInt == (int)SoldierState.Atk && _collider2D.Length > 0))
            {
                if (_animationInfo.normalizedTime < 0.9f) return;
            }
        }
        switch (_soldierChangeState) { 
            case SoldierState.Atk:
                _animator.Play(atkAnimationName[Random.Range(0, atkAnimationName.Count)]);

                if (_collider2D[0].tag.IndexOf("Hero") != -1)
                {
                    Debug.Log("英雄");
                }
                else if (_collider2D[0].tag.IndexOf("Soldier") != -1)
                {
                    _collider2D[0].GetComponent<SoldierScript>().SoldierHP(1);
                }
                else if (_collider2D[0].tag.IndexOf("MainFortress") != -1)
                {
                    MainFortressBaseScript _mainFortressScript = _collider2D[0].GetComponent<MainFortressBaseScript>();
                    _mainFortressScript.MainFortressHit(1);
                    switch (_collider2D[0].tag)
                    {
                        case staticPublicObjectsStaticName.MainFortressTag:
                            break;
                        case staticPublicObjectsStaticName.DarkMainFortressTag:
                            
                            break;
                    }
                }
                break;
            case SoldierState.Hit:
                _animator.Play(hitAnimationName);
                break;
            case SoldierState.Die:
                if (_soldierNowState == SoldierState.Die) return;
                _body2D.bodyType = RigidbodyType2D.Static;
                _animator.Play(dieAnimationName);
                _gameManagerScript._soldierList.Remove(_soldierScript); // 從清單中移除
                Destroy(_gameObject, 1); // 1秒後刪除物件
                return;
        }
        animationTime = 0;
        _soldierNowState = _soldierChangeState;
    }
    public override void Atk()
    {
        _soldierChangeState = SoldierState.Atk;
        SoldierStateAI();
    }
    public override void Die()
    {
        _soldierChangeState = SoldierState.Die;
        SoldierStateAI();
    }
    public override void Hit()
    {
        _soldierChangeState = SoldierState.Hit;
        SoldierStateAI();
    }
    public override void SoldierHP(int hitAmount)
    {
        if (soldierHp <= 0) return;
        Hit();
        soldierHp -= hitAmount;
        if (soldierHp <= 0) Die();
    }
    public override void Idle()
    {
        if (_soldierChangeState == _soldierNowState && _soldierNowState == SoldierState.Idle) return;
        _soldierChangeState = SoldierState.Idle;
        SoldierStateAI();
    }
    public override void Move()
    {
        _SoldierScale = _transform.localScale;
        if (_enemyNowMainFortress.position.x > _transform.position.x)
            _SoldierScale.x = Mathf.Abs(_SoldierScale.x);
        else
            _SoldierScale.x = Mathf.Abs(_SoldierScale.x) * -1;

        _transform.localScale = _SoldierScale;
        if ((_soldierNowState == _soldierChangeState && _soldierNowState == SoldierState.Move) || _soldierNowState == SoldierState.Move) return;
        _soldierChangeState = SoldierState.Move;
        SoldierStateAI();
    }

    public override void PhyOverlapBoxAll(Vector2 Pos, float PosSize)
    {
        Vector2 PhySize = Vector2.one * PosSize;
        _collider2D = Physics2D.OverlapBoxAll(Pos, PhySize, 0, _enemyLayerMask);

    }
    void OnDrawGizmos()
    {
        Vector2 PosSize = Vector2.one * Physics2DSize;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_transform.position, PosSize);
    }
}
