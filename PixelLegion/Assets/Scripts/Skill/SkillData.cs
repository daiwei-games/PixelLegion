using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能相關資料
/// </summary>
[Serializable]
public class SkillData
{
    #region 技能資料
    /// <summary>
    /// 技能ID
    /// </summary>
    [Header("技能ID")]
    public string SkillID;
    /// <summary>
    /// 技能圖示
    /// </summary>
    [Header("技能圖示")]
    public Sprite SkillIcon;
    /// <summary>
    /// 技能名稱
    /// </summary>
    [Header("技能名稱")]
    public string SkillName;
    /// <summary>
    /// 所需經驗值
    /// </summary>
    [Header("所需經驗值")]
    public int SkillExp;
    /// <summary>
    /// 技能預製物件
    /// </summary>
    [Header("技能預製物件")]
    public GameObject SkillGameObject;
    /// <summary>
    /// 技能說明
    /// </summary>
    [Header("技能說明"),TextArea(1,10)]
    public string SkillDescription;
    #endregion

    #region 需要的道具
    /// <summary>
    /// 學習技能所需道具
    /// </summary>
    [Header("學習技能所需道具")]
    public List<GameObject> SkillProps;
    #endregion
}
