using UnityEngine;

public class DefScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 防禦時間條
    /// </summary>
    public Transform _DefTimeTf;
    private Vector2 DefTimeSize; 
    public DefScript DefDataInitializ()
    {
        _Tf = transform;
        _Go = gameObject;

        _DefTimeTf = _Tf.Find("DefTime");
        if (_DefTimeTf != null)
            DefTimeSize = _DefTimeTf.localScale; // 取得防禦時間的大小

        return this;
    }

    public void GetDefTimeMax()
    {
        DefTimeSize = Vector2.one;
        _DefTimeTf.localScale = DefTimeSize;
        Vector2 _pos = Vector2.zero;
        _DefTimeTf.localPosition = _pos;

    }
    public void GetDefTime(float _deftimemax, float _time)
    {
        float percentage = Mathf.InverseLerp(0, _deftimemax, _time);
        DefTimeSize.x -= percentage; // 防禦時間減少
        if(DefTimeSize.x <= 0) DefTimeSize.x = 0; // 防禦時間不會變負的
        _DefTimeTf.localScale = DefTimeSize;

        Vector2 _pos = _DefTimeTf.localPosition;
        _pos.x -= percentage / 2;
        _DefTimeTf.localPosition = _pos;
    }
}
