using Assets.Scripts.IFace;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IPlayerFunc
{
    [Header("玩家資料")]
    public playerDataObject _playerDataObject;
    public Transform _transform;
    public List<Transform> HeroList;
    #region 遊戲管理器
    /// <summary>
    /// 管理器物件
    /// </summary>
    [Header("管理器物件")]
    public GameObject _gameManager;
    /// <summary>
    /// 遊戲管理器腳本
    /// </summary>
    [Header("遊戲管理器腳本")]
    public GameManager _gmaeManagerScript;
    #endregion
    /// <summary>
    /// 產生間格計時
    /// </summary>
    [Header("產生間隔")]
    public float ProduceHeroTime;
    /// <summary>
    /// 產生間隔
    /// </summary>
    [Range(0,10), SerializeField]
    private float ProduceHeroTimeMax;
    private void Awake()
    {
        PlayerDataInitializ();
    }
    public void PlayerDataInitializ()
    {
        _transform = transform;
        HeroList.AddRange(_playerDataObject.SelectedHeroList);
        _gameManager = GameObject.Find("GameManager");
        if (_gameManager != null)
        {
            _gmaeManagerScript = _gameManager.GetComponent<GameManager>();
            _gmaeManagerScript.playerScript = this;
        }
        ProduceHeroTime = 0;
    }
    public void ProduceHero()
    {
        if (ProduceHeroTime < ProduceHeroTimeMax) return;
        if (HeroList.Count == 0) return;
        int HeroListIndex = Random.Range(0, HeroList.Count);
        Instantiate(HeroList[HeroListIndex]);
        HeroList.RemoveAt(HeroListIndex);
        ProduceHeroTime = 0;
    }





}
