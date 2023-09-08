using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameOverScript : UIScript
{
    /// <summary>
    /// 取得重新開始遊戲的按鈕
    /// </summary>
    public Button _Button_GameOver;
    /// <summary>
    /// 只執行一次呼叫UI
    /// 因為裡面有協呈
    /// </summary>
    private bool isGameOver;
    private void Awake()
    {
        GUIDataInitializ();
    }
    public override void GUIDataInitializ()
    {

        _transform = transform;
        Transform _Button_GameOverTransform = transform.Find("Button_GameReset");
        if (_Button_GameOverTransform != null)
        {
            _Button_GameOver = _Button_GameOverTransform.GetComponent<Button>();
            if (_Button_GameOver != null)
            {
                _Button_GameOver.onClick.AddListener(() =>
                {
                    ReStartTheGame();
                });
            }
        }
        isGameOver = true;
    }

    public void GameOverUI()
    {
        if (!isGameOver) return;
        isGameOver = false;
        RectTransform uiElement = GetComponent<RectTransform>();
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

    public void ReStartTheGame()
    {
        Time.timeScale = 1;
        Scene _scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(_scene.name);
    }
}
