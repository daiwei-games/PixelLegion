using UnityEngine;

namespace Assets.Scripts.IFace
{
    /// <summary>
    /// 士兵動作介面
    /// </summary>
    public interface ISoldierFunc : IActionFunc
    {
        /// <summary>
        /// 士兵的資料初始化
        /// </summary>
        public void SoldierDataInitializ();
        /// <summary>
        /// 取得所有動化的名字
        /// </summary>
        public void GetAllAnimationClipName();
        /// <summary>
        /// 狀態機
        /// </summary>
        public void SoldierStateAI();
        /// <summary>
        /// 發出正方形範圍射線
        /// </summary>
        public void PhyOverlapBoxAll(Vector2 Pos, float PosSize);
        /// <summary>
        /// HP 操作
        /// </summary>
        public void SoldierHP(int hitAmount);


    }
}
