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
    [Header("士兵基本素質 (0.01 - 0.3)"), Range(0.01f, 0.3f)]
    public float BasicQuality;
    /// <summary>
    /// 基本體質
    /// </summary>
    [Header("基本體質 (1- 60)"), Range(1, 60)]
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
    public GameManager _gameManagerScript;
    /// <summary>
    /// 士兵方向
    /// </summary>
    public Vector3 _SoldierScale;
    #endregion
    #region 射線
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
    [Header("敵人檢測射線")]
    public Collider2D[] _collider2D;
    /// <summary>
    /// 射線範圍
    /// </summary>
    [Header("射線範圍")]
    public float Physics2DSize;
    /// <summary>
    /// 取得攻擊目標的圖層
    /// </summary>
    public LayerMask _enemyLayerMask;
    /// <summary>
    /// 目前敵人的主堡
    /// </summary>
    [Header("目前敵人的主堡")]
    public Transform _enemyNowMainFortress;
    #endregion
    #region 狀態
    /// <summary>
    /// 死亡之後是否關閉剛體
    /// </summary>
    [Header("死亡之後是否關閉剛體"), SerializeField]
    private bool IsCloseRigidbody;
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
    [Header("被攻擊的特效")]
    public ParticleSystem AtkVfx_2;
    #endregion

    #region 物件
    public HpScript _Hps;
    #endregion

    public virtual void SoldierDataInitializ()
    {
        //_Tf = transform; // 取得物件transform
        //_Go = gameObject; // 取得物件gameobject
        _body2D = GetComponent<Rigidbody2D>(); // 取得物件rigidbody2D
        _animator = GetComponent<Animator>(); // 取得動畫控制器

        _soldierChangeState = SoldierState.Move;
        GetAllAnimationClipName();

        _AudioSource = _Go.AddComponent<AudioSource>();
        _AudioSourceHit = _Go.AddComponent<AudioSource>();

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
            if (nameIndexOf.IndexOf("_die") != -1) dieAnimationName = _name;
            if (nameIndexOf.IndexOf("_run") != -1) runAnimationName.Add(_name);
            if (nameIndexOf.IndexOf("_dash") != -1) dashAnimationName.Add(_name);
        }
    }
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
                if (IsCloseRigidbody) _body2D.bodyType = RigidbodyType2D.Static; // 將剛體設定為靜態
                soldierAnimatorPlay(dieAnimationName);
                _gameManagerScript._soldierList.Remove(this); // 從清單中移除
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
                        List<Collider2D> _ColList = new List<Collider2D>();
                        _ColList.AddRange(_collider2D.ToList());
                        for (int i = 0; i < _ColList.Count; i++) // 尋找敵人
                        {
                            _col = _ColList[i];
                            if (_col == null) continue;
                            _tag = _col.tag;
                            if (_tag.IndexOf("Hero") != -1)
                            {
                                _col.GetComponent<HeroScript>().HeroHit(soldierAtk);
                                _enemyNowMainFortress = _col.transform;
                            }
                            else if (_tag.IndexOf("Soldier") != -1)
                            {
                                _col.GetComponent<SoldierScript>().SoldierHP(soldierAtk);
                                _enemyNowMainFortress = _col.transform;
                            }
                            else if (_tag.IndexOf("MainFortress") != -1)
                            {
                                MainFortressScript _mainFortressScript = _col.GetComponent<MainFortressScript>();
                                _mainFortressScript.MainFortressHit(soldierAtk);
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
    public virtual void MustBeInjured(int hp)
    {
        if (soldierHp <= 0) return;
        if (_soldierChangeState == SoldierState.SoupHit) return;
        Vector2 DieOffset;
        int _hit = hp - soldierDefense;
        if(_hit <= 0) _hit = 1;
        soldierHp -= _hit;
        _Hps.GetHit(soldierHpMax, _hit);
        _soldierChangeState = SoldierState.SoupHit;
        if (soldierHp <= 0)
        {
            _soldierChangeState = SoldierState.Die;
            DieOffset = Vector2.left;
            if (_Tf.localScale.x > 0)
                DieOffset = Vector3.right;

            _body2D.velocity = DieOffset * 5;
        }
        else
        {
            Vector2 BeakBack = Vector2.left * 4;
            if (_Tf.localScale.x < 0) BeakBack *= -1; // 反向
            _body2D.velocity = Vector2.zero;
            _body2D.AddForce(BeakBack, ForceMode2D.Impulse);

            if (AtkVfx_2 == null)
                AtkVfx_2 = Instantiate(_gameManagerScript.ParticleManager.AtfVfx_3, _Tf.position, Quaternion.identity, _Tf);

            AtkVfx_2.Play();
            HitPlaySFX(1);

        }
        SoldierStateAI();
    }
    public virtual void SoldierHP(int hitAmount)
    {
        if (soldierHp <= 0) return;
        Hit();
        int _hit = hitAmount - soldierDefense;
        if (_hit <= 0) _hit = 1;
        soldierHp -= _hit;
        _Hps.GetHit(soldierHpMax, _hit);
        if (soldierHp <= 0) Die();
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
        }
        if (Ac != null)
        {
            _AudioSourceHit.clip = Ac;
            _AudioSourceHit.Play();
        }
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
        PhySize.y *= 2;
        _collider2D = Physics2D.OverlapBoxAll(Physics2DOffset(_Tf, PhyOffset, Pos), PhySize, 0, _enemyLayerMask);
    }
    void OnDrawGizmos()
    {
        PhySize = Vector2.one * Physics2DSize;
        PhySize.y *= 2;

        Vector2 Pos = _Tf.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Physics2DOffset(_Tf, PhyOffset, Pos), PhySize);
    }
    #endregion
}
public enum SoldierState
{
    /// <summary>
    /// 等待
    /// </summary>
    Idle,
    /// <summary>
    /// 移動
    /// </summary>
    Move,
    /// <summary>
    /// 攻擊
    /// </summary>
    Atk,
    /// <summary>
    /// 受傷
    /// </summary>
    Hit,
    /// <summary>
    /// 超級受傷
    /// </summary>
    SoupHit,
    /// <summary>
    /// 死亡
    /// </summary>
    Die
}
