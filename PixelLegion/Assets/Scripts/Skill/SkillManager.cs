using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能管理器
/// </summary>
public class SkillManager : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 道具資料庫
    /// </summary>
    [Header("道具資料庫")]
    public PropsListDataObject _pldo;
    #region 左邊介面
    /// <summary>
    /// 技能資料庫
    /// </summary>
    public SkillListDataObject _sldo;
    /// <summary>
    /// 所有技能按鈕清單
    /// </summary>
    [HideInInspector]
    public List<Button> SkillIconItem;
    /// <summary>
    /// 顯示經驗值的文字物件
    /// </summary>
    [Header("顯示經驗值的文字物件"), HideInInspector]
    public TextMeshProUGUI LeftSkillExpText;
    /// <summary>
    /// 顯示經驗值的文字物件的Transform
    /// </summary>
    Transform LeftSkillExpTextTf;
    #endregion
    #region 右邊介面
    /// <summary>
    /// 技能圖示
    /// </summary>
    [Header("技能圖示"), HideInInspector]
    public Image SkillIcon;
    /// <summary>
    /// 技能名稱
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI SkillName;
    /// <summary>
    /// 右邊介面經驗值文字物件
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI RightSkillExpText;
    /// <summary>
    /// 技能需要的物品
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI SkillNeedItems;
    /// <summary>
    /// 技能需要的物品圖示
    /// </summary>
    [HideInInspector]
    public Image SkillNeedIcons;
    /// <summary>
    /// 技能描述
    /// </summary>
    [HideInInspector]
    public TextMeshProUGUI SkillDescribe;
    /// <summary>
    /// 獲得技能
    /// </summary>
    [Header("獲得技能按鈕"), HideInInspector]
    public Button GetSkillButton;
    #endregion


    private void Start()
    {
        //ReLoadingData();
    }

    /// <summary>
    /// 重新讀取英雄資料
    /// </summary>
    public void ReLoadingData()
    {
        _Tf = transform;
        _Go = gameObject;

        #region 左邊介面


        Transform SkillIconItemParent = _Tf.Find("所有技能/所有技能ICON/Scroll View/Viewport/Content");
        Transform SkillIconItemTf = SkillIconItemParent.transform.Find("SkillIconItem");


        Transform SkillIconItemRtfInsert;
        Button _but;
        Image _si;
        SkillData _sd;
        float ColorA = .3f;
        for (int i = 0; i < _sldo.SkillList.Count; i++)
        {
            SkillIconItemRtfInsert = Instantiate(SkillIconItemTf, SkillIconItemTf.parent);
            SkillIconItemRtfInsert.name = "SkillIconItem" + i;
            _but = SkillIconItemRtfInsert.GetComponent<Button>();
            _si = SkillIconItemRtfInsert.Find("Icon").GetComponent<Image>();
            _sd = _sldo.SkillList[i];
            _si.sprite = _sd.SkillIcon;
            ColorA = .3f;
            if (_sd.IsGetSkill) ColorA = 1;
            _si.color = new Color(1, 1, 1, ColorA);

            int Index = i;
            _but.onClick.AddListener(() => ClickReadData(_sldo.SkillList[Index]));
            SkillIconItem.Add(_but);
        }
        SkillIconItemTf.gameObject.SetActive(false);

        #endregion
        Transform _tf = _Tf.Find("說明");
        if (SkillIcon == null)
            SkillIcon = _tf.Find("ICON").GetComponent<Image>();
        if (SkillName == null)
            SkillName = _tf.Find("名稱").GetComponent<TextMeshProUGUI>();
        if (RightSkillExpText == null)
            RightSkillExpText = _tf.Find("所需經驗").GetComponent<TextMeshProUGUI>();
        if (SkillNeedItems == null)
            SkillNeedItems = _tf.Find("所需物品").GetComponent<TextMeshProUGUI>();
        if (SkillNeedIcons == null)
            SkillNeedIcons = _tf.Find("所需物品/所需物品ICONs").GetComponent<Image>();
        if (SkillDescribe == null)
            SkillDescribe = _tf.Find("技能描述/描述").GetComponent<TextMeshProUGUI>();
        if (GetSkillButton == null)
            GetSkillButton = _tf.Find("獲得技能").GetComponent<Button>();

        UpdateSkillExp(_pldo.AlreadyHaveSkillExp);
    }
    /// <summary>
    /// 更新經驗值數字
    /// </summary>
    /// <param name="_exp">現存經驗直</param>
    private void UpdateSkillExp(int _exp)
    {
        if (LeftSkillExpTextTf == null)
            LeftSkillExpTextTf = _Tf.Find("所有技能/經驗值");
        if (LeftSkillExpTextTf == null) return;

        if (LeftSkillExpText == null)
            LeftSkillExpText = LeftSkillExpTextTf.GetComponent<TextMeshProUGUI>();
        if (LeftSkillExpText == null) return;
        LeftSkillExpText.text = $"經驗值:{_exp}";
    }
    /// <summary>
    /// 點擊按鈕執行後，取得右邊的介面所需資料
    /// </summary>
    /// <param name="_sd">每個按鈕所代表的技能資訊</param>
    public void ClickReadData(SkillData _sd)
    {

        Image _si = _Tf.Find("說明/所需物品/所需物品ICONs/所需物品ICON").GetComponent<Image>();
        Image _siInsert;
        Transform _tf = SkillNeedIcons.transform;
        string _sn = "";
        string _comma = "";
        SkillIcon.sprite = _sd.SkillIcon;
        SkillName.text = $"名     稱:{_sd.SkillName}";
        RightSkillExpText.text = $"所需經驗:{_sd.SkillExp}";
        for (int i = 0; i < _sd.SkillProps.Count; i++)
        {
            _sn += $"{_comma}{_sd.SkillProps[i].name}";
            _comma = ",";

            _siInsert = Instantiate(_si, _tf);
            _siInsert.sprite = _sd.SkillProps[i].GetComponent<SpriteRenderer>().sprite;
        }
        SkillNeedItems.text = $"所需物品:{_sn}";
        SkillDescribe.text = $"描述:\r\n{_sd.SkillDescription}";

        _si.gameObject.SetActive(false);
        GetSkillButton.onClick.RemoveAllListeners();

        GetSkillButton.interactable = true;
        if (_sd.IsGetSkill)
        {
            GetSkillButton.interactable = false;
            return;
        }
        GetSkillButton.onClick.AddListener(() =>
        {
            GetSkill(_sd);
        });
    }
    /// <summary>
    /// 獲得技能
    /// </summary>
    public void GetSkill(SkillData _sd)
    {
        if (_pldo.AlreadyHaveSkillExp < _sd.SkillExp) return;
        _sd.IsGetSkill = true;
        _pldo.AlreadyHaveSkillExp -= _sd.SkillExp;
        UpdateSkillExp(_pldo.AlreadyHaveSkillExp);
        GetSkillButton.interactable = false;
        Debug.Log("已學到技能");
    }
}
