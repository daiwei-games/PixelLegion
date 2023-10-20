using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroItem : MonoBehaviour
{
    /// <summary>
    /// 選擇介面腳本
    /// </summary>
    [Header("選擇介面腳本"), SerializeField]
    UISelectExpeditionCharacters UISetc;
    /// <summary>
    /// 頭像按鈕
    /// </summary>
    [Header("頭像按鈕"), SerializeField]
    Button HeroHeaderButton;
    /// <summary>
    /// 按鈕圖片
    /// </summary>
    [Header("按鈕圖片"), SerializeField]
    Image HeroHeaderButtonImage;
    /// <summary>
    /// 英雄的名稱
    /// </summary>
    [Header("英雄的名稱"), SerializeField]
    TextMeshProUGUI HeroName;
    /// <summary>
    /// 英雄腳本
    /// </summary>
    [Header("英雄腳本"), SerializeField]
    HeroScript _Hs;
    /// <summary>
    /// 資料物件上的清單索引
    /// </summary>
    [Header("資料物件上的清單索引"), SerializeField]
    int _HsListIndex;
    /// <summary>
    /// 將資料賦值讓按鈕有動作
    /// </summary>
    /// <param name="_UISetc">選擇介面腳本</param>
    /// <param name="_Head">按鈕圖片</param>
    /// <param name="_name">英雄的名稱</param>
    /// <param name="_hs">英雄腳本</param>
    /// <param name="_Index">資料物件上的清單索引</param>
    public void HeroDataView(UISelectExpeditionCharacters _UISetc, Sprite _Head, string _name, HeroScript _hs, int _Index)
    {
        UISetc = _UISetc;
        HeroHeaderButtonImage.sprite = _Head;
        HeroName.text = _name;
        _Hs = _hs;
        _HsListIndex = _Index;

        HeroHeaderButton.onClick.AddListener(() => UISetc.AddSelected(_Hs));
    }
}
