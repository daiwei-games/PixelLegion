using UnityEngine;

/// <summary>
/// 守衛產生節點
/// </summary>
public class GuardProduceNodeScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 光明還是黑暗
    /// </summary>
    [Header("光明還是黑暗")]
    public PromisingOrDARK _pod;
    /// <summary>
    /// 職責 用在這裡主要是用來判斷產生守衛還是士兵、野生怪物
    /// </summary>
    [Header("職責")]
    public SoldierPost _Sp;
    /// <summary>
    /// 遊戲管理器
    /// </summary>
    [Header("遊戲管理器")]
    public GameManager _gameManager;
    /// <summary>
    /// 士兵預製物
    /// </summary>
    [Header("要產生的守衛")]
    public GameObject SoldierPrefab;
    /// <summary>
    /// 守衛腳本
    /// </summary>
    [Header("守衛腳本")]
    public SoldierScript _Ss;
    private void Start()
    {

        GameObject _go = Instantiate(SoldierPrefab,transform.position,Quaternion.identity);
        _Ss = _go.GetComponent<SoldierScript>();
        _Ss._Sp = _Sp;

        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.SoldierDataFormat(_Ss);
    }
    
    
}
