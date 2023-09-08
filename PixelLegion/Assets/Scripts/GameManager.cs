using Assets.Scripts;
using Assets.Scripts.BaseClass;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
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
    /// 產生英雄計算間隔
    /// </summary>
    [Header("產生英雄計算間隔"), SerializeField, Range(0.01f, 10)]
    private float ProduceHeroTimeMax;
    private float ProduceHeroTime;
    /// <summary>
    /// 可以操控的英雄
    /// </summary>
    [Header("可以操控的英雄"), SerializeField]
    private HeroScript SelectedHero;
    /// <summary>
    /// 選擇的英雄頭上出現指標
    /// </summary>
    [Header("選擇的英雄頭上出現指標"), SerializeField]
    private Transform SelectedHeroTargetPrefab;
    private Transform SelectedHeroTarget;
    private void Awake()
    {
        isSoldierStateForAction = true;
        isProduceSoldier = true;
    }
    private void Update()
    {
        if (staticPublicGameStopSwitch.gameStop)
        {
            Time.timeScale = 0;
        }

        GameOver();
        SelectedHeroControl();
    }
    private void FixedUpdate()
    {
        if (staticPublicGameStopSwitch.gameStop) return;
        time = Time.deltaTime;

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
        //管理器區塊
        SoldierState();

        HeroAI();
    }

    #region 英雄相關操作
    /// <summary>
    /// 產生英雄
    /// </summary>
    /// <param name="t"></param>
    private void PlayerProduceHero()
    {
        playerScript.ProduceHero(time);
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
        for (int i = 0; i < HeroList.Count; i++) //所有英雄不可操控
        {
            HeroList[i].isPlayerControl = false;
        }
        _hs.isPlayerControl = true; //選擇的英雄可以操控
        _hs.playAnimationTime += 10;
        _hs._tfposition = _hs._transform.position;
        Transform HeroTf = _hs._transform;
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
    }
    private void HeroAI()
    {
        if (HeroList.Count == 0) return;
        HeroScript _hero;
        HeroDataController _hdc = new HeroDataController();
        for (int i = 0; i < HeroList.Count; i++)
        {
            _hero = HeroList[i];
            _hero._Time = time;
            if (!_hero.isPlayerControl)
            {
                _hdc._hs = _hero;
                _hdc._tf = _hero._transform;
                _hdc._rd = _hero._rg;
                _hdc.Move();
            }
        }
    }

    private void SelectedHeroControl()
    {
        if (SelectedHero == null) return;
        SelectedHero._tfposition = SelectedHero._transform.position; //更新位置
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

        public HeroDataController(HeroScript hs)
        {
            _hs = hs;
            _tf = _hs._transform;
            _rd = _hs._rg;
        }
        public void Phy2D()
        {
            Vector2 pos = _hs._tfposition;
            _hs.PhyOverlapBoxAll(pos);
        }
        public void Move()
        {
            _hs._tfposition = _tf.position;
            #region 重新尋找目標
            if (_hs._target == null)
            {

                List<Transform> _etfList = _hs.enemyTargetList; //取得敵人清單
                Transform _tf; //暫存敵人
                Transform _thisTf = _hs._transform; //暫存自己
                float targetDistance = 0; //目標距離
                float nextDistance = 0; //下一個距離
                int targetIndex = 0; //目標索引
                for (int i = 0; i < _etfList.Count; i++)
                {
                    _tf = _etfList[i];
                    if (_tf == null)
                    {
                        _hs.enemyTargetList.RemoveAt(i);
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
            if (_hs.enemyCollider.Length > 0)
            {
                _hs.Atk();
                return;
            }
            _hs.Move();
            _tf.position = Vector2.MoveTowards(Pos, enemyPos, _hs.speed * Time.deltaTime);

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
            _soldierScript._Time = time;
            isGameOver = _mainFortressScript.Count == 0; // 如果光明主堡清單為0，則遊戲結束
            if (isGameOver) //如果遊戲結束，則全體士兵都進入等待
                _soldierScript.Idle();
            _ssd._soldierScript = _soldierScript;
            _ssd._transform = _soldierScript._transform;
            _ssd._gameObject = _soldierScript._gameObject;
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
        public Transform _transform;
        public GameObject _gameObject;
        public Transform _targetTransform;
        public Rigidbody2D _Rigidbody2;
        public SoldierScript _soldierScript;
        public List<MainFortressBaseScript> enemyTarget;
        public float dT;


        public SoldierStateData(SoldierScript _sscript, float time, List<MainFortressBaseScript> _enemy)
        {
            _transform = _sscript._transform;
            _gameObject = _sscript.gameObject;
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
                    enemyTargetTransform = enemyTarget[i]._transform;
                    if (_targetDistance == 0 && _nextDistance == 0)
                        _targetDistance = Vector3.Distance(_transform.position, enemyTargetTransform.position);
                    else
                    {
                        _nextDistance = Vector3.Distance(_transform.position, enemyTargetTransform.position);
                        if (_targetDistance > _nextDistance)
                        {
                            _targetDistance = _nextDistance;
                            enemyTargetIndex = i;
                        }
                    }
                }
                enemyTargetTransform = enemyTarget[enemyTargetIndex]._transform;
                _soldierScript._enemyNowMainFortress = enemyTargetTransform;
                return;
            }
            Vector2 pos = _transform.position;
            _soldierScript.PhyOverlapBoxAll(pos);
            if (_soldierScript._collider2D.Length > 0)
            {
                _soldierScript.Atk();
                return;
            }
            _soldierScript.Move();
            _transform.position = Vector3.MoveTowards(_transform.position, _targetTransform.position, _soldierScript.speed * dT);
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
    /// 本場遊戲結束
    /// </summary>
    public void GameOver()
    {
        if (_mainFortressScript.Count > 0) return;
        GameOverObject.GameOverUI();
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
