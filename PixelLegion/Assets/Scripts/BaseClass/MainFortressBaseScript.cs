using Assets.Scripts.IFace;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.BaseClass
{
    public class MainFortressBaseScript:MonoBehaviour, IMainFortressFunc
    {
        #region 基礎資料
        public bool gameStop;
        /// <summary>
        /// 取得 Transform
        /// </summary>
        [Header("取得 Transform"), SerializeField]
        public Transform _Tf;
        /// <summary>
        /// 取得 GameObject
        /// </summary>
        [Header("取得 GameObject"), SerializeField]
        public GameObject _Go;
        /// <summary>
        /// 取得遊戲管理器
        /// </summary>
        [Header("取得遊戲管理器"), SerializeField]
        protected GameObject _gameManager;
        /// <summary>
        /// 取得遊戲管理器腳本
        /// </summary>
        [Header("取得遊戲管理器腳本"), SerializeField]
        protected GameManager _gameManagerScript;
        #region  主堡資料
        /// <summary>
        /// 主堡血量
        /// </summary>
        [Header("主堡血量")]
        public int _hp;
        /// <summary>
        /// 主堡血量文字
        /// </summary>
        [Header("主堡血量文字")]
        public TextMeshPro _hpMeshPro;
        /// <summary>
        /// 敵人主堡tag
        /// </summary>
        [Header("敵人主堡tag")]
        public string enemyMainFortressTag;
        /// <summary>
        /// 敵人對玩家的主堡清單
        /// </summary>
        [Header("敵人的主堡清單")]
        public List<Transform> enemyMainFortressList;
        /// <summary>
        /// 主堡自身的圖層
        /// </summary>
        [Header("主堡自身的圖層")]
        public LayerMask _layerMask;
        /// <summary>
        /// 主堡敵人的圖層
        /// </summary>
        [Header("主堡敵人的圖層")]
        public LayerMask _mfEnemyLayerMask;
        #endregion
        /// <summary>
        /// 目前剩餘兵數
        /// </summary>
        [Header("目前剩餘兵數")]
        public int _soldierCount;
        /// <summary>
        /// 目前剩餘兵數文字
        /// </summary>
        [Header("目前剩餘兵數文字")]
        public TextMeshPro _soldierCountMeshPro;
        /// <summary>
        /// 目前選擇的英雄
        /// </summary>
        [Header("目前選擇的英雄")]
        public List<Transform> selectedHeroList;
        /// <summary>
        /// 已選擇的士兵清單
        /// </summary>
        [Header("目前已選擇的士兵")]
        public List<Transform> selectedSoldierList;
        /// <summary>
        /// 士兵 tag
        /// </summary>
        [Header("士兵 tag")]
        public string soldierTag;
        /// <summary>
        /// 士兵生產時間
        /// </summary>
        [Header("士兵生產時間(秒)")]
        public float soldierProduceTime;
        /// <summary>
        /// 目前士兵生產時間間隔
        /// </summary>
        [Header("目前士兵生產時間間隔(秒)")]
        public float soldierProduceTimeNow;
        /// <summary>
        /// 誰打我變成目標
        /// </summary>
        public List<Transform> WhoHitMeTransform;
        #endregion
        /// <summary>
        /// 初始化主堡資料
        /// </summary>
        public virtual void MainFortressDataInitializ()
        {
            
        }
        /// <summary>
        /// 取得主堡
        /// </summary>
        public virtual void GetEnemyMainFortress()
        {

        }
        /// <summary>
        /// 主堡血量文字
        /// </summary>
        public virtual void MainFortressHpTextMeshPro()
        {   
        }
        /// <summary>
        /// 目前剩餘兵數文字
        /// </summary>
        public virtual void MainForTressSoldierCountTextMeshPro()
        {   
        }
        /// <summary>
        /// 生產士兵
        /// </summary>
        public virtual void ProduceSoldier()
        {
        }

        public virtual void MainFortressHit(int hit)
        {
        }
    }
}
