using UnityEngine;
using TMPro;
using Assets.Scripts;
using System.Collections.Generic;

public class MainFortressScript : LeadToSurviveGameBaseClass
{
    #region 主堡基礎資料
    /// <summary>
    /// 主堡血量
    /// </summary>
    [Header("主堡血量")]
    public int _hp;
    /// <summary>
    /// 最大血量
    /// </summary>
    [Header("最大血量")]
    public int _MaxHp;
    /// <summary>
    /// 主堡血量文字
    /// </summary>
    [Header("主堡血量文字")]
    public TextMeshPro _hpMeshPro;
    /// <summary>
    /// 敵人主堡tag
    /// </summary>
    [Header("敵人主堡tag")]
    public string enemyMainFortressTag;
    /// <summary>
    /// 敵人對玩家的主堡清單
    /// </summary>
    [Header("敵人的主堡清單")]
    public List<Transform> enemyMainFortressList;
    /// <summary>
    /// 取得遊戲管理器腳本
    /// </summary>
    [Header("取得遊戲管理器腳本"), SerializeField]
    protected GameManager _gameManagerScript;

    /// <summary>
    /// 目前剩餘兵數
    /// </summary>
    [Header("目前剩餘兵數")]
    public int _soldierCount;
    /// <summary>
    /// 目前剩餘兵數文字
    /// </summary>
    [Header("目前剩餘兵數文字")]
    public TextMeshPro _soldierCountMeshPro;
    /// <summary>
    /// 已選擇的士兵清單
    /// </summary>
    [Header("目前已選擇的士兵")]
    public List<SoldierScript> selectedSoldierList;
    /// <summary>
    /// 士兵 tag
    /// </summary>
    [Header("士兵 tag")]
    public string soldierTag;
    /// <summary>
    /// 士兵生產時間
    /// </summary>
    [Header("士兵生產時間(秒)")]
    public float soldierProduceTimeMax;
    /// <summary>
    /// 目前士兵生產時間間隔
    /// </summary>
    [Header("目前士兵生產時間間隔(秒)")]
    public float soldierProduceTime;
    /// <summary>
    /// 誰打我變成目標
    /// </summary>
    public List<Transform> WhoHitMeTransform;


    /// <summary>
    /// 目前選擇的英雄
    /// </summary>
    [Header("目前選擇的英雄")]
    public List<HeroScript> selectedHeroList;
    /// <summary>
    /// 英雄清單
    /// </summary>
    [Header("英雄清單")]
    public List<HeroScript> GetHeroList;
    /// <summary>
    /// 產生間格計時
    /// </summary>
    [Header("產生間格計時")]
    public float ProduceHeroTime;
    /// <summary>
    /// 產生間隔
    /// </summary>
    [Header("產生間隔")]
    public float ProduceHeroTimeMax;
    #endregion

    #region UI
    /// <summary>
    /// 英雄選擇介面
    /// </summary>
    [Header("英雄選擇介面")]
    public UIHeroOptions HeroUI;

    #endregion
    private void Start()
    {
        MainFortressDataInitializ();
    }
    public virtual void MainFortressDataInitializ()
    {
        _Tf = transform; // 取得物件transform
        _Go = gameObject; // 取得物件gameobject
        enemyMainFortressTag = staticPublicObjectsStaticName.DarkMainFortressTag; // 取得敵人主堡tag
        soldierTag = staticPublicObjectsStaticName.PlayerSoldierTag; // 取得士兵tag

        _gameManagerScript = FindFirstObjectByType<GameManager>(); // 取得遊戲管理器腳本

        _Go.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.MainFortressLayer); // 設定主堡圖層
        
        TextMeshPro[] TextMeshProArray = _Tf.GetComponentsInChildren<TextMeshPro>(); // 取得所有子物件的TextMeshPro
        for (int i = 0; i < TextMeshProArray.Length; i++)
        {
            switch (TextMeshProArray[i].name)
            {
                case staticPublicObjectsStaticName.MainFortressHpObjectName:
                    _hpMeshPro = TextMeshProArray[i]; // 取得主堡血量文字物件
                    break;
                case staticPublicObjectsStaticName.MainFortressSoldierObjectName:
                    _soldierCountMeshPro = TextMeshProArray[i]; // 取得主堡兵數文字物件
                    break;
            }
        }
        soldierProduceTime = soldierProduceTimeMax;
        ProduceHeroTime = ProduceHeroTimeMax;

