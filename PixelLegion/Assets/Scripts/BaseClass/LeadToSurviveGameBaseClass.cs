using UnityEngine;
/// <summary>
/// 所有共用變數、函式基類
/// </summary>
public class LeadToSurviveGameBaseClass : MonoBehaviour
{
    /// <summary>
    /// 物件自身 Transform
    /// </summary>
    [HideInInspector]
    public Transform _Tf;
    /// <summary>
    /// 物件自身 GameObject
    /// </summary>
    [HideInInspector]
    public GameObject _Go;

}
