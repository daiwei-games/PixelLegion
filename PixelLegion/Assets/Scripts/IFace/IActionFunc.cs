using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.IFace
{
    /// <summary>
    /// 所有公用的動作函數
    /// </summary>
    public interface IActionFunc
    {
        /// <summary>
        /// 攻擊
        /// </summary>
        public void Atk();
        /// <summary>
        /// 移動
        /// </summary>
        public void Move();
        /// <summary>
        /// 死亡
        /// </summary>
        public void Die();
        /// <summary>
        /// 受傷
        /// </summary>
        public void Hit();
        /// <summary>
        /// 等待
        /// </summary>
        public void Idle();
        /// <summary>
        /// 必須受傷
        /// </summary>
        public void MustBeInjured(int hp);

    }
}
