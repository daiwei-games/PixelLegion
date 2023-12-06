using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 場景判斷
    /// <summary>
    /// 目前是哪種場景
    /// </summary>
    public ScenesType NowScenes;
    #endregion

    #region 管理器、集中資料
    /// <summary>
    /// 玩家資料管理器
    /// </summary>
    [Header("玩家資料管理器")]
    public PlayerScript _Ps;
    /// <summary>
    /// 玩家資料物件
    /// </summary>
    public playerDataObject _Pdo;
    /// <summary>
    /// 地圖英雄、士兵、怪物管理器
    /// </summary>
    public GameLevelManager _Glm;
    #endregion

    #region 清單
    /// <summary>
    /// 主堡清單
    /// </summary>
    [Header("主堡清單")]
    public List<MainFortressScript> _MainFortressScriptList;
    /// <summary>
    /// 所有士兵清單
    /// </summary>
    [Header("所有士兵清單")]
    public List<SoldierScript> _soldierList;
    /// <summary>
    /// 已經產生的英雄清單
    /// </summary>
    [Header("已經產生的英雄清單")]
    public List<HeroScript> HeroList;

    /// <summary>
    /// 怪物產生節點
    /// </summary>
    [Header("怪物產生節點")]
    public List<MonsterNodeScript> _monsterNodesList;
    /// <summary>
    /// 投擲武器、道具物件清單
    /// </summary>
    [Header("投擲武器、道具物件清單"),HideInInspector]
    public List<ParabolaScript> _PsList;
    #endregion

    #region 計算時間
    /// <summary>
    /// 毫秒
    /// </summary>
    float time;
    #endregion

    #region UI
    /// <summary>
    /// UI腳本
    /// </summary>
    [Header("UI腳本")]
    public UIScript uiScript;
    /// <summary>
    /// 玩家英雄操作的操控介面
    /// </summary>
    public UIHeroController _UIHc;
    /// <summary>
    /// 英雄選擇介面
    /// </summary>
    public UIHeroOptions HeroUI;
    #endregion

    #region 程式控制
    /// <summary>
    /// 士兵數量限制
    /// </summary>
    [Header("士兵數量限制")]
    public int _GmSoldierCountMax;
    /// <summary>
    /// 是否再次執行士兵動作迴圈
    /// </summary>
    [HideInInspector]
    bool isSoldierStateForAction;
    /// <summary>
    /// 是否再次執行英雄動作迴圈的判斷
    /// </summary>
    [HideInInspector]
    public bool isHeroStateForAction;
    /// <summary>
    /// 是否執行生產士兵的函數，還是正在執行
    /// </summary>
    [HideInInspector]
    public bool isProduce;
    /// <summary>
    /// 是否遊戲結束
    /// </summary>
    [HideInInspector]
    public bool isGameOver;
    /// <summary>
    /// 怪物節可不可以運行
    /// </summary>
    [HideInInspector]
    public bool isMonsterNodes;
    /// <summary>
    /// 是否再次執行投擲回圈
    /// </summary>
    [HideInInspector]
    public bool isParabolaGoto;
    #endregion

    #region 操控類、指標類
    /// <summary>
    /// 選擇的英雄頭上出現指標
    /// </summary>
    [Header("選擇的英雄頭上出現指標")]
    public Transform SelectedHeroTarget;
    /// <summary>
    /// 搖桿腳本
    /// </summary>
    [Header("搖桿")]
    public VariableJoystick _Joystick;
    /// <summary>
    /// 可以操控的英雄
    /// </summary>
    [Header("可以操控的英雄")]
    public HeroScript SelectedHero;
    #endregion

    #region 圖層
    /// <summary>
    /// 取得光明圖層
    /// </summary>
    public LayerMask _LayerMask;
    /// <summary>
    /// 取得黑暗圖層
    /// </summary>
    public LayerMask _DarkLayerMask;
    #endregion

    #region 攝影機
    /// <summary>
    /// 監視器中心點
    /// </summary>
    public CameraCenterScript CameraCenterScript;
    #endregion

    #region 預置物件
    [Header("英雄頭上指標預置物")]
    public Transform SelectedHeroTargetPrefab;
    /// <summary>
    /// 血條物件
    /// </summary>
    [Header("血條物件")]
    public HpScript _HpPrefabs;
    /// <summary>
    /// 防禦條預置物件
    /// </summary>
    [Header("防禦條預置物件")]
    public DefScript _DefPrefabs;
    #endregion

    #region VFX、SFX 管理器
    /// <summary>
    /// VFX 管理器
    /// </summary>
    [Header("VFX 管理器")]
    public ParticleListManagerScript ParticleManager;
    /// <summary>
    /// SFX 管理器
    /// </summary>
    [Header("SFX 管理器")]
    public SFXListScript SFXList;
    #endregion

    /// <summary>
    /// 遊戲管理資料格式化
    /// </summary>
    private void GameManagerDataInitializ()
    {

        _Ps = FindFirstObjectByType<PlayerScript>();
        _Glm = FindFirstObjectByType<GameLevelManager>();
        _Pdo = _Ps.PlayerDataObject;

        ParticleManager = FindFirstObjectByType<ParticleListManagerScript>();
        SFXList = FindFirstObjectByType<SFXListScript>();
        _Joystick = FindFirstObjectByType<VariableJoystick>();
        HeroUI = FindFirstObjectByType<UIHeroOptions>();

        isSoldierStateForAction = true;
        isHeroStateForAction = true;
        isProduce = true;
        isParabolaGoto = true;

        _LayerMask = LayerMask.GetMask(staticPublicObjectsStaticName.PlayerSoldierLayer, staticPublicObjectsStaticName.HeroLayer, staticPublicObjectsStaticName.MainFortressLayer);
        _DarkLayerMask = LayerMask.GetMask(staticPublicObjectsStaticName.DarkSoldierLayer, staticPublicObjectsStaticName.DarkHeroLayer, staticPublicObjectsStaticName.DarkMainFortressLayer, staticPublicObjectsStaticName.WildSoldierLayer);
    }
    private void Awake()
    {
        GameManagerDataInitializ();
    }
    private void Update()
    {
        if (staticPublicGameStopSwitch.gameStop)
        {
            Time.timeScale = 0;
        }
        if (!isGameOver)
        {
            if (SelectedHero == null)
            {
                SelectedHeroFunc();
            }
            SelectedHeroControl();
        }
        switch (NowScenes)
        {
            case ScenesType.village:

                break;
            case ScenesType.prairie:

                break;
            case ScenesType.mine:

                break;
            case ScenesType.dark:

                break;
            case ScenesType.practise:
            case ScenesType.battlefield:

                break;
        }
    }
    private void FixedUpdate()
    {
        if (staticPublicGameStopSwitch.gameStop) return;
        time = Time.deltaTime;
        switch (NowScenes)
        {
            case ScenesType.village:
            case ScenesType.mine:
            case ScenesType.dark:
                ProduceWildSoldier(time);
                SoldierState(time, NowScenes);
                
                // 投擲武器、道具
                ParabolaGoto();
                break;
            case ScenesType.prairie:
                ProduceWildSoldier(time);
                break;
            case ScenesType.practise:
            case ScenesType.battlefield:
                if (!isGameOver)
                {
                    //生產區塊
                    ProduceOrStateFunc(time, Time.time);

                    //士兵動作
                    SoldierState(time, NowScenes);
                    HeroAI(time);

                    // 投擲武器、道具
                    ParabolaGoto();
                }
                break;
        }
        CameraToPlayerPosition();
    }

    #region 攝影機操作
    /// <summary>
    /// 取得玩家選擇的英雄 Transform
    /// </summary>
    public void CameraGetPlayerTransform(Transform HeroTf)
    {
        if (SelectedHero == null) return;
        if (CameraCenterScript == null) return;
        CameraCenterScript.PlayerTf = HeroTf;
    }
    /// <summary>
    /// 捨影機跟隨玩家選擇的英雄
    /// </summary>
    public void CameraToPlayerPosition()
    {
        if (SelectedHero == null) return;
        if (CameraCenterScript == null) return;
        CameraCenterScript.GotoPlyer();
    }
    #endregion

    #region UI 相關
    /// <summary>
    /// 打開決鬥介面
    /// </summary>
    public void OpenDuelUI()
    {
        uiScript.OpenDuelUI();
    }
    /// <summary>
    /// 關閉決鬥介面
    /// </summary>
    public void CloseDuelUI()
    {
        uiScript.CloseDuelUI();
    }
    #endregion

    #region 英雄相關
    /// <summary>
    /// 產生英雄
    /// </summary>
    private void ProduceHero(MainFortressScript _mfbs)
    {
        if (_mfbs == null)
        {
            _MainFortressScriptList.Remove(_mfbs);
        }
        else
        {
            _mfbs.ProduceHero(time);
        }
    }

    /// <summary>
    /// 取得角色控制
    /// </summary>
    private void SelectedHeroControl()
    {
        if (SelectedHero == null) return;

        Vector2 Pos = SelectedHero._Tf.position; //取得位置
        SelectedHero.PhyOverlapBoxAll(Pos);

        Vector2 Pos2 = Pos; //取得位置
        Pos2.y += 4;
        Pos2.y += Mathf.PingPong(.5f + Time.time, 1f);
        SelectedHeroTarget.position = Pos2;
    }
    /// <summary>
    /// 當可操控角色為null時，就查找符合可操作的角色
    /// </summary>
    private void SelectedHeroFunc()
    {
        HeroScript _hero;
        if (SelectedHero != null) return;
        if (HeroList.Count == 0) return;

        for (int i = 0; i < HeroList.Count; i++)
        {
            _hero = HeroList[i];
            if (_hero == null) continue;
            if (_hero.CompareTag(staticPublicObjectsStaticName.HeroTag)) // 如果是光明英雄
            {
                SelectedHeroFunc(_hero);
                return;
            }
        }
    }

    /// <summary>
    /// 賦予英雄可以操控
    /// </summary>
    /// <param name="_hs">英雄腳本</param>
    public void SelectedHeroFunc(HeroScript _hs)
    {
        int _index = HeroList.IndexOf(_hs);
        if (_hs == null) return;
        if (_index == -1) return;
        if (_hs.Hp <= 0) return;
        HeroScript _Hs;
        for (int i = 0; i < HeroList.Count; i++) //所有英雄不可操控
        {
            _Hs = HeroList[i];
            if (_Hs == null) continue;
            _Hs._Hc.enabled = false;
        }
        _hs._Hc.enabled = true;

        Transform HeroTf = _hs._Tf;
        Vector3 HeroV3 = _hs._Tf.position;
        HeroV3.y += 3f;
        SelectedHero = _hs;
        if (SelectedHeroTarget == null)
            SelectedHeroTarget = Instantiate(SelectedHeroTargetPrefab, HeroV3, Quaternion.identity, HeroTf);
        else
        {
            SelectedHeroTarget.parent = null;
            SelectedHeroTarget.position = HeroV3;
            SelectedHeroTarget.parent = HeroTf;
        }
        CameraGetPlayerTransform(HeroTf);
    }
    /// <summary>
    /// 英雄AI
    /// </summary>
    private void HeroAI(float _time)
    {
        if (HeroList.Count == 0) return; //如果沒有英雄就不執行
        if (!isHeroStateForAction) return; //如果迴圈還在執行動作就不執行
        HeroScript _hero;
        Vector2 _MoveDirection;
        for (int i = 0; i < HeroList.Count; i++)
        {
            isHeroStateForAction = false;
            _hero = HeroList[i];
            if (_hero == null) continue;
            if (!_hero.isfloot) continue;

            if (_hero.Hp <= 0)
            {
                _hero.HeroDuelStateFunc(HeroState.Die);
                continue;
            }
            if (_hero.IsItPossibleToDuel || _hero.isPlayerControl) continue;
            if (_MainFortressScriptList.Count == 1)
            {
                _hero.HeroDuelStateFunc();
                continue;
            }

            _hero._Time = _time;
            _hero.AtkTime += _time;
            if (_hero.isAnimationFrameStorp)
            {
                _hero.AnimationFrameStorpTime += _time;
                if (_hero.AnimationFrameStorpTime >= _hero.AnimationFrameStorpTimeMax)
                {
                    _hero.AnimationFrameStorpTime = 0;
                    _hero.isAnimationFrameStorp = false;
                }
                if (!_hero.isAnimationFrameStorp)
                {
                    _hero._animator.speed = 1;
                }
            }
            _hero.PhyOverlapBoxAll(_hero._Tf.position);
            if (_hero._enemyNowMainFortress == null) _hero.GetEmenyTarget(_MainFortressScriptList); // 取得敵人目標

            Collider2D[] ColliderArray = _hero.enemyCollider; // 取得敵人碰撞器
            if (ColliderArray.Length > 0)
            {
                if (_hero.IsAtkLimit())
                    _hero.HeroDuelStateFunc();
                else
                    _hero.HeroDuelStateFunc(HeroState.Attack, ColliderArray);

                continue;
            }
            Transform _targetTf;
            if (_hero._enemyNowMainFortress != null)
            {
                _targetTf = _hero._enemyNowMainFortress;
                if (_hero._target != null) _targetTf = _hero._target;
                _MoveDirection = CorrectionDirection(_hero._Tf, _targetTf, false);
                _hero.HeroDuelStateFunc(HeroState.Run, _MoveDirection);
            }
        }
        isHeroStateForAction = true;
    }

    /// <summary>
    /// 英雄資料初始化
    /// </summary>
    /// <param name="_Hs"></param>
    /// <param name="isDark"></param>
    public void HeroDataFormat(HeroScript _Hs, bool isDark = true)
    {
        int _HeroLv = _Pdo.PlayerHeroLv;
        float _Hp = _HeroLv * _Pdo.AddHp;
        float _Attack = _HeroLv * _Pdo.AddAtk;
        float _Def = _HeroLv * _Pdo.AddDef;
        LayerMask _Lm = _DarkLayerMask;
        if (!isDark) //如果是黑暗陣營
        {
            _HeroLv = _Glm.HeroLv;
            _Hp = _HeroLv * _Glm.AddHp;
            _Attack = _HeroLv * _Glm.AddAtk;
            _Def = _HeroLv * _Glm.AddDef;
            _Lm = _LayerMask;
        }
        _Hs._Tf = _Hs.transform;
        _Hs._Go = _Hs.gameObject;
        _Hs.Hp = (int)Mathf.Ceil(1000 * _Hp) + _Hs.BasicHp;
        _Hs.HpMax = _Hs.Hp;
        _Hs.Attack = (int)Mathf.Ceil(151 * _Attack) + (int)(_Hs.BasicConstitution * _Hs.BasicQuality);
        _Hs.Def = (int)Mathf.Ceil(76 * _Def) + (int)(_Hs.BasicConstitution * _Hs.BasicQuality);
        _Hs._gameManagerScript = this;
        _Hs._UIHc = _UIHc;
        _Hs._Joystick = _Joystick;
        _Hs.enemyLayerMask = _Lm;
        _Hs._Pdo = _Pdo;
        _Hs.CameraCenterScript = CameraCenterScript;
        Transform _tf = _Hs._Tf;
        Vector2 _pos = _tf.position;
        _pos.y += 2;
        _Hs._Hps = Instantiate(_HpPrefabs, _pos, Quaternion.identity, _tf).HpDataInitializ();
        _pos.y -= .2f;
        _Hs._Defs = Instantiate(_DefPrefabs, _pos, Quaternion.identity, _tf).DefDataInitializ();
        _Hs.HeroInitializ();
        HeroList.Add(_Hs);
    }
    #endregion

    #region 士兵相關動作
    /// <summary>
    /// 兩邊士兵生產
    /// </summary>
    private void ProduceSoldier(MainFortressScript _mfbs, float _time)
    {
        if (_soldierList.Count >= _GmSoldierCountMax) return;
        if (_mfbs != null)
            _mfbs.soldierProduceTime += _time;
        _mfbs.ProduceSoldier();

    }


    /// <summary>
    /// 士兵的動作資訊
    /// </summary>
    /// <param name="_time">時間</param>
    /// <param name="_st">場景類型</param>
    public void SoldierState(float _time, ScenesType _st)
    {
        if (_soldierList.Count == 0 || !isSoldierStateForAction) return;//如果士兵清單為0，或主堡清單為1，或執行迴圈狀態為false (上一輪還再執行)，則不執行


        SoldierScript _soldierScript;
        Transform _tf;
        Vector2 Pos;

        for (int i = 0; i < _soldierList.Count; i++)
        {
            isSoldierStateForAction = false;
            _soldierScript = _soldierList[i];
            if (_soldierScript == null) continue;

            switch (_st)
            {
                case ScenesType.practise:
                case ScenesType.battlefield:
                    if (_MainFortressScriptList.Count == 1 || isGameOver)  //如果遊戲結束，則全體士兵都進入等待
                    {
                        _soldierScript.Idle();
                        continue;
                    }
                    _soldierScript.GetEmenyMainFortress(_MainFortressScriptList);
                    break;
                default:
                    _soldierScript.Idle();
                    break;
            }

            if (_soldierScript.soldierHp <= 0)
            {
                _soldierScript.Die();
                continue;
            }
            _soldierScript._Time = _time;

            _tf = _soldierScript._Tf;
            if (_tf == null) continue;
            Pos = _tf.position;

            _soldierScript._animator.speed = 1;

            
            _soldierScript.PhyOverlapBoxAll(Pos);
            if (_soldierScript._collider2D.Length > 0)
            {
                if (_soldierScript.AttackingTime <= 0)
                {
                    switch (_soldierScript._At)
                    {
                        case AttackType.RemoteAttack:
                            _soldierScript.RemoteAtk();
                            break;
                        case AttackType.MeleeAttack:
                            _soldierScript.Atk();
                            break;
                        case AttackType.RemoteAndMelee:
                            _soldierScript.RemoteOrMeleeAttacking();
                            break;
                    }
                }
                else
                {
                    _soldierScript.Idle();
                    _soldierScript.AttackingTime -= _time;
                }
                continue;
            }
            if (!_soldierScript.isNowHit) //如果沒有被攻擊
            {
                if (_soldierScript._enemyNowMainFortress == null) continue;
                _soldierScript.Move();

                Transform _targetTf = _soldierScript._enemyNowMainFortress;
                if (_soldierScript._target != null) _targetTf = _soldierScript._target;
                _tf.position = Vector3.MoveTowards(Pos, _targetTf.position, _soldierScript.speed * _time);
            }
            else
            {
                //被攻擊
                _soldierScript.isNowHitTime += _time;
                if (_soldierScript.isNowHitTimeMax <= _soldierScript.isNowHitTime)
                {
                    _soldierScript.isNowHit = false;
                    _soldierScript.isNowHitTime = 0;
                }
                continue;
            }

        }
        switch (_st)
        {
            case ScenesType.practise:
            case ScenesType.battlefield:
                if (isGameOver)
                {
                    isSoldierStateForAction = false; //遊戲結束，士兵不再執行
                    return;
                }
                break;
        }

        
        isSoldierStateForAction = true;
    }



    /// <summary>
    /// 士兵資料初始化
    /// </summary>
    /// <param name="_Ss">士兵腳本</param>
    public void SoldierDataFormat(SoldierScript _Ss, bool isDark = true)
    {
        int _soldierLv = _Pdo.soldierLv;
        float _soldierHp = _soldierLv * _Pdo.soldierHp;
        float _soldierAtk = _soldierLv * _Pdo.soldierAtk;
        float _soldierDefense = _soldierLv * _Pdo.soldierDefense;
        int BasicInt = (int)Mathf.Ceil(_Ss.BasicConstitution * _Ss.BasicQuality);

        float _percentage = _Pdo.soldierPercentage - (0.005f * _Pdo.soldierLv); //輸出偏移值
        LayerMask _Lm = _DarkLayerMask;
        if (!isDark) //如果是黑暗士兵
        {
            _soldierLv = _Glm.soldierLv;
            _soldierHp = _soldierLv * _Glm.soldierHp;
            _soldierAtk = _soldierLv * _Glm.soldierAtk;
            _soldierDefense = _soldierLv * _Glm.soldierDefense;
            _Lm = _LayerMask;
            _percentage = _Glm.soldierPercentage - (0.005f * _Glm.soldierLv);
        }
        
        _Ss._Tf = _Ss.transform;
        _Ss._Go = _Ss.gameObject;
        if (_Ss._At != AttackType.MeleeAttack)
        {
            Transform _tf = _Ss._Tf.Find("遠程武器預製物集合");
            if (_tf != null)
            {
                _Ss._ammunitionScript = _tf.GetComponent<AmmunitionScript>();
                _Ss._ammunitionTf = _Ss._ammunitionScript.transform;
            }
        }
        _Ss.soldierHp = (int)Mathf.Ceil(500 * _soldierHp) + _Ss.BasicHp;
        _Ss.soldierHpMax = _Ss.soldierHp;
        _Ss.soldierAtk = (int)Mathf.Ceil(102 * _soldierAtk) + BasicInt;
        _Ss.soldierDefense = (int)Mathf.Ceil(51 * _soldierDefense) + BasicInt;
        _Ss.Percentage = _percentage;
        Vector2 _pos = _Ss._Tf.position;
        _pos.y += 2;
        _Ss._Hps = Instantiate(_HpPrefabs, _pos, Quaternion.identity, _Ss._Tf).HpDataInitializ();
        _Ss._gameManagerScript = this;
        _Ss._enemyLayerMask = _Lm;
        _Ss.SoldierDataInitializ();
        _soldierList.Add(_Ss);
    }


    /// <summary>
    /// 刪除士兵清單以及士兵本體
    /// </summary>
    /// <param name="_ss">士兵腳本</param>
    public void SolidListRemove(SoldierScript _ss)
    {
        _soldierList.Remove(_ss); // 從清單中移除
        Destroy(_ss._Go, 2); // 1秒後刪除物件
    }
    #endregion

    #region 功能型方法
    /// <summary>
    ///矯正方向
    /// </summary>
    /// <param name="_tf">玩家 Transform</param>
    /// <param name="_EnemyTf">敵人 Transform</param>
    public void CorrectionDirection(Transform _tf, Transform _EnemyTf, out Vector2 _Direction)
    {
        int _direction = 1;
        Vector2 _scale = _tf.localScale;
        _scale.x = Mathf.Abs(_scale.x);
        if (_tf.position.x > _EnemyTf.position.x)
        {
            _scale.x *= -1;
            _direction = -1;
        }
        _tf.localScale = _scale;
        _Direction = Vector2.right * _direction;
    }
    /// <summary>
    /// 會同時調整兩個目標的方向
    /// </summary>
    /// <param name="_tf">主要角色</param>
    /// <param name="_EnemyTf">對手角色</param>
    /// <param name="SettingEnemyDirection">是否需要設定對手角色的方向</param></param>
    /// <returns>返回方向X軸1或-1的Vector2</returns>
    public Vector2 CorrectionDirection(Transform _tf, Transform _EnemyTf, bool SettingEnemyDirection = true)
    {
        if (_EnemyTf == null) return Vector2.zero;
        int _direction = 1;
        Vector2 _scale = _tf.localScale;  // _tf的 scale
        _scale.x = Mathf.Abs(_scale.x);
        if (_tf.position.x > _EnemyTf.position.x)
        {
            _scale.x *= -1;
            _direction = -1;
        }
        _tf.localScale = _scale; // 設定 _tf 的 scale

        if (SettingEnemyDirection) // 如果需要設定 _EnemyTf 的 scales
        {
            _scale = _EnemyTf.localScale;
            _scale.x = Mathf.Abs(_scale.x);
            if (_tf.position.x < _EnemyTf.position.x)
            {
                _scale.x *= -1;
            }
            _EnemyTf.localScale = _scale; // 設定 _EnemyTf 的 scales
        }

        return Vector2.right * _direction; // 只返回 _tf 的方向
    }
    /// <summary>
    /// 移除清單中的物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">要清除的List</param>
    /// <param name="item">要清除的物件</param>
    public void RemoveFromList<T>(List<T> list, T item) //移除清單中的物件
    {
        int index = list.IndexOf(item);
        if (index != -1) list.RemoveAt(index);
    }
    #endregion

    #region 遊戲結束
    /// <summary>
    /// 本場遊戲結束
    /// </summary>
    public void GameOver()
    {
        if (_MainFortressScriptList.Count > 1 && isGameOver) return;
        for (int i = 0; i < _MainFortressScriptList.Count; i++)
        {
            if (_MainFortressScriptList[i].CompareTag(staticPublicObjectsStaticName.MainFortressTag))
            {
                uiScript.NameOpenCentralizedManagementUI("TeleportationArrayUI");
                return;
            }
        }
        isGameOver = true;
        uiScript.GameOverUI();
    }
    #endregion

    #region 主堡相關
    /// <summary>
    /// 主堡產生、執行狀態相關
    /// </summary>
    public void ProduceOrStateFunc(float _deltatime, float _time)
    {

        if (!isProduce) return;
        if (_MainFortressScriptList.Count <= 1) return;

        MainFortressScript _mfbs;
        for (int mfIndex = 0; mfIndex < _MainFortressScriptList.Count; mfIndex++)
        {
            isProduce = false;
            _mfbs = _MainFortressScriptList[mfIndex];
            if (_mfbs == null) continue;

            switch (_mfbs.tag)
            {
                case staticPublicObjectsStaticName.DarkMainFortressTag:
                    _mfbs.PhyOverlapBoxAll(_LayerMask);
                    break;
                case staticPublicObjectsStaticName.MainFortressTag:
                    _mfbs.PhyOverlapBoxAll(_DarkLayerMask);
                    break;
            }
            ProduceSoldier(_mfbs, _deltatime); //兩邊士兵生產
            ProduceHero(_mfbs); //產生英雄

            _mfbs.MainFortressHitVFX(_deltatime, _time); //受傷效果
        }
        isProduce = true;
    }
    /// <summary>
    /// 主堡爆了
    /// </summary>
    public void MainFortressOver(MainFortressScript _mfb)
    {
        if (_mfb == null) return;
        RemoveFromList(_MainFortressScriptList, _mfb);
        GameOver();
    }
    /// <summary>
    /// 主堡被攻擊
    /// </summary>
    /// <param name="_mfbTf">主堡本身</param>
    /// <param name="_enemyList">攻擊主堡的人</param>
    public void CastleUnderAttack(Transform _mfbTf, List<Collider2D> _enemyList)
    {
        if (_enemyList.Count == 0) return;
        Transform _tf;
        Collider2D _col;
        SoldierScript _Ss;
        if (_soldierList.Count > 0) // 如果有士兵
        {
            for (int i = 0; i < _soldierList.Count; i++)
            {
                _Ss = _soldierList[i];
                if (_Ss == null) continue;
                for (int c = 0; c < _enemyList.Count; c++)
                {
                    _col = _enemyList[c];
                    if (_col != null)
                    {
                        if (_Ss._target == null)
                        {
                            _tf = _col.transform;
                            switch (_mfbTf.tag)
                            {
                                case staticPublicObjectsStaticName.DarkMainFortressTag: // 如果是黑暗主堡
                                    if (_Ss.CompareTag(staticPublicObjectsStaticName.DARKSoldierTag)) // 如果是黑暗士兵
                                    {
                                        _Ss._target = _tf;
                                    }
                                    break;
                                case staticPublicObjectsStaticName.MainFortressTag: // 如果是光明主堡
                                    if (_Ss.CompareTag(staticPublicObjectsStaticName.PlayerSoldierTag)) // 如果是玩家士兵
                                    {
                                        _Ss._target = _tf;
                                    }
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        HeroScript _hs;
        if (HeroList.Count > 0) // 如果有英雄
        {
            for (int i = 0; i < HeroList.Count; i++) // 檢查每個英雄
            {
                _hs = HeroList[i];
                if (_hs == null) continue;
                for (int c = 0; c < _enemyList.Count; c++) // 檢查每個攻擊主堡的人
                {
                    _col = _enemyList[c];
                    if (_col != null)
                    {
                        if (_hs._target == null)
                        {
                            _tf = _col.transform;
                            switch (_mfbTf.tag) // 檢查主堡的tag
                            {
                                case staticPublicObjectsStaticName.DarkMainFortressTag: // 如果是黑暗主堡
                                    if (_hs.CompareTag(staticPublicObjectsStaticName.DarkHeroTag)) // 如果是黑暗英雄
                                    {
                                        _hs._target = _tf;
                                    }
                                    break;
                                case staticPublicObjectsStaticName.MainFortressTag: // 如果是光明主堡
                                    if (_hs.CompareTag(staticPublicObjectsStaticName.HeroTag)) // 如果是玩家英雄
                                    {
                                        _hs._target = _tf;
                                    }
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 在村莊時產生一位已選擇的英雄
    /// </summary>
    /// <param name="_hs">英雄</param>
    /// <param name="_tf">主堡Transform</param>
    private void VillageScenesProduceHero(HeroScript _hs, Transform _tf)
    {
        if (_hs == null) return; //如果沒有英雄就不執行
        HeroScript _hero = Instantiate(_hs, _tf.position, Quaternion.identity, null); //產生英雄
        _hero.tag = staticPublicObjectsStaticName.HeroTag; //設定英雄tag
        _hero.gameObject.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.HeroLayer); //設定英雄圖層
        HeroDataFormat(_hero); //設定英雄資料
    }
    /// <summary>
    /// 光明主堡資料重置
    /// </summary>
    /// <param name="_mfb">主堡腳本</param>
    public void MainFortressDataFormat(MainFortressScript _mfb)
    {
        if (_Pdo == null)
        {
            MainFortressDataFormat(_mfb);
            return;
        }
        if (_mfb == null) return;

        _mfb._hp = _Pdo.maxhp;
        _mfb._MaxHp = _Pdo.maxhp;
        _mfb.soldierProduceTimeMax = _Pdo.soldierProduceTimeMax; // 設定士兵生產間隔時間
        _mfb.soldierProduceTime = _mfb.soldierProduceTimeMax; // 設定士兵生產時間
        _mfb.ProduceHeroTimeMax = _Pdo.ProduceHeroTimeMax; // 設定英雄生產間隔時間
        _mfb.ProduceHeroTime = _mfb.ProduceHeroTimeMax;  // 設定英雄生產時間
        _mfb._soldierCount = _Pdo.soldierCount;
        _mfb.HeroUI = HeroUI;
        _mfb.selectedSoldierList.AddRange(_Pdo.soldierSelectedList); // 把士兵加入士兵清單
        _mfb.GetHeroList.AddRange(_Pdo.SelectedHeroList); // 把英雄加入英雄清單
        _mfb.selectedHeroList.AddRange(_Pdo.SelectedHeroList); // 把英雄加入英雄清單
        _MainFortressScriptList.Add(_mfb);
        _mfb.MainFortressHpTextMeshPro(); // 更新主堡血量文字
        _mfb.MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字

        switch (NowScenes)
        {
            case ScenesType.village:
                if (SelectedHero == null)
                {
                    VillageScenesProduceHero(_mfb.GetHeroList[Random.Range(0, _mfb.GetHeroList.Count)], _mfb._Tf);
                }
                break;
            case ScenesType.practise:
            case ScenesType.battlefield:
            case ScenesType.prairie:
            case ScenesType.mine:
            case ScenesType.dark:
                break;

        }
    }
    /// <summary>
    /// 黑暗主堡資料重置
    /// </summary>
    /// <param name="_mfb">主堡腳本</param>
    public void MainFortressDataFormat(DarkMainFortressScript _mfb)
    {
        if (_Glm == null)
        {
            _Glm = FindObjectOfType<GameLevelManager>();
            MainFortressDataFormat(_mfb);
            return;
        }
        if (_mfb == null) return;
        _mfb._hp = _Glm.maxhp;
        _mfb._MaxHp = _Glm.maxhp;

        _mfb.soldierProduceTimeMax = _Glm.soldierProduceTimeMax; // 士兵生產間隔時間
        _mfb.soldierProduceTime = _mfb.soldierProduceTimeMax; // 士兵生產時間
        _mfb.ProduceHeroTimeMax = _Glm.ProduceHeroTimeMax; // 英雄生產間隔時間
        _mfb.ProduceHeroTime = _mfb.ProduceHeroTimeMax; // 英雄生產時間
        _mfb._soldierCount = _Glm.soldierCount;
        _mfb.selectedSoldierList.AddRange(_Glm.soldierSelectedList); // 把士兵加入士兵清單
        _mfb.GetHeroList.AddRange(_Glm.SelectedHeroList); // 把英雄加入英雄清單
        _mfb.selectedHeroList.AddRange(_Glm.SelectedHeroList); // 把英雄加入英雄清單
        _MainFortressScriptList.Add(_mfb);
        _mfb.MainFortressHpTextMeshPro(); // 更新主堡血量文字
        _mfb.MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字

    }
    #endregion

    #region 村莊、野生場景
    /// <summary>
    /// 野生節點資料初始化
    /// </summary>
    /// <param name="_mns">野生節點腳本</param>
    public void MonsterNodeDataGet(MonsterNodeScript _mns)
    {
        _monsterNodesList.Add(_mns);
        // _monsterNodesList 代表傳送點的List有東西，可以生產地圖怪物了
        isMonsterNodes = true;
    }
    /// <summary>
    /// 野生怪物、士兵產生
    /// </summary>
    /// <param name="_time"></param>
    void ProduceWildSoldier(float _time)
    {
        if (!isMonsterNodes || _monsterNodesList.Count < 1) return;
        for (int i = 0; i < _monsterNodesList.Count; i++)
        {
            if (_monsterNodesList[i].SoldierCount <= 0) return;
            _monsterNodesList[i].ProduceWildSoldier(_LayerMask);
        }
    }


    #endregion

    /// <summary>
    /// 執行投擲武器、道具清單
    /// </summary>
    public void ParabolaGoto() {
        if (_PsList.Count == 0 ||
            !isParabolaGoto) return;
        ParabolaScript _ps;
        for (int i = 0; i < _PsList.Count; i++)
        {
            isParabolaGoto = false;
            _ps = _PsList[i];
            if (_ps == null)
            {
                _PsList.RemoveAt(i);
                continue;
            }
            _ps.Goto();
        }
        isParabolaGoto = true;
    }
    /// <summary>
    /// 新增物件到投擲武器、道具清單
    /// </summary>
    /// <param name="_ps">投擲武器、道具</param>
    public void ParabolaListAdd(ParabolaScript _ps)
    {
        if (_ps == null) return;
        _PsList.Add(_ps);
    }
}

