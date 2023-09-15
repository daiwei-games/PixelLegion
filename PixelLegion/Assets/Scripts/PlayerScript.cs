using Assets.Scripts;
using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IPlayerFunc, IProduceHeroFunc
{
    [Header("玩家資料")]
    public playerDataObject _playerDataObject;
    public Transform _transform;
    /// <summary>
    /// 英雄清單
    /// </summary>
    [Header("英雄清單"),SerializeField]
    private List<Transform> GetHeroList;
    /// <summary>
    /// 產生間格計時
    /// </summary>
    [Header("產生間隔")]
    public float ProduceHeroTime;
    /// <summary>
    /// 產生間隔
    /// </summary>
    [Range(0,10), SerializeField]
    private float ProduceHeroTimeMax;
    /// <summary>
    /// 取得敵人圖層
    /// </summary>
    private LayerMask _enemyLayerMask;
    #region UI
    /// <summary>
    /// 英雄選擇介面
    /// </summary>
    [Header("英雄選擇介面")]
    public UIHeroOptions HeroUI;
    #endregion
    #region 遊戲管理器
    /// <summary>
    /// 管理器物件
    /// </summary>
    [Header("管理器物件")]
    public GameObject _gameManager;
    /// <summary>
    /// 遊戲管理器腳本
    /// </summary>
    [Header("遊戲管理器腳本")]
    public GameManager _gameManagerScript;
    #endregion
    private void Awake()
    {
        PlayerDataInitializ();
    }
    /// <summary>
    /// 初始化腳本
    /// </summary>
    public void PlayerDataInitializ()
    {
        _transform = transform;
        GetHeroList = new List<Transform>();
        GetHeroList.AddRange(_playerDataObject.SelectedHeroList);
        _gameManager = GameObject.Find("GameManager");
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>();
            _gameManagerScript.playerScript = this;
        }

        _enemyLayerMask = LayerMask.GetMask(staticPublicObjectsStaticName.DarkHeroLayer,
            staticPublicObjectsStaticName.DarkSoldierLayer,
            staticPublicObjectsStaticName.DarkMainFortressLayer); //取得敵人圖層

        ProduceHeroTimeMax = _playerDataObject.ProduceHeroTimeMax; //取得產生間隔
        //ProduceHeroTime = ProduceHeroTimeMax;
    }
    /// <summary>
    /// 創建英雄
    /// </summary>
    /// <param name="_time">計算產生英雄的時間</param>
    public void ProduceHero(float _time)
    {
        _transform.position = _gameManagerScript._mainFortressScript[0]._transform.position; //設定玩家位置
        if (GetHeroList.Count == 0) return; //如果沒有英雄就不執行
        ProduceHeroTime += _time; //計算時間
        if (ProduceHeroTime < ProduceHeroTimeMax) return; //如果時間沒有到就不執行
        ProduceHeroTime = 0; //重置時間

        int HeroListIndex = Random.Range(0, GetHeroList.Count); //隨機選擇英雄
        GameObject _hero = Instantiate(GetHeroList[HeroListIndex], _transform).gameObject; //產生英雄
        _hero.tag = staticPublicObjectsStaticName.HeroTag; //設定英雄tag
        _hero.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.HeroLayer); //設定英雄圖層

        HeroScript _heroScript = _hero.GetComponent<HeroScript>(); //取得英雄腳本
        _heroScript.enemyLayerMask = _enemyLayerMask; //設定敵人圖層
        _heroScript._gameManagerScript = _gameManagerScript; //設定遊戲管理器

        _gameManagerScript.HeroList.Add(_heroScript); //將英雄加入遊戲管理器
        HeroUI.ButtonAddEvent(_heroScript); //將英雄加入選擇介面

        _heroScript.HeroInitializ(); //初始化英雄

        List<DarkMainFortressScript> _dmfList = new List<DarkMainFortressScript>();
        _dmfList.AddRange(_gameManagerScript._darkMainFortressScript); //取得所有敵方主堡
        DarkMainFortressScript _dmf;
        Transform _tf;
        Transform _thisTf = _heroScript._transform;
        float targetDistance = 0;
        float nextDistance = 0;
        int targetIndex = 0;
        for (int i = 0; i < _dmfList.Count; i++)
        {
            _dmf = _dmfList[i];
            _tf = _dmf._transform;
            if (_dmf == null) continue;
            _heroScript.enemyTargetList.Add(_tf);
            if (targetDistance != 0 && nextDistance != 0)
            {
                nextDistance = Vector2.Distance(_thisTf.position, _tf.position); //取得距離
                if(targetDistance > nextDistance) //如果距離比較小
                {
                    targetDistance = nextDistance; //設定距離
                    targetIndex = i; //設定目標索引
                }
            }
            else
            {
                targetDistance = Vector2.Distance(_thisTf.position, _tf.position); //取得距離
            }
        }
        _heroScript._target = _dmfList[targetIndex]._transform; //設定目標

        GetHeroList.RemoveAt(HeroListIndex); //將英雄從清單中移除
        
    }





}
