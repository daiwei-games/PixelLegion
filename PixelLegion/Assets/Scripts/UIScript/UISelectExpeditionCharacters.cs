using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 選擇出戰英雄
/// </summary>
public class UISelectExpeditionCharacters : LeadToSurviveGameBaseClass
{
    #region 選擇介面 (右邊)
    /// <summary>
    /// 這個玩家所擁有的角色不管事正在收集，或者已經收集完畢的
    /// </summary>
    [Header("已經在收集或收集完畢的角色")]
    public CharacterListDataObject AllCharactersList;

    /// <summary>
    /// 選擇英雄介面的可選擇介面 (右邊)
    /// </summary>
    [Header("選擇英雄介面的可選擇介面 (右邊)"), SerializeField]
    Transform ContentTf;
    /// <summary>
    /// 第一個可選擇的英雄物件
    /// </summary>
    [Header("第一個可選擇的英雄物件"), SerializeField]
    UIHeroItem HeroItem;
    /// <summary>
    /// 選擇按鈕陣列
    /// </summary>
    List<UIHeroItem> HeroItemList;
    #endregion

    #region 選擇介面 (左邊)
    /// <summary>
    /// 可以出征的角色
    /// </summary>
    [Header("可以出征的角色")]
    public playerDataObject OwnedCharactersList;
    /// <summary>
    /// 左邊已選擇的英雄
    /// </summary>
    [Header("左邊英雄"), SerializeField]
    Transform HeroHeadItemTf;
    /// <summary>
    /// 左邊已選擇的英雄頭像
    /// </summary>
    [Header("左邊已選擇的英雄頭像"), SerializeField]
    UISelectedHeroHead SelectedHeroItem;
    /// <summary>
    /// 已選擇的英雄頭像按鈕清單
    /// </summary>
    [Header("已選擇的英雄頭像按鈕清單"), SerializeField]
    List<UISelectedHeroHead> SelectedHeroList;
    #endregion
    /// <summary>
    /// 警告標語
    /// </summary>
    [Header("警告標語"), SerializeField]
    TextMeshProUGUI Notification;

    private void OnEnable()
    {
        UISetcInitializ();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void UISetcInitializ()
    {
        _Tf = transform;
        _Go = gameObject;
        #region 選擇介面 (右邊)
        HeroItemList = new List<UIHeroItem>();
        List<HeroScript> _hsList = AllCharactersList.HeroList;
        ContentTf = _Tf.Find("Scroll View/Viewport/Content");
        if (ContentTf != null)
        {
            HeroItem = ContentTf.GetComponentInChildren<UIHeroItem>();
            if (HeroItem != null)
            {
                HeroScript _hs = AllCharactersList.HeroList[0];
                HeroItem.HeroDataView(this, _hs.HeroAvatar, _hs.HeroName, _hs, 0);
                HeroItemList.Add(HeroItem);


                UIHeroItem _uIHeroItem;
                for (int i = 1; i < _hsList.Count; i++)
                {
                    _hs = _hsList[i];
                    if (_hs == null) continue;
                    _uIHeroItem = Instantiate(HeroItem, ContentTf);
                    _uIHeroItem.HeroDataView(this, _hs.HeroAvatar, _hs.HeroName, _hs, i);
                    HeroItemList.Add(_uIHeroItem);
                }
            }
        }
        #endregion

        #region 選擇介面 (左邊)
        SelectedHeroList = new List<UISelectedHeroHead>();
        List<HeroScript> OwnedList = OwnedCharactersList.SelectedHeroList;
        HeroHeadItemTf = _Tf.Find("出戰英雄_Head");
        if (HeroHeadItemTf != null)
        {
            SelectedHeroItem = HeroHeadItemTf.GetComponentInChildren<UISelectedHeroHead>();
            if (SelectedHeroItem != null)
            {
                if (OwnedCharactersList.SelectedHeroList.Count == 0) return;
                HeroScript _hs2;
                UISelectedHeroHead _UIShh;
                SelectedHeroItem.gameObject.SetActive(false);
                for (int i = 0; i < OwnedList.Count; i++)
                {
                    _hs2 = OwnedList[i];
                    if (_hs2 == null) continue;
                    _UIShh = Instantiate(SelectedHeroItem, HeroHeadItemTf);
                    _UIShh.SelectedHeroDataView(this, _hs2.HeroAvatar, _hs2);
                    _UIShh.gameObject.SetActive(true);
                    SelectedHeroList.Add(_UIShh);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 刪除已選擇英雄
    /// </summary>
    /// <param name="_hs">已被選擇的英雄</param>
    public void RemoveSelected(HeroScript _hs, UISelectedHeroHead _UISh)
    {
        List<HeroScript> OwnedList = OwnedCharactersList.SelectedHeroList;
        if (OwnedList.Count < 2) {
            Notification.gameObject.SetActive(true);
            Notification.text = "提醒:至少要 1 位英雄";
            return;
        }
        if (OwnedList.Contains(_hs))
        {
            OwnedCharactersList.SelectedHeroList.Remove(_hs);
            if (SelectedHeroList.Contains(_UISh))
            {
                SelectedHeroList.Remove(_UISh);
                Destroy(_UISh.gameObject);
            }
        }

    }

    /// <summary>
    /// 選擇英雄
    /// </summary>
    /// <param name="_hs">要加入的英雄</param>
    public void AddSelected(HeroScript _hs)
    {
        List<HeroScript> OwnedList = OwnedCharactersList.SelectedHeroList;
        if (OwnedList.Contains(_hs))
        {
            Notification.gameObject.SetActive(true);
            Notification.text = "提醒:此英雄已存在";
            return;
        }
        if(OwnedList.Count == 3)
        {
            Notification.gameObject.SetActive(true);
            Notification.text = "提醒:只能選擇3位英雄";
            return;
        }
        OwnedCharactersList.SelectedHeroList.Add(_hs);
        UISelectedHeroHead _UIShh = Instantiate(SelectedHeroItem, HeroHeadItemTf);
        _UIShh.SelectedHeroDataView(this, _hs.HeroAvatar, _hs);
        _UIShh.gameObject.SetActive(true);
        SelectedHeroList.Add(_UIShh);
    }
}
