using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1.英雄個別起始資料
/// 2.讀取資料庫將英雄個別附值
/// </summary>
[CreateAssetMenu(fileName = "New Hero Data", menuName = "Data Object/Hero Data")]
public class heroDataObject : ScriptableObject
{
    /// <summary>
    /// 生產時間
    /// </summary>
    [Header("生產時間")]
    public float produceTime;
    /// <summary>
    /// 英雄預制物
    /// </summary>
    [Header("英雄預制物")]
    public Transform heroPrefab;
    /// <summary>
    /// 英雄名稱
    /// </summary>
    [Header("英雄名稱")]
    public string heroName;
}
