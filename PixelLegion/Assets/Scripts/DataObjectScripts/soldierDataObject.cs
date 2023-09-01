using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 士兵的中央資料攻擊、防禦、HP都是以百分比增加
/// </summary>
[CreateAssetMenu(fileName = "New Soldier Data", menuName = "Data Object/Soldier Data")]
public class soldierDataObject : ScriptableObject
{
    /// <summary>
    /// 士兵的等級上限
    /// </summary>
    public const int soldierLvMax = 50000;
    /// <summary>
    /// 士兵的等級
    /// 每增加等級就會增加每項素質 0.02%
    /// </summary>
    [Header("總軍團的等級"),Range(1, soldierLvMax)]
    public int soldierLv;
    /// <summary>
    /// 士兵血量增加百分比
    /// </summary>
    [Header("士兵血量"), Range(.02f, .02f * soldierLvMax)]
    public float soldierHp = .02f;
    /// <summary>
    /// 士兵攻擊力增加百分比
    /// </summary>
    [Header("士兵攻擊力"), Range(.02f, .02f * soldierLvMax)]
    public float soldierAtk = .02f;
    /// <summary>
    /// 士兵防禦力增加百分比
    /// </summary>
    [Header("士兵防禦力"), Range(.02f, .02f * soldierLvMax)]
    public float soldierDefense = .02f;

}
