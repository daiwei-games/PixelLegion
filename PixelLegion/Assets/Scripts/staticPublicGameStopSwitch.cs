using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// 公用的暫停布林值
    /// false = 沒有打開繼續執行
    /// true = 打開暫停執行
    /// </summary>
    public class staticPublicGameStopSwitch
    {
        /// <summary>
        /// 主堡暫停
        /// </summary>
        public static bool mainFortressStop = false;
        /// <summary>
        /// 士兵暫停
        /// </summary>
        public static bool soldierStop = false;
        /// <summary>
        /// 英雄暫停
        /// </summary>
        public static bool heroStop = false;
        /// <summary>
        /// 遊戲暫停
        /// </summary>
        public static bool gameStop = false;
    }
}
