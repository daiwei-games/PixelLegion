using UnityEngine;

public class ParticleListManagerScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_01
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_01_110")]
    public ParticleSystem AtfVfx_1;
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_02
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_02_110")]
    public ParticleSystem AtfVfx_2;
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_03
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_03_110")]
    public ParticleSystem AtfVfx_3;
    /// <summary>
    /// 怪物被打擊到的特效 AtfVfx_04
    /// </summary>
    [Header("怪物被打擊到的特效 AtfVfx_04_110")]
    public ParticleSystem AtfVfx_4;

    [Header("報擊傷害打擊特效 CameraShakeHit_01_110")]
    public ParticleSystem CameraShakeHit_1;

    [Header("主爆受傷效果 MainFortress")]
    public ParticleSystem MainFortress_1;
    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;
    }
}
