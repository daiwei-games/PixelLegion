using Assets.Scripts;
using UnityEngine;
/// <summary>
/// 怪物產生節點
/// </summary>
public class MonsterNodeScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 光明還是黑暗
    /// </summary>
    [Header("光明還是黑暗")]
    public PromisingOrDARK _pod;
    /// <summary>
    /// 職責 用在這裡主要是用來判斷產生守衛還是士兵、野生怪物
    /// </summary>
    [Header("職責")]
    public SoldierPost _Sp;
    /// <summary>
    /// 遊戲管理器
    /// </summary>
    [HideInInspector]
    public GameManager _gameManager;
    /// <summary>
    /// 英雄、士兵、怪物總資料庫
    /// </summary>
    [Header("英雄、士兵、怪物總資料庫"), SerializeField]
    protected CharacterListDataObject _cldo;
    /// <summary>
    /// 野外士兵、英雄、妖怪資料庫
    /// </summary>
    public GameLevelManager _Glm;
    /// <summary>
    /// 士兵數量
    /// </summary>
    [Header("士兵數量 / 數量上限")]
    public int SoldierCount;
    public int SoldierCountMax;
    /// <summary>
    /// 士兵重置時間
    /// </summary>
    [Header("士兵數量重置時間")]
    public float SoldierReTime;

    public string _Tag;


    private void OnEnable()
    {
        WildInitializ();
    }
    /// <summary>
    /// 野生節點初始化
    /// </summary>
    public void WildInitializ()
    {
        _Tf = transform;
        _Go = gameObject;
        _Go.tag = staticPublicObjectsStaticName.wildNodeTag;

        SoldierReTime = Random.Range(100, 1000); // 設定節點重置時間
        _Glm = FindObjectOfType<GameLevelManager>();
        if(_Glm != null)
        {
            if (_Glm.soldierLv < 1) _Glm.soldierLv = 1;
            SoldierCountMax = _Glm.soldierLv * Random.Range(1, 11); // 設定士兵最大數量
        }
        if (SoldierCountMax > 100) SoldierCountMax = 100; // 如果士兵最大數量大於100，就設定為100
        SoldierCount = SoldierCountMax; // 設定士兵數量

        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.MonsterNodeDataGet(this);

        _Tag = staticPublicObjectsStaticName.WildSoldierTag;

    }
    /// <summary>
    /// 產生野生怪獸、士兵
    /// 取得玩家圖層，判斷到玩家或是光明所有圖層都會進行攻擊
    /// </summary>
    /// <param name="_PlayLayerMask">玩家的圖層</param>
    public void ProduceWildSoldier(LayerMask _PlayLayerMask)
    {
        if (SoldierCount <= 0) return;
        Vector3 _pos = _Tf.position;
        SoldierScript _ss;
        while (SoldierCount > 0) {
            _pos.x = Random.Range(_pos.x - 2, _pos.x + 2);
            _ss = Instantiate(_cldo.WildSoldierList[Random.Range(0, _cldo.WildSoldierList.Count)], _pos, Quaternion.identity, null);
            _ss._gameManagerScript = _gameManager;
            _ss._enemyLayerMask = _PlayLayerMask;
            _ss._Sp = _Sp;
            GameObject _go = _ss.gameObject;
            _go.tag = _Tag;
            _go.layer = LayerMask.NameToLayer(staticPublicObjectsStaticName.WildSoldierLayer);
            _gameManager.SoldierDataFormat(_ss, false);
            SoldierCount -= 1;
        }
    }
}
