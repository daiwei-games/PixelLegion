using UnityEngine;
/// <summary>
/// 道具資料
/// </summary>
public class PropsDataScript : MonoBehaviour
{
    /// <summary>
    /// 道具ID
    /// </summary>
    [Header("道具ID")]
    public string PropsID;
    /// <summary>
    /// 道具名稱
    /// </summary>
    [Header("道具名稱")]
    public string PropsName;
    /// <summary>
    /// 道具說明
    /// </summary>
    [Header("道具說明"),TextArea(1,10)]
    public string PropsDescription;
    /// <summary>
    /// 道具圖示
    /// </summary>
    [Header("道具圖示")]
    public SpriteRenderer PropsIcon;
    /// <summary>
    /// 道具價格
    /// </summary>
    [Header("道具價格")]
    public int PropsPrice;
    /// <summary>
    /// 販賣道具獲得的技能經驗值
    /// </summary>
    [Header("技能經驗值")]
    public int PropsJobExp;
    /// <summary>
    /// 獲得道具機率
    /// </summary>
    [Header("獲得道具機率")]
    public float PropsGetRate;
    /// <summary>
    /// 可以做甚麼?
    /// </summary>
    [Header("可以做甚麼?")]
    public PropsType PropsType;
}
