using UnityEngine;
using UnityEngine.UI;

public class HeroController : LeadToSurviveGameBaseClass
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
    /// 衝刺
    /// </summary>
    public Button Dash;
    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;
        GameManagerScript = FindObjectOfType<GameManager>();
        GameManagerScript.heroControl = this;
        Transform _tfbutton = _Tf.Find("Attack");
        if (_tfbutton != null)
        {
            Attakc = _tfbutton.GetComponent<Button>();
            Attakc.onClick.AddListener(() =>
            {
                GameManagerScript.gmHeroAttack();
            });
        }

        _tfbutton = _Tf.Find("Dash");
        if (_tfbutton != null)
        {
            Dash = _tfbutton.GetComponent<Button>();
            if (Dash != null)
            {
                Dash.onClick.AddListener(() =>
                {
                    //GameManagerScript.gmHeroDash(Time.time);
                    GameManagerScript.gmHeroMiss(Time.time);

                });
            }
        }

        _tfbutton = _Tf.Find("Duel");
        if (_tfbutton != null)
        {
            Button Duel = _tfbutton.GetComponent<Button>();
            if (Duel != null)
            {
                Duel.onClick.AddListener(() =>
                {
                    GameManagerScript.Duel();
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
}
