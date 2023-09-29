using System.Collections;
using UnityEngine;
/// <summary>
/// 音效清單管理
/// </summary>
public class SFXListScript : MonoBehaviour
{
    #region 英雄使用音效
    /// <summary>
    /// 英雄拔刀音效 - 刀鞘.ogg
    /// </summary>
    [Header("英雄拔刀音效 - 刀鞘.ogg")]
    public AudioClip Arms01;
    /// <summary>
    /// 劍揮空
    /// </summary>
    [Header("劍揮空 - 劍揮空.ogg")]
    public AudioClip Arms02;
    /// <summary>
    /// 劍揮空 - 大劍揮空-快1.mp3
    /// </summary>
    [Header("劍揮空 - 大劍揮空-快1.mp3")]
    public AudioClip Arms03;
    /// <summary>
    /// 劍揮空 - 大劍揮空-快2.mp3
    /// </summary>
    [Header("劍揮空 - 大劍揮空-快2.wav")]
    public AudioClip Arms04;
    /// <summary>
    /// 劍揮空 - 大劍揮空-快3.mp3
    /// </summary>
    [Header("劍揮空 - 大劍揮空-快3.mp3")]
    public AudioClip Arms05;
    /// <summary>
    /// 受傷毆打1 - 毆打1.ogg
    /// </summary>
    [Header("受傷毆打1 - 毆打1.ogg")]
    public AudioClip HeroHit01;
    /// <summary>
    /// 受傷毆打2 - 毆打2.ogg
    /// </summary>
    [Header("受傷毆打2 - 毆打2.ogg")]
    public AudioClip HeroHit02;
    /// <summary>
    /// 受傷砍傷1 - 打擊1.ogg
    /// </summary>
    [Header("受傷砍傷1 - 打擊1.ogg")]
    public AudioClip HeroHit03;
    /// <summary>
    /// 受傷砍傷2 - 打擊2.mp3
    /// </summary>
    [Header("受傷砍傷2 - 打擊2.mp3")]
    public AudioClip HeroHit04;
    /// <summary>
    /// 隔擋1 - 隔擋1.wav
    /// </summary>
    [Header("隔擋1 - 隔擋1.wav")]
    public AudioClip Def01;
    #endregion
    #region 士兵使用音效
    /// <summary>
    /// 士兵受傷音效 - 打擊8.ogg
    /// </summary>
    [Header("士兵受傷音效 - 打擊1.ogg")]
    public AudioClip SoldierHit01;
    #endregion

    #region 共用
    /// <summary>
    /// 爆擊 - 重擊.mp3
    /// </summary>
    [Header("爆擊 - 重擊.mp3")]
    public AudioClip CriticalStrike;
    #endregion

    #region AudioSource 設定管理
    [Header("AudioSource 設定管理")]
    public float MaxDistance;
    public float MinDistance;
    public float SpatialBlend;
    public AudioRolloffMode RolloffMode;
    #endregion
}
