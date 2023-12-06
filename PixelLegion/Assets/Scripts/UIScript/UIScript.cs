using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 取得遊戲管理器腳本
    /// </summary>
    [Header("取得遊戲管理器腳本"), SerializeField]
    protected GameManager _Gm;
    /// <summary>
    /// 玩家操控的UI
    /// </summary>
    [Header("玩家操控的UI"), SerializeField]
    private RectTransform PlayerMoveUI;
    /// <summary>
    /// 英雄選擇UI
    /// </summary>
    [Header("英雄選擇UI"), SerializeField]
    private RectTransform HeroOptionsUI;
    /// <summary>
    /// 搖桿UI
    /// </summary>
    [Header("搖桿UI"), SerializeField]
    private RectTransform JoystickUI;
    /// <summary>
    /// Game Over
    /// 只執行一次呼叫UI
    /// 因為裡面有協呈
    /// </summary>
    [Header("Game Over"), SerializeField]
    private RectTransform _GameOverUI;
    private bool isGameOver;
    /// <summary>
    /// 決鬥UI
    /// </summary>
    [Header("決鬥UI"), SerializeField]
    private RectTransform DuelUI;
    /// <summary>
    /// 功能型UI
    /// 例 : 對話、買賣等...
    /// </summary>
    [Header("功能型UI"), SerializeField]
    private RectTransform FuncUI;
    /// <summary>
    /// 傳送陣的介面
    /// </summary>
    [Header("傳送陣的介面"), SerializeField]
    private RectTransform TeleportationArrayUI;
    /// <summary>
    /// 技能介面
    /// </summary>
    [Header("技能介面"), SerializeField]
    private RectTransform SkillManagerUI;
    /// <summary>
    /// 選擇技能介面
    /// </summary>
    [Header("選擇技能介面"), SerializeField]
    private RectTransform SkillOptionUI;
    /// <summary>
    /// 需要集中管理的介面
    /// </summary>
    [Header("需要集中管理的介面"), SerializeField]
    private List<RectTransform> CentralizedManagementUI;
    /// <summary>
    /// 必須存在一個的UI介面
    /// </summary>
    [Header("必須存在一個的UI介面"), SerializeField]
    private List<RectTransform> OnlyUI;

    private void Awake()
    {
        GUIDataInitializ();
    }
    public virtual void GUIDataInitializ()
    {
        _Tf = transform;
        _Go = gameObject;

        Transform GetUI = _Tf.Find("UI_GameOver");
        if (GetUI != null)
            _GameOverUI = GetUI.GetComponent<RectTransform>();
        isGameOver = true;

        GetUI = _Tf.Find("英雄選角面板");
        if (GetUI != null)
            HeroOptionsUI = GetUI.GetComponent<RectTransform>();

        GetUI = _Tf.Find("搖桿");
        if (GetUI != null)
            JoystickUI = GetUI.GetComponent<RectTransform>();

        GetUI = _Tf.Find("決鬥 Duel");
        if (GetUI != null)
        {
            DuelUI = GetUI.GetComponent<RectTransform>();
            CloseDuelUI();
        }

        GetUI = _Tf.Find("英雄操作");
        if (GetUI != null)
        {
            PlayerMoveUI = GetUI.GetComponent<RectTransform>();
            OnlyUI.Add(PlayerMoveUI);
        }

        GetUI = _Tf.Find("功能型UI");
        if (GetUI != null)
        {
            FuncUI = GetUI.GetComponent<RectTransform>();
            OnlyUI.Add(FuncUI);
        }

        GetUI = _Tf.Find("Menu");
        if (GetUI != null)
            CentralizedManagementUI.Add(GetUI.GetComponent<RectTransform>());

        GetUI = _Tf.Find("設定");
        if (GetUI != null)
            CentralizedManagementUI.Add(GetUI.GetComponent<RectTransform>());

        GetUI = _Tf.Find("使用角色選擇");
        if (GetUI != null)
            CentralizedManagementUI.Add(GetUI.GetComponent<RectTransform>());

        GetUI = _Tf.Find("TeleportationArrayUI");
        if (GetUI != null)
        {
            TeleportationArrayUI = GetUI.GetComponent<RectTransform>();
            CentralizedManagementUI.Add(TeleportationArrayUI);
        }
        GetUI = _Tf.Find("背包");
        if (GetUI != null)
            CentralizedManagementUI.Add(GetUI.GetComponent<RectTransform>());

        GetUI = _Tf.Find("技能");
        if (GetUI != null)
        {
            SkillManagerUI = GetUI.GetComponent<RectTransform>();
            CentralizedManagementUI.Add(SkillManagerUI);
        }

        GetUI = _Tf.Find("技能選擇");
        if (GetUI != null)
        {
            SkillOptionUI = GetUI.GetComponent<RectTransform>();
            CentralizedManagementUI.Add(SkillOptionUI);
        }


        _Gm = FindFirstObjectByType<GameManager>();
        if (_Gm != null)
        {
            _Gm.uiScript = this;
            switch (_Gm.NowScenes)
            {
                case ScenesType.practise:
                case ScenesType.battlefield:
                case ScenesType.prairie:
                    CloseFuncUI();
                    break;
                case ScenesType.village:
                    CloseHeroOptionsUI();
                    ClosePlayerMoveUI();
                    break;
            }
        }

        Canvas _cv = GetComponent<Canvas>();
        if (_cv != null)
            _cv.worldCamera = Camera.main;
        #region 介面初始化
        CloseGameOverUI();
        CloseCentralizedManagementUI();
        #endregion
    }
    #region Game Over UI
    /// <summary>
    /// 關閉失敗畫面
    /// </summary>
    public void CloseGameOverUI()
    {
        if (_GameOverUI == null) return;
        Vector2 pos = DuelUI.anchoredPosition;
        pos.y = 720 * 2;
        _GameOverUI.anchoredPosition = pos;
    }
    public void GameOverUI()
    {
        if (_GameOverUI == null) return;
        if (!isGameOver) return;
        isGameOver = false;
        StartCoroutine("GameOverUIView", _GameOverUI);

    }
    IEnumerator GameOverUIView(RectTransform _gameOverUI)
    {
        while (_gameOverUI.anchoredPosition.y != 0)
        {
            _gameOverUI.anchoredPosition = Vector2.Lerp(_gameOverUI.anchoredPosition, Vector2.zero, 0.1f);
            yield return new WaitForSeconds(.02f);
        }
        Time.timeScale = 0;
    }
    #endregion

    #region 攻擊、技能按鈕
    /// <summary>
    /// 關閉玩家操作UI
    /// </summary>
    public void ClosePlayerMoveUI()
    {
        if (PlayerMoveUI == null) return;
        Vector2 pos = PlayerMoveUI.anchoredPosition;
        pos.x = 1280 * 2;
        PlayerMoveUI.anchoredPosition = pos;
    }
    /// <summary>
    /// 開啟玩家操作UI
    /// </summary>
    public void OpenPlayerMoveUI()
    {
        if (PlayerMoveUI == null) return;
        PlayerMoveUI.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region 英雄選擇角色
    /// <summary>
    /// 關閉英雄選擇介面
    /// </summary>
    public void CloseHeroOptionsUI()
    {
        if (HeroOptionsUI == null) return;
        Vector2 pos = HeroOptionsUI.anchoredPosition;
        pos.y = -720 * 2;
        HeroOptionsUI.anchoredPosition = pos;
    }
    /// <summary>
    /// 開啟英雄選擇介面
    /// </summary>
    public void OpenHeroOptionsUI()
    {
        if (HeroOptionsUI == null) return;
        HeroOptionsUI.anchoredPosition = Vector2.zero;
    }


    #endregion

    #region 搖桿
    /// <summary>
    /// 移開搖桿介面
    /// </summary>
    public void CloseJoystickUI()
    {
        if (JoystickUI == null) return;
        Vector2 pos = JoystickUI.anchoredPosition;
        pos.x = -1280 * 2;
        JoystickUI.anchoredPosition = pos;
    }
    /// <summary>
    /// 開啟搖桿介面
    /// </summary>
    public void OpenJoystickUI()
    {
        if (JoystickUI == null) return;
        JoystickUI.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region 決鬥
    /// <summary>
    /// 關閉決鬥UI
    /// </summary>
    public void CloseDuelUI()
    {
        if (DuelUI == null) return;
        Vector2 pos = DuelUI.anchoredPosition;
        pos.y = 720 * 2;
        DuelUI.anchoredPosition = pos;
    }
    /// <summary>
    /// 開啟決鬥UI
    /// </summary>
    public void OpenDuelUI()
    {
        if (DuelUI == null) return;
        DuelUI.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region 功能
    /// <summary>
    /// 關閉功能型UI
    /// </summary>
    public void CloseFuncUI()
    {
        if (FuncUI == null) return;
        Vector2 pos = FuncUI.anchoredPosition;
        pos.y = 720*2;
        FuncUI.anchoredPosition = pos;
    }
    /// <summary>
    /// 開啟功能型UI
    /// </summary>
    public void OpenFuncUI()
    {
        if (FuncUI == null) return;
        FuncUI.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region 集中管理介面
    /// <summary>
    /// 所有集中管理介面關閉
    /// </summary>
    public void CloseCentralizedManagementUI()
    {
        Time.timeScale = 1f;

        Vector2 pos;
        foreach (RectTransform item in CentralizedManagementUI)
        {
            pos = item.anchoredPosition;
            pos.y = 720 * 2;
            item.anchoredPosition = pos;
        }
    }
    /// <summary>
    /// 使用選單名稱開啟選單
    /// </summary>
    /// <param name="_name">選單物件的名稱</param>
    public void NameOpenCentralizedManagementUI(string _name)
    {
        Time.timeScale = 0f;

        Vector2 pos;
        foreach (RectTransform item in CentralizedManagementUI)
        {
            if (item.name == _name)
            {
                item.anchoredPosition = Vector2.zero;
                continue;
            }
            pos = item.anchoredPosition;
            pos.y = 720 * 2;
            item.anchoredPosition = pos;
        }
    }
    #endregion


    /// <summary>
    /// 使用選單名稱開功能選單或其他操控選單
    /// </summary>
    /// <param name="_name">選單物件的名稱</param>
    public void NameOpenOnlyUI(string _name)
    {
        Time.timeScale = 0f;

        Vector2 pos;
        foreach (RectTransform item in OnlyUI)
        {
            if (item.name == _name)
            {
                item.anchoredPosition = Vector2.zero;
                continue;
            }
            pos = item.anchoredPosition;
            pos.y = 720 * 2;
            item.anchoredPosition = pos;
        }
        Time.timeScale = 1f;
    }

    #region 離開遊戲
    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void GameExiting()
    {
        Application.Quit();
    }
    #endregion
}

