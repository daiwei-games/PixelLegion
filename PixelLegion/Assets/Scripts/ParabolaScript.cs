using Assets.Scripts;
using UnityEngine;
/// <summary>
/// 投擲類武器、道具或魔法物件的腳本
/// </summary>
public class ParabolaScript : LeadToSurviveGameBaseClass
{
    #region 基礎資料
    /// <summary>
    /// 光明發出來的，還是黑暗發出來的
    /// </summary>
    [Header("光明還是黑暗發出來的")]
    public PromisingOrDARK _promisingOrDARK;
    /// <summary>
    /// 武器或魔法的投擲軌道類型
    /// </summary>
    [Header("武器或魔法的投擲軌道類型")]
    public LinearType _Pt;
    /// <summary>
    /// 腳本管理
    /// </summary>
    [HideInInspector]
    public GameManager _gameManager;
    /// <summary>
    /// 投擲物攻擊數值
    /// </summary>
    [Header("投擲物攻擊數值")]
    public int Atk;
    /// <summary>
    ///是否爆擊
    /// </summary>
    [Header("是否爆擊"), HideInInspector]
    public bool isCriticalHitRate;
    /// <summary>
    /// 是否碰到地板會消失
    /// </summary>
    [Header("是否碰到地板會消失")]
    public bool isFlootDestroy;
    #endregion

    #region 二次貝茲曲線
    /// <summary>
    /// 物件起始點
    /// </summary>
    public Transform startPoint;
    /// <summary>
    /// 控制點
    /// </summary>
    public Vector3 controlPoint;
    /// <summary>
    /// 物件結束點
    /// </summary>
    public Vector3 endPoint;
    /// <summary>
    /// 速度
    /// </summary>
    private float speed;
    /// <summary>
    /// 進度時間
    /// </summary>
    private float t;
    #endregion
    private void OnEnable()
    {
        _Tf = transform;
        _Go = gameObject;

        if (speed < 1) speed = 1;
        t = 0f;
    }
    /// <summary>
    /// 前往目標
    /// </summary>
    public void Goto()
    {

        // 如果t等於1，表示物件已經移動到了控制點P2，這裡你可以觸發相應的事件或做其他處理
        if (t >= 1f) return;

        // 在0到1之間變化的參數t
        t += speed * Time.deltaTime;
        // 限制t的範圍在0到1之間
        t = Mathf.Clamp01(t);
        // 計算二次貝茲曲線上的點
        Vector3 targetPosition = CalculateQuadraticBezierPoint(t, startPoint.position, controlPoint, endPoint);
        Vector3 direction = targetPosition - _Tf.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // 移動物件到計算得到的點
        //_Tf.position = targetPosition;
        //_Tf.rotation = rotation;
        _Tf.SetPositionAndRotation(targetPosition, rotation);

        
    }
    /// <summary>
    /// 二次貝茲曲線
    /// </summary>
    /// <param name="t">時間</param>
    /// <param name="p0">起點座標</param>
    /// <param name="p1">控制點座標</param>
    /// <param name="p2">終點座標</param>
    /// <returns>計算後的座標</returns>
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HeroScript _hs;
        MainFortressScript _mfs;
        SoldierScript _ss;
        if (_promisingOrDARK == PromisingOrDARK.Promising) //如果是光明的投擲物
        {
            switch (collision.tag)
            {
                case staticPublicObjectsStaticName.DarkHeroTag:
                    _hs = collision.GetComponent<HeroScript>();
                    if (_hs != null)
                    {
                        if (isCriticalHitRate)
                        {
                            _hs.MustBeInjured(Atk);
                            return;
                        }
                        _hs.HeroHit(Atk);
                    }
                    Destroy(_Go);
                    break;
                case staticPublicObjectsStaticName.DarkMainFortressTag:
                    _mfs = collision.GetComponent<DarkMainFortressScript>();
                    if (_mfs != null)
                        _mfs.MainFortressHit(Atk);
                    Destroy(_Go);
                    break;
                case staticPublicObjectsStaticName.DARKSoldierTag:
                case staticPublicObjectsStaticName.WildSoldierTag:
                    _ss = collision.GetComponent<SoldierScript>();
                    if (_ss != null)
                        _ss.SoldierHP(Atk);
                    Destroy(_Go);
                    break;
            }
        }
        if (_promisingOrDARK == PromisingOrDARK.Dark) //如果是黑暗的投擲物
        {
            switch (collision.tag)
            {
                case staticPublicObjectsStaticName.HeroTag:
                    _hs = collision.GetComponent<HeroScript>();
                    if (_hs != null)
                    {
                        if (isCriticalHitRate)
                        {
                            _hs.MustBeInjured(Atk);
                            return;
                        }
                        _hs.HeroHit(Atk);
                    }
                    Destroy(_Go);
                    break;
                case staticPublicObjectsStaticName.MainFortressTag:
                    _mfs = collision.GetComponent<MainFortressScript>();
                    if (_mfs != null)
                        _mfs.MainFortressHit(Atk);
                    Destroy(_Go);
                    break;
                case staticPublicObjectsStaticName.PlayerSoldierTag:
                    _ss = collision.GetComponent<SoldierScript>();
                    if (_ss != null)
                        _ss.SoldierHP(Atk);
                    Destroy(_Go);
                    break;
            }
        }

        if (isFlootDestroy && collision.CompareTag("floot")) //如果碰到地板會消失
        {
            Destroy(_Go);
        }
    }
}
