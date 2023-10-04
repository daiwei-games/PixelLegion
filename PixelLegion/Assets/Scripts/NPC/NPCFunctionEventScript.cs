using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// NPC功能、事件
/// </summary>
public class NPCFunctionEventScript : LeadToSurviveGameBaseClass
{
    #region 可對話物件
    /// <summary>
    /// 可是對話物件預製物
    /// </summary>
    [Header("可是對話物件預製物"), SerializeField]
    Transform DialoguePrefab;
    /// <summary>
    /// 可視對話物件並取得 Transform
    /// </summary>
    Transform Dialogue;
    /// <summary>
    /// 可視對話物件取得 GameObject
    /// </summary>
    GameObject DialogueGameObject;
    /// <summary>
    /// 對話物件偏移
    /// </summary>
    [Header("對話物件偏移"), SerializeField]
    Vector2 DialoguePosition;
    /// <summary>
    /// 對話事件是否有開啟
    /// </summary>
    [HideInInspector]
    public bool DialogueIsOpen;
    #endregion

    #region 射線偵測
    /// <summary>
    /// 偵測到的物件
    /// </summary>
    Collider2D _col;
    /// <summary>
    /// 射線半徑
    /// </summary>
    [Header("射線半徑"), SerializeField]
    float PhysicsRange;
    /// <summary>
    /// 射線範圍
    /// </summary>
    Vector2 PhysicsSize;
    #endregion

    #region UI
    /// <summary>
    /// UI管理腳本
    /// </summary>
    UIScript UIScript;
    /// <summary>
    /// 功能UI腳本
    /// </summary>
    UIFunctionalScript UIFunc;
    /// <summary>
    /// 要開啟的介面名稱
    /// </summary>
    [Header("要開啟的介面名稱"), SerializeField]
    string OpenUIName;
    #endregion
    private void Awake()
    {
        _Tf = transform;
        _Go = gameObject;

        Dialogue = Instantiate(DialoguePrefab, _Tf);
        if (Dialogue != null)
        {
            Dialogue.localPosition = DialoguePosition;
            DialogueGameObject = Dialogue.gameObject;
            DialogueGameObject.SetActive(false);
        }

        UIScript = FindObjectOfType<UIScript>();
        UIFunc = FindObjectOfType<UIFunctionalScript>();
    }
    #region 對話事件
    /// <summary>
    /// 產生對話事件
    /// </summary>
    public void DialogueSystem()
    {
        if (_col != null)
        {
            if (_col.CompareTag(staticPublicObjectsStaticName.HeroTag))
            {
                DialogueGameObject.SetActive(true);
                if (UIFunc != null)
                    UIFunc.ChandeNPCSCript(this);
            }
        }
        else
        {
            DialogueGameObject.SetActive(false);
        }
        DialogueIsOpen = DialogueGameObject.activeSelf;
    }
    /// <summary>
    /// 開始對話
    /// </summary>
    public void DialogueStart()
    {
        if (!DialogueIsOpen) return;
        if (UIScript != null && OpenUIName != "")
        {
            UIScript.NameOpenCentralizedManagementUI(OpenUIName);
        }
    }
    #endregion

    #region 射線
    /// <summary>
    /// 射線偵測
    /// </summary>
    /// <param name="_lm">圖層</param>
    public void PhyOverlapBoxAll(LayerMask _lm)
    {
        PhysicsSize = Vector2.one * PhysicsRange;
        _col = Physics2D.OverlapBox(_Tf.position, PhysicsSize, 0, _lm);
    }

    void OnDrawGizmos()
    {
        if (_Tf == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_Tf.position, PhysicsSize);
    }
    #endregion
}
