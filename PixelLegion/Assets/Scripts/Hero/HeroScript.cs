using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroScript : LeadToSurviveGameBaseClass
{
    #region 英雄基本資料
    /// <summary>
    /// 玩家資料物件
    /// </summary>
    public playerDataObject _Pdo;
    /// <summary>
    /// 英雄基本素質
    /// </summary>
    [Header("英雄基本素質 (0.001 - 100)"), Range(0.001f, 100f)]
    public float BasicQuality;
    /// <summary>
    /// 基本體質
    /// </summary>
    [Header("基本體質 (1 - 300)"), Range(1, 300)]
    public int BasicConstitution;
    /// <summary>
    /// 基本血量
    /// </summary>
    [Header("基本體質 (100 - 6000)"), Range(100, 6000)]
    public int BasicHp;
    /// <summary>
    /// 英雄ID (系統判斷的ID)
    /// </summary>
    [Header("英雄ID")]
    public string HeroID;
    /// <summary>
    /// 英雄名稱
    /// </summary>
    [Header("英雄名稱")]
    public string HeroName;
    /// <summary>
    /// 英雄頭像
    /// </summary>
    [Header("英雄頭像")]
    public Sprite HeroAvatar;
    /// <summary>
    /// 英雄血量
    /// </summary>
    [Header("英雄血量")]
    public int Hp;
    [HideInInspector]
    public int HpMax;
    /// <summary>
    /// 攻擊力
    /// </summary>
    [Header("攻擊力"), HideInInspector]
    public int Attack;
    /// <summary>
    /// 爆擊率
    /// </summary>
    [Header("爆擊率"), Range(0, 100), SerializeField]
    private int AttackCriticalHitRate;
    /// <summary>
    /// 爆擊倍率
    /// </summary>
    [Header("爆擊倍率"), Range(1, 100), SerializeField]
    private int AttackMagnification;
    /// <summary>
    /// 攻擊力抵銷
    /// </summary>
    [Header("攻擊力抵銷"), HideInInspector]
    public int Def;
    /// <summary>
    /// 範圍百分比 當等級越高，這個數值越小，代表範圍越精準
    /// [計算方式為，每增加一等範圍百分比減少0.005]
    /// </summary>
    [Header("影響攻擊、防禦範圍值"), HideInInspector]
    public float Percentage;
    #endregion

    #region 英雄調用腳本
    /// <summary>
    /// GM管理器腳本
    /// </summary>
    [Header("GM管理器腳本"),HideInInspector]
    public GameManager _gameManagerScript;
    /// <summary>
    /// 英雄操作介面
    /// </summary>
    [Header("英雄操作介面"),HideInInspector]
    public UIHeroController heroController;
    #endregion

    #region 英雄預先取得資料
    /// <summary>
    /// 剛體
    /// </summary>
    public Rigidbody2D _rg;
    /// <summary>
    /// 取得自己身上的所有碰撞
    /// </summary>
    public Collider2D[] _colliders;
    /// <summary>
    /// 動畫播放組件
    /// </summary>
    [Header("動畫播放組件")]
    [HideInInspector]
    public Animator _animator;
    /// <summary>
    /// 取得動畫狀態
    /// </summary>
    [HideInInspector]
    public AnimatorStateInfo _animatorStateInfo;
    [HideInInspector]
    public float _Time;
    #endregion

    #region 射線
    /// <summary>
    /// 英雄判斷敵人的圖層
    /// </summary>
    [Header("英雄判斷敵人的圖層")]
    public LayerMask enemyLayerMask;
    /// <summary>
    /// 射線取得敵人的碰撞
    /// </summary>
    [Header("射線取得敵人的碰撞")]
    public Collider2D[] enemyCollider;
    /// <summary>
    /// 射線的範圍
    /// </summary>
    [Header("射線的範圍")]
    public float phySize;
    /// <summary>
    /// 射線偏移
    /// </summary>
    [Header("射線偏移")]
    public float PhyOffset;
    /// <summary>
    /// 射線範圍
    /// </summary>
    protected Vector2 PhySizeVector2;

    /// <summary>
    /// 地板圖層
    /// </summary>
    public LayerMask flootLayer;
    /// <summary>
    /// 是否碰到地板
    /// </summary>
    public bool isfloot;
    #endregion

    #region 目標
    /// <summary>
    /// AI操控時的主要目標
    /// </summary>
    [Header("AI操控時的主要目標")]
    public Transform _enemyNowMainFortress;
    /// <summary>
    /// AI操控時的緊急目標
    /// </summary>
    [Header("AI操控時的緊急目標")]
    public Transform _target;
    #endregion

    #region 狀態
    /// <summary>
    /// 狀態機現在的狀態
    /// </summary>
    [Header("狀態機現在的狀態")]
    public HeroState _HeroNowState;
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
    /// 移動速度
    /// </summary>
    [Header("移動速度"), Range(1, 30)]
    public float speed;
    /// <summary>
    /// 目前是否爆擊
    /// </summary>
    [Header("目前是否爆擊"), HideInInspector]
    public bool isCriticalHitRate;
    /// <summary>
    /// 是否晃動攝影機
    /// </summary>
    [Header("是否晃動攝影機"), HideInInspector]
    public bool isCameraShake;
    /// <summary>
    /// 是否停止動畫，false = 不停止
    /// </summary>
    [Header("是否停止動畫，false = 不停止"), HideInInspector]
    public bool isAnimationFrameStorp;
    #endregion

    #region 動畫清單
    /// <summary>
    /// 移動相關攻擊動畫
    /// </summary>
    [Header("移動相關攻擊動畫")]
    [HideInInspector] public List<string> movAtk;
    [HideInInspector] public bool isMovAtk;
    /// <summary>
    /// 跳躍攻擊動畫
    /// </summary>
    [Header("跳躍攻擊動畫")]
    [HideInInspector] public string jumpAtk;
    [HideInInspector] public bool isJumpAtk;
    /// <summary>
    /// 重擊
    /// </summary>
    [Header("重擊")]
    [HideInInspector] public string heavyAttack;
    [HideInInspector] public bool isHeavyAttack;
    /// <summary>
    /// 衝刺動畫
    /// </summary>
    [Header("衝刺動畫")]
    [HideInInspector] public string dash;
    [HideInInspector] public bool isDash;
    /// <summary>
    /// 是否正在衝刺
    /// </summary>
    [HideInInspector] public bool IsItPossibleToDash;
    /// <summary>
    /// 衝刺時間
    /// </summary>
    [HideInInspector] private float DashTimeStart;
    ///<summary>
    /// 受傷動畫
    ///</summary>
    [Header("受傷動畫")]
    public string hit;
    [HideInInspector] public bool isHit;
    /// <summary>
    /// 受到攻擊的防禦
    /// </summary>
    [Header("受到攻擊的防禦")]
    [HideInInspector] public string def1;
    [HideInInspector] public bool isDef1;
    /// <summary>
    /// 沒受到攻擊的防禦
    /// </summary>
    [Header("沒受到攻擊的防禦")]
    [HideInInspector] public string def2;
    [HideInInspector] public bool isDef2;
    /// <summary>
    /// 翻滾
    /// </summary>
    [Header("翻滾")]
    [HideInInspector] public string roll;
    [HideInInspector] public bool isRoll;


    #region 組動畫
    /// <summary>
    /// 閃現 消失
    /// </summary>
    [Header("閃現 消失")]
    [HideInInspector] public string miss0;
    [HideInInspector] public bool isMiss0;
    [HideInInspector] public float missTimeStart;
    /// <summary>
    /// 閃現 出現
    /// </summary>
    [Header("閃現 出現")]
    [HideInInspector] public string miss1;
    [HideInInspector] public bool isMiss1;
    /// <summary>
    /// 是否正在閃現
    /// </summary>
    public bool IsItPossibleMiss;
    /// <summary>
    /// 如果有順移目標
    /// </summary>
    private Transform MissTargetTf;
    /// <summary>
    /// 衝刺時方向一致不能中途修改
    /// </summary>
    protected Vector2 MissOrDashDirection;

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

    #region 音效
    /// <summary>
    /// 英雄音效播放管理器
    /// </summary>
    [Header("英雄音效播放管理器")]
    public AudioSource _AudioSource;
    /// <summary>
    /// 播放受傷音效
    /// </summary>
    [Header("播放受傷音效")]
    public AudioSource _AudioSourceHit;
    /// <summary>
    /// 音效清單管理
    /// </summary>
    protected SFXListScript _ScriptList;
    #endregion

    #region 操控
    /// <summary>
    /// 搖桿腳本
    /// </summary>
    [Header("搖桿")]
    public VariableJoystick _Joystick;
    /// <summary>
    /// 玩家操控腳本
    /// </summary>
    [Header("玩家操控腳本"), HideInInspector]
    public HeroController _Hc;
    /// <summary>
    /// UI操控腳本
    /// </summary>
    [Header("UI操控腳本")]
    public UIHeroController _UIHc;
    /// <summary>
    /// 目前攻擊次數，
    /// 每攻擊一次 +1 跟攻擊動畫數量相同時重置為 0，
    /// 如果攻擊次數超過動畫就將時間跟次數歸 0。
    /// </summary>
    [Header("攻擊招式"), HideInInspector]
    public int AtkCount;
    /// <summary>
    /// 監視器中心點
    /// </summary>
    [HideInInspector]
    public CameraCenterScript CameraCenterScript;
    #endregion

    #region 狀態時間設定
    /// <summary>
    /// 每攻擊一次重置時間
    /// </summary>
    [Header("每攻擊一次重置時間"), HideInInspector]
    public float AtkTime;
    /// <summary>
    /// 每一輪攻擊間隔
    /// 超過限制時間將攻擊次數歸 0
    /// </summary>
    [Header("每一輪攻擊間隔")]
    public float AtkTimeMax;
    /// <summary>
    /// 快速移動的時間設定
    /// </summary>
    [Header("快速移動的時間設定\n閃現")]
    public float MissTime;
    /// <summary>
    /// 衝刺
    /// </summary>
    [Header("衝刺")]
    public float DashTime;
    /// <summary>
    /// 翻滾
    /// </summary>
    [Header("翻滾")]
    public float RollTime;
    /// <summary>
    /// 防禦時間間隔
    /// </summary>
    [Header("防禦時間設定")]
    public float DefTimeMax;
    /// <summary>
    /// 防禦計時
    /// </summary>
    [Header("防禦計時"),HideInInspector]
    public float DefTime;
    /// <summary>
    /// 是否可以防禦或變更為其他動作
    /// </summary>
    [HideInInspector]
    public bool isNowDef;
    /// <summary>
    /// 停頓幀時間
    /// </summary>
    [HideInInspector]
    public float AnimationFrameStorpTime;
    /// <summary>
    /// 停頓幀時間最大值
    /// </summary>
    [Header("停頓幀時間最大值"), HideInInspector]
    public float AnimationFrameStorpTimeMax;
    /// <summary>
    /// 晃動攝影機計時
    /// </summary>
    [HideInInspector]
    public float CameraShakeTime;
    /// <summary>
    /// 晃動攝影機計時最大值
    /// </summary>
    [Header("晃動攝影機計時最大值"), HideInInspector]
    public float CameraShakeTimeMax;
    #endregion

    #region VFX
    /// <summary>
    /// 防禦被擊中的粒子
    /// </summary>
    [HideInInspector]
    public ParticleSystem DefParticle;
    /// <summary>
    /// 被擊中的粒子
    /// </summary>
    [HideInInspector]
    public ParticleSystem HitParticle;
    /// <summary>
    /// 被爆擊粒子
    /// </summary>
    [HideInInspector]
    public ParticleSystem CameraShakeParticle;
    #endregion

    #region 物件
    /// <summary>
    /// 血條腳本
    /// </summary>
    [HideInInspector]
    public HpScript _Hps;
    /// <summary>
    /// 防禦條腳本
    /// </summary>
    [HideInInspector]
    public DefScript _Defs;
    #endregion

    public virtual void HeroInitializ()
    {
        _Hc = GetComponent<HeroController>();
        if (_Hc != null)
        {
            _Hc._UIHc = _UIHc;
            _Hc._Joystick = _Joystick;
            _Hc._Hs = this;
            _Hc.enabled = false;
        }
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
                if (_acpNameLower.LastIndexOf("_heavy_attack") != -1)
                {
                    heavyAttack = _acpName;
                    isHeavyAttack = true;
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

        IsItPossibleMiss = false; // 是否可以閃現
        IsItPossibleToDash = false; // 是否可以衝刺
        MissTargetTf = null; // 閃現的目標

        isfloot = false; // 是否在地板上
        isNowDef = true; // 是否可以防禦或變更為其他動作

        isAnimationFrameStorp = false; // 是否停止動畫

        MissOrDashDirection = Vector2.zero; // 閃現或衝刺的方向

        _AudioSource = _Go.AddComponent<AudioSource>(); // 音源
        _AudioSourceHit = _Go.AddComponent<AudioSource>(); // 受傷音源

        AtkCount = 0; // 攻擊次數
        AtkTime = 0;

        if (this.CompareTag(staticPublicObjectsStaticName.DarkHeroTag))
        {
            AtkTimeMax++;
            Def *= (int)Mathf.Ceil(.7f);
        }

        AnimationFrameStorpTimeMax = .25f;
        CameraShakeTimeMax = .5f;


        if (AtkTimeMax < 1) AtkTimeMax = 1;
        if (AttackMagnification < 1) AttackMagnification = 1;
    }
    #region 狀態機
    /// <summary>
    /// 判斷狀態機只會出現死亡
    /// </summary>
    /// <param name="_heroChengState">目前的狀態機</param>
    /// <returns>返回原本的狀態機，或是死亡</returns>
    protected virtual HeroState IfHpToDie(HeroState _heroChengState)
    {
        if (Hp <= 0) _heroChengState = HeroState.Die;
        return _heroChengState;
    }
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="_heroChengState">狀態</param>
    /// <param name="_MoveDirection">移動方向</param>
    /// <param name="time">時間 Time.time</param>
    /// <param name="EnemyTf">如果有目標位置</param>
    public virtual void HeroDuelStateFunc(HeroState _heroChengState, Vector2 _MoveDirection, float time = 0, Transform EnemyTf = null)
    {
        _heroChengState = IfHpToDie(_heroChengState);
        switch (_heroChengState)
        {
            case HeroState.Run:
                if (!AnimationPlayOver(_HeroNowState)) return;
                HeroMove(_MoveDirection);
                break;
            case HeroState.Dash:
                if (_HeroNowState != HeroState.Def)
                {
                    if (!AnimationPlayOver(_HeroNowState)) return;
                }
                HeroDash(time);
                break;
            case HeroState.Miss:
                if (_HeroNowState != HeroState.Def)
                {
                    if (!AnimationPlayOver(_HeroNowState)) return;
                }
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
        _HeroNowState = _heroChengState;
    }
    /// <summary>
    /// 攻擊
    /// </summary>
    /// <param name="_heroChengState">狀態</param>
    /// <param name="_Collider2D">射片判斷的碰撞體陣列</param>
    /// <param name="_heroScript">對手的腳本</param>
    public virtual void HeroDuelStateFunc(HeroState _heroChengState, Collider2D[] _Collider2D)
    {
        _heroChengState = IfHpToDie(_heroChengState);
        switch (_heroChengState)
        {
            case HeroState.Attack:
                if (AtkCount == atk.Count && AtkTime < AtkTimeMax) return;
                if (!AnimationPlayOver(_HeroNowState)) return;
                HeroAttack(_Collider2D);
                break;
            case HeroState.HeavyAttack:
                if (_HeroNowState != HeroState.Attack && _HeroNowState != HeroState.Def)
                {
                    if (!AnimationPlayOver(_HeroNowState)) return;
                }
                HeavyAttack();
                break;
            default:
                HeroDuelStateFunc(_heroChengState);
                break;
        }
        _HeroNowState = _heroChengState;
    }


    /// <summary>
    /// 基本狀態機
    /// </summary>
    public virtual void HeroDuelStateFunc(HeroState _heroChengState = HeroState.Idle)
    {
        _heroChengState = IfHpToDie(_heroChengState);
        switch (_heroChengState)
        {
            case HeroState.Hit:
                if (!AnimationPlayOver(_HeroNowState)) return;
                Hit();
                break;
            case HeroState.Die:
                Die();
                break;
            case HeroState.Idle:
                if (!AnimationPlayOver(_HeroNowState)) return;
                Idle();
                break;
            case HeroState.Def:
                if (!AnimationPlayOver(_HeroNowState)) return;
                Defense();
                break;
        }
        _HeroNowState = _heroChengState;
    }
    #endregion
    /// <summary>
    /// 操控的移動方法
    /// </summary>
    public virtual void HeroMove(Vector2 Direction)
    {
        if (Direction == Vector2.zero)
        {
            HeroDuelStateFunc();
        }
        else
        {
            Vector2 PlayerScale = _Tf.localScale;
            if (Direction.x != 1 && Direction.x != -1) Direction = Vector2.zero;
            if (Direction.x != 0) PlayerScale.x = Mathf.Abs(PlayerScale.x);
            if (Direction.x < 0) PlayerScale.x *= -1;
            Direction.x *= speed;
            _rg.MovePosition(_rg.position + Direction * Time.fixedDeltaTime);
            _Tf.localScale = PlayerScale;
            _animator.Play(run);
        }
    }


    /// <summary>
    /// 有兩段防禦的英雄
    ///</summary>
    public virtual void Defense(int DefType = 1)
    {
        if (isDef1 || isDef2)
        {
            if (DefType == 1)
            {
                _animator.Play(def1);
                DefTime = DefTimeMax; //重置防禦時間
                _Defs.GetDefTimeMax();
                isNowDef = false; // 已經開始執行所以目前不能再防禦
            }
            if (DefType == 2)
            {
                DefTime -= .2f; //每次被攻擊減少防禦時間
                _Defs.GetDefTime(DefTimeMax, .2f); //傳遞防禦時間更改時間條
                if (isDef2)
                    _animator.Play(def2);
                else
                {
                    if (DefParticle != null)
                    {
                        DefParticle.Play();
                    }
                    else
                    {
                        Vector2 _Ptc = _Tf.position;
                        _Ptc.y--;
                        DefParticle = Instantiate(_gameManagerScript.ParticleManager.AtfVfx_4, _Ptc, Quaternion.identity, _Tf);
                        DefParticle.Play();
                    }
                }
                _animator.Play(def1);
                HitPlaySFX(10);
            }
        }
        else
        {
            HeroDuelStateFunc();
        }
    }
    


    /// <summary>
    /// 一般的受傷
    /// </summary>
    /// <param name="t">傷害值</param>
    /// <param name="hitbool">是否受傷</param>
    public virtual void HeroHit(int hp)
    {
        if (_HeroNowState == HeroState.Def)
        {
            Defense(2);
            return;
        }

        int _hit = hp - OffsetValue(_Pdo.Percentage, Def, _Pdo.PlayerHeroLv);
        if (_hit <= 0) _hit = 1;
        Hp -= _hit;
        _Hps.GetHit(HpMax, _hit);
        HeroDuelStateFunc(HeroState.Hit);
        HitPlaySFX(Random.Range(3, 5));
        if (HitParticle != null)
        {
            HitParticle.Play();
        }
        else
        {
            Vector2 _ptc = _Tf.position;
            _ptc.y -= .5f;
            HitParticle = Instantiate(_gameManagerScript.ParticleManager.AtfVfx_1, _ptc, Quaternion.identity, _Tf);
            HitParticle.Play();
        }
    }
    /// <summary>
    /// 粒子轉向
    /// </summary>
    /// <param name="_Ps">粒子物件</param>
    /// <param name="_tf">跟隨方向的物件</param>
    private void ParticleRotation(ParticleSystem _Ps, Transform _tf)
    {
        if (_Ps != null)
        {
            Transform _Ptf = CameraShakeParticle.transform;
            Quaternion _Rotation = _Ptf.localRotation;
            _Rotation.y = 0;
            if (_tf.localScale.x < 0) _Rotation.y = 180;
            _Ptf.localRotation = _Rotation;
        }
    }
    /// <summary>
    /// 必須受傷 被爆擊、或決鬥或特殊狀態
    /// </summary>
    /// <param name="hp">傷害</param>
    public virtual void MustBeInjured(int hp)
    {
        int _hit = hp - OffsetValue(_Pdo.Percentage, Def, _Pdo.PlayerHeroLv);
        if (_hit <= 0) _hit = 1;
        Hp -= _hit;
        _Hps.GetHit(HpMax, _hit);
        HeroDuelStateFunc(HeroState.Hit);
        HitPlaySFX(20);
        if (CameraShakeParticle != null)
        {
            ParticleRotation(CameraShakeParticle, _Tf);
            CameraShakeParticle.Play();
        }
        else
        {
            Vector2 _ptc = _Tf.position;
            _ptc.y -= .5f;
            CameraShakeParticle = Instantiate(_gameManagerScript.ParticleManager.CameraShakeHit_1, _ptc, Quaternion.identity, _Tf);
            ParticleRotation(CameraShakeParticle, _Tf);
            CameraShakeParticle.Play();
        }
    }
    public virtual void Idle()
    {
        _animator.Play(idle);
    }
    public virtual void Die()
    {
        _animator.Play(die);
        _rg.gravityScale = 0;
        ColliderClose(false);
        _gameManagerScript.HeroList.Remove(this);
        Destroy(gameObject, 1);
    }

    public virtual void Hit()
    {
        if (!isHit) return;
        _animator.Play(hit);


        Vector2 BeakBack = Vector2.left;
        if (_Tf.localScale.x < 0) BeakBack *= -1; // 反向
        _rg.AddForce(BeakBack, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 翻滾
    /// </summary>
    public virtual void Roll()
    {

    }


    /// <summary>
    /// 帶攻擊的移動方式
    /// </summary>
    /// <param name="Tf"></param>
    public virtual void MoveAtk(Transform Tf)
    {
        if (!isMovAtk) return;

    }
    /// <summary>
    /// 判斷是否需要晃動攝影機
    /// </summary>
    public void AnimationFrameStop()
    {
        if (isAnimationFrameStorp)
        {
            _animator.speed = 0f;
            isCameraShake = true;
        }
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
    public bool AnimationPlayOver(HeroState heroNowState)
    {
        float s = AnimatorStateInfoNormalizedTime();
        bool PlayOkay = false;
        if (heroNowState != HeroState.Die)
        {
            switch (heroNowState)
            {
                case HeroState.Def:
                    if (DefTime <= 0)
                    {
                        animatorPlay("", -1, 0);
                        PlayOkay = isNowDef;
                    }
                    break;
                case HeroState.Attack:
                case HeroState.HeavyAttack:
                case HeroState.MovAtk:
                if (s > 0.6f)
                    {
                        animatorPlay("", -1, 0);
                        PlayOkay = true;
                    }
                    break;
                case HeroState.Hit:
                case HeroState.Dash:
                    if (s > 0.9f)
                    {
                        animatorPlay("", -1, 0);
                        PlayOkay = true;
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
                        if (s > 0.9f)
                        {
                            animatorPlay("", -1, 0);
                            PlayOkay = true;

                        }
                    }
                    break;
                case HeroState.Die:
                    PlayOkay = false;
                    break;
                default:
                    PlayOkay = true;
                    break;
            }
        }
        //if (isPlayerControl)
        //{
        //    Debug.Log(s);
        //    Debug.Log(heroNowState);
        //    Debug.Log(PlayOkay);
        //}
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
    /// 重擊
    /// </summary>
    /// <param name="_Collider2D"></param>
    public void HeavyAttack()
    {
        _animator.Play(heavyAttack);
    }
    /// <summary>
    /// 攻擊動畫
    /// </summary>
    public virtual void HeroAttack()
    {
        if (AtkTime >= AtkTimeMax || AtkCount >= atk.Count)
        {
            AtkCount = 0;
            AtkTime = 0;
        }
        _animator.Play(atk[AtkCount]);
        AtkCount++;

        AtkTime = 0;
        AemsPlaySFX(2);
    }
    /// <summary>
    /// 攻擊射線取得的碰撞體 Collider2D 判斷到的物件
    /// </summary>
    /// <param name="_Collider2D"></param>
    public virtual void HeroAttack(Collider2D[] _Collider2D)
    {
        if (IsAtkLimit())
        {
            HeroDuelStateFunc();
            return;
        }
        HeroAttack();
    }

    /// <summary>
    /// 設定放在第幾幀開始攻擊
    /// </summary>
    public void AnimationHeroAtkTarget(int atktype = 0)
    {
        if (enemyCollider.Length == 0) return;

        int _atk = OffsetValue(_Pdo.Percentage, Attack, _Pdo.PlayerHeroLv);
        isCriticalHitRate = CriticalHitRate();
        if (isCriticalHitRate) _atk *= AttackMagnification;
        if(atktype == 1) _atk *= 2;
        HeroAtkTarget(_atk);
    }
    /// <summary>
    /// 攻擊所有敵人
    /// </summary>
    /// <param name="Hit">傷害</param>
    public virtual void HeroAtkTarget(int Hit = 1)
    {
        string _colTag;
        Collider2D _col;
        HeroScript _hs; // 敵方英雄
        SoldierScript _ss; // 敵方士兵
        MainFortressScript _mf; // 敵方主堡
        List<Collider2D> _ColList = new List<Collider2D>(PhyOverlapBoxAll().ToList());
        if (_ColList.Count > 0)
        {
            
            isAnimationFrameStorp = true;
            AnimationFrameStop();
            for (int i = 0; i < _ColList.Count; i++)
            {
                _col = _ColList[i];
                if (_col == null) continue;
                _enemyNowMainFortress = _col.transform;

                _colTag = _col.tag;
                if (_colTag == staticPublicObjectsStaticName.HeroTag ||
                    _colTag == staticPublicObjectsStaticName.DarkHeroTag)
                {
                    if (_col == null) continue;
                    _hs = _col.GetComponent<HeroScript>();
                    if (_hs != null)
                    {
                        if (isCriticalHitRate)
                        {
                            _hs.MustBeInjured(Hit);
                            continue;
                        }
                        _hs.HeroHit(Hit);
                    }
                }
                else if (_colTag == staticPublicObjectsStaticName.PlayerSoldierTag ||
                    _colTag == staticPublicObjectsStaticName.DARKSoldierTag ||
                    _colTag == staticPublicObjectsStaticName.WildSoldierTag)
                {
                    if (_col == null) continue;
                    _ss = _col.GetComponent<SoldierScript>();
                    if (_ss != null)
                    {
                        _ss.MustBeInjured(Hit, isCriticalHitRate);
                        _ss.HitMeTransform(_Tf);
                    }
                }
                else if (_colTag == staticPublicObjectsStaticName.MainFortressTag ||
                    _colTag == staticPublicObjectsStaticName.DarkMainFortressTag)
                {
                    if (_col == null) continue;
                    _mf = _col.GetComponent<MainFortressScript>();
                    if (_mf != null)
                        _mf.MainFortressHit(Hit);
                }
            }
        }
        isCriticalHitRate = false;
    }

    /// <summary>
    /// 攻擊目標對目標造成傷害
    /// </summary>
    /// <param name="_enemyHeroScript">敵人英雄的腳本</param>
    public virtual void HeroAtkTarget(HeroScript _enemyHeroScript, int Hit = 1)
    {
        if (_enemyHeroScript == null) return;
        _enemyHeroScript.HeroHit(Hit);
        AemsPlaySFX(2);
    }
    public virtual void HeroAtkTarget(SoldierScript _enemySoldierScript, int Hit = 1)
    {
        if (_enemySoldierScript == null) return;
        _enemySoldierScript.MustBeInjured(Hit);
    } 


    #region 衝刺、瞬移
    /// <summary>
    /// 關閉或打開碰撞器
    /// </summary>
    public void ColliderClose(bool oc)
    {
        if (_colliders.Length == 0) return;
        Collider2D _col;
        for (int i = 0; i < _colliders.Length; i++)
        {
            _col = _colliders[i];
            if (_col == null) continue;
            _col.enabled = oc;
        }
    }
    /// <summary>
    /// 閃現
    /// </summary>
    public virtual void Miss(Transform TargetTf = null, float time = 0)
    {
        if (IsItPossibleMiss) return;
        if (!isMiss0 && !isMiss1) return; // 沒有完整閃現動畫
        missTimeStart = time + MissTime; // 閃現開始時間
        if (TargetTf != null) MissTargetTf = TargetTf;
        _animator.Play(miss0); // 閃現動畫
        IsItPossibleMiss = true; // 正在閃現
        // 先飛起來
        //Vector2 Pos = Vector2.up;
        //_Tf.Translate(Pos);
        // 重力歸零、碰撞關閉
        _rg.gravityScale = 0;
        ColliderClose(false);
    }
    /// <summary>
    /// 閃現出現
    /// </summary>
    /// <param name="time"></param>
    public void Miss1(float time = 0)
    {
        if (!isMiss1) return; // 沒有完整閃現動畫


        Vector2 Pos = Vector2.right;
        if (MissOrDashDirection == Vector2.zero)
        {
            if (MissTargetTf == null)
            {
                if (_Tf.localScale.x < 0) Pos = Vector2.left;
            }
            else
            {
                if (_Tf.localScale.x < MissTargetTf.localScale.x) Pos = Vector2.left;
            }
            MissOrDashDirection = Pos;
        }
        if (missTimeStart > time)
        {
            _Tf.Translate(MissOrDashDirection * .35f); // 移動中
        }
        if (missTimeStart <= time)
        {
            HeroDuelStateFunc(HeroState.Miss1, Vector2.zero, time, null);
            _animator.Play(miss1); // 閃現(出現)動畫
            // 重力恢復、碰撞開啟
            ColliderClose(true);
            _rg.gravityScale = 1;
            //目前沒有閃現
            MissTargetTf = null;
            if (missTimeStart + 0.2f <= time)
            {
                IsItPossibleMiss = false; // 更長時之後才可以再度閃現
                MissOrDashDirection = Vector2.zero;
                HeroDuelStateFunc();
            }
        }
    }
    /*------------------------------------------------------------*/

    protected virtual void HeroDashBaseFun(float time)
    {
        DashTimeStart = time + DashTime; // 衝刺開始時間
        IsItPossibleToDash = true; // 正在衝刺
        _animator.Play(dash); // 衝刺動畫

        // 先飛起來
        //Vector2 Pos = Vector2.up;
        //_Tf.Translate(Pos);
    }
    /// <summary>
    /// 衝刺
    /// </summary>
    public virtual void HeroDash(float time)
    {

        if (IsItPossibleToDash) return;
        if (!isDash) return;
        if (time == 0) return; // 當時間為零，直接停止衝刺
        HeroDashBaseFun(time);
    }
    public void FastForward(Transform _Tf, float time)
    {
        Vector2 Pos = Vector2.right;
        if (MissOrDashDirection == Vector2.zero) // 如果沒有方向
        {
            if (_Tf.localScale.x < 0) Pos = Vector2.left;
            MissOrDashDirection = Pos; // 設定衝刺方向
        }
        Vector2 Scale = _Tf.localScale;
        Scale.x = Mathf.Abs(Scale.x);
        if (MissOrDashDirection == Vector2.left) Scale.x *= -1;
        _Tf.localScale = Scale; // 設定人物方向

        if (DashTimeStart > time) _Tf.Translate(MissOrDashDirection * .5f); // 衝刺中

        if (DashTimeStart + 0.1f <= time)
        {
            IsItPossibleToDash = false;
            MissOrDashDirection = Vector2.zero;
            HeroDuelStateFunc();
        }
    }
    #endregion

    #region 射線、其他
    /// <summary>
    /// 判斷是否產生爆擊
    /// </summary>
    /// <returns>true = 爆擊</returns>
    private bool CriticalHitRate()
    {
        return Random.Range(0, 101) <= AttackCriticalHitRate;
    }

    /// <summary>
    /// 是不是需要等待攻擊續力
    /// </summary>
    /// <returns>如果為true代表目前沒辦法攻擊</returns>
    public bool IsAtkLimit()
    {
        return AtkCount >= atk.Count && AtkTime < AtkTimeMax;
    }


    /// <summary>
    /// 重新尋找目標
    /// </summary>
    /// <param name="_mfList"></param>
    public virtual void GetEmenyTarget(List<MainFortressScript> _mfList)
    {
        if (_target != null) return;
        MainFortressScript _mf;
        for (int i = 0; i < _mfList.Count; i++)
        {
            _mf = _mfList[i];
            if (_mf == null) continue;
            switch (tag)
            {
                case staticPublicObjectsStaticName.HeroTag:
                    if (_mf.CompareTag(staticPublicObjectsStaticName.DarkMainFortressTag))
                    {
                        _enemyNowMainFortress = _mf._Tf;
                        return;
                    }
                    break;
                case staticPublicObjectsStaticName.DarkHeroTag:
                    if (_mf.CompareTag(staticPublicObjectsStaticName.MainFortressTag))
                    {
                        _enemyNowMainFortress = _mf._Tf;
                        return;
                    }
                    break;
            }
        }
    }


    /// <summary>
    /// 音效組件基礎設定
    /// </summary>
    /// <param name="AudioSourceType">音效組件模式判斷</param>
    /// <returns></returns>
    protected virtual bool PlaySFXBaseFunc(string AudioSourceType = "")
    {
        if (_ScriptList == null) _ScriptList = _gameManagerScript.SFXList;
        if (AudioSourceType == "")
        {
            _AudioSource.volume = 1f;
            _AudioSource.maxDistance = _ScriptList.MaxDistance;
            _AudioSource.minDistance = _ScriptList.MinDistance;
            _AudioSource.spatialBlend = _ScriptList.SpatialBlend;
            _AudioSource.rolloffMode = _ScriptList.RolloffMode;
            return _AudioSource == null;
        }
        else if (AudioSourceType == "hit")
        {
            _AudioSourceHit.volume = 1f;
            _AudioSourceHit.maxDistance = _ScriptList.MaxDistance;
            _AudioSourceHit.minDistance = _ScriptList.MinDistance;
            _AudioSourceHit.spatialBlend = _ScriptList.SpatialBlend;
            _AudioSourceHit.rolloffMode = _ScriptList.RolloffMode;
            return _AudioSourceHit == null;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 播放音效，提供 SFXListScript類別中，變數尾號相同的音效
    /// </summary>
    /// <param name="SFXIndex">武器音效變數後面的編號</param>
    public virtual void AemsPlaySFX(int SFXIndex)
    {
        if (PlaySFXBaseFunc()) return;
        AudioClip Ac = null;
        switch (SFXIndex)
        {
            case 1:
                Ac = _gameManagerScript.SFXList.Arms01;
                _AudioSource.volume = 0.3f;
                break;
            case 2:
                Ac = _gameManagerScript.SFXList.Arms02;
                _AudioSource.volume = 0.5f;
                break;
            case 3:
                Ac = _gameManagerScript.SFXList.Arms03;
                _AudioSource.volume = 0.5f;
                break;
            case 4:
                Ac = _gameManagerScript.SFXList.Arms04;
                _AudioSource.volume = 0.5f;
                break;
            case 5:
                Ac = _gameManagerScript.SFXList.Arms05;
                _AudioSource.volume = 0.5f;
                break;
        }

        if (Ac == null) return;

        _AudioSource.clip = Ac;
        _AudioSource.Play();
    }
    /// <summary>
    /// 受傷音效
    /// </summary>
    /// <param name="SFXIndex"></param>
    public virtual void HitPlaySFX(int SFXIndex)
    {
        if (PlaySFXBaseFunc("hit")) return;
        AudioClip Ac = null;
        switch (SFXIndex)
        {
            case 1:
                Ac = _gameManagerScript.SFXList.HeroHit01;
                break;
            case 2:
                Ac = _gameManagerScript.SFXList.HeroHit02;
                break;
            case 3:
                Ac = _gameManagerScript.SFXList.HeroHit03;
                break;
            case 4:
                Ac = _gameManagerScript.SFXList.HeroHit04;
                break;
            case 10:
                Ac = _gameManagerScript.SFXList.Def01;
                break;
            case 20:
                Ac = _gameManagerScript.SFXList.CriticalStrike;
                break;
        }

        if (Ac == null) return;

        _AudioSourceHit.clip = Ac;
        _AudioSourceHit.Play();
    }


    /// <summary>
    /// 計算本身輸出數值偏移值
    /// </summary>
    /// <param name="_ov">偏移值</param>
    /// <param name="targetValue">要輸出的數值</param>
    /// <param name="_lv">目前等級</param>
    public virtual int OffsetValue(float _ov, int targetValue, int _lv)
    {
        int hit = (int)Mathf.Ceil(targetValue * _ov - (0.005f * _lv));
        return Random.Range(targetValue - hit, targetValue + 1);
    }

    /// <summary>
    /// 計算射線偏移量
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="offset"></param>
    /// <param name="Pos"></param>
    /// <returns></returns>
    private Vector2 Physics2DOffset(Transform tf, float offset, Vector2 Pos)
    {
        Vector2 _Pos = Pos;
        float _PhyOffset = offset;
        if (tf.localScale.x < 0) _PhyOffset *= -1;
        _Pos.x += _PhyOffset;
        return _Pos;
    }
    /// <summary>
    /// 執行射線
    /// </summary>
    /// <param name="Pos">自身座標</param>
    /// <returns></returns>
    public virtual Collider2D[] PhyOverlapBoxAll(Vector2 Pos)
    {
        PhySizeVector2 = Vector2.one * phySize;
        PhySizeVector2.y *= 1.5f;
        Collider2D[] _ColArray = Physics2D.OverlapBoxAll(Physics2DOffset(_Tf, PhyOffset, Pos), PhySizeVector2, 0, enemyLayerMask);
        enemyCollider = _ColArray;
        return _ColArray;
    }
    /// <summary>
    /// 執行射線
    /// </summary>
    /// <returns></returns>
    public virtual Collider2D[] PhyOverlapBoxAll()
    {
        PhySizeVector2 = Vector2.one * phySize;
        PhySizeVector2.y *= 1.5f;
        return Physics2D.OverlapBoxAll(Physics2DOffset(_Tf, PhyOffset, _Tf.position), PhySizeVector2, 0, enemyLayerMask);
    }


    void OnDrawGizmos()
    {
        if (_Tf == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Physics2DOffset(_Tf, PhyOffset, _Tf.position), PhySizeVector2);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 碰到地板
        if (collision.gameObject.CompareTag(staticPublicObjectsStaticName.FlootTag)) isfloot = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 離開地板
        if (collision.gameObject.CompareTag(staticPublicObjectsStaticName.FlootTag)) isfloot = false;
    }
    #endregion
}

