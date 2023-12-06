using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 背包界面操作
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
    /// 背包顯示地目前金錢
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI _backpackMoney;
    /// <summary>
    ///金幣文字物件的Transform
    /// </summary>
    Transform _money;
    /// <summary>
    /// 道具圖片
    /// </summary>
    [HideInInspector]
    public Image _propsIcon;
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
        GETPropsDataObject();
    }

    /// <summary>
    /// 背包資料取得
    /// </summary>
    public void BackpackData()
    {
        Transform ContentTf = _Tf.Find("物品欄邊框/Scroll View/Viewport/Content");
        Button _but = ContentTf.Find("BackpackSlot").GetComponent<Button>(); // 被複製按鈕

        PropsBackpackDataScript _pbds;
        Button _InsBut; // 複製按鈕
        Image _InsButImage; // 道具圖片
        TextMeshProUGUI _StockText; // 數量文字
        for (int i = 0; i < _pldo.AlreadyHaveProps.Count; i++)
        {
            int Index = i;
            _pbds = _pldo.AlreadyHaveProps[Index];
            if (_pbds != null)
            {
                _InsBut = Instantiate(_but, ContentTf); // 複製按鈕
                _InsBut.name = _but.name + Index; // 複製按鈕名稱

                // 複製按鈕內容(道具圖片)
                _InsButImage = _InsBut.transform.Find("PropsIcon").GetComponent<Image>();
                _InsButImage.sprite = _pbds.PropsData.GetComponent<SpriteRenderer>().sprite;
                // 複製按鈕內容(數量文字)
                _StockText = _InsBut.transform.Find("存量").GetComponent<TextMeshProUGUI>();
                _StockText.text = _pbds.PropsCount.ToString();

                _InsBut.onClick.AddListener(() =>
                {
                    OnClickPropsRowButton(_pldo.AlreadyHaveProps[Index]);
                });
                PropsRowButtonList.Add(_InsBut);
                _InsBut = null;
            }
        }
        _but.gameObject.SetActive(false);

        _money = _Tf.Find("物品欄邊框/金幣");
        UpdateMoneyText(_pldo.Money);
    }
    /// <summary>
    /// 更新金錢的數字
    /// </summary>
    /// <param name="_nowMoney">現在的金錢</param>
    public void UpdateMoneyText(int _nowMoney)
    {
        if (_money == null) return;
        _backpackMoney = _money.GetComponent<TextMeshProUGUI>();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        nfi.CurrencyDecimalDigits = 0;
        _backpackMoney.text = _pldo.Money.ToString("c", nfi);
    }
    /// <summary>
    /// 取得資訊顯示界面
    /// </summary>
    private void GETPropsDataObject()
    {
        Transform _PropsTf;
        _PropsTf = _Tf.Find("物品資訊欄");
        _propsIcon = _PropsTf.Find("ICON").GetComponent<Image>();
        _propsName = _PropsTf.Find("名稱").GetComponent<TextMeshProUGUI>();
        _propsJobExp = _PropsTf.Find("技能經驗").GetComponent<TextMeshProUGUI>();
        _propsMoney = _PropsTf.Find("販售價格").GetComponent<TextMeshProUGUI>();
        _propsDescribe = _PropsTf.Find("物品描述/描述").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 點下道具顯示道具資料
    /// </summary>
    /// <param name="_pbds">道具本身</param>
    public void OnClickPropsRowButton(PropsBackpackDataScript _pbds)
    {
        _propsIcon.sprite = _pbds.PropsData.PropsIcon.sprite;
        _propsName.text = $"名     稱:{_pbds.PropsData.PropsName}";
        _propsJobExp.text = $"技能經驗:{_pbds.PropsData.PropsJobExp}";
        _propsMoney.text = $"販售價格:{_pbds.PropsData.PropsPrice}";
        _propsDescribe.text = $"描述:\n{_pbds.PropsData.PropsDescription}";
    }
}
