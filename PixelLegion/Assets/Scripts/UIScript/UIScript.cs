using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIScript : MonoBehaviour, IGUIFunc
{
    public Transform _transform;
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

    public List<UIScript> UIList;
    private void Awake()
    {
        GUIDataInitializ();
    }
    public virtual void GUIDataInitializ()
    {
        _transform = transform;
        _gameManager = GameObject.Find("GameManager");
        if (_gameManager != null)
        {
            _gameManagerScript = _gameManager.GetComponent<GameManager>();
                if (_gameManagerScript != null)
                    _gameManagerScript.uiScript = GetComponent<UIScript>();
        }
    }
    /// <summary>
    /// 重新開始遊戲
    /// </summary>
    public virtual void ReStartTheGame()
    {
        
    }
}
