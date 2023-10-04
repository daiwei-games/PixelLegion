using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能型UI
/// </summary>
public class UIFunctionalScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 對話按鈕
    /// </summary>
    [Header("對話按鈕")]
    public Button DialogueButton;
    /// <summary>
    /// 現在的NPC腳本
    /// </summary>
    [Header("現在的NPC腳本")]
    public NPCFunctionEventScript NowNPCScript;
    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;

        Transform _tfFind = _Tf.Find("對話");
        if (_tfFind != null)
        {
            DialogueButton = _tfFind.GetComponent<Button>();
            DialogueButton.onClick.AddListener(() => {
                if (NowNPCScript != null)
                {
                    NowNPCScript.DialogueStart();
                }
            });
        }
    }


    public void ChandeNPCSCript(NPCFunctionEventScript _npc)
    {
        NowNPCScript = _npc;
    }
}
