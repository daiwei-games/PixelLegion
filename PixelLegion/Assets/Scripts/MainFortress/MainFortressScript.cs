using UnityEngine;
using TMPro;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.MemoryProfiler;

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
    public List<Collider2D> WhoHitMeTransform;


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

    #region 射線
    public float Physics2DSize;
    public Vector2 PhySize;
    #endregion

    #region 音效
    /// <summary>
    /// 音效清單管理
    /// </summary>
    protected SFXListScript _ScriptList;
    /// <summary>
    /// 被打音效
    /// </summary>
    [Header("被打音效")]
    public AudioSource _AudioSourceHit;
    #endregion

    #region 狀態判斷
    /// <summary>
    /// 是否受到攻擊
    /// </summary>
    [HideInInspector]
    public bool isHit;
    /// <summary>
    /// 晃動左邊或右邊
    /// </summary>
    protected bool LeftOrRight;
    #endregion

    #region 效果、特別組件
    /// <summary>
    /// 受傷效果
    /// </summary>
    public Queue<MfHitVfxScript> MfHitVFX;
    /// <summary>
    /// SpriteRenderer組件
    /// </summary>
    [Header("SpriteRenderer組件")]
    public SpriteRenderer MfSpriteRenderer;
    #endregion

    #region 時間計算
    /// <summary>
    ///受傷效果時間
    /// </summary>
    [Header("受傷效果時間"), HideInInspector]
    public float HitTimeMax;
    /// <summary>
    /// 受傷效果時間
    /// </summary>
    public float HitTime;
    /// <summary>
    /// 主堡原本的座標
    /// </summary>
    protected Vector3 MfPos;
    /// <summary>
    /// 主堡晃動座標
    /// </summary>
    protected Vector3 MfHitPos;
    #endregion
    private void Start()
    {
        MainFortressDataInitializ();
    }
    /// <summary>
    /// 主堡資料初始化
    /// </summary>
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
        _gameManagerScript.MainFortressDataFormat(this); // 將主堡資料傳給
        GetEnemyMainFortress();

        _AudioSourceHit = _Go.AddComponent<AudioSource>(); // 添加音效撥放器

        MfSpriteRenderer = GetComponent<SpriteRenderer>(); // 取得SpriteRenderer組件
        HitTimeMax = .3f; // 設定受傷效果時間

        MfHitVFX = new Queue<MfHitVfxScript>(); // 建立受傷效果物件佇列
        GameObject _go;
        Transform _tf;
        SpriteRenderer _sr;
        MfHitVfxScript _mhvs;
        for (int i = 0; i < 5; i++)
        {
            LeftOrRight = !LeftOrRight;
            _go = new GameObject("MfHitGo");
            _sr = _go.AddComponent<SpriteRenderer>();
            _sr.sprite = MfSpriteRenderer.sprite;
            if(LeftOrRight)
                //_sr.color = new Color(0.25f, 0, 0.53f, .7f);
                _sr.color = new Color(0, 0.9f, 1, .7f);
            else
                _sr.color = new Color(1, 0, 0.02f, .7f);
            _sr.sortingOrder = MfSpriteRenderer.sortingOrder - 1;
            _mhvs = _go.AddComponent<MfHitVfxScript>();
            _mhvs._Mfs = this;
            _mhvs.LeftOrRight = LeftOrRight;
            _tf = _go.transform;
            _tf.parent = _Tf;
            _tf.localPosition = Vector3.zero;
            _tf.localScale = Vector3.one;
            MfHitVFX.Enqueue(_mhvs);

            _go.SetActive(false);
        }

        MfPos = _Tf.position;
        MfHitPos = MfPos;
    }


    /// <summary>
    /// 產生士兵
    /// </summary>
    public virtual void ProduceSoldier()
    {
        if (_soldierCount == 0) return;
        if (selectedSoldierList.Count == 0) return;
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
                    if (_enemymf.CompareTag(staticPublicObjectsStaticName.DarkMainFortressTag))
                    {
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
                            Collider2D _Col2D;
                            for (int whoHitMeIndex = 0; whoHitMeIndex < WhoHitMeTransform.Count; whoHitMeIndex++)
                            {
                                _Col2D = WhoHitMeTransform[whoHitMeIndex];
                                if (_Col2D == null)
                                {
                                    WhoHitMeTransform.RemoveAt(whoHitMeIndex);
                                    continue;
                                }
                                _whotf = _Col2D.transform;
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
    /// <summary>
    /// 主堡受傷
    /// </summary>
    /// <param name="hit">傷害指數</param>
    public virtual void MainFortressHit(int hit)
    {
        if (_hp <= 0) return;
        _hp -= hit;
        _gameManagerScript.CastleUnderAttack(_Tf, WhoHitMeTransform);
        MainFortressHpTextMeshPro();
        HitPlaySFX(1);
        HitTime = HitTimeMax;
        if (_hp <= 0)
        {
            _gameManagerScript.MainFortressOver(this);
            Destroy(_Go, 1);
        }
    }
    /// <summary>
    /// 受傷特效
    /// </summary>
    public virtual void MainFortressHitVFX(float _deltatime, float _time)
    {

        if (HitTime > 0)
        {
            HitTime -= _time;
            foreach (var mfhit in MfHitVFX)
            {
                mfhit.gameObject.SetActive(true);
                mfhit.OpenTimeStart = _time + HitTimeMax;
            }
            MfHitPos.x -= Mathf.PingPong(HitTime, .1f);
            MfHitPos.y += Mathf.PingPong(HitTime, .1f);
            _Tf.position = MfHitPos;
        }
        else
        {
            MfHitPos = MfPos;
            _Tf.position = MfPos;
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
    /// <summary>
    /// 村莊產生英雄
    /// </summary>
    /// <param name="_hs"></param>
    public virtual void ProduceHero(HeroScript _hs)
    {

    }

    #region 其他

    /// <summary>
    /// 音效組件基礎設定
    /// </summary>
    /// <param name="AudioSourceType">音效組件模式判斷</param>
    /// <returns></returns>
    protected virtual bool PlaySFXBaseFunc(string AudioSourceType = "")
    {
        if (_ScriptList == null) _ScriptList = _gameManagerScript.SFXList;
        if (AudioSourceType == "hit")
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
    public virtual void HitPlaySFX(int SFXIndex)
    {
        if (PlaySFXBaseFunc("hit")) return;
        AudioClip Ac = null;
        switch (SFXIndex)
        {
            case 1:
                Ac = _gameManagerScript.SFXList.MfHit01;
                break;
        }

        if (Ac == null) return;

        _AudioSourceHit.clip = Ac;
        _AudioSourceHit.Play();
    }

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
    /// <summary>
    /// 主堡判斷射線
    /// </summary>
    /// <param name="_layermask">判斷圖層</param>
    public virtual void PhyOverlapBoxAll(LayerMask _layermask)
    {

        PhySize = Vector2.one * Physics2DSize;
        PhySize.y *= 2;
        Collider2D[] _collider2D = Physics2D.OverlapBoxAll(_Tf.position, PhySize, 0, _layermask);
        if (_collider2D.Length > 0)
        {
            WhoHitMeTransform = _collider2D.ToList();
        }
    }
    void OnDrawGizmos()
    {
        if (_Tf == null) return;
        PhySize = Vector2.one * Physics2DSize;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_Tf.position, PhySize);
    }

    #endregion

}
