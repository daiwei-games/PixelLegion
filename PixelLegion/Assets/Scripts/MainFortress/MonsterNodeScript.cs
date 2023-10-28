using UnityEngine;
/// <summary>
/// 怪物產生節點
/// </summary>
public class MonsterNodeScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 這張地圖野生士兵的等級
    /// </summary>
    [Header("這張地圖野生士兵的等級")]
    public int MapLv;
    /// <summary>
    /// 遊戲管理器
    /// </summary>
    public GameManager _gameManager;
    /// <summary>
    /// 士兵數量
    /// </summary>
    [Header("士兵數量")]
    public int SoldierCount;
    /// <summary>
    /// 士兵重置時間
    /// </summary>
    [Header("士兵數量重置時間")]
    public float SoldierReTime;
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
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.MonsterNodeDataGet(this);
    }

    public void ProduceSoldier()
    {

    }
}
