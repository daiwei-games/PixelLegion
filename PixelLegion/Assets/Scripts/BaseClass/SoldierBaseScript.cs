using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBaseScript : MonoBehaviour, ISoldierFunc
{
    #region 基本資料
    /// <summary>
    /// 士兵的資料庫
    /// </summary>
    [Header("士兵的資料庫")]
    public soldierDataObject _soldierDataObject;
    public Transform _transform;
    public GameObject _gameObject;
    public Rigidbody2D _body2D;
    public SoldierScript _soldierScript;
    /// <summary>
    /// 士兵的名稱
    /// </summary>
    [Header("士兵的名稱")]
    public string soldierName;
    /// <summary>
    /// 士兵的攻擊力
    /// </summary>
    [Header("士兵的攻擊力")]
    public int soldierAtk;
    /// <summary>
    /// 士兵防禦力
    /// </summary>
    [Header("士兵防禦力")]
    public int soldierDefense;
    /// <summary>
    /// 士兵血量
    /// </summary>
    [Header("士兵血量")]
    public int soldierHp;
    /// <summary>
    /// 移動速度
    /// </summary>
    [Header("移動速度")]
    public float speed;
    /// <summary>
    /// 士兵等級
    /// </summary>
    [Header("士兵等級"), Range(1, 50)]
    public int soldierLv;
    /// <summary>
    /// 士兵等級上限
    /// </summary>
    [Header("士兵等級上限"), Range(1,50)]
    public int soldierLvMax;
    /// <summary>
    /// 動畫字首
    /// </summary>
    [Header("動畫字首")]
    public string animPrefix;
    /// <summary>
    /// 取得 GameManager 腳本
    /// </summary>
    public GameManager _gameManagerScript;
    #endregion
    #region 射線
    /// <summary>
    /// 敵人檢測射線
    /// </summary>
    [Header("敵人檢測射線")]
    public Collider2D[] _collider2D;
    /// <summary>
    /// 射線範圍
    /// </summary>
    [Header("射線範圍")]
    public float Physics2DSize;
    /// <summary>
    /// 敵人士兵的圖層
    /// </summary>
    [Header("敵人士兵的圖層")]
    public int _enemySoldierLayer;
    /// <summary>
    /// 敵人士兵的圖層
    /// </summary>
    [Header("敵人主堡的圖層")]
    public int _enemyMainFortressLayer;
    /// <summary>
    /// 敵人士兵的圖層
    /// </summary>
    [Header("敵人英雄的圖層")]
    public int _enemyHeroLayer;
    /// <summary>
    /// 取得攻擊目標的圖層
    /// </summary>
    public LayerMask _enemyLayerMask;
    /// <summary>
    /// 目前敵人的主堡
    /// </summary>
    [Header("目前敵人的主堡")]
    public Transform _enemyNowMainFortress;
    #endregion
    #region 動畫控制
    /// <summary>
    /// 動畫控制器
    /// </summary>
    [Header("動畫控制器")]
    public Animator _animator;
    /// <summary>
    /// 動畫資訊
    /// </summary>
    [Header("動畫資訊")]
    public AnimatorStateInfo _animationInfo;
    /// <summary>
    /// 所有動畫
    /// </summary>
    [Header("所有動畫")]
    public List<AnimationClip> _animationClip;
    /// <summary>
    /// 動畫間隔
    /// </summary>
    [Header("動畫間隔")]
    public float animationTime;
    /// <summary>
    /// 動畫間隔限制
    /// </summary>
    [Header("動畫間隔限制")]
    public float animationTimerMax;
    #region 單一動畫
    /// <summary>
    /// 等待動畫
    /// </summary>
    [Header("等待動畫")]
    public string idleAnimationName;
    /// <summary>
    /// 受傷動畫
    /// </summary>
    [Header("受傷動畫")]
    public string hitAnimationName;
    /// <summary>
    /// 死亡動畫
    /// </summary>
    [Header("死亡動畫")]
    public string dieAnimationName;
    #endregion
    #region 多種動畫清單
    /// <summary>
    /// 攻擊動畫
    /// </summary>
    [Header("攻擊動畫")]
    public List<string> atkAnimationName;
    /// <summary>
    /// 跑步動畫
    /// </summary>
    [Header("跑步動畫")]
    public List<string> runAnimationName;

    /// <summary>
    /// 衝刺動畫
    /// </summary>
    [Header("衝刺動畫")]
    public List<string> dashAnimationName;
    #endregion
    /// <summary>
    /// 目前狀態
    /// </summary>
    [Header("目前狀態")]
    public SoldierState _soldierNowState;
    /// <summary>
    /// 想切換的狀態
    /// </summary>
    [Header("想切換的狀態")]
    public SoldierState _soldierChangeState;
    #endregion


    /// <summary>
    /// 士兵的資料初始化
    /// </summary>
    public virtual void SoldierDataInitializ()
    {

    }
    public virtual void GetAllAnimationClipName()
    {

    }
    public virtual void SoldierStateAI()
    {

    }
    public virtual void PhyOverlapBoxAll(Vector2 Pos, float PosSize)
    {

    }

    public virtual void Atk()
    {
        
    }

    public virtual void Die()
    {
        
    }

    public virtual void Hit()
    {
        
    }
    public virtual void SoldierHP(int hitAmount)
    {

    }
    public virtual void Idle()
    {
        
    }

    public virtual void Move()
    {
        
    }


    public enum SoldierState
    {
        /// <summary>
        /// 等待
        /// </summary>
        Idle,
        /// <summary>
        /// 移動
        /// </summary>
        Move,
        /// <summary>
        /// 攻擊
        /// </summary>
        Atk,
        /// <summary>
        /// 受傷
        /// </summary>
        Hit,
        /// <summary>
        /// 死亡
        /// </summary>
        Die
    }
}
