using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 決鬥的介面操作
/// </summary>
public class UIDuel : LeadToSurviveGameBaseClass
{
    public GameManager GameManagerScript;
    
    /// <summary>
    /// 攻擊按鈕
    /// </summary>
    public Button DuelAttack;
    /// <summary>
    /// 防禦按鈕
    /// </summary>
    public Button DuelDef;

    public Button DuelAUTO;
    public Button DuelOff;

    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;

        GameManagerScript = FindObjectOfType<GameManager>();


        Transform ButtonTf;
        ButtonTf = _Tf.Find("Duel-Attack");
        if(ButtonTf != null)
        {
            DuelAttack = ButtonTf.GetComponent<Button>();
            if(DuelAttack != null)
            {
                DuelAttack.onClick.AddListener(() =>
                {
                    GameManagerScript.OnDuelAttack();
                });
            }
        }

        ButtonTf = null;
        ButtonTf = _Tf.Find("Duel-Def");
        if (ButtonTf != null)
        {
            DuelDef = ButtonTf.GetComponent<Button>();
            if (DuelDef != null)
            {
                DuelDef.onClick.AddListener(() =>
                {
                    GameManagerScript.OnDuelDef();
                });
            }

        }
        ButtonTf = null;
        ButtonTf = _Tf.Find("Duel-AUTO");
        if (ButtonTf != null)
        {
            DuelAUTO = ButtonTf.GetComponent<Button>();
            if (DuelAUTO != null)
            {
                DuelAUTO.onClick.AddListener(() =>
                {
                    GameManagerScript.OnHeroMoveAuto();
                });
            }
        }


        ButtonTf = null;
        ButtonTf = _Tf.Find("Duel-Off");
        if(ButtonTf != null)
        {
            DuelOff = ButtonTf.GetComponent<Button>(); 
            if (DuelOff != null)
            {
                DuelOff.onClick.AddListener(() =>
                {
                    // 關閉決鬥
                    GameManagerScript.DuelEnd();
                });
            }
        }
    }
}
