using UnityEngine;

public class MfHitVfxScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 主堡腳本
    /// </summary>
    public MainFortressScript _Mfs;
    /// <summary>
    /// 主堡的 localPosition
    /// </summary>
    Vector3 _MfsPos;
    /// <summary>
    /// 晃動力道
    /// </summary>
    float Strength;
    /// <summary>
    /// 晃動開始時間
    /// </summary>
    public float OpenTimeStart;
    /// <summary>
    /// 晃動左邊或右邊
    /// </summary>
    public bool LeftOrRight;
    private void OnEnable()
    {
        if(_Tf == null)
            _Tf = transform;
        if(_Go == null)
            _Go = gameObject;

        _MfsPos = _Tf.localPosition;
        
    }
    private void Update()
    {
        Hit(Time.time);
    }
    private void OnDisable()
    {
        _Tf.localPosition = Vector3.zero;
    }
    void Hit(float _time)
    {
        if (_Mfs == null) return;
        if (_time >= OpenTimeStart)
        {
            _Go.SetActive(false);
            return;
        }
        Strength += .01f;
        if(LeftOrRight)
            _MfsPos.x = Mathf.PingPong(Strength, .15f);
        else
            _MfsPos.x = Mathf.PingPong(Strength, -.15f);
        _MfsPos.y = Mathf.PingPong(Strength, .1f);
        _Tf.localPosition = _MfsPos;
    }
}
