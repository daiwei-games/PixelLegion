using System;
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
    public DateTime SoldierReTime;
    private void OnEnable()
    {
        _Tf = transform;
        _Go = gameObject;

        _gameManager = FindObjectOfType<GameManager>();

        SoldierReTime = DateTime.Now;

        if (MapLv < 1) MapLv = 1;
        Debug.Log("士兵數量重置時間:" + SoldierReTime);
    }
}
