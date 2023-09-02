using Assets.Scripts;
using Assets.Scripts.BaseClass;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

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
    private void Awake()
    {
        isSoldierStateForAction = true;
        isProduceSoldier = true;
    }
    private void Start()
    {

    }
    private void Update()
    {
        if (staticPublicGameStopSwitch.gameStop)
        {
            Time.timeScale = 0;
        }

        GameOver();
    }
    private void FixedUpdate()
    {
        if (staticPublicGameStopSwitch.gameStop) return;
        time = Time.deltaTime;

        ProduceSoldierTime += time;
        if (ProduceSoldierTime >= ProduceSoldierTimeMax)
        {
            ProduceSoldierTime = 0;
            //_mainFortressScript.ProduceSoldier(); //玩家士兵生產
            //_darkMainFortressScript.ProduceSoldier(); //敵人士兵生產
            MainFortressBaseProduceSoldier(); //兩邊士兵生產


        }
        //管理器區塊
        SoldierState();


    }
    /// <summary>
    /// 兩邊士兵生產
    /// </summary>
    private void MainFortressBaseProduceSoldier()
    {
        if (!isProduceSoldier) return;
        for (int i = 0; i < _mainFortressScriptList.Count; i++)
        {
            isProduceSoldier = false;
            if (_mainFortressScriptList[i] != null)
                _mainFortressScriptList[i].ProduceSoldier();
        }
        isProduceSoldier = true;
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
        if (index != -1)
        {
            list.RemoveAt(index);
        }
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
    public void GameOver()
    {
        if (_mainFortressScript.Count > 0) return;
        GameOverObject.GameOverUI();
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
        for (int i = 0; i < _soldierList.Count; i++)
        {
            isSoldierStateForAction = false;
            isGameOver = _mainFortressScript.Count == 0; // 如果光明主堡清單為0，則遊戲結束
            if (isGameOver) //如果遊戲結束，則全體士兵都進入等待
                _soldierList[i].Idle();
            _ssd._soldierScript = _soldierList[i];
            _ssd._transform = _soldierList[i]._transform;
            _ssd._gameObject = _soldierList[i]._gameObject;
            _ssd._targetTransform = _soldierList[i]._enemyNowMainFortress;
            _ssd._Rigidbody2 = _soldierList[i]._body2D;
            _ssd.dT = time;
            _ssd.enemyTarget = _mainFortressScriptList;
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
    public struct SoldierStateData
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
        /// 士兵的射線
        /// </summary>
        public void SoldierTarget()
        {
            if (_targetTransform == null)
            {
                _soldierScript.Idle();
                for (int i = 0; i < enemyTarget.Count; i++)
                {
                    switch (_soldierScript.tag)
                    {
                        case staticPublicObjectsStaticName.PlayerSoldierTag:
                            if (enemyTarget[i] is DarkMainFortressScript)
                                _soldierScript._enemyNowMainFortress = enemyTarget[i]._transform;
                            break;
                        case staticPublicObjectsStaticName.DARKSoldierTag:
                            if (enemyTarget[i] is MainFortressScript)
                                _soldierScript._enemyNowMainFortress = enemyTarget[i]._transform;
                            break;
                    }
                    if(_soldierScript._enemyNowMainFortress != null) return;
                }
                return;
            }
            _soldierScript.PhyOverlapBoxAll(_transform.position, _soldierScript.Physics2DSize);
            if (_soldierScript._collider2D.Length > 0)
            {
                _soldierScript.Atk();
                return;
            }
            _soldierScript.Move();
            _transform.position = Vector3.MoveTowards(_transform.position, _targetTransform.position, _soldierScript.speed * dT);
        }
        public void isMoveOrDie()
        {
            _soldierScript.animationTime += dT;
            if (_soldierScript.soldierHp > 0)
            {
                SoldierTarget();
            }
            else
            {
                _soldierScript.Die();
            }
        }
    }
}
