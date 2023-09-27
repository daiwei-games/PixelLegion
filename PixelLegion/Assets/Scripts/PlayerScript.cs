using UnityEngine;

public class PlayerScript : LeadToSurviveGameBaseClass
{
    
    /// <summary>
    /// 取得遊戲管理器
    /// </summary>
    [HideInInspector]
    public GameManager _GameManager;
    /// <summary>
    /// 英雄選擇介面
    /// </summary>
    [Header("英雄選擇介面"), HideInInspector]
    public UIHeroOptions HeroUI;
    /// <summary>
    /// 這場有多少隻英雄
    /// </summary>
    [Header("這場有多少隻英雄"), HideInInspector]
    public int HeroCount;
    /// <summary>
    /// 玩家總資料庫
    /// </summary>
    [Header("玩家總資料庫")]
    public playerDataObject PlayerDataObject;
    private void OnEnable()
    {
        _Tf = transform;
        _Go = gameObject;
        HeroCount = PlayerDataObject.SelectedHeroList.Count;
        _GameManager = FindObjectOfType<GameManager>();
    }
}
