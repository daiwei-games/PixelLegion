using Assets.Scripts;
using Assets.Scripts.BaseClass;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 光明主堡腳本
    /// </summary>
    [Header("光明主堡腳本")]
    public List<MainFortressScript> _mainFortressScript;
    /// <summary>
    /// 黑暗主堡腳本
    /// </summary>
    [Header("黑暗主堡腳本")]
    public List<DarkMainFortressScript> _darkMainFortressScript;
    /// <summary>
    /// 存放敵人產生英雄的腳本
    /// </summary>
    public List<DarkProduceHeroScript> DarkHeroScript;
    /// <summary>
    /// 融合兩個主堡清單
    /// </summary>
    [Header("融合兩個主堡清單")]
    public List<MainFortressBaseScript> _mainFortressScriptList;
    /// <summary>
    /// 是否執行生產士兵的函數，還是正在執行
    /// </summary>
    private bool isProduceSoldier;
    /// <summary>
    /// 所有士兵清單
    /// </summary>
    [Header("所有士兵清單")]
    public List<SoldierScript> _soldierList;
    /// <summary>
    /// 士兵計算生產間隔
    /// </summary>
    [Header("士兵計算生產間隔"), SerializeField, Range(0.01f, 10)]
    private float ProduceSoldierTimeMax;
    private float ProduceSoldierTime;
    /// <summary>
    /// 毫秒
    /// </summary>
    float time;
    /// <summary>
    /// 是否再次執行迴圈
    /// </summary>
    bool isSoldierStateForAction;
    /// <summary>
    /// UI腳本
    /// </summary>
    public UIScript uiScript;
    /// <summary>
    /// 遊戲結束物件
    /// </summary>
    public UIGameOverScript GameOverObject;
    /// <summary>
    /// 玩家管理器腳本
    /// </summary>
    public PlayerScript playerScript;
    /// <summary>
    /// 已經產生的英雄清單
    /// </summary>
    [Header("已經產生的英雄清單")]
    public List<HeroScript> HeroList;
    /// <summary>
    /// 是否再次執行英雄動作迴圈的判斷
    /// </summary>
    public bool isHeroStateForAction;
    /// <summary>
    /// 產生英雄計算間隔
    /// </summary>
    [Header("產生英雄計算間隔"), SerializeField, Range(0.01f, 10)]
    private float ProduceHeroTimeMax;
    private float ProduceHeroTime;
    /// <summary>
    /// 選擇的英雄頭上出現指標
    /// </summary>
    [Header("選擇的英雄頭上出現指標"), SerializeField]
    private Transform SelectedHeroTargetPrefab;
    private Transform SelectedHeroTarget;


    private float MissOrDashStartTimr;

    #region 決鬥
    /// <summary>
    /// 是否決鬥
    /// </summary>
    [Header("是否決鬥")]
    public bool isDuel;
    /// <summary>
    /// 決鬥腳本
    /// </summary>
    public DuelScript duelScript;
    /// <summary>
    /// 可以操控的英雄
    /// </summary>
    [Header("可以操控的英雄"), SerializeField]
    private HeroScript SelectedHero;
    /// <summary>
    /// 搖桿腳本
    /// </summary>
    [Header("搖桿")]
    public VariableJoystick _Joystick;
    /// <summary>
    /// 搖桿方向
    /// </summary>
    Vector2 _JoystickDirection;
    /// <summary>
    /// 玩家英雄操作的操控介面
    /// </summary>
    public HeroController heroControl;


    #region 攝影機
    /// <summary>
    /// 監視器中心點
    /// </summary>
    public CameraCenterScript CameraCenterScript;
    #endregion
    #endregion
    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;

        isSoldierStateForAction = true;
        isHeroStateForAction = true;
        isProduceSoldier = true;
        _Joystick = FindObjectOfType<VariableJoystick>();

        isDuel = false;
    }
    private void Update()
    {
        if (staticPublicGameStopSwitch.gameStop)
        {
            Time.timeScale = 0;
        }

        GameOver();
        SelectedHeroControl();

        // 取得搖桿位移
        _JoystickDirection = Vector2.right * _Joystick.Horizontal;

        MissOrDashStartTimr = Time.time;
        //當玩家按下衝刺
        PlayGMHeroDash(MissOrDashStartTimr);
        //當玩家按下閃現
        PlayGMHeroMiss(MissOrDashStartTimr);

    }
    private void FixedUpdate()
    {
        if (staticPublicGameStopSwitch.gameStop) return;
        time = Time.deltaTime;
        if (!isDuel)
        {
            ProduceSoldierTime += time;
            if (ProduceSoldierTime >= ProduceSoldierTimeMax)
            {
                ProduceSoldierTime = 0;
                MainFortressBaseProduceSoldier(); //兩邊士兵生產
            }
            ProduceHeroTime += time;
            if (ProduceHeroTime >= ProduceHeroTimeMax)
            {
                ProduceHeroTime = 0;
                PlayerProduceHero(); //產生英雄
            }
            if (SelectedHero != null)
            {
                //英雄的移動
                SelectedHero.HeroDuelStateFunc(HeroState.Run, _JoystickDirection);
            }
        }
        //管理器區塊
        SoldierState();
        HeroAI();
        if (isDuel)
        {
            duelScript.MoveAction();
        }
        CameraToPlayerPosition();
    }

    #region 英雄相關操作
    #region 決鬥相關操作
    /// <summary>
    /// 按下決鬥
    /// </summary>
    public void Duel()
    {
        if (SelectedHero == null) return;
        isDuel = !isDuel;
        SoldierState();
        HeroAI();
        if (isDuel)
        {
            OpenDuelUI();
            ClosePlayerMoveUI();
        }
        if (!isDuel)
        {
            CloseDuelUI();
            OpenPlayerMoveUI();
            return;
        }
        HeroScript _hscript;
        int _hsIndex = 0;
        bool _isDarkHero = false;
        float _distanceStart = 0;
        float _distanceEnd = 0;
        for (int i = 0; i < HeroList.Count; i++)
        {
            _hscript = HeroList[i];
            if (_hscript == null) continue;
            if (_hscript.CompareTag(staticPublicObjectsStaticName.HeroTag)) continue;
            if (_hscript.CompareTag(staticPublicObjectsStaticName.DarkHeroTag))
            {
                _isDarkHero = true;
                if (_distanceStart == 0)
                {
                    _hsIndex = i;
                    _distanceStart = Vector3.Distance(SelectedHero._Tf.position, _hscript._Tf.position);
                }
                _distanceEnd = Vector3.Distance(SelectedHero._Tf.position, _hscript._Tf.position);
                if (_distanceStart > _distanceEnd)
                {
                    _hsIndex = i;
                    _distanceStart = _distanceEnd;
                }
            }
        }
        if (!_isDarkHero) //沒有敵方英雄
        {
            CloseDuelUI(); //關閉決鬥介面
            OpenPlayerMoveUI(); //開啟英雄移動介面
            if (isDuel) Duel();
            return;
        }
        _hscript = HeroList[_hsIndex]; //取得最近的敵方英雄
        if (_hscript != null)
        {
            _hscript.IsItPossibleToDuel = true;
            SelectedHero.IsItPossibleToDuel = true;
        }
        if (duelScript != null) // 如果決鬥腳本不是空的
            duelScript.HeroData(SelectedHero, _hscript); //設定決鬥資料
    }

    /// <summary>
    /// 取消
    /// </summary>
    public void DuelEnd()
    {
        duelScript.DuelFormat();
    }
    /// <summary>
    /// 自動選擇動作
    /// </summary>
    public void OnHeroMoveAuto()
    {
        duelScript.PlayerMoveClick();
    }
    /// <summary>
    /// 增加一個攻擊動作
    /// </summary>
    public void OnDuelAttack()
    {
        duelScript.PlayerMoveClick(HeroState.Attack);
    }
    /// <summary>
    /// 增加一個防禦動作
    /// </summary>
    public void OnDuelDef()
    {
        duelScript.PlayerMoveClick(HeroState.Def);
    }
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
    /// <summary>
    /// 開啟英雄移動介面
    /// </summary>
    public void OpenPlayerMoveUI()
    {
        uiScript.OpenPlayerMoveUI();
    }
    /// <summary>
    /// 關閉英雄移動介面
    /// </summary>
    public void ClosePlayerMoveUI()
    {
        uiScript.ClosePlayerMoveUI();
    }
    #endregion
    #region 玩家操作英雄
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
        if (CameraCenterScript == null) return;
        CameraCenterScript.GotoPlyer();
    }
    #region 玩家手動操作
    /// <summary>
    /// 玩家操做攻擊
    /// </summary>
    public void gmHeroAttack()
    {
        if (isDuel) return;
        if (SelectedHero == null) return;
        SelectedHero.HeroDuelStateFunc(HeroState.Attack, SelectedHero._colliders, null);
    }
    /// <summary>
    /// 閃現
    /// </summary>
    public void gmHeroMiss(float time)
    {
        if (isDuel) return;
        if (SelectedHero == null) return;
        SelectedHero.HeroDuelStateFunc(HeroState.Miss, Vector2.zero, time, null);

    }
    private void PlayGMHeroMiss(float time)
    {
        if (isDuel) return;
        if (SelectedHero == null) return;
        if (!SelectedHero.IsItPossibleMiss) return;
        SelectedHero.Miss1(time);
    }
    /// <summary>
    /// 手動衝刺決鬥不使用
    /// </summary>
    public void gmHeroDash(float time)
    {
        if (isDuel) return;
        if (SelectedHero == null) return;
        SelectedHero.HeroDuelStateFunc(HeroState.Dash, Vector2.zero, time, null);
    }
    /// <summary>
    /// 持續判斷是否衝刺 決鬥不使用
    /// </summary>
    private void PlayGMHeroDash(float time)
    {
        if (isDuel) return;
        if (SelectedHero == null) return;
        if (!SelectedHero.IsItPossibleToDash) return;
        SelectedHero.FastForward(SelectedHero._Tf, time);

    }
    #endregion
    #region 決鬥或是AI操作時
    /// <summary>
    /// 閃現
    /// </summary>
    /// <param name="heroScript"></param>
    /// <param name="EnemyTf"></param>
    public void gmHeroMiss(HeroScript heroScript, Transform EnemyTf)
    {
        if (heroScript == null) return;
    }
    private void PlayGMHeroMiss(HeroScript heroScript, Transform EnemyTf)
    {
        if (heroScript == null) return;
        if (!heroScript.IsItPossibleMiss) return;
        heroScript.HeroDuelStateFunc(HeroState.Miss1);
    }

    /// <summary>
    /// 玩家操作衝刺
    /// </summary>
    public void gmHeroDash(HeroScript heroScript)
    {
        if (SelectedHero == null) return;
        heroScript.HeroDuelStateFunc(HeroState.Dash, Vector2.zero, Time.time);
    }
    /// <summary>
    /// 持續判斷是否衝刺
    /// </summary>
    private void PlayGMHeroDash(HeroScript heroScript)
    {
        if (heroScript == null) return;
        if (!heroScript.IsItPossibleToDash) return;
        heroScript.FastForward(heroScript._Tf, Time.time);
    }
    #endregion
    #endregion
    /// <summary>
    /// 產生英雄
    /// </summary>
    private void PlayerProduceHero()
    {
        if (_mainFortressScript.Count == 0 || DarkHeroScript.Count == 0) return;
        if (_mainFortressScriptList.Count < 2) return;

        playerScript.ProduceHero(time);

        //敵人英雄產生
        if (DarkHeroScript.Count > 0)
        {
            DarkProduceHeroScript _dark;
            for (int i = 0; i < DarkHeroScript.Count; i++)
            {
                _dark = DarkHeroScript[i];
                if (_dark == null)
                {
                    DarkHeroScript.RemoveAt(i);
                    continue;
                }
                _dark.ProduceHero(time);
            }
        }
    }

    /// <summary>
    /// 賦予英雄可以操控
    /// </summary>
    /// <param name="_hs">英雄腳本</param>
    public void SelectedHeroFunc(HeroScript _hs)
    {
        if (isDuel) return; // 如果是決鬥模式不能換人物

        int _index = HeroList.IndexOf(_hs);
        if (_hs == null) return;
        if (_index == -1) return;
        for (int i = 0; i < HeroList.Count; i++) //所有英雄不可操控
        {
            HeroList[i].isPlayerControl = false;
        }
        _hs.isPlayerControl = true; //選擇的英雄可以操控
        _hs.heroController = heroControl; //設定玩家控制介面
        _hs.playAnimationTime += 10;
        _hs._tfposition = _hs._Tf.position;
        Transform HeroTf = _hs._Tf;
        Vector3 HeroV3 = _hs._tfposition;
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
        SelectedHero.Idle();
        CameraGetPlayerTransform(HeroTf);
        heroControl.DashOpenOrClose(_hs.isDash); //設定英雄控制器
    }
    /// <summary>
    /// 英雄AI
    /// </summary>
    private void HeroAI()
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
            _hero._tfposition = _hero._Tf.position;
            if(_hero.Hp <= 0)
            {
                _hero.HeroDuelStateFunc(HeroState.Die);
                continue;
            }
            if (_hero.IsItPossibleToDuel || _hero.isPlayerControl) continue;
            if (_mainFortressScript.Count == 0 || _darkMainFortressScript.Count == 0)
            {
                _hero.HeroDuelStateFunc();
                continue;
            }

            _hero._Time = time;
            _hero._animator.speed = 1;
            if (isDuel) _hero._animator.speed = 0.2f;

            _hero.PhyOverlapBoxAll(_hero._tfposition);
            if (_hero._target == null)
            {
                MainFortressBaseScript _mfbs;
                switch (_hero.tag)
                {
                    case staticPublicObjectsStaticName.HeroTag:
                        for (int drakMfItem = 0; drakMfItem < _darkMainFortressScript.Count; drakMfItem++)
                        {
                            _mfbs = _darkMainFortressScript[drakMfItem];
                            if (_mfbs != null) _hero._target = _mfbs._Tf;
                        }
                        break;
                    case staticPublicObjectsStaticName.DarkHeroTag:
                        for (int drakMfItem = 0; drakMfItem < _mainFortressScript.Count; drakMfItem++)
                        {
                            _mfbs = _mainFortressScript[drakMfItem];
                            if (_mfbs != null) _hero._target = _mfbs._Tf;
                        }
                        break;
                }
            }
            else
            {
                _MoveDirection = CorrectionDirection(_hero._Tf, _hero._target);
                Collider2D[] ColliderArray = _hero.enemyCollider;
                if (ColliderArray.Length > 0)
                {
                    _hero.HeroDuelStateFunc(HeroState.Attack, ColliderArray, null);
                    continue;
                }
                _hero.HeroDuelStateFunc(HeroState.Run, _MoveDirection);
            }
        }
        isHeroStateForAction = true;
    }

    /// <summary>
    /// 取得角色控制
    /// </summary>
    private void SelectedHeroControl()
    {
        if (SelectedHero == null) return;
        SelectedHero._tfposition = SelectedHero._Tf.position; //更新位置
        Vector2 Pos = SelectedHero._tfposition; //取得位置
        SelectedHero.PhyOverlapBoxAll(SelectedHero._tfposition);

        Vector2 Pos2 = Pos; //取得位置
        Pos2.y += 4;
        Pos2.y += Mathf.PingPong(.5f + Time.time, 1f);
        SelectedHeroTarget.position = Pos2;
    }
    /// <summary>
    /// 英雄資料應用與操作
    /// </summary>
    private struct HeroDataController
    {
        /// <summary>
        /// 英雄腳本
        /// </summary>
        public HeroScript _hs;
        /// <summary>
        /// 英雄Transform
        /// </summary>
        public Transform _tf;
        /// <summary>
        /// 英雄Rigidbody2D
        /// </summary>
        public Rigidbody2D _rd;
        /// <summary>
        /// 敵人目標
        /// </summary>
        public Transform _enemyTarget;
        /// <summary>
        /// 移動方向
        /// </summary>
        public Vector2 _MoveDirection;

        public HeroDataController(HeroScript hs)
        {
            _hs = hs;
            _tf = _hs._Tf;
            _rd = _hs._rg;
            _enemyTarget = _hs._target;
            _MoveDirection = Vector2.zero;
        }
        public void Phy2D()
        {
            Vector2 pos = _hs._tfposition;
            _hs.PhyOverlapBoxAll(pos);
        }
        public void Move(Vector2 _dir)
        {
            _hs._tfposition = _tf.position;
            #region 重新尋找目標
            if (_hs._target == null)
            {

                List<Transform> _etfList = new List<Transform>(); //取得敵人清單
                Transform _tf; //暫存敵人
                Transform _thisTf = _hs._Tf; //暫存自己
                float targetDistance = 0; //目標距離
                float nextDistance = 0; //下一個距離
                int targetIndex = 0; //目標索引
                for (int i = 0; i < _etfList.Count; i++)
                {
                    _tf = _etfList[i];
                    if (_tf == null)
                    {

                        continue;
                    }
                    if (i == 0) targetDistance = Vector2.Distance(_thisTf.position, _tf.position); //取得距離
                    nextDistance = Vector2.Distance(_thisTf.position, _tf.position); //取得距離
                    targetIndex = i;
                    if (targetDistance > nextDistance) //如果距離比較小
                    {
                        targetDistance = nextDistance; //設定距離
                        targetIndex = i; //設定目標索引
                    }
                }
                _hs._target = _etfList[targetIndex]; //設定目標
                return;
            }
            #endregion
            Vector2 Pos = _hs._tfposition;
            Vector2 Scale = _tf.localScale;
            Vector2 enemyPos = _hs._target.position;
            Scale.x = Mathf.Abs(Scale.x);
            if (Pos.x > enemyPos.x)
            {
                Scale.x *= -1;
            }
            _tf.localScale = Scale;
            Phy2D();
            #region 衝刺攻擊 (未完成)

            #endregion
            if (_hs.enemyCollider.Length > 0)
            {
                _hs.HeroDuelStateFunc(HeroState.Attack, _hs._colliders, null);
                return;
            }
            _hs.HeroDuelStateFunc(HeroState.Run, _dir);
        }
    }

    #endregion

    #region 士兵相關動作
    /// <summary>
    /// 兩邊士兵生產
    /// </summary>
    private void MainFortressBaseProduceSoldier()
    {
        if (!isProduceSoldier) return;
        if (_mainFortressScriptList.Count == 0) return;
        MainFortressBaseScript _mfbs;
        for (int i = 0; i < _mainFortressScriptList.Count; i++)
        {
            _mfbs = _mainFortressScriptList[i];
            isProduceSoldier = false;
            if (_mfbs != null)
                _mfbs.soldierProduceTimeNow += time;
            _mfbs.ProduceSoldier();
        }
        isProduceSoldier = true;
    }
    /// <summary>
    /// 士兵的動作資訊
    /// </summary>
    public void SoldierState()
    {
        bool isGameOver = false;
        if (_soldierList.Count < 0) return;
        SoldierStateData _ssd = new SoldierStateData();
        if (!isSoldierStateForAction) return;
        SoldierScript _soldierScript;
        for (int i = 0; i < _soldierList.Count; i++)
        {
            isSoldierStateForAction = false;
            _soldierScript = _soldierList[i];
            if (_soldierScript == null) continue;
            _soldierScript._Time = time;
            isGameOver = _mainFortressScript.Count == 0; // 如果光明主堡清單為0，則遊戲結束
            if (isGameOver) //如果遊戲結束，則全體士兵都進入等待
                _soldierScript.Idle();

            _soldierScript._animator.speed = 1;
            if (isDuel)
            {
                _soldierScript.Idle();
                _soldierScript._animator.speed = 0.2f;
                continue;
            }
            _ssd._soldierScript = _soldierScript;
            _ssd._Tf = _soldierScript._Tf;
            _ssd._Go = _soldierScript._Go;
            _ssd._targetTransform = _soldierScript._enemyNowMainFortress;
            _ssd._Rigidbody2 = _soldierScript._body2D;
            _ssd.dT = time;
            _ssd.enemyTarget = new List<MainFortressBaseScript>(); ;
            if (_soldierScript.tag == staticPublicObjectsStaticName.DARKSoldierTag)
            {
                if (_mainFortressScript.Count > 0)
                    _ssd.enemyTarget.AddRange(_mainFortressScript);
            }
            else if (_soldierScript.tag == staticPublicObjectsStaticName.PlayerSoldierTag)
            {
                if (_darkMainFortressScript.Count > 0)
                    _ssd.enemyTarget.AddRange(_darkMainFortressScript);
            }

            _ssd.isMoveOrDie();
        }
        if (isGameOver)
        {
            isSoldierStateForAction = false; //遊戲結束，士兵不再執行
            return;
        }
        isSoldierStateForAction = true;
    }

    /// <summary>
    /// 士兵的動作資訊
    /// </summary>
    private struct SoldierStateData
    {
        public Transform _Tf;
        public GameObject _Go;
        public Transform _targetTransform;
        public Rigidbody2D _Rigidbody2;
        public SoldierScript _soldierScript;
        public List<MainFortressBaseScript> enemyTarget;
        public float dT;


        public SoldierStateData(SoldierScript _sscript, float time, List<MainFortressBaseScript> _enemy)
        {
            _Tf = _sscript._Tf;
            _Go = _sscript.gameObject;
            _targetTransform = _sscript._enemyNowMainFortress;
            _Rigidbody2 = _sscript._body2D;
            _soldierScript = _sscript;
            enemyTarget = _enemy;
            dT = time;
        }
        /// <summary>
        /// 士兵的射線跟行動
        /// </summary>
        public void SoldierTarget()
        {
            if (_targetTransform == null)
            {
                _soldierScript.Idle();
                float _targetDistance = 0;
                float _nextDistance = 0;
                Transform enemyTargetTransform;
                int enemyTargetIndex = 0;
                for (int i = 0; i < enemyTarget.Count; i++)
                {
                    if (enemyTarget[i] == null) continue;
                    enemyTargetTransform = enemyTarget[i]._Tf;
                    if (_targetDistance == 0 && _nextDistance == 0)
                        _targetDistance = Vector3.Distance(_Tf.position, enemyTargetTransform.position);
                    else
                    {
                        _nextDistance = Vector3.Distance(_Tf.position, enemyTargetTransform.position);
                        if (_targetDistance > _nextDistance)
                        {
                            _targetDistance = _nextDistance;
                            enemyTargetIndex = i;
                        }
                    }
                }
                enemyTargetTransform = enemyTarget[enemyTargetIndex]._Tf;
                _soldierScript._enemyNowMainFortress = enemyTargetTransform;
                return;
            }
            Vector2 pos = _Tf.position;
            _soldierScript.PhyOverlapBoxAll(pos);
            if (_soldierScript._collider2D.Length > 0)
            {
                _soldierScript.Atk();
                return;
            }
            _soldierScript.Move();
            _Tf.position = Vector3.MoveTowards(_Tf.position, _targetTransform.position, _soldierScript.speed * dT);
        }
        /// <summary>
        /// 是否已經死亡
        /// </summary>
        public void isMoveOrDie()
        {
            if (_soldierScript.soldierHp > 0)
                SoldierTarget();
            else
                _soldierScript.Die();
        }
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
    public Vector2 CorrectionDirection(Transform _tf, Transform _EnemyTf)
    {
        if (_EnemyTf == null) return Vector2.zero;
        int _direction = 1;
        Vector2 _scale = _tf.localScale;
        _scale.x = Mathf.Abs(_scale.x);
        if (_tf.position.x > _EnemyTf.position.x)
        {
            _scale.x *= -1;
            _direction = -1;
        }
        _tf.localScale = _scale;
        return Vector2.right * _direction;
    }
    /// <summary>
    /// 本場遊戲結束
    /// </summary>
    public void GameOver()
    {
        if (_mainFortressScript.Count > 0) return;
        uiScript.GameOverUI();
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
    /// <summary>
    /// 主堡爆了
    /// </summary>
    public void MainFortressOver(MainFortressBaseScript _mfb)
    {
        if (_mfb == null) return;
        RemoveFromList(_mainFortressScriptList, _mfb);
        if (_mfb is MainFortressScript)
            RemoveFromList(_mainFortressScript, _mfb as MainFortressScript);
        else if (_mfb is DarkMainFortressScript)
            RemoveFromList(_darkMainFortressScript, _mfb as DarkMainFortressScript);
    }
    #endregion
}
