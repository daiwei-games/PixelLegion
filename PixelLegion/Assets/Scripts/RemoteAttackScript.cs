using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 遠程攻擊腳本
/// </summary>
public class RemoteAttackScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 遊戲管理器
    /// </summary>
    protected GameManager _gameManager;

    #region 子彈或是箭矢
    /// <summary>
    /// 裝備所有投擲、遠程武器的腳本
    /// </summary>
    [Header("所有投擲、遠程武器的腳本")]
    public AmmunitionScript _ammunitionScript;
    /// <summary>
    /// Y軸偏移
    /// </summary>
    [Header("Y軸偏移")]
    public float OffsetY;
    /// <summary>
    /// 子彈、箭矢一次產生的數量
    /// </summary>
    [Header("子彈、箭矢一次產生的數量"), SerializeField]
    protected int Quantity;
    /// <summary>
    /// 子彈或是箭矢
    /// </summary>
    List<Transform> BulletsOrArrows;
    /// <summary>
    /// 座標位置
    /// </summary>
    Vector3 Pos;
    #endregion
    private void Start()
    {
        _Tf = transform;
        _Go = gameObject;

        Transform _tf = _Tf.Find("遠程武器預製物集合");
        if(_tf != null)
            _ammunitionScript = _tf.GetComponent<AmmunitionScript>();

        _gameManager = FindObjectOfType<GameManager>();

        Pos = _Tf.position;
        Pos.y -= OffsetY; // Y軸偏移
        BulletsOrArrows = new List<Transform>();
        if (Quantity < 5) Quantity = 5;

        Replenish();
    }

    public virtual void Replenish()
    {
        Transform _tf;
        GameObject _go;
        ParabolaScript _parabolaScript;
        for (int i = 0; i < Quantity; i++)
        {
            _tf = Instantiate(_ammunitionScript.ArrowPrefab, Pos, Quaternion.identity, _Tf);
            _go = _tf.gameObject;
            _parabolaScript = _tf.GetComponent<ParabolaScript>();
            _parabolaScript.startPoint = _Tf;
            BulletsOrArrows.Add(_tf);

            _go.SetActive(false);
        }
    }
}
