using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 已經選擇的英雄頭像
/// </summary>
public class UISelectedHeroHead : MonoBehaviour
{
    /// <summary>
    /// 選擇介面腳本
    /// </summary>
    [Header("選擇介面腳本"), SerializeField]
    UISelectExpeditionCharacters UISetc;
    /// <summary>
    /// 已選擇的頭像按鈕
    /// </summary>
    [Header("已選擇的頭像按鈕"), SerializeField]
    Button HeroHeadButton;
    /// <summary>
    /// 頭像圖片
    /// </summary>
    [Header("頭像圖片"), SerializeField]
    Image HeroHeadImage;
    /// <summary>
    /// 已選擇的英雄
    /// </summary>
    [Header("已選擇的英雄"), SerializeField]
    HeroScript _Hs;
    

    /// <summary>
    /// 將資料賦值讓按鈕有動作
    /// </summary>
    /// <param name="_UISetc">選擇英雄介面腳本</param>
    /// <param name="_Head">英雄頭像</param>
    /// <param name="_hs">英雄腳本</param>
    public void SelectedHeroDataView(UISelectExpeditionCharacters _UISetc, Sprite _Head, HeroScript _hs)
    {
        UISetc = _UISetc;
        HeroHeadImage.sprite = _Head;
        HeroHeadButton.onClick.AddListener(() => UISetc.RemoveSelected(_hs, this));
    }
}
