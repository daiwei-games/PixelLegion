using Assets.Scripts.IFace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIScript : LeadToSurviveGameBaseClass, IGUIFunc
{
    /// <summary>
    /// 取得遊戲管理器
    /// </summary>
    [Header("取得遊戲管理器"), SerializeField]
    protected GameObject _gameManager;
    /// <summary>
    /// 取得遊戲管理器腳本
    /// </summary>
    [Header("取得遊戲管理器腳本"), SerializeField]
    protected GameManager _gameManagerScript;
    /// <summary>
    /// 取得子物件
    /// </summary>
    [Header("取得子物件"), SerializeField]
    private List<UIScript> UIList;
    /// <summary>
    /// 玩家操控的UI
    /// </summary>
    [Header("玩家操控的UI")]
    private RectTransform PlayerMoveUI;
    /// <summary>
    /// Game Over
    /// 只執行一次呼叫UI
    /// 因為裡面有協呈
    /// </summary>
    [Header("Game Over")]
    private bool isGameOver;
    private RectTransform uiElement;
    /// <summary>
    /// 決鬥UI
    /// </summary>
    private RectTransform DuelUI;
    private void Awake()
    {
        GUIDataInitializ();
    }
    public virtual void GUIDataInitializ()
    {
        _Tf = transform;
        _Go = gameObject;

        _gameManager = GameObject.Find("GameManager");
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>();
            if (_gameManagerScript != null)
                _gameManagerScript.uiScript = this;
        }

        for (int i = 0; i < UIList.Count; i++)
        {
            UIList[i]._gameManager = _gameManager;
            UIList[i]._gameManagerScript = _gameManagerScript;
        }
        Transform GameOverTf = _Tf.Find("UI_GameOver");
        if (GameOverTf != null)
            uiElement = GameOverTf.GetComponent<RectTransform>();
        isGameOver = true;
        Transform PlayerMoveUITf = _Tf.Find("英雄操作");
        if (PlayerMoveUITf != null)
            PlayerMoveUI = PlayerMoveUITf.GetComponent<RectTransform>();

        Transform durlTf = _Tf.Find("決鬥 Duel");
        if(durlTf != null)
        {
            DuelUI = durlTf.GetComponent<RectTransform>();
            CloseDuelUI();
        }
    }
    #region Game Over UI
    public void GameOverUI()
    {
        if (uiElement == null) return;
        if (!isGameOver) return;
        isGameOver = false;
        StartCoroutine("GameOverUIView", uiElement);

    }
    IEnumerator GameOverUIView(RectTransform _uiElement)
    {
        while (_uiElement.anchoredPosition.y != 0)
        {
            _uiElement.anchoredPosition = Vector2.Lerp(_uiElement.anchoredPosition, Vector2.zero, 0.1f);
            yield return new WaitForSeconds(.02f);
        }
        Time.timeScale = 0;
    }
    #endregion

    #region 決鬥時對一般UI的操作
    public void ClosePlayerMoveUI()
    {
        if (PlayerMoveUI == null) return;
        Vector2 pos = PlayerMoveUI.anchoredPosition;
        pos.y = 720;
        PlayerMoveUI.anchoredPosition = pos;
    }

    public void OpenPlayerMoveUI()
    {
        if (PlayerMoveUI == null) return;
        PlayerMoveUI.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region 決鬥UI
    public void CloseDuelUI()
    {
        if (DuelUI == null) return;
        Vector2 pos = DuelUI.anchoredPosition;
        pos.y = 720;
        DuelUI.anchoredPosition = pos;
    }
    public void OpenDuelUI()
    {
        if (DuelUI == null) return;
        DuelUI.anchoredPosition = Vector2.zero;
    }
    #endregion
}
