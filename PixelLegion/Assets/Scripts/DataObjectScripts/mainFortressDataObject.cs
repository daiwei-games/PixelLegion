using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1.主堡的起始資料
/// 2.讀取資料附值
/// </summary>
[CreateAssetMenu(fileName = "New Main Fortress Data", menuName = "Data Object/Main Fortress Data")]
public class mainFortressDataObject : ScriptableObject
{
    /// <summary>
    /// 最大血量
    /// </summary>
    [Header("最大血量")]
    public int maxhp;
    /// <summary>
    /// 最大士兵人數
    /// </summary>
    [Header("最大士兵人數")]
    public int soldierCount;
    /// <summary>
    /// 已選擇的士兵清單
    /// </summary>
    [Header("已選擇的士兵清單")]
    public List<Transform> soldierSelectedList;
    /// <summary>
    /// 士兵生產時間
    /// </summary>
    [Header("士兵生產時間")]
    public float soldierProduceTimeMax;
    /// <summary>
    /// 目前選擇的英雄
    /// </summary>
    [Header("目前選擇的英雄")]
    public List<Transform> selectedHeroList;
    /// <summary>
    /// 英雄生產時間
    /// 每一位英雄所需要產生的時間加總
    /// </summary>
    [Header("英雄生產時間")]
    public float heroProduceTimeMax;
}
