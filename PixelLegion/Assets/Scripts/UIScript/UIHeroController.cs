using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroController : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 取得 GameManager 腳本
    /// </summary>
    public GameManager GameManagerScript;
    /// <summary>
    /// 攻擊
    /// </summary>
    public Button Attakc;
    /// <summary>
    /// 重攻擊
    /// </summary>
    public Button HeavyAttack;
    /// <summary>
    /// 衝刺
    /// </summary>
    public Button Dash;
    /// <summary>
    /// 防禦
    /// </summary>
    public Button Def;
    /// <summary>
    /// 決鬥按鈕
    /// </summary>
    public Button Duel;
    /// <summary>
    /// 目前控制的英雄操控腳本
    /// </summary>
    public HeroController _Hc;
    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;
        GameManagerScript = FindFirstObjectByType<GameManager>();
        GameManagerScript._UIHc = this;
        Transform _tfbutton = _Tf.Find("Attack");
        if (_tfbutton != null)
        {
            Attakc = _tfbutton.GetComponent<Button>();
            Attakc.onClick.AddListener(() =>
            {
                if(_Hc == null) return;
                _Hc.gmHeroAttack();
            });
        }

        _tfbutton = _Tf.Find("Heavy_Attack");
        if (_tfbutton != null)
        {
            HeavyAttack = _tfbutton.GetComponent<Button>();
            if (HeavyAttack != null)
            {
                HeavyAttack.onClick.AddListener(() =>
                {
                    if (_Hc == null) return;
                    _Hc.gmHeroHeavyAttack();
                });
            }
        }
        _tfbutton = _Tf.Find("Dash");
        if (_tfbutton != null)
        {
            Dash = _tfbutton.GetComponent<Button>();
            if (Dash != null)
            {
                Dash.onClick.AddListener(() =>
                {
                    if (_Hc == null) return;
                    _Hc.gmHeroDash(Time.time);
                    //GameManagerScript.gmHeroMiss(Time.time);

                });
            }
        }

        _tfbutton = _Tf.Find("Def");
        if (_tfbutton != null)
        {
            Def = _tfbutton.GetComponent<Button>();
            if (Def != null)
            {
                Def.onClick.AddListener(() => {
                    if (_Hc == null) return;
                    _Hc.gmHeroDef();
                });
            }
        }

        _tfbutton = _Tf.Find("Duel");
        if (_tfbutton != null)
        {
            Duel = _tfbutton.GetComponent<Button>();
            if (Duel != null)
            {
                Duel.onClick.AddListener(() =>
                {
                    if (GameManagerScript == null) return;
                    GameManagerScript.OpenDuelUI();
                });
            }
        }
    }

    public void AttackOpenOrClose(bool b)
    {
        Attakc.gameObject.SetActive(b);
    }
    public void DashOpenOrClose(bool b)
    {
        Dash.gameObject.SetActive(b);
    }
    public void HeavyAttackOpenOrClose(bool b)
    {
        HeavyAttack.gameObject.SetActive(b);
    }

    public void GetNowHeroController(HeroController _Hc)
    {
        this._Hc = _Hc;
    }
}
