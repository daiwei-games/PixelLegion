using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;
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
    /// <summary>
    /// 取得子物件
    /// </summary>
    [Header("取得子物件"), SerializeField]
    private List<UIScript> UIList;
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

        for (int i = 0; i < UIList.Count; i++)
        {
            UIList[i]._gameManager = _gameManager;
            UIList[i]._gameManagerScript = _gameManagerScript;
        }

        
    }
}
