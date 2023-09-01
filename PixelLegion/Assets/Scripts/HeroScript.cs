using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    #region 單一動畫
    /// <summary>
    /// 等待動畫
    /// </summary>
    [Header("等待動畫")]
    public AnimationClip idleAnimationClip;
    /// <summary>
    /// 受傷動畫
    /// </summary>
    [Header("受傷動畫")]
    public AnimationClip hitAnimationClip;
    /// <summary>
    /// 死亡動畫
    /// </summary>
    [Header("死亡動畫")]
    public AnimationClip dieAnimationClip;
    /// <summary>
    /// 翻滾動畫
    /// </summary>
    [Header("翻滾動畫")]
    public AnimationClip threshAnimationClip;
    /// <summary>
    /// 翻滾攻擊動畫
    /// </summary>
    [Header("翻滾攻擊動畫")]
    public AnimationClip threshAtkAnimationClip;
    #region 跳躍動畫組
    /// <summary>
    /// 跳躍動畫
    /// </summary>
    [Header("跳躍動畫")]
    public AnimationClip jumpAnimationClip;
    /// <summary>
    /// 掉落動畫
    /// </summary>
    [Header("掉落動畫")]
    public AnimationClip dropAnimationClip;
    /// <summary>
    /// 著地動畫
    /// </summary>
    [Header("著地動畫")]
    public AnimationClip landAnimationClip;
    #endregion
    #endregion
    #region 多種動畫清單
    /// <summary>
    /// 攻擊動畫
    /// </summary>
    [Header("攻擊動畫")]
    public List<AnimationClip> atkAnimationClip;
    /// <summary>
    /// 跑步動畫
    /// </summary>
    [Header("跑步動畫")]
    public List<AnimationClip> runAnimationClip;
    /// <summary>
    /// 閃避動畫
    /// </summary>
    [Header("翻滾攻擊動畫")]
    public List<AnimationClip> missAnimationClip;
    /// <summary>
    /// 衝刺動畫
    /// </summary>
    [Header("衝刺動畫")]
    public List<AnimationClip> dashAnimationClip;
    /// <summary>
    /// 跳躍攻擊動畫
    /// </summary>
    [Header("跳躍攻擊動畫")]
    public List<AnimationClip> jumpAtkAnimationClip;
    /// <summary>
    /// 防禦動畫
    /// </summary>
    [Header("防禦動畫")]
    public List<AnimationClip> defenseAnimationClip;
    #endregion
}
