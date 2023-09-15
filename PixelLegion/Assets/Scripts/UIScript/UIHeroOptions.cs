using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroOptions : UIScript
{
    /// <summary>
    /// 取得玩家管理器
    /// </summary>
    [Header("取得玩家管理器、腳本"), SerializeField]
    private GameObject _playerManager;
    /// <summary>
    /// 取得玩家管理器腳本
    /// </summary>
    [SerializeField]
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
    public int ButtonCount;
    /// <summary>
    /// 英雄清單按鈕的索引
    /// </summary>
    private int HeroListIndex;
    private void Awake()
    {
        GUIDataInitializ();
    }

    public override void GUIDataInitializ()
    {
        _transform = transform;

        _playerManager = GameObject.Find("PlayerManager");
        if (_playerManager != null)
        {
            _playerScript = _playerManager.GetComponent<PlayerScript>();
            if (_playerScript != null)
            {
                _playerScript.HeroUI = this;
            }
            ButtonCount = _playerScript._playerDataObject.SelectedHeroList.Count;
        }

        Button _but = HeroButtonUI[0];
        if (ButtonCount < 3) ButtonCount = 3;
        while (HeroButtonUI.Count < ButtonCount)
        {
            HeroButtonUI.Add(Instantiate(_but, _but.transform.parent));
        }
        if (HeroButtonUI.Count > 0)
        {
            for (int i = 0; i < HeroButtonUI.Count; i++)
            {
                HeroSpriteRenderer.Add(HeroButtonUI[i].GetComponent<Image>());
            }
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
        if (_gameManagerScript.HeroList.Count == 0) return;
        _gameManagerScript.SelectedHeroFunc(_hero);
    }
}
