using UnityEngine;
/// <summary>
/// 彈葯收集器 (軍火庫)
/// </summary>
public class AmmunitionScript : MonoBehaviour
{
    public Transform ArrowPrefab;
    /// <summary>
    /// 使用變數名稱取得發射物件
    /// </summary>
    /// <param name="PrefabName">發射物件的變數名稱</param>
    /// <returns>返回發射物件</returns>
    public Transform PrefabNameGetPrefab(string PrefabName)
    {
        return PrefabName switch
        {
            "ArrowPrefab" => ArrowPrefab,
            _ => null,
        };
    }
}
