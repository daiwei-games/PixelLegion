using Assets.Scripts.BaseClass;
using UnityEngine;

public class SoldierScript : SoldierBaseScript
{
    /// <summary>
    /// 矩形射線偏移
    /// </summary>
    [Header("矩形射線偏移")]
    public float PhyOffset;

    protected Vector2 PhySize;
    private void Awake()
    {
        SoldierDataInitializ();
    }
    public override void SoldierDataInitializ()
    {
        _Tf = transform; // 取得物件transform
        _Go = gameObject; // 取得物件gameobject
        _body2D = GetComponent<Rigidbody2D>(); // 取得物件rigidbody2D
        _animator = GetComponent<Animator>(); // 取得動畫控制器

        _soldierChangeState = SoldierState.Move;
        GetAllAnimationClipName();
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
            if (nameIndexOf.IndexOf("_idle") != -1) idleAnimationName = _name;
            if (nameIndexOf.IndexOf("_hit") != -1) hitAnimationName = _name;
            if (nameIndexOf.IndexOf("_atk") != -1) atkAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_die") != -1) dieAnimationName = _name;
            if (nameIndexOf.IndexOf("_run") != -1) runAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_dash") != -1) dashAnimationName.Add(_name);
        }
    }
    public override void SoldierStateAI()
    {
        _animationInfo = _animator.GetCurrentAnimatorStateInfo(0); // 取得動畫資訊
        float _normalizedTime = _animationInfo.normalizedTime;
        _normalizedTime -= Mathf.Floor(_normalizedTime);
        animationTime += _Time;
        if (animationTime < animationTimerMax) return;
        /*
         * 先判斷新的狀態指令後判斷當前狀態
         * 
         * 當狀態機「受傷、攻擊、死亡」與目前狀態相同時"並且"動畫為播完時將會retun
         * 當狀態機不是等待時且其他狀態動畫已經播放完畢，而參數也為等待時將會進入等待
         * 
         */
        if (soldierHp <= 0) _soldierChangeState = SoldierState.Die;// 當血量小於0時將會進入死亡狀態
        if (_soldierChangeState != SoldierState.Die) // 當狀態不是死亡時將會進入判斷
        {
            if (_soldierChangeState != SoldierState.SoupHit)
            {
                if (_soldierNowState == SoldierState.Atk && _normalizedTime < 0.9f) return; // 當狀態為攻擊將會return
                if (_soldierNowState == SoldierState.Hit && _normalizedTime < 0.9f) return; // 當狀態為受傷將會return
            }
            if (_soldierNowState == SoldierState.SoupHit && _normalizedTime < 0.9f) return; // 當狀態為超級受傷將會return

        }
        switch (_soldierChangeState)
        {
            case SoldierState.Idle:
                _animator.Play(idleAnimationName); // 播放待機動畫
                break;
            case SoldierState.Move:
                _animator.Play(runAnimationName[0]); // 播放跑步動畫
                break;
            case SoldierState.Die:
                if (_soldierNowState == SoldierState.Die) return; // 當前狀態為死亡時將會retun
                _body2D.bodyType = RigidbodyType2D.Static; // 將剛體設定為靜態
                soldierAnimatorPlay(dieAnimationName);
                _gameManagerScript._soldierList.Remove(_soldierScript); // 從清單中移除
                Destroy(_Go, 2); // 1秒後刪除物件
                break;
            default:
                switch (_soldierChangeState)
                {
                    case SoldierState.Atk:
                        if (_collider2D.Length == 0) return;
                        soldierAnimatorPlay(atkAnimationName[Random.Range(0, atkAnimationName.Count)]);
                        string _tag;
                        Collider2D _col;
                        for (int i = 0; i < _collider2D.Length; i++) // 尋找敵人
                        {
                            _col = _collider2D[i];
                            _tag = _col.tag;
                            if (_tag.IndexOf("Hero") != -1)
                            {
                                _col.GetComponent<HeroScript>().HeroHit(soldierAtk);
                                _enemyNowMainFortress = _col.transform;
                            }
                            else if (_tag.IndexOf("Soldier") != -1)
                            {
                                _col.GetComponent<SoldierScript>().SoldierHP(1);
                                _enemyNowMainFortress = _col.transform;
                            }
                            else if (_tag.IndexOf("MainFortress") != -1)
                            {
                                MainFortressBaseScript _mainFortressScript = _col.GetComponent<MainFortressBaseScript>();
                                _mainFortressScript.MainFortressHit(1);
                                _mainFortressScript.WhoHitMeTransform.Add(_Tf); // 將攻擊主堡者加入清單
                                _enemyNowMainFortress = _col.transform;
                            }
                        }
                        break;
                    case SoldierState.SoupHit:
                    case SoldierState.Hit:
                        soldierAnimatorPlay(hitAnimationName);
                        break;




                }
                break;
        }
        animationTime = 0; // 將計時器歸零
        _soldierNowState = _soldierChangeState; // 將當前狀態設定為新的狀態
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
    /// <summary>
    /// 必須受傷
    /// </summary>
    /// <param name="hp">傷害</param>
    public override void MustBeInjured(int hp)
    {
        if (soldierHp <= 0) return;
        if (_soldierChangeState == SoldierState.SoupHit) return;
        Vector2 DieOffset;
        soldierHp -= hp;
        _soldierChangeState = SoldierState.SoupHit;
        if (soldierHp <= 0)
        {
            _soldierChangeState = SoldierState.Die;
            DieOffset = Vector2.left;
            if (_Tf.localScale.x > 0)
                DieOffset = Vector3.right;

            _body2D.velocity = DieOffset * 5;
        }
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
        _SoldierScale = _Tf.localScale;
        _SoldierScale.x = Mathf.Abs(_SoldierScale.x);
        if (_enemyNowMainFortress.position.x < _Tf.position.x)
            _SoldierScale.x = Mathf.Abs(_SoldierScale.x) * -1;

        _Tf.localScale = _SoldierScale;
        if ((_soldierNowState == _soldierChangeState && _soldierNowState == SoldierState.Move) || _soldierNowState == SoldierState.Move) return;
        _soldierChangeState = SoldierState.Move;
        SoldierStateAI();
    }

    public override void PhyOverlapBoxAll(Vector2 Pos)
    {
        PhySize = Vector2.one * Physics2DSize;
        PhySize.y *= 2;
        _collider2D = Physics2D.OverlapBoxAll(Pos, PhySize, 0, _enemyLayerMask);
    }
    void OnDrawGizmos()
    {
        PhySize = Vector2.one * Physics2DSize;
        PhySize.y *= 2;

        Vector2 Pos = _Tf.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Pos, PhySize);
    }
}
