using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有角色的清單包括英雄、士兵
/// </summary>
[CreateAssetMenu(fileName = "新角色清單", menuName = "Data Object/New Character List")]
public class CharacterListDataObject : ScriptableObject
{
    /// <summary>
    /// 英雄清單
    /// </summary>
    [Header("英雄清單")]
    public List<HeroScript> HeroList;
    /// <summary>
    /// 士兵清單
    /// </summary>
    [Header("士兵清單")]
    public List<SoldierScript> SoldierList;
}
