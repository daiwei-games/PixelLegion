using Assets.Scripts;
using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵人的英雄產生資料
/// </summary>
public class DarkProduceHeroScript : LeadToSurviveGameBaseClass, IPlayerFunc, IProduceHeroFunc
{
        /// <summary>
    /// 已經選擇的英雄
    /// </summary>
    [Header("已經選擇的英雄")]
    public List<Transform> SelectedHeroList;
    /// <summary>
    /// 英雄清單
    /// </summary>
    [Header("英雄清單"), SerializeField]
    private List<Transform> GetHeroList;
    /// <summary>
    /// 產生間格計時
    /// </summary>
    [Header("產生間隔")]
    public float ProduceHeroTime;
    /// <summary>
    /// 產生間隔
    /// </summary>
    [Range(0, 10), SerializeField]
    private float ProduceHeroTimeMax;
    /// <summary>
    /// 取得敵人圖層
    /// </summary>
    private LayerMask _enemyLayerMask;
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
        _Tf = transform;
        GetHeroList = new List<Transform>();
        GetHeroList.AddRange(SelectedHeroList);
        _gameManager = GameObject.Find("GameManager");
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>();
            _gameManagerScript.DarkHeroScript.Add(this);
        }
        ProduceHeroTime = 0;

        _enemyLayerMask = LayerMask.GetMask(staticPublicObjectsStaticName.HeroLayer,
            staticPublicObjectsStaticName.PlayerSoldierLayer,
            staticPublicObjectsStaticName.MainFortressLayer); //取得敵人圖層

        //ProduceHeroTime = ProduceHeroTimeMax;

    }
    /// <summary>
    /// 創建英雄
    /// </summary>
    /// <param name="_time">計算產生英雄的時間</param>
    public void ProduceHero(float _time)
    {
        if (GetHeroList.Count == 0) return; //如果沒有英雄就不執行
        ProduceHeroTime += _time; //計算時間
        if (ProduceHeroTime < ProduceHeroTimeMax) return; //如果時間沒有到就不執行

        int HeroListIndex = Random.Range(0, GetHeroList.Count); //隨機選擇英雄
        GameObject _hero = Instantiate(GetHeroList[HeroListIndex], _Tf.position, Quaternion.identity,null).gameObject; //產生英雄
        _hero.tag = staticPublicObjectsStaticName.DarkHeroTag; //設定英雄tag
        _hero.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.DarkHeroLayer); //設定英雄圖層

        HeroScript _heroScript = _hero.GetComponent<HeroScript>(); //取得英雄腳本
        _heroScript.enemyLayerMask = _enemyLayerMask; //設定敵人圖層
        _heroScript._gameManagerScript = _gameManagerScript; //設定遊戲管理器

        _gameManagerScript.HeroList.Add(_heroScript); //將英雄加入遊戲管理器

        _heroScript.HeroInitializ(); //初始化英雄

        List<MainFortressScript> _dmfList = new List<MainFortressScript>();
        _dmfList.AddRange(_gameManagerScript._mainFortressScript); //取得所有敵方主堡
        MainFortressScript _dmf;
        Transform _tf;
        Transform _thisTf = _heroScript._Tf;
        float targetDistance = 0;
        float nextDistance = 0;
        int targetIndex = 0;
        for (int i = 0; i < _dmfList.Count; i++)
        {
            _dmf = _dmfList[i];
            _tf = _dmf._Tf;
            if (_dmf == null) continue;
            if (targetDistance != 0 && nextDistance != 0)
            {
                nextDistance = Vector2.Distance(_thisTf.position, _tf.position); //取得距離
                if (targetDistance > nextDistance) //如果距離比較小
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
        _heroScript._target = _dmfList[targetIndex]._Tf; //設定目標

        GetHeroList.RemoveAt(HeroListIndex); //將英雄從清單中移除
        ProduceHeroTime = 0; //重置時間
    }
}
