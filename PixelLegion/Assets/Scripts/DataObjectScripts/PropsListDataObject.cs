using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Props Data", menuName = "Data Object/Props Data")]
public class PropsListDataObject : ScriptableObject
{
    #region 道具清單
    /// <summary>
    /// 所有道具清單
    /// </summary>
    [Header("所有道具清單")]
    public List<PropsDataScript> AllProps;
    /// <summary>
    /// 已經擁有的道具清單 (背包)
    /// </summary>
    [Header("已經擁有的道具清單 (背包)")]
    public List<PropsBackpackDataScript> AlreadyHaveProps;

    /// <summary>
    /// 已知的道具圖鑑
    /// </summary>
    [Header("已知的道具圖鑑")]
    public List<PropsDataScript> AlreadyKnowProps;

    /// <summary>
    /// 金錢
    /// </summary>
    [Header("金錢")]
    public int Money;
    /// <summary>
    /// 技能經驗值
    /// </summary>
    [Header("已經獲得的技能經驗值")]
    public int AlreadyHaveSkillExp;
    #endregion
}
