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

    private void Awake()
    {
        GUIDataInitializ();
    }
    public override void GUIDataInitializ()
    {

        _Tf = transform;
        _Go = gameObject;
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
    }
    public void ReStartTheGame()
    {
        Time.timeScale = 1;
        Scene _scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(_scene.name);
    }
}