        HeroUI = FindFirstObjectByType<UIHeroOptions>(); // 取得英雄選擇介面
        _gameManagerScript.MainFortressDataFormat(this); // 將主堡資料傳給
        GetEnemyMainFortress();
    }



    public virtual void ProduceSoldier()
    {
        if (_soldierCount == 0) return;
        if (selectedSoldierList.Count == 0) return;
        if (_gameManagerScript._soldierList.Count >= 300) return; // 士兵數量超過300不產生
        if (soldierProduceTime >= soldierProduceTimeMax)
        {
            //產生士兵
            Vector3 ParentPosition = _Tf.position; // 取得主堡位置
            ParentPosition.y += 1;
            Vector2 ParentScale;
            Transform InstantiateTransform;
            GameObject _go;
            SoldierScript _soldierScript;
            Transform _enemymf;
            if (enemyMainFortressList.Count > 0)
            {
                for (int i = 0; i < enemyMainFortressList.Count; i++)
                {
                    _enemymf = enemyMainFortressList[i];
                    if (_enemymf == null)
                    {
                        enemyMainFortressList.RemoveAt(i);
                        continue;
                    }
                    if (_enemymf.CompareTag(staticPublicObjectsStaticName.DarkMainFortressTag)) {
                        int u = Random.Range(0, selectedSoldierList.Count);
                        _soldierScript = Instantiate(selectedSoldierList[u], ParentPosition, Quaternion.identity);
                        _go = _soldierScript.gameObject;
                        _go.tag = soldierTag;
                        _go.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.PlayerSoldierLayer);
                        InstantiateTransform = _go.transform;
                        ParentScale = InstantiateTransform.localScale; // 取得士兵的Scale
                        if (_enemymf.position.x < _Tf.position.x) ParentScale.x *= -1; // 判斷敵人主堡的位置，決定士兵的Scale
                        InstantiateTransform.localScale = ParentScale; // 更新士兵的Scale
                        _soldierScript._enemyNowMainFortress = _enemymf;
                        _gameManagerScript.SoldierDataFormat(_soldierScript);


                        if (WhoHitMeTransform.Count > 0) // 如果有被攻擊，就將敵人變成目標
                        {
                            Transform _whotf;
                            for (int whoHitMeIndex = 0; whoHitMeIndex < WhoHitMeTransform.Count; whoHitMeIndex++)
                            {
                                _whotf = WhoHitMeTransform[whoHitMeIndex];
                                if (_whotf == null)
                                {
                                    WhoHitMeTransform.RemoveAt(whoHitMeIndex);
                                    continue;
                                }
                                if (_whotf != null)
                                {
                                    _soldierScript._enemyNowMainFortress = _whotf;
                                    break;
                                }
                            }
                        }
                        _soldierCount -= 1; // 士兵數量減少
                    }
                }
            }
            soldierProduceTime = 0; // 重置士兵產生時間
            MainForTressSoldierCountTextMeshPro(); // 更新主堡兵數文字
        }
    }

    public virtual void MainFortressHit(int hit)
    {
        if (_hp <= 0) return;
        _hp -= hit;
        MainFortressHpTextMeshPro();

        if (_hp <= 0)
        {
            _gameManagerScript.MainFortressOver(this);
            Destroy(_Go, 1);
        }
    }
    /// <summary>
    /// 取得敵人主堡
    /// </summary>
    public virtual void GetEnemyMainFortress()
    {
        GameObject[] enemyMainFortressArray = GameObject.FindGameObjectsWithTag(enemyMainFortressTag); // 取得敵人主堡清單
        if (enemyMainFortressArray.Length > 0)
        {
            for (int i = 0; i < enemyMainFortressArray.Length; i++)
            {
                enemyMainFortressList.Add(enemyMainFortressArray[i].transform);
            }
        }
    }

    /// <summary>
    /// 創建英雄
    /// </summary>
    /// <param name="_time">計算產生英雄的時間</param>
    public virtual void ProduceHero(float _time)
    {
        if (GetHeroList.Count == 0) return; //如果沒有英雄就不執行
        ProduceHeroTime += _time; //計算時間
        if (ProduceHeroTime < ProduceHeroTimeMax) return; //如果時間沒有到就不執行
        ProduceHeroTime = 0; //重置時間

        int HeroListIndex = Random.Range(0, GetHeroList.Count); //隨機選擇英雄
        HeroScript _hero = Instantiate(GetHeroList[HeroListIndex], _Tf.position, Quaternion.identity, null); //產生英雄
        GetHeroList.RemoveAt(HeroListIndex); //將英雄從清單中移除

        _hero.tag = staticPublicObjectsStaticName.HeroTag; //設定英雄tag
        _hero.gameObject.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.HeroLayer); //設定英雄圖層
        HeroUI.ButtonAddEvent(_hero); //將英雄加入選擇介面
        _hero.GetEmenyTarget(_gameManagerScript._MainFortressScriptList);
        _gameManagerScript.HeroDataFormat(_hero); //設定英雄資料
    }



    #region 其他
    /// <summary>
    /// 更新主堡血量文字
    /// </summary>
    public virtual void MainFortressHpTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_hpMeshPro == null) return;
        _hpMeshPro.text = $"HP: {_hp} /{_MaxHp}";
    }
    /// <summary>
    /// 更新主堡兵數文字
    /// </summary>
    public virtual void MainForTressSoldierCountTextMeshPro()
    {
        if (staticPublicGameStopSwitch.mainFortressStop) return;
        if (_soldierCountMeshPro == null) return;
        _soldierCountMeshPro.text = $"{_soldierCount}";
    }
    #endregion
}
