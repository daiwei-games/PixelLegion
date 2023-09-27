using UnityEngine;

public class ParticleListManagerScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_01
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_01")]
    public ParticleSystem AtfVfx_1;
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_02
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_02")]
    public ParticleSystem AtfVfx_2;
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_03
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_03")]
    public ParticleSystem AtfVfx_3;
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_04
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_04")]
    public ParticleSystem AtfVfx_4;

    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;
    }
}
