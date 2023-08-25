using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1.玩家起始資料
/// 2.讀取資料庫重新附值
/// 3.所選擇的英雄
/// </summary>
[CreateAssetMenu(fileName = "New Player Data", menuName = "Data Object/Player Data")]
public class playerDataObject : ScriptableObject
{
    /// <summary>
    /// 除存資料的金鑰
    /// </summary>
    [Header("玩家金鑰")]
    public string PlayerKey;
    /// <summary>
    /// 玩家名稱
    /// </summary>
    [Header("玩家名稱")]
    public string PlayerName;
    /// <summary>
    /// 已經選擇的英雄
    /// </summary>
    [Header("已經選擇的英雄")]
    public List<Transform> SelectedHeroList;
    ///<summary>
    ///玩家金幣
    ///</summary>
    [Header("玩家金幣")]
    public int PlayerCoin;
    ///<summary>
    ///玩家鑽石
    ///</summary>
    [Header("玩家鑽石")]
    public int PlayerDiamond;
    ///<summary>
    ///玩家體力
    ///</summary>
    [Header("玩家體力")]
    public int PlayerEnergy;
}
