using System.Collections.Generic;
using UnityEngine;

public class HeroBaseScript : MonoBehaviour
{
    #region 英雄基本資料
    /// <summary>
    /// 英雄資料庫
    /// </summary>
    public heroDataObject _heroDataObject;
    /// <summary>
    /// 英雄ID (系統判斷的ID)
    /// </summary>
    [Header("英雄ID")]
    public string HeroID;
    /// <summary>
    /// 英雄名稱
    /// </summary>
    [Header("英雄名稱")]
    public string HeroName;
    /// <summary>
    /// 英雄血量
    /// </summary>
    [Header("英雄血量")]
    public int Hp;
    /// <summary>
    /// 攻擊力
    /// </summary>
    [Header("攻擊力")]
    public int Attack;
    /// <summary>
    /// 攻擊力抵銷
    /// </summary>
    [Header("攻擊力抵銷")]
    public int Def;
    /// <summary>
    /// 英雄頭像
    /// </summary>
    [Header("英雄頭像")]
    public Sprite HeroAvatar;
    #endregion
    #region 英雄調用腳本
    /// <summary>
    /// GM管理器腳本
    /// </summary>
    [Header("GM管理器腳本")]
    public GameManager _gameManagerScript;
    /// <summary>
    /// 英雄操作介面
    /// </summary>
    [Header("英雄操作介面")]
    public HeroController heroController;
    #endregion
    #region 英雄預先取得資料
    /// <summary>
    /// 取得座標物件
    /// </summary>
    public Transform _transform;
    /// <summary>
    /// 取得物件座標
    /// </summary>
    public Vector2 _tfposition;
    /// <summary>
    /// 剛體
    /// </summary>
    public Rigidbody2D _rg;
    /// <summary>
    /// 取得自己身上的所有碰撞
    /// </summary>
    public Collider2D[] _colliders;
    /// <summary>
    /// 動畫播放組件
    /// </summary>
    [Header("動畫播放組件")]
    public Animator _animator;
    /// <summary>
    /// 取得動畫狀態
    /// </summary>
    public AnimatorStateInfo _animatorStateInfo;
    /// <summary>
    /// 攻擊目標清單
    /// </summary>
    [Header("攻擊目標清單")]
    public List<Transform> enemyTargetList;
    /// <summary>
    /// 攻擊間隔計時
    /// </summary>
    [Header("攻擊計時間隔"),SerializeField]
    protected float playAnimationTimeMax;
    public float playAnimationTime;
    public float _Time;
    #endregion
    #region 射線
    /// <summary>
    /// 英雄判斷敵人的圖層
    /// </summary>
    [Header("英雄判斷敵人的圖層")]
    public LayerMask enemyLayerMask;
    /// <summary>
    /// 射線取得敵人的碰撞
    /// </summary>
    [Header("射線取得敵人的碰撞")]
    public Collider2D[] enemyCollider;
    /// <summary>
    /// 值線射線，用來製作衝刺動作
    /// </summary>
    [Header("值線射線判斷衝刺")]
    public RaycastHit2D[] enemyRayRaycastAll;
    /// <summary>
    /// 衝刺距離
    /// 是射線範為的倍數
    /// </summary>
    [Header("衝刺距離"), Range(0.01f, 5)]
    public float dashDistance;
    /// <summary>
    /// 射線的範圍
    /// </summary>
    [Header("射線的範圍")]
    public float phySize;
    /// <summary>
    /// 射線範圍
    /// </summary>
    protected Vector2 PhySizeVector2;
    /// <summary>
    /// 射線偏移
    /// </summary>
    public float PhyOffset;
    /// <summary>
    /// 地板圖層
    /// </summary>
    public LayerMask flootLayer;
    /// <summary>
    /// 是否碰到地板
    /// </summary>
    public bool isfloot;
    #endregion
}
