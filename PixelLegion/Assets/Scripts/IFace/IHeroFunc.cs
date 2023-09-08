namespace Assets.Scripts.IFace
{
    public interface IHeroFunc : IActionFunc
    {
        /// <summary>
        /// 英雄初始化
        /// </summary>
        public void HeroInitializ();
        /// <summary>
        /// 英雄決鬥狀態機
        /// </summary>
        public void HeroDuelStateFunc();
        /// <summary>
        /// 英雄狀態機
        /// </summary>
        public void HeroStateFunc();
        /// <summary>
        /// 決鬥受傷不執行受傷動畫
        /// </summary>
        /// <param name="t"></param>
        public void HeroHit(int t);
        /// <summary>
        /// 決鬥移動方法
        /// </summary>
        public void HeroMove();
        /// <summary>
        /// 英雄血量的操作
        /// </summary>
        /// <param name="hitAmount">血量漸少或增加</param>
        public void HeroHP(int hitAmount);
        /// <summary>
        /// 防禦
        ///</summary>
        public void Defense();
        /// <summary>
        /// 翻滾
        /// </summary>
        public void Roll();
        /// <summary>
        /// 跳躍
        /// </summary>
        public void Jump();
        /// <summary>
        /// 跳躍攻擊
        /// </summary>
        public void JumpAtk();
        /// <summary>
        /// 衝刺
        /// </summary>
        public void Dash();
        /// <summary>
        /// 下落
        /// </summary>
        public void Drop();
        /// <summary>
        /// 著地
        /// </summary>
        public void Land();
        /// <summary>
        /// 閃避
        /// </summary>
        public void Miss();
        /// <summary>
        /// 移動相關攻擊
        /// </summary>
        public void MoveAtk();
    }
}
