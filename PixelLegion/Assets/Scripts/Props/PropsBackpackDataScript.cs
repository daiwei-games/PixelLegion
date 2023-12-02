using System;
using UnityEngine;
/// <summary>
/// 背包裡面的物品資料
/// </summary>
[Serializable]
public class PropsBackpackDataScript
{
    /// <summary>
    /// 道具預製物
    /// </summary>
    [Header("道具預製物")]
    public PropsDataScript PropsData;
    /// <summary>
    /// 道具數量
    /// </summary>
    [Header("道具數量")]
    public int PropsCount;
}
