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
        /// 防禦
        ///</summary>
        public void Def()
        {
        }
        /// <summary>
        /// 翻滾
        /// </summary>
        public void Roll()
        {
        }
        /// <summary>
        /// 翻滾攻擊
        /// </summary>
        public void RollAtk()
        {
        }
        /// <summary>
        /// 跳躍
        /// </summary>
        public void Jump()
        {
        }
        /// <summary>
        /// 跳躍攻擊
        /// </summary>
        public void JumpAtk()
        {
        }
        /// <summary>
        /// 衝刺
        /// </summary>
        public void Dash()
        {
        }
        /// <summary>
        /// 下落
        /// </summary>
        public void Drop()
        {
        }
        /// <summary>
        /// 著地
        /// </summary>
        public void Land()
        {
        }
        /// <summary>
        /// 閃避
        /// </summary>
        public void Miss()
        {
        }
    }
}
