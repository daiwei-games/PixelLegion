using UnityEngine;
/// <summary>
/// 1.英雄個別起始資料
/// 2.讀取資料庫將英雄個別附值
/// </summary>
[CreateAssetMenu(fileName = "New Hero Data", menuName = "Data Object/Hero Data")]
public class heroDataObject : ScriptableObject
{
    /// <summary>
    /// 總英雄等級
    /// </summary>
    [Header("玩家英雄等級"),Range(1, PlayerHeroLvMax)]
    public int PlayerHeroLv = 1;
    /// <summary>
    /// 總英雄等級上限
    /// 每增加一級所有屬性增加0.2f
    /// </summary>
    public const int PlayerHeroLvMax = 50000;
    /// <summary>
    /// 總增加的HP百分比
    /// </summary>
    [Header("英雄增加的HP百分比")]
    public float AddHp = 0.2f;
    /// <summary>
    /// 總增加的攻擊力百分比
    /// </summary>
    [Header("英雄增加的攻擊力百分比")]
    public float AddAtk = 0.2f;
    /// <summary>
    /// 總增加的防禦力百分比
    /// </summary>
    [Header("英雄增加的防禦力百分比")]
    public float AddDef = 0.2f;

    /// <summary>
    /// 範圍百分比 當等級越高，這個數值越小，代表範圍越精準
    /// [計算方式為，每增加一等範圍百分比減少0.005]
    /// </summary>
    [Header("影響攻擊、防禦範圍值")]
    public float Percentage = 0.505f;
}
