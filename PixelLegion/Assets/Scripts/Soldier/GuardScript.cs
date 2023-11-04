using UnityEngine;

public class GuardScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 目前是遠攻、近戰守衛或者戰場上的士兵
    /// </summary>
    public SoldierPost _Sp;
    /// <summary>
    /// 新增士兵腳本
    /// </summary>
    public SoldierScript _Ss;

    #region 基礎數值
    /// <summary>
    /// 士兵基本素質
    /// </summary>
    [Header("士兵基本素質 (0.01 - 70)"), Range(0.01f, 20f)]
    public float BasicQuality;
    /// <summary>
    /// 基本體質
    /// </summary>
    [Header("基本體質 (1- 120)"), Range(1, 120)]
    public int BasicConstitution;
    /// <summary>
    /// 基本血量
    /// </summary>
    [Header("基本體質 (100 - 3000)"), Range(100, 3000)]
    public int BasicHp;
    /// <summary>
    /// 士兵的名稱
    /// </summary>
    [Header("士兵的名稱")]
    public string soldierName;
    /// <summary>
    /// 移動速度
    /// </summary>
    [Header("移動速度")]
    public float speed;
    /// <summary>
    /// 裝備所有投擲、遠程武器的腳本
    /// </summary>
    [Header("所有投擲、遠程武器的腳本")]
    public AmmunitionScript _ammunitionScript;
    #endregion

    #region 射線
    /// <summary>
    /// 射線範圍
    /// </summary>
    [Header("射線範圍")]
    public float Physics2DSize;
    /// <summary>
    /// 矩形射線偏移
    /// </summary>
    [Header("矩形射線偏移")]
    public float PhyOffset;
    #endregion

    private void Start()
    {
        _Tf = transform;
        _Go = gameObject;

        _Ss = _Go.AddComponent<SoldierScript>();

        Transform _tf = _Tf.Find("遠程武器預製物集合");
        if(_tf != null)
        {
            _ammunitionScript = _tf.GetComponent<AmmunitionScript>();
            if (_ammunitionScript != null) _Sp = SoldierPost.RemoteAttackGuard;
        }
    }
}
