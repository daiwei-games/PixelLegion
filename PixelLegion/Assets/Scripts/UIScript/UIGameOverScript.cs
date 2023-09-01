using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverScript : UIScript
{

    public Button _Button_GameOver;
    private void Awake()
    {

        _Button_GameOver = transform.Find("Button_GameReset").GetComponent<Button>();

        GUIDataInitializ();

        GameOverUI();
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

        _gameManager = GameObject.Find("GameManager");
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>();
            if (_gameManagerScript != null)
                _gameManagerScript.GameOverObject = GetComponent<UIGameOverScript>();
        }


    }

    public void GameOverUI()
    {
        RectTransform uiElement = GetComponent<RectTransform>();
        

        uiElement.anchoredPosition = Vector2.zero;
        Debug.Log(uiElement.anchoredPosition);
    }
    public override void ReStartTheGame()
    {


    }
}
