using Assets.Scripts.IFace;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IPlayerFunc
{
    [Header("玩家資料")]
    public playerDataObject _playerDataObject;
    public Transform _transform;
    public GameObject _gameManager;
    public GameManager _gmaeManagerScript;

    private void Awake()
    {
        _transform = transform;
        _gameManager = GameObject.Find("GameManager");
        if(_gameManager != null)
            _gmaeManagerScript = _gameManager.GetComponent<GameManager>();
    }
    public void PlayerDataInitializ()
    {

    }


}
