using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有技能清單已擁有的技能清單
/// </summary>
[CreateAssetMenu(fileName = "New Skill Data", menuName = "Data Object/Skill List Data")]
public class SkillListDataObject : ScriptableObject
{
    /// <summary>
    /// 所有技能清單
    /// </summary>
    [Header("所有技能清單")]
    public List<SkillData> SkillList;
    /// <summary>
    /// 已經擁有的技能清單
    /// </summary>
    [Header("已經擁有的技能清單")]
    public List<SkillData> AlreadyHaveSkillList;
}
