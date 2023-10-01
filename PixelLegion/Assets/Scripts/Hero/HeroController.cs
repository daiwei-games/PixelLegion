using UnityEngine;
/// <summary>
/// 玩家控制角色時的腳本
/// </summary>
public class HeroController : MonoBehaviour
{
    #region UI
    /// <summary>
    /// 操控介面
    /// </summary>
    [Header("操控介面")]
    public UIHeroController _UIHc;
    #endregion

    #region 搖桿操作
    /// <summary>
    /// 搖桿
    /// </summary>
    [Header("搖桿")]
    public VariableJoystick _Joystick;
    /// <summary>
    /// 搖桿方向
    /// </summary>
    Vector2 _JoystickDirection;
    #endregion

    #region 英雄相關
    /// <summary>
    /// 當前英雄
    /// </summary>
    [Header("當前英雄")]
    public HeroScript _Hs;
    #endregion

    #region 計時相關
    /// <summary>
    /// 衝刺、閃現或翻滾的計時
    /// </summary>
    float MissOrDashStartTime;
    /// <summary>
    /// 攻擊間段計時
    /// </summary>
    float time;
    #endregion

    #region 場景物建
    /// <summary>
    /// 攝影機
    /// </summary>
    [HideInInspector]
    public Transform CameraTf;
    public Vector3 CameraPos;
    #endregion

    float g;
    void Start()
    {

    }
    private void OnEnable()
    {
        CameraTf = Camera.main.transform;

        _UIHc.GetNowHeroController(this);
        _UIHc.DashOpenOrClose(_Hs.isDash); // 開啟或關閉衝刺按鈕
        _UIHc.HeavyAttackOpenOrClose(_Hs.isHeavyAttack); // 開啟或關閉重攻擊按鈕

        _Hs.isPlayerControl = true; //選擇的英雄可以操控
        _Hs.HeroDuelStateFunc(); // 進入Idle狀態
    }
    private void OnDisable()
    {
        _Hs.DefTime = 0;
        _Hs.isNowDef = true;
        _Hs.isPlayerControl = false;
    }
    // Update is called once per frame
    void Update()
    {
        MissOrDashStartTime = Time.time; // 計算衝刺、閃現或翻滾的計時
        // 取得搖桿位移
        _JoystickDirection = Vector2.right * _Joystick.Horizontal;
        PlayGMHeroMiss(MissOrDashStartTime);
        PlayGMHeroDash(MissOrDashStartTime);

        CameraPingPong();
    }

    private void FixedUpdate()
    {
        time = Time.deltaTime;

        _Hs.AtkTime += time;
        _Hs.HeroDuelStateFunc(HeroState.Run, _JoystickDirection);
        if (!_Hs.isNowDef && _Hs.DefTime > 0)
        {
            _Hs.DefTime -= time;
            _Hs._Defs.GetDefTime(_Hs.DefTimeMax, time);
        }
        if (_Hs.DefTime <= 0) _Hs.isNowDef = true;

        if (_Hs.isAnimationFrameStorp)
        {
            _Hs.AnimationFrameStorpTime += time;
            if (_Hs.AnimationFrameStorpTime >= _Hs.AnimationFrameStorpTimeMax)
            {
                _Hs.AnimationFrameStorpTime = 0;
                _Hs.isAnimationFrameStorp = false;
            }
            if (!_Hs.isAnimationFrameStorp) _Hs._animator.speed = 1;
        }
    }
    #region 玩家手動操作
    /// <summary>
    /// 玩家操做攻擊
    /// </summary>
    public void gmHeroAttack()
    {
        if (_Hs == null) return;
        _Hs.HeroDuelStateFunc(HeroState.Attack, _Hs.enemyCollider);
    }
    public void gmHeroHeavyAttack()
    {
        if (_Hs == null) return;
        _Hs.HeroDuelStateFunc(HeroState.HeavyAttack, _Hs.enemyCollider);
    }

    public void gmHeroDef()
    {
        if (_Hs == null) return;
        _Hs.HeroDuelStateFunc(HeroState.Def);
    }

    /// <summary>
    /// 閃現
    /// </summary>
    public void gmHeroMiss(float time)
    {

        if (!_Hs.isfloot) return;
        if (_Hs == null) return;
        _Hs.HeroDuelStateFunc(HeroState.Miss, Vector2.zero, time, null);

    }
    private void PlayGMHeroMiss(float time)
    {
        if (!_Hs.IsItPossibleMiss) return;
        _Hs.Miss1(time);
    }
    /// <summary>
    /// 手動衝刺決鬥不使用
    /// </summary>
    public void gmHeroDash(float time)
    {
        if (!_Hs.isfloot) return;
        if (_Hs == null) return;
        _Hs.HeroDuelStateFunc(HeroState.Dash, Vector2.zero, time, null);
    }
    /// <summary>
    /// 持續判斷是否衝刺 決鬥不使用
    /// </summary>
    private void PlayGMHeroDash(float time)
    {
        if (_Hs == null) return;
        if (!_Hs.IsItPossibleToDash) return;
        _Hs.FastForward(_Hs._Tf, time);
    }
    #endregion

    public void CameraPingPong()
    {
        if (_Hs.isCameraShake)
        {
            CameraPos = CameraTf.position;
            if (_Hs.isCriticalHitRate)
            {
                g += .05f;
                CameraPos.x -= Mathf.PingPong(g, .4f);
                CameraPos.y += Mathf.PingPong(g, .4f);
            }
            else
            {
                g += .02f;
                CameraPos.x -= Mathf.PingPong(g, .2f);
                CameraPos.y += Mathf.PingPong(g, .2f);
            }
            CameraTf.position = CameraPos;
            _Hs.CameraShakeTime += time;
            if (_Hs.CameraShakeTime >= _Hs.CameraShakeTimeMax)
            {
                _Hs.CameraShakeTime = 0;
                _Hs.isCameraShake = false;
            }
        }
    }
}
