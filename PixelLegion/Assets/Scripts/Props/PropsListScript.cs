using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 道具清單
/// </summary>
public class PropsListScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 道具資料庫
    /// </summary>
    [Header("道具資料庫")]
    public PropsListDataObject _pldo;

    /// <summary>
    /// 背包欄位按鈕
    /// </summary>
    [Header("背包欄位按鈕")]
    public List<Button> PropsRowButtonList;

    /// <summary>
    /// 道具名稱
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI _propsName;
    /// <summary>
    /// 販賣道具技能經驗值
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI _propsJobExp;
    /// <summary>
    /// 販賣道具獲得金錢
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI _propsMoney;
    /// <summary>
    /// 道具描述
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI _propsDescribe;

    private void Start()
    {
        _Tf = transform;
        _Go = gameObject;


        BackpackData();

    }

    /// <summary>
    /// 背包資料取得
    /// </summary>
    public void BackpackData()
    {
        Transform ContentTf = _Tf.Find("物品欄邊框/Scroll View/Viewport/Content");
        Button _but = ContentTf.Find("BackpackSlot").GetComponent<Button>();
        PropsBackpackDataScript _pbds;
        Button _InsBut;
        Image _InsButImage;
        for (int i = 0; i < _pldo.AlreadyHaveProps.Count; i++)
        {
            int Index = i;
            _pbds = _pldo.AlreadyHaveProps[Index];
            if (_pbds != null)
            {
                _InsBut = Instantiate(_but, ContentTf);
                _InsBut.name = _but.name + Index;
                _InsButImage = _InsBut.transform.Find("PropsIcon").GetComponent<Image>();
                _InsButImage.sprite = _pbds.PropsData.GetComponent<SpriteRenderer>().sprite;

                _InsBut.onClick.AddListener(() => {
                    OnClickPropsRowButton(_pbds);
                });
                PropsRowButtonList.Add(_InsBut);
                _InsBut = null;
            }
        }
        _but.gameObject.SetActive(false);
    }

    public void OnClickPropsRowButton(PropsBackpackDataScript _pbds)
    {
        
    }
}
