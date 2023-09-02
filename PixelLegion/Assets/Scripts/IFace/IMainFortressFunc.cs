using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.IFace
{
    /// <summary>
    /// 主堡功能介面
    /// </summary>
    public interface IMainFortressFunc
    {
        /// <summary>
        /// 初始化主堡資料
        /// </summary>
        public void MainFortressDataInitializ();
        /// <summary>
        /// 主堡血量文字
        /// </summary>
        public void MainFortressHpTextMeshPro();
        /// <summary>
        /// 目前剩餘兵數文字
        /// </summary>
        public void MainForTressSoldierCountTextMeshPro();
        /// <summary>
        /// 生產士兵
        /// </summary>
        public void ProduceSoldier();
        /// <summary>
        /// 城堡受傷
        /// </summary>
        public void MainFortressHit(int hit);
    }
}
