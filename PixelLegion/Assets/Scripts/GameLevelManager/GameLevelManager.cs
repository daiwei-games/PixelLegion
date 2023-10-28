using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 關卡管理器
/// </summary>
public class GameLevelManager : MonoBehaviour
{
    #region 英雄
    /// <summary>
    /// 已經選擇的英雄
    /// </summary>
    [Header("已經選擇的英雄")]
    public List<HeroScript> SelectedHeroList;
    /// <summary>
    /// 總英雄等級上限
    /// 每增加一級所有屬性增加0.2f
    /// </summary>
    public const int PlayerHeroLvMax = 50000;
    /// <summary>
    /// 總英雄等級
    /// </summary>
    [Header("英雄等級"), Range(1, PlayerHeroLvMax)]
    public int HeroLv;
    /// <summary>
    /// 總增加的HP百分比
    /// </summary>
    [Header("英雄增加的HP百分比"), Range(0.2f, 0.2f * PlayerHeroLvMax)]
    public float AddHp;
    /// <summary>
    /// 總增加的攻擊力百分比
    /// </summary>
    [Header("英雄增加的攻擊力百分比"), Range(0.2f, 0.2f * PlayerHeroLvMax)]
    public float AddAtk;
    /// <summary>
    /// 總增加的防禦力百分比
    /// </summary>
    [Header("英雄增加的防禦力百分比"), Range(0.2f, 0.2f * PlayerHeroLvMax)]
    public float AddDef;
    /// <summary>
    /// 範圍百分比 當等級越高，這個數值越小，代表範圍越精準
    /// [計算方式為，每增加一等範圍百分比減少0.005]
    /// </summary>
    [Header("影響攻擊、防禦範圍值"), Range(0.505f, 0)]
    public float Percentage;
    /// <summary>
    /// 英雄產生間隔
    /// </summary>
    [Header("英雄產生間隔 (1 - 10)"), Range(1, 10)]
    public float ProduceHeroTimeMax;
    #endregion


    #region 士兵
    /// <summary>
    /// 已選擇的士兵清單
    /// </summary>
    [Header("已選擇的士兵清單")]
    public List<SoldierScript> soldierSelectedList;
    /// <summary>
    /// 士兵的等級上限
    /// </summary>
    public const int soldierLvMax = 50000;
    /// <summary>
    /// 士兵的等級
    /// 每增加等級就會增加每項素質 0.02%
    /// </summary>
    [Header("總士兵等級"), Range(1, soldierLvMax)]
    public int soldierLv;
    /// <summary>
    /// 士兵血量增加百分比
    /// </summary>
    [Header("士兵血量"), Range(.02f, .02f * soldierLvMax)]
    public float soldierHp;
    /// <summary>
    /// 士兵攻擊力增加百分比
    /// </summary>
    [Header("士兵攻擊力"), Range(.02f, .02f * soldierLvMax)]
    public float soldierAtk;
    /// <summary>
    /// 士兵防禦力增加百分比
    /// </summary>
    [Header("士兵防禦力"), Range(.02f, .02f * soldierLvMax)]
    public float soldierDefense;
    #endregion


    #region 主堡
    /// <summary>
    /// 最大血量
    /// </summary>
    [Header("最大血量 (1 - 100000)"), Range(1, 100000)]
    public int maxhp;
    /// <summary>
    /// 最大士兵人數
    /// </summary>
    [Header("最大士兵人數 (1 - 200)"), Range(1, 200)]
    public int soldierCount;
    /// <summary>
    /// 士兵生產時間
    /// </summary>
    [Header("士兵生產時間 (1 - 10)"), Range(1, 10)]
    public float soldierProduceTimeMax;
    #endregion
}
