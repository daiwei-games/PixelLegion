using Assets.Scripts;
using Assets.Scripts.BaseClass;
using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HeroScript : HeroBaseScript, IHeroFunc
{
    /// <summary>
    /// 是否被玩家操控
    /// </summary>
    [Header("是否被玩家操控")]
    public bool isPlayerControl;
    /// <summary>
    /// AI操控時的目標
    /// </summary>
    [Header("AI操控時的目標")]
    public Transform _target;
    /// <summary>
    /// 移動速度
    /// </summary>
    [Header("移動速度"), Range(1, 30)]
    public float speed;
    /// <summary>
    /// 狀態機現在的狀態
    /// </summary>
    [Header("狀態機現在的狀態")]
    public HeroState _HeroNowState;
    /// <summary>
    /// 狀態機下一個更換的狀態
    /// </summary>
    [Header("狀態機下一個更換的狀態")]
    public HeroState _HeroChengState;

    #region 動畫清單
    /// <summary>
    /// 移動相關攻擊動畫
    /// </summary>
    [Header("移動相關攻擊動畫")]
    public List<string> movAtk;
    public bool isMovAtk;
    /// <summary>
    /// 跳躍攻擊動畫
    /// </summary>
    [Header("跳躍攻擊動畫")]
    public string jumpAtk;
    public bool isJumpAtk;
    /// <summary>
    /// 衝刺動畫
    /// </summary>
    [Header("衝刺動畫")]
    public string dash;
    public bool isDash;
    ///<summary>
    /// 受傷動畫
    ///</summary>
    [Header("受傷動畫")]
    public string hit;
    public bool isHit;
    /// <summary>
    /// 受到攻擊的防禦
    /// </summary>
    [Header("受到攻擊的防禦")]
    public string def1;
    public bool isDef1;
    /// <summary>
    /// 沒受到攻擊的防禦
    /// </summary>
    [Header("沒受到攻擊的防禦")]
    public string def2;
    public bool isDef2;
    /// <summary>
    /// 翻滾
    /// </summary>
    [Header("翻滾")]
    public string roll;
    public bool isRoll;
    #region 組動畫
    /// <summary>
    /// 跳躍
    /// </summary>
    [Header("跳躍")]
    public string jump;
    public bool isJump;
    /// <summary>
    /// 降落
    /// </summary>
    [Header("降落")]
    public string drop;
    public bool isDrop;
    /// <summary>
    /// 落地
    /// </summary>
    [Header("落地")]
    public string land;
    public bool isLand;

    /// <summary>
    /// 閃現 消失
    /// </summary>
    [Header("閃現 消失")]
    public string miss0;
    public bool isMiss0;
    /// <summary>
    /// 閃現 出現
    /// </summary>
    [Header("閃現 出現")]
    public string miss1;
    public bool isMiss1;
    #endregion
    #region 一定有的動畫
    /// <summary>
    /// 攻擊動畫
    /// </summary>
    [Header("攻擊動畫")]
    public List<string> atk;
    /// <summary>
    /// 跑步動畫
    /// </summary>
    [Header("跑步動畫")]
    public string run;
    /// <summary>
    /// 等待動畫
    /// </summary>
    [Header("等待動畫")]
    public string idle;
    /// <summary>
    /// 死亡
    /// </summary>
    [Header("死亡")]
    public string die;
    #endregion
    #endregion
    public virtual void HeroInitializ()
    {
        _transform = transform;
        _tfposition = _transform.position;
        _rg = GetComponent<Rigidbody2D>(); // 取得剛體
        _colliders = GetComponents<Collider2D>(); // 取得所有碰撞器
        if (_gameManagerScript.HeroList.Count == 1) _gameManagerScript.SelectedHeroFunc(this); // 如果只有一個英雄，就選擇這個英雄
        _animator = GetComponent<Animator>(); // 取得動畫控制器
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0); // 取得動畫狀態
        if (_animator != null)
        {
            AnimationClip[] _animationClips = _animator.runtimeAnimatorController.animationClips;
            AnimationClip _acp;
            string _acpName;
            string _acpNameLower;
            for (int i = 0; i < _animationClips.Length; i++)
            {
                _acp = _animationClips[i];
                _acpName = _acp.name;
                _acpNameLower = _acpName.ToLower();
                //不一定有的動畫區
                if (_acpNameLower.LastIndexOf("run_atk") != -1 || _acpNameLower.LastIndexOf("idle_atk") != -1 || _acpNameLower.LastIndexOf("roll_atk") != -1) // 帶著移動相關的攻擊
                {
                    movAtk.Add(_acpName);
                    isMovAtk = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("jump_atk") != -1) // 跳躍攻擊
                {
                    jumpAtk = _acpName;
                    isJumpAtk = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("dash") != -1) // 衝刺
                {
                    dash = _acpName;
                    isDash = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("hit") != -1) // 受傷
                {
                    hit = _acpName;
                    isHit = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("defense1") != -1) // 防禦被擊中
                {
                    def1 = _acpName;
                    isDef1 = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("defense2") != -1) // 防禦
                {
                    def2 = _acpName;
                    isDef2 = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("roll") != -1) // 翻滾
                {
                    roll = _acpName;
                    isRoll = true;
                    continue;
                }
                //同一組動畫
                if (_acpNameLower.LastIndexOf("jump") != -1) // 跳躍
                {
                    jump = _acpName;
                    isJump = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("drop") != -1) // 掉落
                {
                    drop = _acpName;
                    isDrop = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("land") != -1) // 著地
                {
                    land = _acpName;
                    isLand = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("miss0") != -1) // 閃現 消失
                {
                    miss0 = _acpName;
                    isMiss0 = true;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("miss1") != -1) // 閃現 出現
                {
                    miss1 = _acpName;
                    isMiss1 = true;
                    continue;
                }
                //一定有的動畫區
                if (_acpNameLower.LastIndexOf("atk") != -1) // 一般攻擊
                {
                    atk.Add(_acpName);
                    continue;
                }
                if (_acpNameLower.LastIndexOf("run") != -1) // 移動
                {
                    run = _acpName;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("idle") != -1) // 等待
                {
                    idle = _acpName;
                    continue;
                }
                if (_acpNameLower.LastIndexOf("die") != -1) // 死亡
                {
                    die = _acpName;
                    continue;
                }
            }
        }
    }

    public virtual void HeroDuelStateFunc()
    {

    }
    /// <summary>
    /// AI控制時的狀態機
    /// </summary>
    public virtual void HeroStateFunc()
    {
        _tfposition = _transform.position; // 更新位置

        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float _normalizedTime = _animatorStateInfo.normalizedTime;
        _normalizedTime -= Mathf.Floor(_normalizedTime); // 取得動畫時間

        playAnimationTime += _Time;
        if (!isPlayerControl)
        {
            if (playAnimationTime < playAnimationTimeMax) return;
            if (_HeroNowState == HeroState.Attack && _normalizedTime < 0.9f) return;
            if (_HeroNowState == HeroState.Hit && _normalizedTime < 0.9f) return;
        }
        switch (_HeroChengState)
        {
            case HeroState.Idle:
                _animator.Play(idle);
                break;
            case HeroState.Run:
                _animator.Play(run);
                break;
            case HeroState.Die:
                _animator.Play(die);
                break;
            default:
                switch (_HeroChengState)
                {
                    case HeroState.Def:
                        Defense();
                        break;
                    case HeroState.Hit:
                        animatorPlay(hit);
                        break;
                    case HeroState.Attack:
                        animatorPlay(atk[Random.Range(0, atk.Count)]);
                        Collider2D _col;
                        string _colTag;
                        HeroScript _hs; // 敵方英雄
                        SoldierScript _ss; // 敵方士兵
                        MainFortressBaseScript _mf; // 敵方主堡

                        if (enemyCollider.Length > 0)
                        {
                            for (int i = 0; i < enemyCollider.Length; i++)
                            {
                                _col = enemyCollider[i];
                                if (_col == null) continue;
                                _colTag = _col.tag;
                                if (_colTag == staticPublicObjectsStaticName.DarkHeroTag)
                                {
                                    _hs = _col.GetComponent<HeroScript>();
                                    if (_hs != null)
                                        _hs.HeroHP(1);
                                }
                                else if (_colTag == staticPublicObjectsStaticName.DARKSoldierTag)
                                {
                                    _ss = _col.GetComponent<SoldierScript>();
                                    if (_ss != null)
                                        _ss.MustBeInjured(1);
                                }
                                else if (_colTag == staticPublicObjectsStaticName.DarkMainFortressTag)
                                {
                                    _mf = _col.GetComponent<MainFortressBaseScript>();
                                    if (_mf != null)
                                        _mf.MainFortressHit(1);
                                }
                            }
                        }
                        break;
                    case HeroState.Dash:
                        break;
                    case HeroState.Miss:
                        break;
                    case HeroState.MovAtk:
                        RigidbodyFunc(-0.2f, false);
                        animatorPlay(movAtk[Random.Range(0, movAtk.Count)]);
                        break;
                    case HeroState.Jump:
                        break;
                    default:
                        break;
                }
                break;
        }
        playAnimationTime = 0;
        _HeroNowState = _HeroChengState;
    }
    /// <summary>
    /// 播放動畫的函數
    /// </summary>
    /// <param name="AnimaName">動畫名稱</param>
    /// <param name="Layer">動畫控制器圖層</param>
    /// <param name="NormalTime">0~1的浮點數</param>
    protected virtual void animatorPlay(string AnimaName, int Layer = 0, float NormalTime = 0)
    {
        _animator.Play(AnimaName, Layer, NormalTime);
    }
    /// <summary>
    /// 開啟或關閉重力、剛體
    /// </summary>
    /// <param name="gs">重力值</param>
    /// <param name="isCollider">是否開啟碰撞器</param>
    public virtual void RigidbodyFunc(float gs, bool isCollider)
    {
        _rg.gravityScale = 0;
        if (_colliders.Length == 0) return;
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = isCollider;
        }
    }

    /// <summary>
    /// 決鬥移動方法
    /// </summary>
    public virtual void HeroMove()
    {

    }

    public virtual void Atk()
    {
        _HeroChengState = HeroState.Attack;
        HeroStateFunc();
    }
    /// <summary>
    /// 一般時的移動方法
    /// </summary>
    public virtual void Move()
    {
        if (_HeroNowState == _HeroChengState && _HeroNowState == HeroState.Run) return;
        _HeroChengState = HeroState.Run;
        HeroStateFunc();
    }
    /// <summary>
    /// 一般受傷
    /// </summary>
    /// <param name="hitAmount">執行受傷動畫</param>
    public virtual void HeroHP(int hitAmount)
    {
        Hit();
    }
    public virtual void Die()
    {
        RigidbodyFunc(0, false);
    }

    public virtual void Hit()
    {
        if (!isHit) return;

    }
    /// <summary>
    /// 必須受傷
    /// </summary>
    /// <param name="hp">傷害</param>
    public virtual void MustBeInjured(int hp)
    {

    }
    /// <summary>
    /// 決鬥受傷不執行受傷動畫
    /// </summary>
    /// <param name="t"></param>
    public virtual void HeroHit(int t)
    {
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_HeroNowState == HeroState.Def || _HeroChengState == HeroState.Def)
        {
            if(isDef1 && isDef2)
            {
                Defense();
            }
            else if(isDef1)
            {
                Defense(2);
            }

            return;
        }
        Hp -= t;
        if(Hp <= 0)
        {
            Die();
        }
    }
    public virtual void Idle()
    {
        if (_HeroNowState == _HeroChengState && _HeroNowState == HeroState.Idle) return;
        _HeroChengState = HeroState.Idle;
        HeroStateFunc();
    }
    #region 英雄才有的動作
    public virtual void Defense()
    {
        if (!isDef1) return;
        animatorPlay(def1); //始終都是這個防禦效果
    }
    /// <summary>
    /// 防禦
    ///</summary>
    public virtual void Defense(int DefType = 1)
    {
        if (!isDef1 && !isDef2) return;
        //二段式防禦
        if (isDef1 && isDef2)
        {
            switch (DefType)
            {
                case 1:
                    animatorPlay(def2); //一開始先使用沒特效的防禦
                    break;
                case 2:
                    animatorPlay(def1); //一開始先使用沒特效的防禦
                    break;
            }
        }
    }
    /// <summary>
    /// 翻滾
    /// </summary>
    public virtual void Roll()
    {
        if (!isLand) return;
    }

    /// <summary>
    /// 跳躍攻擊
    /// </summary>
    public virtual void JumpAtk()
    {
        if (!isJumpAtk) return;
    }
    /// <summary>
    /// 衝刺
    /// </summary>
    public virtual void Dash()
    {
        if (!isDash) return;
    }
    /// <summary>
    /// 跳躍
    /// </summary>
    public virtual void Jump()
    {
        if (!isJump || !isDrop) return;
    }
    /// <summary>
    /// 下落
    /// </summary>
    public virtual void Drop()
    {

    }
    /// <summary>
    /// 著地
    /// </summary>
    public virtual void Land()
    {
        if (!isLand) return;
    }
    /// <summary>
    /// 閃避
    /// </summary>
    public virtual void Miss()
    {
        if (!isMiss0 || !isMiss1) return; // 沒有完整閃現動畫
    }

    public virtual void MoveAtk()
    {
        if (!isMovAtk) return;
        _HeroChengState = HeroState.MovAtk;
    }
    #endregion


    /// <summary>
    /// 英雄狀態
    /// </summary>
    public enum HeroState
    {
        /// <summary>
        /// 等待
        /// </summary>
        Idle,
        /// <summary>
        /// 移動
        /// </summary>
        Run,
        /// <summary>
        /// 防禦
        /// </summary>
        Def,
        /// <summary>
        /// 受傷
        /// </summary>
        Hit,
        /// <summary>
        /// 死亡
        /// </summary>
        Die,
        /// <summary>
        /// 攻擊
        /// </summary>
        Attack,
        /// <summary>
        /// 衝刺
        /// </summary>
        Dash,
        /// <summary>
        /// 閃現
        /// </summary>
        Miss,
        /// <summary>
        /// 移動攻擊
        ///</summary>
        MovAtk,
        /// <summary>
        /// 跳躍
        /// </summary>
        Jump,
    }
    public virtual void PhyOverlapBoxAll(Vector2 Pos)
    {
        PhySizeVector2 = Vector2.one * phySize;
        PhySizeVector2.y *= 1.5f;
        float f = phySize / 5f;
        if (_transform.localScale.x > 0)
            Pos.x += f;
        else
            Pos.x -= f;
        PhyOffset = Pos.x;
        enemyCollider = Physics2D.OverlapBoxAll(Pos, PhySizeVector2, 0, enemyLayerMask);

        Vector2 direction;
        direction = Vector2.right.normalized;
        if (_transform.localScale.x < 0)
            direction = Vector2.left.normalized;
        enemyRayRaycastAll = Physics2D.RaycastAll(_transform.position, direction, phySize * dashDistance, enemyLayerMask);
    }

    void OnDrawGizmos()
    {
        if (_transform == null) return;
        Vector2 pos = _tfposition;
        pos.x = PhyOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos, PhySizeVector2);




        Vector2 PosSizeEnd = _tfposition;
        Vector2 direction;
        direction = Vector2.right.normalized;
        if (_transform.localScale.x < 0)
            direction = Vector2.left.normalized;

        PosSizeEnd += dashDistance * phySize * direction;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_tfposition, PosSizeEnd);
    }
}
