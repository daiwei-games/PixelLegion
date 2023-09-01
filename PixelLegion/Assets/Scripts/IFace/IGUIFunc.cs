using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.IFace
{
    /// <summary>
    /// GUI功能介面
    /// </summary>
    public interface IGUIFunc
    {
        /// <summary>
        /// GUI物件初始化
        /// </summary>
        public void GUIDataInitializ();
        /// <summary>
        /// 重新開始遊戲
        /// </summary>
        public void ReStartTheGame() { }
    }
}
