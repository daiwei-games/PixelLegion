using Assets.Scripts;
using Assets.Scripts.BaseClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroScript : HeroBaseScript
{
    /// <summary>
    /// 是否被玩家操控
    /// </summary>
    [Header("是否被玩家操控")]
    public bool isPlayerControl;
    /// <summary>
    /// 是否正在決鬥
    /// </summary>
    [Header("是否正在決鬥")]
    public bool IsItPossibleToDuel;
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
    /// <summary>
    /// 是否正在衝刺
    /// </summary>
    public bool IsItPossibleToDash;
    /// <summary>
    /// 衝刺時間
    /// </summary>
    private float DashTimeStart;
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
    public float missTimeStart;
    /// <summary>
    /// 閃現 出現
    /// </summary>
    [Header("閃現 出現")]
    public string miss1;
    public bool isMiss1;
    /// <summary>
    /// 是否正在閃現
    /// </summary>
    public bool IsItPossibleMiss;
    /// <summary>
    /// 如果有順移目標
    /// </summary>
    private Transform MissTargetTf;
    /// <summary>
    /// 決鬥
    /// </summary>
    [Header("決鬥")]
    public string duel;
    public bool isDuel;
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
        IsItPossibleToDuel = false; // 是否正在決鬥

        _colliders = GetComponents<Collider2D>(); // 取得所有碰撞器

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
                if (_acpNameLower.LastIndexOf("duel") != -1)
                {
                    duel = _acpName;
                    isDuel = true;
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
        if (_gameManagerScript.HeroList.Count == 1) _gameManagerScript.SelectedHeroFunc(this); // 如果只有一個英雄，就選擇這個英雄

        Attack = (int)Mathf.Ceil(302 * (_heroDataObject.PlayerHeroLv * _heroDataObject.AddAtk));

        IsItPossibleMiss = false;
        IsItPossibleToDash = false;
    }

    #region 狀態機
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="_heroChengState">狀態</param>
    /// <param name="_MoveDirection">移動方向</param>
    /// <param name="time">時間 Time.time</param>
    /// <param name="EnemyTf">如果有目標位置</param>
    public virtual void HeroDuelStateFunc(HeroState _heroChengState, Vector2 _MoveDirection, float time = 0, Transform EnemyTf = null)
    {
        switch (_heroChengState)
        {
            case HeroState.Run:
                if (!AnimationPlayOver(_HeroNowState)) return;
                HeroMove(_MoveDirection);
                break;
            case HeroState.Dash:
                if (!AnimationPlayOver(_HeroNowState)) return;
                HeroDash(time);
                break;
            case HeroState.Miss:
                if (!AnimationPlayOver(_HeroNowState)) return;
                Miss(EnemyTf, time);
                break;
            case HeroState.Miss1:
                //果是閃現(消失)的話，就可以不用判斷直接進入閃現(出現)
                if (_HeroNowState != HeroState.Miss)
                    if (!AnimationPlayOver(_HeroNowState)) return;
                break;
            default:
                HeroDuelStateFunc(_heroChengState);
                break;
        }
        playAnimationTime = 0;
        _HeroNowState = _heroChengState;
    }
    /// <summary>
    /// 攻擊
    /// </summary>
    /// <param name="_heroChengState">狀態</param>
    /// <param name="_Collider2D">射片判斷的碰撞體陣列</param>
    /// <param name="_heroScript">對手的腳本</param>
    public virtual void HeroDuelStateFunc(HeroState _heroChengState, Collider2D[] _Collider2D, HeroScript _heroScript)
    {
        switch (_heroChengState)
        {
            case HeroState.Attack:
                if (!AnimationPlayOver(_HeroNowState)) return;
                if (_Collider2D.Length > 0)
                    HeroAttack(_Collider2D);
                if (_heroScript != null)
                    HeroAttack(_heroScript);
                break;
            default:
                HeroDuelStateFunc(_heroChengState);
                break;
        }
        playAnimationTime = 0;
        _HeroNowState = _heroChengState;
    }


    /// <summary>
    /// 基本狀態機
    /// </summary>
    public virtual void HeroDuelStateFunc(HeroState _heroChengState = HeroState.Idle)
    {

        _tfposition = _transform.position; // 更新位置

        if (!isPlayerControl && !IsItPossibleToDuel)
        {
            playAnimationTime += _Time;
            if (playAnimationTime < playAnimationTimeMax) return;
        }
        if (!AnimationPlayOver(_HeroNowState)) return;
        if (Hp <= 0) _heroChengState = HeroState.Die;
        switch (_heroChengState)
        {
            case HeroState.Die:
                Die();
                break;
            case HeroState.Idle:
                Idle();
                break;
            case HeroState.Def:
                Defense();
                break;
        }

        playAnimationTime = 0;
        _HeroNowState = _heroChengState;
    }
    #endregion
    /// <summary>
    /// 操控的移動方法
    /// </summary>
    public virtual void HeroMove(Vector2 Direction)
    {
        if (!isPlayerControl && !IsItPossibleToDuel) return;
        if (Direction == Vector2.zero)
        {
            Idle();
        }
        else
        {
            Vector2 PlayerScale = _transform.localScale;
            if (Direction.x != 1 && Direction.x != -1) Direction = Vector2.zero;
            if (Direction.x != 0) PlayerScale.x = Mathf.Abs(PlayerScale.x);
            if (Direction.x < 0) PlayerScale.x *= -1;
            Direction.x *= speed;
            _rg.MovePosition(_rg.position + Direction * Time.fixedDeltaTime);
            _transform.localScale = PlayerScale;
            _tfposition = _transform.position;
            _animator.Play(run);
        }
    }

    /// <summary>
    /// 決鬥時的受傷
    /// </summary>
    /// <param name="t"></param>
    public virtual void HeroHit(int t)
    {
        Hp -= t;
        if (Hp <= 0)
        {
            //Die();
        }
    }
    /// <summary>
    /// 有兩段防禦的英雄
    ///</summary>
    public virtual void Defense(int DefType = 1)
    {
        if (isDef1 || isDef2)
        {
            if (DefType == 1) _animator.Play(def1);
            if (DefType == 2) _animator.Play(def2);
        }
        else
        {
            Idle();
        }
    }


    public virtual void HeroAttack()
    {
        _animator.Play(atk[Random.Range(0, atk.Count)]);
    }
    public virtual void HeroAttack(HeroScript _heroScript)
    {
        if (_heroScript == null) return;
        HeroAttack();
        HeroAtkTarget(_heroScript, Attack);
    }
    public virtual void HeroAttack(RaycastHit2D[] _RaycastHit2D)
    {
        if (_RaycastHit2D.Length == 0) return;
        HeroAttack();
        HeroAtkTarget(_RaycastHit2D, Attack);
    }
    public virtual void HeroAttack(Collider2D[] _Collider2D)
    {
        if (_Collider2D.Length == 0) return;
        HeroAttack();
        HeroAtkTarget(enemyCollider, Attack);
    }
    /// <summary>
    /// 一般時的移動方法
    /// </summary>
    public virtual void Move()
    {
        if (_HeroNowState == _HeroChengState && _HeroNowState == HeroState.Run) return;
        _animator.Play(run);
    }
    /// <summary>
    /// 一般受傷
    /// </summary>
    /// <param name="hitAmount">執行受傷動畫</param>
    public virtual void HeroHP(int hitAmount)
    {
        if (_gameManagerScript.isDuel)
        {
            HeroHit(hitAmount);
            return;
        }
        Hp -= hitAmount;
        Hit();
    }
    /// <summary>
    /// 一般受傷 會傳入碰撞器
    /// </summary>
    /// <param name="hitAmount"></param>
    /// <param name="_Tf">攻擊者的 Transform</param>
    public virtual void HeroHP(int hitAmount, Transform _Tf)
    {
        HeroHP(hitAmount);
    }
    public virtual void Idle()
    {
        _animator.Play(idle);
    }
    public virtual void Die()
    {
        _animator.Play(die);
    }

    public virtual void Hit()
    {
        if (!isHit) return;
        _animator.Play(hit);
    }
    /// <summary>
    /// 必須受傷
    /// </summary>
    /// <param name="hp">傷害</param>
    public virtual void MustBeInjured(int hp)
    {

    }
    /// <summary>
    /// 翻滾
    /// </summary>
    public virtual void Roll()
    {
        if (!isLand) return;
    }
    #region 跳躍相關
    /// <summary>
    /// 跳躍攻擊
    /// </summary>
    public virtual void JumpAtk()
    {
        if (!isJumpAtk) return;
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
    #endregion

    /// <summary>
    /// 帶攻擊的移動方式
    /// </summary>
    /// <param name="Tf"></param>
    public virtual void MoveAtk(Transform Tf)
    {
        if (!isMovAtk) return;

    }


    /// <summary>
    /// 計算動畫進度
    /// </summary>
    public float AnimatorStateInfoNormalizedTime()
    {
        _animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return _animatorStateInfo.normalizedTime;
    }
    /// <summary>
    /// 判斷目前動畫是否執行結束，未結束，判斷是否為不可切換的動畫
    /// </summary>
    /// <returns>反為布林值，只要為true就代表有不可切換的動畫正在播放</returns>
    public bool AnimationPlayOver()
    {
        bool IsOver = false;
        float s = AnimatorStateInfoNormalizedTime();
        //if (s < 0.9f)
        //{
        //    if (_HeroNowState == HeroState.Attack) IsOver = true;
        //    else if (_HeroNowState == HeroState.Dash) IsOver = true;
        //    else if (_HeroNowState == HeroState.MovAtk) IsOver = true;
        //    else if (_HeroNowState == HeroState.Dash) IsOver = true;
        //    else if (_HeroNowState == HeroState.Miss) IsOver = true;
        //    else if (_HeroNowState == HeroState.Miss1) IsOver = true;
        //}



        return IsOver;
    }

    public bool AnimationPlayOver(HeroState heroNowState)
    {
        float s = AnimatorStateInfoNormalizedTime();
        bool PlayOkay = false;
        if (heroNowState != HeroState.Die)
        {
            switch (heroNowState)
            {
                case HeroState.Attack:
                case HeroState.Dash:
                case HeroState.MovAtk:
                    if (s > 0.99f)
                    {
                        animatorPlay("", -1, 0);
                        PlayOkay = true;
                    }
                    else
                    {
                        PlayOkay = false;
                    }
                    break;
                case HeroState.Miss:
                case HeroState.Miss1:
                    if (IsItPossibleMiss)
                    {
                        if (heroNowState == HeroState.Miss) PlayOkay = false;
                        if (heroNowState == HeroState.Miss1) PlayOkay = false;
                    }
                    if (heroNowState == HeroState.Miss1 && !IsItPossibleMiss)
                    {
                        if (s > 0.99f) PlayOkay = true;
                    }
                    break;
                default:
                    PlayOkay = true;
                    break;
            }
        }
        if (isPlayerControl)
        {
            //Debug.Log(s);
            //Debug.Log(heroNowState);
            //Debug.Log(PlayOkay);
        }
        return PlayOkay;
    }
    /// <summary>
    /// 播放動畫的函數
    /// </summary>
    /// <param name="AnimaName">動畫名稱</param>
    /// <param name="Layer">動畫控制器圖層</param>
    /// <param name="NormalTime">0~1的浮點數</param>
    public virtual void animatorPlay(string AnimaName, int Layer = 0, float NormalTime = 0)
    {
        _animator.Play(AnimaName, Layer, NormalTime);
    }
    /// <summary>
    /// 關閉或打開碰撞器
    /// </summary>
    public void ColliderClose(bool oc)
    {
        if (_colliders.Length == 0) return;
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = oc;

            Debug.Log(_colliders[i].enabled);
        }
    }

    /// <summary>
    /// 射線判斷到的碰撞器
    /// 攻擊時的的判斷 Collider2D 版
    /// </summary>
    /// <param name="_Collider2D">碰撞體</param>
    /// <param name="Hit">傷害數字</param>
    public virtual void HeroAtkTarget(Collider2D[] _Collider2D, int Hit = 1)
    {
        Collider2D _col;
        string _colTag;
        HeroScript _hs; // 敵方英雄
        SoldierScript _ss; // 敵方士兵
        MainFortressBaseScript _mf; // 敵方主堡
        if (AnimationPlayOver()) return;
        if (enemyCollider.Length > 0)
        {
            //如果是黑暗英雄
            if (this.CompareTag(staticPublicObjectsStaticName.DarkHeroTag))
            {
                for (int i = 0; i < _Collider2D.Length; i++)
                {
                    _col = _Collider2D[i];
                    if (_col == null) continue;
                    _colTag = _col.tag;

                    if (_colTag == staticPublicObjectsStaticName.HeroTag)
                    {
                        _hs = _col.GetComponent<HeroScript>();
                        if (_hs != null)
                            _hs.HeroHP(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.PlayerSoldierTag)
                    {
                        _ss = _col.GetComponent<SoldierScript>();
                        if (_ss != null)
                            _ss.MustBeInjured(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.MainFortressTag)
                    {
                        _mf = _col.GetComponent<MainFortressBaseScript>();
                        if (_mf != null)
                            _mf.MainFortressHit(Hit);
                    }
                }
            }
            // 如果是光明英雄
            if (this.CompareTag(staticPublicObjectsStaticName.HeroTag))
            {
                for (int i = 0; i < _Collider2D.Length; i++)
                {
                    _col = _Collider2D[i];
                    if (_col == null) continue;
                    _colTag = _col.tag;

                    if (_colTag == staticPublicObjectsStaticName.DarkHeroTag)
                    {
                        _hs = _col.GetComponent<HeroScript>();
                        if (_hs != null)
                            _hs.HeroHP(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.DARKSoldierTag)
                    {
                        _ss = _col.GetComponent<SoldierScript>();
                        if (_ss != null)
                            _ss.MustBeInjured(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.DarkMainFortressTag)
                    {
                        _mf = _col.GetComponent<MainFortressBaseScript>();
                        if (_mf != null)
                            _mf.MainFortressHit(Hit);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 射線判斷到的 RaycastHit2D
    /// 攻擊時的的判斷 RaycastHit2D 版
    /// </summary>
    /// <param name="_RaycastHit2D">射線偵測</param>
    /// <param name="Hit">傷害數值</param>
    public virtual void HeroAtkTarget(RaycastHit2D[] _RaycastHit2D, int Hit = 1)
    {
        RaycastHit2D _col;
        Transform _Tf;
        string _colTag;
        HeroScript _hs; // 敵方英雄
        SoldierScript _ss; // 敵方士兵
        MainFortressBaseScript _mf; // 敵方主堡
        if (AnimationPlayOver()) return;
        if (enemyCollider.Length > 0)
        {
            if (this.CompareTag(staticPublicObjectsStaticName.DarkHeroTag))
            {

                for (int i = 0; i < _RaycastHit2D.Length; i++)
                {
                    _col = _RaycastHit2D[i];
                    if (!_col) continue;
                    _Tf = _col.transform;
                    _colTag = _Tf.tag;
                    if (_colTag == staticPublicObjectsStaticName.HeroTag)
                    {
                        _hs = _Tf.GetComponent<HeroScript>();
                        if (_hs != null)
                            _hs.HeroHP(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.PlayerSoldierTag)
                    {
                        _ss = _Tf.GetComponent<SoldierScript>();
                        if (_ss != null)
                            _ss.MustBeInjured(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.MainFortressTag)
                    {
                        _mf = _Tf.GetComponent<MainFortressBaseScript>();
                        if (_mf != null)
                            _mf.MainFortressHit(Hit);
                    }
                }
            }
            if (this.CompareTag(staticPublicObjectsStaticName.HeroTag))
            {
                for (int i = 0; i < _RaycastHit2D.Length; i++)
                {
                    _col = _RaycastHit2D[i];
                    if (!_col) continue;
                    _Tf = _col.transform;
                    _colTag = _Tf.tag;
                    if (_colTag == staticPublicObjectsStaticName.DarkHeroTag)
                    {
                        _hs = _Tf.GetComponent<HeroScript>();
                        if (_hs != null)
                            _hs.HeroHP(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.DARKSoldierTag)
                    {
                        _ss = _Tf.GetComponent<SoldierScript>();
                        if (_ss != null)
                            _ss.MustBeInjured(Hit);
                    }
                    else if (_colTag == staticPublicObjectsStaticName.DarkMainFortressTag)
                    {
                        _mf = _Tf.GetComponent<MainFortressBaseScript>();
                        if (_mf != null)
                            _mf.MainFortressHit(Hit);
                    }
                }
            }

        }
    }
    /// <summary>
    /// 攻擊目標對目標造成傷害
    /// </summary>
    /// <param name="_enemyHeroScript">敵人英雄的腳本</param>
    public virtual void HeroAtkTarget(HeroScript _enemyHeroScript, int Hit = 1)
    {
        if (_enemyHeroScript == null) return;
        _enemyHeroScript.HeroHit(Hit);
    }


    #region 衝刺、瞬移
    /// <summary>
    /// 閃現
    /// </summary>
    public virtual void Miss(Transform TargetTf = null, float time = 0)
    {
        if (IsItPossibleMiss) return;
        if (!isMiss0 && !isMiss1) return; // 沒有完整閃現動畫
        missTimeStart = time + +.2f; // 閃現開始時間
        if (TargetTf != null) MissTargetTf = TargetTf;
        _animator.Play(miss0); // 閃現動畫
        IsItPossibleMiss = true; // 正在閃現
        // 先飛起來
        Vector2 Pos = Vector2.up;
        _transform.Translate(Pos);
        // 重力歸零、碰撞關閉
        _rg.gravityScale = 0;
        ColliderClose(false);
    }
    public void Miss1(float time = 0)
    {
        if (!isMiss1) return; // 沒有完整閃現動畫
        Vector2 Pos = Vector2.right;

        if (MissTargetTf == null)
            if (_transform.localScale.x < 0) Pos = Vector2.left;
        else
            if (_transform.localScale.x < MissTargetTf.localScale.x) Pos = Vector2.left;

        if (missTimeStart > time) _transform.Translate(Pos * .35f); // 移動中
        if (missTimeStart <= time)
        {
            HeroDuelStateFunc(HeroState.Miss1, Vector2.zero, time, null);
            _animator.Play(miss1); // 閃現(出現)動畫
            // 重力恢復、碰撞開啟
            ColliderClose(true);
            _rg.gravityScale = 1;
            //目前沒有閃現
            MissTargetTf = null;

            if (missTimeStart + 0.3f <= time) IsItPossibleMiss = false; // 更長時之後才可以再度閃現
        }


    }
    /*-------------------------------------------------------------*/
    /// <summary>
    /// 衝刺
    /// </summary>
    public virtual void HeroDash(float time)
    {
        if (_HeroNowState == HeroState.Dash) return;
        if (IsItPossibleToDash) return;
        if (!isDash) return;
        if (time == 0) return; // 當時間為零，直接停止衝刺
        DashTimeStart = time + .3f;
        IsItPossibleToDash = true;
        _animator.Play(dash);

        Vector2 Pos = Vector2.up;
        _transform.Translate(Pos);
    }
    public void FastForward(Transform _Tf, float time)
    {

        Vector2 Pos = Vector2.right;
        if (_Tf.localScale.x < 0) Pos = Vector2.left;
        if (DashTimeStart > time) _Tf.Translate(Pos * .2f);
        if (DashTimeStart + 0.1f <= time) IsItPossibleToDash = false;
    }
    #endregion
    public virtual void PhyOverlapBoxAll(Vector2 Pos)
    {
        PhySizeVector2 = Vector2.one * phySize;
        PhySizeVector2.y *= 1.5f;
        enemyCollider = Physics2D.OverlapBoxAll(Pos, PhySizeVector2, 0, enemyLayerMask);

        Vector2 direction;
        direction = Vector2.right.normalized; //向右
        if (_transform.localScale.x < 0)
            direction = Vector2.left.normalized; //向左

        //射線取得RaycastHit2D
        enemyRayRaycastAll = Physics2D.RaycastAll(_transform.position, direction, phySize * dashDistance, enemyLayerMask);
    }

    void OnDrawGizmos()
    {
        if (_transform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_tfposition, PhySizeVector2);

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
    /// 閃現 - 消失
    /// </summary>
    Miss,
    /// <summary>
    /// 閃現 - 出現
    /// </summary>
    Miss1,
    /// <summary>
    /// 移動攻擊
    ///</summary>
    MovAtk,
    /// <summary>
    /// 跳躍
    /// </summary>
    Jump
}
