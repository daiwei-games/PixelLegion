using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoldierScript : LeadToSurviveGameBaseClass
{
    #region 基本資料

    /// <summary>
    /// 剛體
    /// </summary>
    public Rigidbody2D _body2D;
    /// <summary>
    /// 士兵基本素質
    /// </summary>
    [Header("士兵基本素質 (0.01 - 70)"), Range(0.01f, 20f)]
    public float BasicQuality;
    /// <summary>
    /// 基本體質
    /// </summary>
    [Header("基本體質 (1- 120)"), Range(1, 120)]
    public int BasicConstitution;
    /// <summary>
    /// 基本血量
    /// </summary>
    [Header("基本體質 (100 - 3000)"), Range(100, 3000)]
    public int BasicHp;
    /// <summary>
    /// 士兵的名稱
    /// </summary>
    [Header("士兵的名稱")]
    public string soldierName;
    /// <summary>
    /// 士兵的攻擊力
    /// </summary>
    [Header("士兵的攻擊力")]
    public int soldierAtk;
    /// <summary>
    /// 士兵防禦力
    /// </summary>
    [Header("士兵防禦力")]
    public int soldierDefense;
    /// <summary>
    /// 士兵血量
    /// </summary>
    [Header("士兵血量")]
    public int soldierHp;
    [HideInInspector]
    public int soldierHpMax;
    /// <summary>
    /// 移動速度
    /// </summary>
    [Header("移動速度")]
    public float speed;
    /// <summary>
    /// 取得 GameManager 腳本
    /// </summary>
    [HideInInspector]
    public GameManager _gameManagerScript;
    /// <summary>
    /// 士兵方向
    /// </summary>
    [HideInInspector]
    public Vector3 _SoldierScale;
    /// <summary>
    /// 輸出偏移值
    /// </summary>
    [HideInInspector]
    public float Percentage;
    #endregion

    #region 射線
    /// <summary>
    /// 射線範圍
    /// </summary>
    [Header("射線範圍")]
    public float Physics2DSize;
    /// <summary>
    /// 矩形射線偏移
    /// </summary>
    [Header("矩形射線偏移")]
    public float PhyOffset;
    /// <summary>
    /// 矩形射線大小
    /// </summary>
    [Header("矩形射線大小")]
    protected Vector2 PhySize;
    /// <summary>
    /// 敵人檢測射線
    /// </summary>
    [Header("敵人檢測射線"), HideInInspector]
    public Collider2D[] _collider2D;

    /// <summary>
    /// 取得攻擊目標的圖層
    /// </summary>
    [HideInInspector]
    public LayerMask _enemyLayerMask;
    /// <summary>
    /// 目前敵人的主堡或主要攻擊目標
    /// </summary>
    [Header("主要攻擊目標"), HideInInspector]
    public Transform _enemyNowMainFortress;
    /// <summary>
    /// 緊急目標，當自家主堡被揍了，將會被賦予攻擊目標，並趕回來守護
    /// </summary>
    [Header("緊急目標"), HideInInspector]
    public Transform _target;
    #endregion

    #region 狀態
    /// <summary>
    /// 死亡之後是否關閉剛體
    /// </summary>
    [Header("死亡之後是否關閉剛體"), SerializeField]
    private bool IsCloseRigidbody;
    /// <summary>
    /// 現在是否正在受傷 false = 沒有受傷
    /// </summary>
    [Header("現在是否正在受傷 false = 沒有受傷"), HideInInspector]
    public bool isNowHit;
    #endregion

    #region 狀態時間計時
    /// <summary>
    /// 受傷僵直時間計算
    /// </summary>
    [HideInInspector]
    public float isNowHitTime;
    /// <summary>
    /// 受傷僵直時間
    /// </summary>
    [HideInInspector]
    public float isNowHitTimeMax;
    /// <summary>
    /// 攻擊間隔計算
    /// </summary>
    public float AttackingTime;
    /// <summary>
    /// 攻擊間隔
    /// </summary>
    public float AttackingTimeMax;
    #endregion

    #region 動畫控制
    /// <summary>
    /// 動畫控制器
    /// </summary>
    [Header("動畫控制器")]
    public Animator _animator;
    /// <summary>
    /// 動畫資訊
    /// </summary>
    [Header("動畫資訊")]
    public AnimatorStateInfo _animationInfo;
    /// <summary>
    /// 所有動畫
    /// </summary>
    [Header("所有動畫")]
    public List<AnimationClip> _animationClip;
    /// <summary>
    /// 動畫間隔
    /// </summary>
    [Header("動畫間隔")]
    public float animationTime;
    /// <summary>
    /// 動畫間隔限制
    /// </summary>
    [Header("動畫間隔限制")]
    public float animationTimerMax;
    public float _Time;
    #region 單一動畫
    /// <summary>
    /// 等待動畫
    /// </summary>
    [Header("等待動畫")]
    public string idleAnimationName;
    /// <summary>
    /// 受傷動畫
    /// </summary>
    [Header("受傷動畫")]
    public string hitAnimationName;
    /// <summary>
    /// 死亡動畫
    /// </summary>
    [Header("死亡動畫")]
    public string dieAnimationName;
    #endregion
    #region 多種動畫清單
    /// <summary>
    /// 攻擊動畫
    /// </summary>
    [Header("攻擊動畫")]
    public List<string> atkAnimationName;
    /// <summary>
    /// 遠程攻擊動畫
    /// </summary>
    [Header("遠程攻擊動畫")]
    public List<string> remoteAtkAnimationName;
    /// <summary>
    /// 跑步動畫
    /// </summary>
    [Header("跑步動畫")]
    public List<string> runAnimationName;

    /// <summary>
    /// 衝刺動畫
    /// </summary>
    [Header("衝刺動畫")]
    public List<string> dashAnimationName;
    #endregion
    /// <summary>
    /// 目前狀態
    /// </summary>
    [Header("目前狀態")]
    public SoldierState _soldierNowState;
    /// <summary>
    /// 想切換的狀態
    /// </summary>
    [Header("想切換的狀態")]
    public SoldierState _soldierChangeState;
    #endregion

    #region 特效、音效
    /// <summary>
    /// 音效組件管理器
    /// </summary>
    private SFXListScript _ScriptList;
    /// <summary>
    /// 音效播放組件
    /// </summary>
    private AudioSource _AudioSource;
    /// <summary>
    /// 受傷音效組件
    /// </summary>
    private AudioSource _AudioSourceHit;
    #endregion

    #region 特效
    /// <summary>
    /// 被攻擊特效
    /// </summary>
    [Header("被攻擊的特效"), HideInInspector]
    public ParticleSystem AtkVfx_1;

    /// <summary>
    /// 被爆擊粒子
    /// </summary>
    [HideInInspector]
    public ParticleSystem CameraShakeParticle;
    #endregion

    #region 物件
    /// <summary>
    /// 血條物件
    /// </summary>
    [Header("血條物件"), HideInInspector]
    public HpScript _Hps;
    /// <summary>
    /// 裝備所有投擲、遠程武器的腳本
    /// </summary>
    [Header("所有投擲、遠程武器的腳本")]
    public AmmunitionScript _ammunitionScript;
    /// <summary>
    /// 投擲物件的座標
    /// </summary>
    [HideInInspector]
    public Transform _ammunitionTf;
    /// <summary>
    /// 控制點
    /// </summary>
    [HideInInspector]
    public Transform controlPoint;
    #endregion

    #region 型態資料
    /// <summary>
    /// 職責 目前是遠攻、近戰守衛或者戰場上的士兵
    /// </summary>
    [Header("職責")]
    public SoldierPost _Sp;
    /// <summary>
    /// 是哪種攻擊型態, 建立任務預先設定
    /// </summary>
    [Header("攻擊型態")]
    public AttackType _At;
    #endregion
    /// <summary>
    /// 資料初始化
    /// </summary>
    public virtual void SoldierDataInitializ()
    {
        _body2D = GetComponent<Rigidbody2D>(); // 取得物件rigidbody2D
        _animator = GetComponent<Animator>(); // 取得動畫控制器

        _soldierChangeState = SoldierState.Move;
        GetAllAnimationClipName();

        _AudioSource = _Go.AddComponent<AudioSource>();
        _AudioSourceHit = _Go.AddComponent<AudioSource>();

        isNowHitTimeMax = .5f;
        AttackingTimeMax = .6f;


        soldierAtk *= _Sp switch
        {
            SoldierPost.MeleeAttackGuard or SoldierPost.RemoteAttackGuard => 2,
            _ => 1
        };

        switch (_At)
        {
            case AttackType.RemoteAttack:
                break;
            case AttackType.MeleeAttack:
                break;
            case AttackType.RemoteAndMelee:
                break;
        }

        
    }
    public virtual void GetAllAnimationClipName()
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
            if (nameIndexOf.IndexOf("_remoteatk") != -1) remoteAtkAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_die") != -1) dieAnimationName = _name;
            if (nameIndexOf.IndexOf("_run") != -1) runAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_dash") != -1) dashAnimationName.Add(_name);
        }
    }
    /// <summary>
    /// 士兵狀態機
    /// </summary>
    public virtual void SoldierStateAI()
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
                if ((_soldierNowState == SoldierState.Atk && _normalizedTime < 0.8f) ||
                    (_soldierNowState == SoldierState.RemoteAtk && _normalizedTime < 0.8f) ||
                    (_soldierNowState == SoldierState.Hit && _normalizedTime < 0.9f)) return; // 當狀態為攻擊將會return
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
                if (IsCloseRigidbody) _body2D.bodyType = RigidbodyType2D.Static; // 將剛體設定為靜態
                soldierAnimatorPlay(dieAnimationName);
                _gameManagerScript.SolidListRemove(this);
                break;
            default:
                switch (_soldierChangeState)
                {
                    case SoldierState.Atk:
                        if (atkAnimationName.Count == 0) return;
                        soldierAnimatorPlay(atkAnimationName[Random.Range(0, atkAnimationName.Count)]);
                        break;
                    case SoldierState.RemoteAtk:
                        if (remoteAtkAnimationName.Count == 0) return;
                        soldierAnimatorPlay(remoteAtkAnimationName[Random.Range(0, remoteAtkAnimationName.Count)]);
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
    /// <summary>
    /// 播放動畫的函數
    /// </summary>
    /// <param name="AnimaName">動畫名稱</param>
    /// <param name="Layer">動畫控制器圖層</param>
    /// <param name="NormalTime">0~1的浮點數</param>
    protected virtual void soldierAnimatorPlay(string AnimaName, int Layer = 0, float NormalTime = 0)
    {
        _animator.Play(AnimaName, Layer, NormalTime);
    }
    public virtual void Atk()
    {
        _soldierChangeState = SoldierState.Atk;
        SoldierStateAI();
    }
    /// <summary>
    /// 遠程攻擊
    /// </summary>
    public void RemoteAtk()
    {
        if (_ammunitionScript == null) return; // 當沒有彈藥庫時將會return
        _soldierChangeState = SoldierState.RemoteAtk;
        SoldierStateAI();
    }
    /// <summary>
    /// 遠程攻擊或近戰攻擊
    /// </summary>
    public void RemoteOrMeleeAttacking()
    {
        if (_collider2D.Length == 0) return;
        Collider2D _col;
        for (int i = 0; i < _collider2D.Length; i++)
        {
            _col = _collider2D[i];
            if (_col == null) continue;
            if (Vector2.Distance(_Tf.position, _col.transform.position) < 3)
                Atk();
            else            
                RemoteAtk();
        }
    }
    /// <summary>
    /// 遠程物件發射
    /// </summary>
    public void RemoteAttacking(string RemoteObjectName)
    {
        if (_ammunitionScript == null ||
            _collider2D.Length == 0) return; // 當沒有彈藥庫或沒有敵人時將會return
        if(string.IsNullOrWhiteSpace(RemoteObjectName)) return;
        Transform _tf = Instantiate(_ammunitionScript.PrefabNameGetPrefab(RemoteObjectName), _ammunitionTf.position,Quaternion.identity,_Tf);
        ParabolaScript _ps = _tf.GetComponent<ParabolaScript>();
         //計算加乘後的攻擊力
        int _atk = OffsetValue(Percentage, soldierAtk);
        int _remoteatk = _ps.Atk;
        _ps.Atk = _atk + Mathf.CeilToInt((_atk + _remoteatk) * Mathf.InverseLerp(0, _atk, _remoteatk));

        //判斷是光明還是黑暗發出的投擲武器
        _ps._promisingOrDARK = gameObject.tag switch
        {
            staticPublicObjectsStaticName.DarkHeroTag or
            staticPublicObjectsStaticName.DARKSoldierTag or
            staticPublicObjectsStaticName.DarkMainFortressTag or
            staticPublicObjectsStaticName.WildSoldierTag => PromisingOrDARK.Dark,
            _ => PromisingOrDARK.Promising,
        };

        //控制點
        if (controlPoint == null)
        {
            GameObject _cPoint = new GameObject("controlPoint");
            _cPoint.transform.parent = _Tf;
            controlPoint = _cPoint.transform;
        }
        Vector3 _pos = (_Tf.position + _collider2D[0].transform.position) / 2;
        _pos.y = _Tf.position.y + 1;
        controlPoint.position = _pos;
        _ps.controlPoint = controlPoint.position;

        //起始點
        _ps.startPoint = _Tf;

        //終點
        _ps.endPoint = _collider2D[0].transform.position;
        _ps.endPoint.y -= 2;
        _ps._gameManager = _gameManagerScript;
        _gameManagerScript.ParabolaListAdd(_ps);
    }
    /// <summary>
    /// 近戰攻擊
    /// </summary>
    public void MeleeAttacking()
    {
        if (_collider2D.Length == 0) return;
        string _tag;
        Collider2D _col;
        List<Collider2D> _ColList = new List<Collider2D>();
        _ColList.AddRange(_collider2D.ToList());
        AttackingTime = AttackingTimeMax;

        int _atk = OffsetValue(Percentage, soldierAtk);
        for (int i = 0; i < _ColList.Count; i++) // 尋找敵人
        {

            _col = _ColList[i];
            if (_col == null) continue;
            _enemyNowMainFortress = _col.transform;
            _tag = _col.tag;
            if (_tag.IndexOf("Hero") != -1)
            {
                HeroScript _Hs = _col.GetComponent<HeroScript>();
                _Hs.HeroHit(soldierAtk);
            }
            else if (_tag.IndexOf("Soldier") != -1)
            {
                SoldierScript _Ss = _col.GetComponent<SoldierScript>();
                _Ss.SoldierHP(soldierAtk);
                _Ss.HitMeTransform(_Tf);
            }
            else if (_tag.IndexOf("MainFortress") != -1)
            {
                MainFortressScript _mainFortressScript = _col.GetComponent<MainFortressScript>();
                _mainFortressScript.MainFortressHit(soldierAtk);
            }
        }
    }
    public virtual void Die()
    {
        _soldierChangeState = SoldierState.Die;
        SoldierStateAI();
    }
    public virtual void Hit()
    {
        _soldierChangeState = SoldierState.Hit;
        SoldierStateAI();
    }
    /// <summary>
    /// 必須受傷
    /// </summary>
    /// <param name="hp">傷害</param>
    /// <param name="isCriticalStrike">是否被爆擊 true = 被爆擊</param>
    public virtual void MustBeInjured(int hp, bool isCriticalStrike = false)
    {
        if (soldierHp <= 0) return;
        //if (_soldierChangeState == SoldierState.SoupHit) return;
        //Vector2 DieOffset;
        int _hit = hp - soldierDefense;
        if (_hit <= 0) _hit = 1;
        soldierHp -= _hit;
        _Hps.GetHit(soldierHpMax, _hit);
        _soldierChangeState = SoldierState.SoupHit;
        SoldierStateAI();
        if (soldierHp <= 0)
        {
            _soldierChangeState = SoldierState.Die;
        }
        else
        {

            isNowHit = true;
            BeakBack();

            if (isCriticalStrike)
            {
                if (CameraShakeParticle != null)
                {
                    Transform _Ptf = CameraShakeParticle.transform;
                    Quaternion _Rotation = _Ptf.localRotation;
                    _Rotation.y = 0;
                    if (_Tf.localScale.x < 0) _Rotation.y = 180;
                    _Ptf.localRotation = _Rotation;

                    CameraShakeParticle.Play();
                }
                else
                {
                    Vector2 _ptc = _Tf.position;
                    _ptc.y -= .5f;
                    CameraShakeParticle = Instantiate(_gameManagerScript.ParticleManager.CameraShakeHit_1, _ptc, Quaternion.identity, _Tf);
                    Transform _Ptf = CameraShakeParticle.transform;
                    Quaternion _Rotation = _Ptf.localRotation;
                    _Rotation.y = 0;
                    if (_Tf.localScale.x < 0) _Rotation.y = 180;
                    _Ptf.localRotation = _Rotation;
                    CameraShakeParticle.Play();
                }
            }
            else
            {
                if (AtkVfx_1 == null)
                {
                    Vector2 _ptc = _Tf.position;
                    _ptc.y -= .5f;
                    AtkVfx_1 = Instantiate(_gameManagerScript.ParticleManager.AtfVfx_1, _ptc, Quaternion.identity, _Tf);
                }

                AtkVfx_1.Play();
            }
            if (isCriticalStrike)
            {
                HitPlaySFX(20);
                return;
            }
            HitPlaySFX(1);

        }
        SoldierStateAI();
    }
    /// <summary>
    /// 一般受傷
    /// </summary>
    /// <param name="hitAmount">傷害值</param>
    public virtual void SoldierHP(int hitAmount)
    {
        if (soldierHp <= 0) return;
        Hit();
        int _hit = hitAmount - soldierDefense;
        if (_hit <= 0) _hit = 1;
        soldierHp -= _hit;
        HitPlaySFX(1);
        _Hps.GetHit(soldierHpMax, _hit);
        if (soldierHp <= 0) Die();
    }
    /// <summary>
    /// 傷害我的角色
    /// </summary>
    public void HitMeTransform(Transform _who)
    {
        //被攻擊了，如果目前沒有目標，也沒有緊急目標，就將目標轉移為攻擊者
        if (_enemyNowMainFortress == null)
        {
            _enemyNowMainFortress = _who;
        }

    }
    public virtual void Idle()
    {
        if (_soldierChangeState == _soldierNowState && _soldierNowState == SoldierState.Idle) return;
        _soldierChangeState = SoldierState.Idle;
        SoldierStateAI();
    }
    public virtual void Move()
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


    #region 射線、音效、其他
    /// <summary>
    /// 擊退
    /// </summary>
    void BeakBack()
    {
        Vector2 BeakBack = Vector2.left * 3;
        if (_Tf.localScale.x < 0) BeakBack *= -1; // 反向
        _body2D.velocity = BeakBack;
        //_body2D.AddForce(BeakBack, ForceMode2D.Impulse);
    }
    /// <summary>
    /// 取得敵人主堡並放入目標
    /// </summary>
    public virtual void GetEmenyMainFortress(List<MainFortressScript> _MfList)
    {
        if (_enemyNowMainFortress != null) return; // 如果目標不是空的就不執行

        MainFortressScript _mf;
        for (int i = 0; i < _MfList.Count; i++)
        {
            _mf = _MfList[i];
            if (_mf == null) continue;
            switch (tag)
            {
                case staticPublicObjectsStaticName.DARKSoldierTag: // 如果是黑暗兵
                    if (_mf.CompareTag(staticPublicObjectsStaticName.MainFortressTag)) // 如果是光明主堡
                    {
                        _enemyNowMainFortress = _mf._Tf; // 將光明主堡放入目標
                        return;
                    }
                    break;
                case staticPublicObjectsStaticName.PlayerSoldierTag: // 如果是光明兵
                    if (_mf.CompareTag(staticPublicObjectsStaticName.DarkMainFortressTag)) // 如果是黑暗主堡
                    {
                        _enemyNowMainFortress = _mf._Tf; // 將黑暗主堡放入目標
                        return;
                    }
                    break;
            }
        }
    }


    /// <summary>
    /// 音效撥放器資料初始化並判斷是否有此播放器
    /// hit = 受傷音效
    /// </summary>
    /// <param name="AudioSourceType">
    /// 哪一個播放器的類別
    /// </param>
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
    /// 受傷音效
    /// </summary>
    /// <param name="SFXIndex"></param>
    public void HitPlaySFX(int SFXIndex)
    {
        if (PlaySFXBaseFunc("hit")) return;
        AudioClip Ac = null;
        switch (SFXIndex)
        {
            case 1:
                Ac = _ScriptList.SoldierHit01;
                _AudioSourceHit.volume = 0.3f;
                break;
            case 20:
                Ac = _ScriptList.CriticalStrike;
                break;
        }
        if (Ac != null)
        {
            _AudioSourceHit.clip = Ac;
            _AudioSourceHit.Play();
        }
    }
    /// <summary>
    /// 計算本身輸出數值偏移值
    /// </summary>
    /// <param name="_ov">偏移值</param>
    /// <param name="targetValue">要輸出的數值</param>
    public virtual int OffsetValue(float _ov, int targetValue)
    {
        int hit = Mathf.CeilToInt(targetValue * _ov);
        return Random.Range(targetValue - hit, targetValue + 1);
    }
    /// <summary>
    /// 計算射線偏移量
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="offset"></param>
    /// <param name="Pos"></param>
    /// <returns></returns>
    protected virtual Vector2 Physics2DOffset(Transform tf, float offset, Vector2 Pos)
    {
        Vector2 _Pos = Pos;
        float _PhyOffset = offset;
        if (tf.localScale.x < 0) _PhyOffset *= -1;
        _Pos.x += _PhyOffset;
        return _Pos;
    }
    public virtual void PhyOverlapBoxAll(Vector2 Pos)
    {
        PhySize = Vector2.one * Physics2DSize;
        if (_At == AttackType.MeleeAttack)
            PhySize.y /= 2;
        else
            PhySize.y *= 2;

        _collider2D = Physics2D.OverlapBoxAll(Physics2DOffset(_Tf, PhyOffset, Pos), PhySize, 0, _enemyLayerMask);
    }
    void OnDrawGizmos()
    {
        if (_Tf == null) return;
        PhySize = Vector2.one * Physics2DSize;
        if (_At == AttackType.MeleeAttack)
            PhySize.y /= 2;
        else
            PhySize.y *= 2;

        Vector2 Pos = _Tf.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Physics2DOffset(_Tf, PhyOffset, Pos), PhySize);
    }
    #endregion

    private void OnParticleCollision(GameObject other)
    {
        ParabolaScript _ps = other.GetComponent<ParabolaScript>();
        if(_ps != null)
        {
            Debug.Log(_ps.Atk);
        }
    }
}

