/// <summary>
/// 光明或者黑暗
/// </summary>
public enum PromisingOrDARK
{
    /// <summary>
    /// 光明的物件
    /// </summary>
    Promising,
    /// <summary>
    /// 黑暗的物件
    /// </summary>
    Dark
}
#region 場景類型
/// <summary>
/// 場景類型
/// </summary>
public enum ScenesType
{
    /// <summary>
    /// 村莊場景
    /// </summary>
    village,
    /// <summary>
    /// 練習場景
    /// </summary>
    practise,
    /// <summary>
    /// 戰場場景
    /// </summary>
    battlefield,
    /// <summary>
    /// 草原場景
    /// </summary>
    prairie,
    /// <summary>
    /// 礦洞場景
    /// </summary>
    mine,
    /// <summary>
    /// 黑暗場景
    /// </summary>
    dark
}
#endregion

#region 英雄狀態
/// <summary>
/// 英雄狀態
/// </summary>
public enum HeroState
{
    /// <summary>
    /// 等待
    /// </summary>
    Idle,
    /// <summary>
    /// 移動
    /// </summary>
    Run,
    /// <summary>
    /// 防禦
    /// </summary>
    Def,
    /// <summary>
    /// 受傷
    /// </summary>
    Hit,
    /// <summary>
    /// 死亡
    /// </summary>
    Die,
    /// <summary>
    /// 攻擊
    /// </summary>
    Attack,
    /// <summary>
    /// 重攻擊
    /// </summary>
    HeavyAttack,
    /// <summary>
    /// 衝刺
    /// </summary>
    Dash,
    /// <summary>
    /// 閃現 - 消失
    /// </summary>
    Miss,
    /// <summary>
    /// 閃現 - 出現
    /// </summary>
    Miss1,
    /// <summary>
    /// 移動攻擊
    ///</summary>
    MovAtk,
    /// <summary>
    /// 跳躍
    /// </summary>
    Jump
}
#endregion

#region 士兵狀態
/// <summary>
/// 士兵狀態機
/// </summary>
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
    /// 遠攻
    /// </summary>
    RemoteAtk,
    /// <summary>
    /// 受傷
    /// </summary>
    Hit,
    /// <summary>
    /// 超級受傷
    /// </summary>
    SoupHit,
    /// <summary>
    /// 死亡
    /// </summary>
    Die
}

/// <summary>
/// 目前職位
/// </summary>
public enum SoldierPost {
    /// <summary>
    /// 一般士兵
    /// </summary>
    Sodlier,
    /// <summary>
    /// 遠端攻擊守衛 不會走動
    /// </summary>
    RemoteAttackGuard,
    /// <summary>
    /// 進戰攻擊守衛 範圍移動
    /// </summary>
    MeleeAttackGuard
}
/// <summary>
/// 攻擊型態
/// </summary>
public enum AttackType
{
    /// <summary>
    /// 遠程攻擊
    /// </summary>
    RemoteAttack,
    /// <summary>
    /// 進戰攻擊
    /// </summary>
    MeleeAttack,
    /// <summary>
    /// 遠程+進戰攻擊
    /// </summary>
    RemoteAndMelee
}
#endregion

/// <summary>
/// 投擲類武器、道具或魔法物件分類
/// </summary>
public enum ParabolaType
{
    /// <summary>
    /// 拋物線 需要控制拋物至高點
    /// </summary>
    parabola,
    /// <summary>
    /// 直線前進
    /// </summary>
    straightLine,
    /// <summary>
    /// 直接落在地面
    /// </summary>
    ground,
    /// <summary>
    /// 漂浮
    /// </summary>
    levitate
}