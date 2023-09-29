using UnityEngine;

public class HpScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 血條
    /// </summary>
    [Header("血條")]
    public Transform _Hp;
    private Vector2 _HpSize;
    public HpScript HpDataInitializ()
    {
        _Tf = transform;
        _Go = gameObject;

        _Hp = _Tf.Find("Hp");
        if(_Hp != null) _HpSize = _Hp.localScale;

        return this;
    }

    public void GetHit(int HpMax, int _hit)
    {
        float percentage = Mathf.InverseLerp(0, HpMax, _hit);
        _HpSize.x -= percentage; // 血條減少
        if(_HpSize.x <= 0) _HpSize.x = 0; // 血條不會變負的
        _Hp.localScale = _HpSize;

        Vector2 _pos = _Hp.localPosition;
        _pos.x -= percentage / 2;
        _Hp.localPosition = _pos;
    }
}
