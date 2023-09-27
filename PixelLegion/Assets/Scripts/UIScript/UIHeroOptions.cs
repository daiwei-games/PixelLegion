using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHeroOptions : LeadToSurviveGameBaseClass
{
    private GameManager _Gm;
    /// <summary>
    /// 取得玩家管理器腳本
    /// </summary>
    private PlayerScript _playerScript;
    /// <summary>
    /// 英雄按鈕清單
    /// </summary>
    [Header("英雄按鈕清單")]
    public List<Button> HeroButtonUI;
    public List<Image> HeroSpriteRenderer;
    /// <summary>
    /// 英雄按鈕總數
    /// </summary>
    [HideInInspector]
    public int ButtonCount;
    /// <summary>
    /// 英雄清單按鈕的索引
    /// </summary>
    [HideInInspector]
    public int HeroListIndex;
    private void Start()
    {
        GUIDataInitializ();
    }

    public void GUIDataInitializ()
    {
        _Tf = transform;
        _Go = gameObject;
        _Gm = FindFirstObjectByType<GameManager>();
        _playerScript = FindFirstObjectByType<PlayerScript>();
        if (_playerScript != null) _playerScript.HeroUI = this;

        ButtonCount = _playerScript.HeroCount;
        Button _but = HeroButtonUI[0];
        if (ButtonCount < 1) ButtonCount = 1;
        while (HeroButtonUI.Count < ButtonCount)
        {
            HeroButtonUI.Add(Instantiate(_but, _but.transform.parent));
        }
        for (int e = 0; e < ButtonCount; e++)
        {
            HeroSpriteRenderer.Add(HeroButtonUI[e].GetComponent<Image>());
        }
        HeroListIndex = 0;
    }


    /// <summary>
    /// 按鈕綁定事件並將按鈕圖片變成英雄
    /// 當 HeroListIndex 大於 HeroButtonUI.Count 時不會綁定事件
    /// </summary>
    /// <param name="_hero">英雄腳本</param>
    public void ButtonAddEvent(HeroScript _hero)
    {
        if (HeroListIndex >= HeroButtonUI.Count) return;
        HeroButtonUI[HeroListIndex].onClick.AddListener(() =>
        { // 綁定事件
            SelectedHeroEvent(_hero);
        });
        HeroSpriteRenderer[HeroListIndex].sprite = _hero.HeroAvatar; // 將按鈕圖片變成英雄
        HeroListIndex++;
    }
    /// <summary>
    /// 被按鈕綁定的事件
    /// </summary>
    public void SelectedHeroEvent(HeroScript _hero)
    {
        if (_Gm.HeroList.Count == 0) return;
        _Gm.SelectedHeroFunc(_hero);
    }


}
