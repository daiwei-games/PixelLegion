using UnityEngine;

/// <summary>
/// 守衛產生節點
/// </summary>
public class GuardProduceNodeScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// f士兵預製物
    /// </summary>
    public GameObject SoldierPrefab;
    private void Start()
    {
        Instantiate(SoldierPrefab,transform.position,Quaternion.identity);
        Destroy(gameObject, 2);
    }
    
}
